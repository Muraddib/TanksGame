using UnityEngine;
using System.Collections;

// This script must be attached to all the Driving Wheels.

public class Wheel_Rotate_CS : MonoBehaviour
{
    private bool isLeft;
    private Rigidbody thisRigidbody;
    private float maxAngVelocity;
    private Wheel_Control_CS controlScript;

    private Transform thisTransform;
    private Vector3 initialAng;

    private void Start()
    {
        //this.gameObject.layer = 9 ; // Layer9 >> for wheels.
        thisRigidbody = GetComponent<Rigidbody>();
        // Set direction.
        if (transform.localPosition.y > 0.0f)
        {
            isLeft = true;
        }
        else
        {
            isLeft = false;
        }
        // Get initial rotation.
        thisTransform = transform;
        initialAng = thisTransform.localEulerAngles;
    }

    private void Get_Wheel_Control(Wheel_Control_CS tempScript)
    {
        controlScript = tempScript;
        float radius = GetComponent<SphereCollider>().radius;
        maxAngVelocity = Mathf.Deg2Rad*((controlScript.maxSpeed/(2.0f*Mathf.PI*radius))*360.0f);
    }

    private void FixedUpdate()
    {
        if(!controlScript) return;

        float rate;
        if (isLeft)
        {
            rate = controlScript.leftRate;
        }
        else
        {
            rate = controlScript.rightRate;
        }
        thisRigidbody.AddRelativeTorque(0.0f, Mathf.Sign(rate)*controlScript.wheelTorque, 0.0f);
        thisRigidbody.maxAngularVelocity = Mathf.Abs(maxAngVelocity*rate);
    }

    private void Update()
    {
        // Stabilize wheels.
        float angY = thisTransform.localEulerAngles.y;
        thisTransform.localEulerAngles = new Vector3(initialAng.x, angY, initialAng.z);
    }
}
