using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SteeringWheelManager : MonoBehaviour {
    public CarDriver CarDriver;
    public float Angle { get { return _angle; } }
    public Transform Reference;
    public Vector3 RotationAxis;
    public Vector3 ForwardAxis;

    private float _angle;
    private float _lastAngle;
    private float _angleMax = 180f;
    // Update is called once per frame
    void Update()
    {
        _lastAngle = _angle;
        Vector3 temp = Vector3.Cross(Reference.TransformDirection(ForwardAxis), transform.TransformDirection(ForwardAxis));
        float angleSign = Vector3.Dot(temp, transform.TransformDirection(RotationAxis));
        float angle = Quaternion.Angle(Reference.localRotation, transform.localRotation);
        if (angleSign < 0)
        {
            angle = -angle;
        }
        _angle = angle;
        if (Mathf.Abs(_angle - _lastAngle) > 1E-3)
        {
            DirectionChangedEventArgs args = new DirectionChangedEventArgs(angle / _angleMax);
            CarDriver.OnDirectionChanged(this, args);
        }
    }
}
