using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationChagnedEventArgs : EventArgs {
    public float Acceleration { get; set; }

    public AccelerationChagnedEventArgs(float acceleration)
    {
        Acceleration = acceleration;
    }
}
