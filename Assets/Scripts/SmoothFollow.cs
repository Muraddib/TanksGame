using UnityEngine;
using System.Collections;
using System;
public class SmoothFollow : MonoBehaviour
{
    public Transform FollowTarget;
    public Vector3 TopViewRotation = new Vector3(45f, 270f, 0f);
    private Transform thisTransform;
    public bool ThirdPerson;

    public CameraParameters[] CameraParams;

    [Serializable]
    public struct CameraParameters
    {
        public int Distance;
        public float Height;
        public float Damping;
        public float Offset;
        public float RotationDamping;
        public bool SmoothRotation;
        public bool FollowBehind;
    };

    void Start()
    {
        thisTransform = gameObject.transform;
    }

    void FixedUpdate()
    {
        Vector3 wantedPosition;

        if (Input.GetKeyDown(KeyCode.C))
        {
            ThirdPerson = !ThirdPerson;
        }

        if (ThirdPerson)
        {

            if (CameraParams[0].FollowBehind)
                wantedPosition = FollowTarget.TransformPoint(0, CameraParams[0].Height, -CameraParams[0].Distance);
            else
                wantedPosition = FollowTarget.TransformPoint(0, CameraParams[0].Height, CameraParams[0].Distance);

            transform.position = Vector3.Lerp(thisTransform.position, wantedPosition, Time.deltaTime * CameraParams[0].Damping);

            if (CameraParams[0].SmoothRotation)
            {
                Quaternion wantedRotation = Quaternion.LookRotation(new Vector3(FollowTarget.position.x, FollowTarget.position.y, FollowTarget.position.z) - thisTransform.position);
                thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, wantedRotation, Time.deltaTime * CameraParams[0].RotationDamping);
            }
            else thisTransform.LookAt(FollowTarget, FollowTarget.up);
        }
        else
        {
            wantedPosition = new Vector3(FollowTarget.position.x, FollowTarget.position.y, FollowTarget.position.z) +
                         new Vector3(CameraParams[1].Offset, CameraParams[1].Height, -CameraParams[1].Distance);

            thisTransform.rotation = Quaternion.Lerp(thisTransform.rotation, Quaternion.Euler(TopViewRotation), Time.deltaTime * CameraParams[0].RotationDamping);

            thisTransform.position = Vector3.Lerp(thisTransform.position, wantedPosition, Time.deltaTime * CameraParams[0].Damping);
        }
        
    }
}