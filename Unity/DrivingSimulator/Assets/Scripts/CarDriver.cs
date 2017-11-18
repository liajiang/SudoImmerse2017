using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour {
    public event EventHandler<DirectionChangedEventArgs> DirectionChanged;

    public float WheelDirection { get { return _wheelDirection; } }

    private float _wheelDirection;

    private void Awake()
    {
    }

    void Update () {
    }

    public void OnDirectionChanged(object sender, DirectionChangedEventArgs args)
    {
        _wheelDirection = args.Direction;
        EventHandler<DirectionChangedEventArgs> handler = DirectionChanged;
        if (handler != null)
        {
            handler(this, args);
        }
    }
}
