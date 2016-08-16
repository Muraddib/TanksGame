using UnityEngine;

public class BonusBillboard : MonoBehaviour
{
    private Transform sceneCamera;
    private Transform thisTransform;
    public Transform FollowPos;
    public Vector3 FollowOffset;
    public bool LookAtCamera;

    void Start()
    {
        sceneCamera = Camera.main.transform;
        thisTransform = gameObject.transform;
    }

    void Update()
    {
        thisTransform.rotation = Quaternion.LookRotation(LookAtCamera ? sceneCamera.position - thisTransform.position : Vector3.up, Vector3.up);
        thisTransform.position = FollowPos.position + FollowOffset;

    }
}