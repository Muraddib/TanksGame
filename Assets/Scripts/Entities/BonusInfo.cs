using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[Serializable]
public class BonusInfo {
    public BonusIDs BonusID;
    public GameObject BonusPrefab;
    public AudioIDs AudioID;
    public float HealthBonus;
    public float ArmorBonus;
    public WeaponIDs WeaponUnlockID;
}
