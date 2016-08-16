using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour, IPoolable {

    public Transform NavTarget;
    public EnemyIDs EnemyID;
    [HideInInspector]
    public EnemyInfo EnemyProperties;
    public WeaponInfo EnemyWeapon;
    protected Transform CharacterTransform;
    protected Animator CharacterAnimator;
    protected Vector3 NavDestination;
    protected NavMeshAgent NavAgent;
    public List<GameObject> Ragdoll;
    protected float NextAttack;
    public float Health;
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Die
    }
    public EnemyState CEnemyState;
    public abstract void ChangeState(EnemyState newState);
    public abstract void SetRagdollActive();
    public abstract void TakeDamage(float damageAmount, WeaponIDs weaponSource);
    protected delegate void UpdateState();
    protected UpdateState CurrentUpdateState;
    public abstract void Reset();
    public abstract bool IsActive { get; set; }
    protected bool active;
    protected bool isLive;
}
