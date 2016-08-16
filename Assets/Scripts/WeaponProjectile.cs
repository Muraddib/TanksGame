using UnityEngine;
using System.Collections;

public class WeaponProjectile : MonoBehaviour
{
    [HideInInspector]
    public enum Owner
    {
        Enemy,
        Player
    }
    public Owner ProjectileOwner;
    public float lifeTime = 5.0f;
    public GameObject brokenObject;
    [HideInInspector]
    public WeaponInfo WeaponSource;
    private Transform thisTransform;
    private Rigidbody thisRigidbody;
    public Vector3 MoveDirection;
    private Vector3 rndWeaponAccuracy;

    private void Awake()
    {
        thisTransform = transform;
        thisRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (ProjectileOwner)
        {
            case Owner.Player:
                if (WeaponSource.ExplodingProjectile)
                {
                    Vector3 explosionPos = thisTransform.position;
                    Collider[] colliders = Physics.OverlapSphere(explosionPos, WeaponSource.ExplosionRadius);
                    foreach (Collider hit in colliders)
                    {
                        Rigidbody rb = hit.GetComponent<Rigidbody>();
                        var enemy = hit.gameObject.GetComponent<Enemy>();
                        if (enemy)
                        {
                            float distance = Vector3.Distance(explosionPos, enemy.gameObject.transform.position);
                            enemy.TakeDamage(Mathf.Abs(1f - distance / WeaponSource.ExplosionRadius) * WeaponSource.DamageAmount, WeaponSource.WeaponID);
                        }

                        if (rb != null)
                            rb.AddExplosionForce(2000f, explosionPos, WeaponSource.ExplosionRadius, 3.0F);
                    }
                    EventManager.CallSoundEvent(WeaponSource.ExplosionAudioID, thisTransform.position);
                }
                else
                {
                    var enemy = collision.collider.gameObject.GetComponent<Enemy>();
                    if (enemy)
                    {
                        Debug.Log(enemy.gameObject.name);
                        enemy.TakeDamage(WeaponSource.DamageAmount, WeaponSource.WeaponID);
                    }
                }
                break;
            case Owner.Enemy:
                var player = collision.collider.gameObject.GetComponent<PlayerController>();
                if (player)
                {
                    Debug.Log(player.gameObject.name);
                    player.TakeDamage(WeaponSource.DamageAmount, WeaponSource.WeaponID);
                }
                break;
        }
        Hit();
    }

    private void Hit()
    {
        if (brokenObject)
        {
            Instantiate(brokenObject, thisTransform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
