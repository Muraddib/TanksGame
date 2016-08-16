using UnityEngine;
using System.Collections;

using UnityStandardAssets.CrossPlatformInput;

// This script must be attached to "Cannon_Base".

public class Fire_Control_CS : MonoBehaviour
{
    private bool isReady = true;
    private Transform thisTransform;
    private Rigidbody parentRigidbody;

    private void Start()
    {
        thisTransform = gameObject.transform;
        parentRigidbody = GetComponentInParent<Rigidbody>();
        if (parentRigidbody == null)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)) && isReady)
        {
            HandleFire(GameplayController.Instance.Player.CurrentWeapon);
        }
    }

    private void HandleFire(WeaponInfo currentWeaponInfo)
    {
        EventManager.CallFireEvent(currentWeaponInfo);
        parentRigidbody.AddForceAtPosition(-thisTransform.forward*currentWeaponInfo.RecoilForce, thisTransform.position, ForceMode.Impulse);
        isReady = false;
        StartCoroutine("Reload");
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(GameplayController.Instance.Player.CurrentWeapon.ReloadTime);
        isReady = true;
    }
}
