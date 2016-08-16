using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[Serializable]
public class WeaponInfo
{
    //public GunControl WeaponPrefab;
    public WeaponIDs WeaponID;
    public AudioIDs AudioID;
    public GameObject WeaponProjectile;
    public float ProjectileCount;
    public float DamageAmount;
    public float ProjectileSpeed;
    public float ReloadTime;
    public float Accuracy;
    public bool ExplodingProjectile;
    public float ExplosionRadius;
    public AudioIDs ExplosionAudioID;
    public float RecoilForce;
    public string LocalizedName;
}
