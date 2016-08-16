using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : Enemy {

	public void Initialize()
	{
        gameObject.SetActive(true);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().detectCollisions = true;
        gameObject.GetComponent<Collider>().enabled = true;
	    EnemyProperties = GetEnemyProperties();
	    CharacterTransform = gameObject.transform;
        CharacterAnimator = gameObject.GetComponent<Animator>();
	    CharacterAnimator.enabled = true;
        NavAgent = GetComponent<NavMeshAgent>();
	    NavAgent.enabled = true;
        NavAgent.speed = EnemyProperties.MovementSpeed;
        NavDestination = NavAgent.destination;
	    NavTarget = GameObject.FindGameObjectWithTag("Player").transform;
	    ChangeState(EnemyState.Move);
        if (Ragdoll == null || Ragdoll.Count == 0) GetRagdollParts();
        else SetRagdollParts();
        GetRagdollParts();
	    Health = EnemyProperties.MaxHealth;
	    isLive = true;
	}

    private EnemyInfo GetEnemyProperties()
    {
        foreach (var info in GameplayController.Instance.Settings.Enemies)
        {
            if (info.EnemyID == EnemyID)
            {
                return info;
            }
        }
        return GameplayController.Instance.Settings.Enemies[0];
    }

    public override void Reset()
    {
        gameObject.SetActive(false);
    }

    public override bool IsActive
    {
        get { return active; }
        set
        {
            if (value)
            {
                Initialize();
            }
            else
            {
                Reset();
            }
            active = value;
        }
    }

    private void SetRagdollParts()
    {
        Ragdoll.ForEach(a =>
            {
                a.GetComponent<Collider>().enabled = false;
                a.GetComponent<Rigidbody>().isKinematic = true;
                a.GetComponent<Rigidbody>().detectCollisions = false;
            });
    }

    private void GetRagdollParts()
    {
        Ragdoll = new List<GameObject>();
        foreach (var ragdollpart in CharacterTransform.GetComponentsInChildren<Rigidbody>())
        {
            if (ragdollpart.gameObject != gameObject)
            {
                ragdollpart.gameObject.GetComponent<Collider>().enabled = false;
                ragdollpart.isKinematic = true;
                ragdollpart.detectCollisions = false;
                Ragdoll.Add(ragdollpart.gameObject);
            }
        }
    }

    public override void ChangeState(EnemyState newState)
    {
        CEnemyState = newState;
        CharacterAnimator.ResetTrigger("takedamage");
        CharacterAnimator.ResetTrigger("attack");
        CharacterAnimator.ResetTrigger("move");
        CharacterAnimator.ResetTrigger("idle");
        CharacterAnimator.ResetTrigger("die");
        switch (CEnemyState)
        {
            case EnemyState.Move:
                NavAgent.Resume();
                CharacterAnimator.SetTrigger("move");
                CurrentUpdateState = UpdateStatePosition;
                break;
            case EnemyState.Attack:
                NavAgent.Stop();
                NextAttack = Time.time + EnemyProperties.AttackRate;
                CharacterAnimator.SetTrigger("attack");
                CharacterAnimator.SetFloat("attackblend", Random.Range(0,2));
                CurrentUpdateState = UpdateStateAttack;
                break;
            case EnemyState.Die:
                NavAgent.enabled = false;
                SetRagdollActive();
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                gameObject.GetComponent<Collider>().enabled = false;
                CharacterAnimator.SetTrigger("die");
                CurrentUpdateState = UpdateStateEmpty;
                EventManager.CallSoundEvent(AudioIDs.OrcDie, CharacterTransform.position);
                EventManager.CallEnemyDeathEvent(EnemyProperties);
                GameplayController.Instance.SessionScore += EnemyProperties.Score;
                StartCoroutine(SetPoolableInactive(5f));
                GameObject bonus = ObjectPool.Instance.GetBonus(
                    (BonusIDs) Random.Range(0, BonusIDs.GetNames(typeof (BonusIDs)).Length));
                if (bonus)
                {
                    bonus.transform.position = CharacterTransform.position;
                    bonus.GetComponentInChildren<Rigidbody>().velocity = Vector3.up*10f;
                }
                break;
        }
    }

    private IEnumerator SetPoolableInactive(float time)
    {
        yield return new WaitForSeconds(time);
        IsActive = false;
    }

    private void UpdateStateEmpty(){}

    private void UpdateStatePosition()
    {
        if (Vector3.Distance(NavDestination, NavTarget.position) > 1.0f)
        {
            NavDestination = NavTarget.position;
            NavAgent.destination = NavDestination;
        }

        if (Vector3.Distance(CharacterTransform.position, NavTarget.position) < EnemyProperties.AttackDistance)
        {
            ChangeState(EnemyState.Attack);
        }
    }
    
    public void OnAnimationAttackEvent()
    {
        Debug.Log("OnAnimationAttackEvent()");
        if (EnemyProperties.RangedAttack)
        {
            Vector3 playerDir = NavTarget.position - CharacterTransform.position;
            GameObject projectile =
                Instantiate(EnemyProperties.RangedAttackProjectile,
                            CharacterTransform.position + (playerDir.normalized*5f), Quaternion.identity) as GameObject;

            projectile.GetComponent<Rigidbody>().velocity = playerDir.normalized*15f;
            projectile.GetComponent<WeaponProjectile>().ProjectileOwner = WeaponProjectile.Owner.Enemy;
            projectile.GetComponent<WeaponProjectile>().WeaponSource = EnemyWeapon;
        }
        else
        {
            GameplayController.Instance.Player.TakeDamage(EnemyWeapon.DamageAmount, EnemyWeapon.WeaponID);
        }
    }

    private void UpdateStateAttack()
    {
        Vector3 playerDir = new Vector3(NavTarget.position.x, 0f, NavTarget.position.z) - new Vector3(CharacterTransform.position.x, 0f, CharacterTransform.position.z);

        CharacterTransform.rotation = Quaternion.Lerp(CharacterTransform.rotation, Quaternion.LookRotation(playerDir),
                                                      Time.deltaTime*3.0f);
        if (Time.time > NextAttack)
        {
            ChangeState(EnemyState.Attack);
            NextAttack = Time.time + EnemyProperties.AttackRate;
        }

        if (Vector3.Distance(CharacterTransform.position, NavTarget.position) > EnemyProperties.AttackDistance)
        {
            ChangeState(EnemyState.Move);
        }
    }

    public override void SetRagdollActive()
    {
        CharacterAnimator.enabled = false;
        Ragdoll.ForEach(a =>
            {
                a.GetComponent<Rigidbody>().detectCollisions = true;
                a.GetComponent<Rigidbody>().isKinematic = false;
                a.GetComponent<Collider>().enabled = true;
            }
        );
    }

    public override void TakeDamage(float damageAmount, WeaponIDs weaponSource)
    {
        CharacterAnimator.SetTrigger("takedamage");
        Health -= damageAmount;
        if (Health <= 0 && isLive)
        {
            isLive = false;
            ChangeState(EnemyState.Die);
        }
    }

	void Update ()
	{
        if (CurrentUpdateState != null) CurrentUpdateState();
	}
}
