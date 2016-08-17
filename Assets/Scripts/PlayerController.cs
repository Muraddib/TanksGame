using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public WeaponInfo CurrentWeapon;
    [HideInInspector] public WeaponInfo[] Weapons;
    public float Health;
    public float Armor;
    private bool isLive;
    private Transform thisTransform;
    private Rigidbody thisRigidbody;

    public void TakeDamage(float damageAmount, WeaponIDs weaponSource)
    {
        if (isLive)
        {
            Armor -= 0.01f;
            Armor = Mathf.Clamp(Armor, 0f, 1f);
            Health = Health - damageAmount * (1f-Armor);
            if (Health <= 0f)
            {
                Health = 0f;
                Armor = 0f;
                isLive = false;
                StartCoroutine(WaitBeforeDeathEvent(0f));
            }
        }
        EventManager.CallSoundEvent(AudioIDs.MeleeHit, thisTransform.position);
        EventManager.CallGameEvent(EventManager.GameEvents.PlayerHit);
    }

    private IEnumerator WaitBeforeDeathEvent(float time)
    {
        yield return new WaitForSeconds(time);
        EventManager.CallGameEvent(EventManager.GameEvents.PlayerDied);
    }

    public void Init(GameSettings settings)
    {
        thisTransform = gameObject.transform;
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        Weapons = settings.Weapons;
        SetDefaults();
    }

    public void SetDefaults()
    {
        isLive = true;
        Health = 100f;
        Armor = 0f;
        SetDefaultWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GetPreviousWeapon();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetNextWeapon();
        }
    }

    private void GetNextWeapon()
    {
        if ((byte) CurrentWeapon.WeaponID + 1 <= Weapons.Length - 1)
            CurrentWeapon = Weapons[(byte) CurrentWeapon.WeaponID + 1];
        else
            CurrentWeapon = Weapons[0];
        EventManager.CallGameEvent(EventManager.GameEvents.WeaponChanged);
    }

    private void GetPreviousWeapon()
    {
        if ((byte) CurrentWeapon.WeaponID - 1 >= 0)
            CurrentWeapon = Weapons[(byte) CurrentWeapon.WeaponID - 1];
        else
            CurrentWeapon = Weapons[Weapons.Length - 1];
        EventManager.CallGameEvent(EventManager.GameEvents.WeaponChanged);
    }

    private void SetDefaultWeapon()
    {
        CurrentWeapon = Weapons[0];
    }

    private void AddHealth(BonusInfo bonus)
    {
        Health = Mathf.Clamp(Health + bonus.HealthBonus, 0f, 100f);
    }

    private void AddDefence(BonusInfo bonus)
    {
        Armor = Mathf.Clamp(Armor + bonus.ArmorBonus, 0f, 1f);
    }

    public void ConsumeBonus(Bonus bonus)
    {
        switch (bonus.BonusParameters.BonusID)
        {
            case BonusIDs.Health:
                AddHealth(bonus.BonusParameters);
                break;
            case BonusIDs.Defence:
                AddDefence(bonus.BonusParameters);
                break;
        }
        bonus.IsActive = false;
        EventManager.CallBonusEvent(bonus.BonusParameters);
        EventManager.CallSoundEvent(bonus.BonusParameters.AudioID, thisTransform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision rigidbody velocity:"+thisRigidbody.velocity.magnitude);
        var enemy = collision.collider.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            if (thisRigidbody.velocity.magnitude > 3f)
            {
                Debug.Log(thisRigidbody.velocity.magnitude);
                enemy.TakeDamage(thisRigidbody.velocity.magnitude*10f, WeaponIDs.Blunt);
                TakeDamage(enemy.EnemyWeapon.DamageAmount, enemy.EnemyWeapon.WeaponID);
            }
        }
    }
}
