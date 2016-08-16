using System;
using UnityEngine;
using UnityEngine.Serialization;
    [Serializable]
    public class GameSettings
    {
        [Header("Player")] 
        public GameObject PlayerPrefab;

        [Header("Camera")]
        public GameObject CameraPrefab;

        [Header("Enemies")]
    
        public EnemyInfo[] Enemies;

        [Header("Spawner")]
        public int MaxEnemyCount;
        public float SpawnPointRadius;
        public float SpawnDelay;
        
        [Header("Player Weapons")]
        public WeaponInfo[] Weapons;

        [Header("Enemy Weapons")]
        public WeaponInfo[] EnemyWeapons;

        [Header("Bonuses")]
        public BonusInfo[] Bonuses;

        [Header("ObjectPool")] 
        public int MaxCountPerEnemyType;
        public int MaxCountPerBonusType;
    }
