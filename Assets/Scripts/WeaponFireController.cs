using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponFireController : MonoBehaviour
{
    public GameObject FireFXPrefab;
    public float spawnOffset = 1.0f;
    public Transform FirePointContainer;
    public Dictionary<WeaponIDs, Transform> FirePoints;

    private Transform thisTransform;

    private void Start()
    {
        thisTransform = this.transform;
        EventManager.GenericFireEvent += HandleFireEvent;
        GetFirePoints();
    }

    private void OnDestroy()
    {
        EventManager.GenericFireEvent -= HandleFireEvent;
    }

    private void GetFirePoints()
    {
        FirePoints = new Dictionary<WeaponIDs, Transform>();
        foreach (Transform point in FirePointContainer)
        {
            if (point != FirePointContainer)
            {
                FirePoints.Add(point.gameObject.GetComponent<FirePoint>().PointWeaponID, point);
            }
        }
    }

    private void HandleFireEvent(WeaponInfo weaponSource)
    {
        if (FireFXPrefab)
        {
            GameObject fireObject =
                Instantiate(FireFXPrefab, thisTransform.position, thisTransform.rotation) as GameObject;
            fireObject.transform.parent = thisTransform;
        }
        if (weaponSource.WeaponProjectile)
        {
            for (int i = 0; i < weaponSource.ProjectileCount; i++)
            {
                GameObject bulletObject =
                    Instantiate(weaponSource.WeaponProjectile,
                                FirePoints[weaponSource.WeaponID].position +
                                FirePoints[weaponSource.WeaponID].forward*spawnOffset,
                                FirePoints[weaponSource.WeaponID].rotation) as
                    GameObject;

                float rndRangeX = Random.Range((1f - weaponSource.Accuracy)*-1f, 1f - weaponSource.Accuracy);
                float rndRangeY = Random.Range((1f - weaponSource.Accuracy)*-1f, 1f - weaponSource.Accuracy);
                float rndRangeZ = Random.Range((1f - weaponSource.Accuracy)*-1f, 1f - weaponSource.Accuracy);

                Vector3 rndWeaponAccuracy = new Vector3(rndRangeX*10f, rndRangeY*10f, rndRangeZ*10f);
                bulletObject.GetComponent<Rigidbody>().velocity = (Quaternion.Euler(rndWeaponAccuracy)*
                                                                   FirePoints[weaponSource.WeaponID].forward)*
                                                                  weaponSource.ProjectileSpeed;
                bulletObject.GetComponent<WeaponProjectile>().WeaponSource = weaponSource;
                bulletObject.GetComponent<WeaponProjectile>().ProjectileOwner = WeaponProjectile.Owner.Player;
            }
        }
        EventManager.CallSoundEvent(weaponSource.AudioID, thisTransform.position);
    }
}
