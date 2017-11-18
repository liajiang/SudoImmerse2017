using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintTransform : MonoBehaviour {
    public Transform Reference;
    public Vector3 RotationAxis;
    public Vector3 ForwardAxis;
	// Update is called once per frame
	void Update () {
        Vector3 temp = Vector3.Cross(Reference.TransformDirection(ForwardAxis), transform.TransformDirection(ForwardAxis));
        float angleSign = Vector3.Dot(temp, transform.TransformDirection(RotationAxis));
        float angle = Quaternion.Angle(Reference.localRotation, transform.localRotation);
        if (angleSign < 0)
        {
            angle = -angle;
        }
        Debug.Log(angle);
        //Debug.Log(angle);
	}
}
