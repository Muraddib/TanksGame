using UnityEngine;
using System.Collections;

public class TankTurretController : MonoBehaviour
{
    public Camera TargetCamera;
    public Transform SphereTest;
    public Transform CannonBase;
    private Transform thisTransform;
    private Vector3 lastHitPos;
    public float RotationSpeed;
    private const int layerMask = 1;
    public float AngleHeight;
    public bool LockTurret;

    private void Start()
    {
        thisTransform = gameObject.transform;
        TargetCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LockTurret = !LockTurret;
        }

        if (!LockTurret)
        {
            var ray = TargetCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000f, layerMask))
            {
                //SphereTest.position = hit.point;
            }

            lastHitPos = hit.point;

            float sign = lastHitPos.y > thisTransform.position.y ? 1f : -1f;

            AngleHeight = Vector3.Angle(
                new Vector3(lastHitPos.x, thisTransform.position.y, lastHitPos.z) - thisTransform.position,
                (lastHitPos - thisTransform.position)) * sign;

            AngleHeight = Mathf.Clamp(AngleHeight, -20f, 20f);

            CannonBase.localRotation = Quaternion.RotateTowards(CannonBase.localRotation,
                                                                Quaternion.Euler(-AngleHeight, 0f, 0f),
                                                                RotationSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(
                (new Vector3(lastHitPos.x, 0f, lastHitPos.z) -
                 new Vector3(thisTransform.position.x, 0f, thisTransform.position.z)
                ));

            thisTransform.localRotation = Quaternion.RotateTowards(thisTransform.localRotation,
                                                                   targetRotation*
                                                                   Quaternion.Euler(0f,
                                                                                    360f -
                                                                                    thisTransform.parent
                                                                                                 .localEulerAngles.y,
                                                                                    0f), RotationSpeed);
        }
        else
        {
            CannonBase.localRotation = Quaternion.identity;
            thisTransform.localRotation = Quaternion.identity;
        }
    }
}
