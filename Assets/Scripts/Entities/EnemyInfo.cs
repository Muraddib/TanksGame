using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[Serializable]
public class EnemyInfo
{
    public EnemyIDs EnemyID;
    public float MaxHealth;
    public float Damage;
    public float AttackRate;
    public float AttackDistance;
    public bool RangedAttack;
    public float MovementSpeed;
    public GameObject RangedAttackProjectile;
    public float SpawnProbability;
    public GameObject EnemyPrefab;
    public int Score;
}
