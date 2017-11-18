using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour {
    public event EventHandler<DirectionChangedEventArgs> DirectionChanged;
    public event EventHandler<GearChangedEventArgs> GearChanged;
    
    public bool IsReverse { get { return _isReverse; } }
    public float WheelDirection { get { return _wheelDirection; } }
    public float Acceleration { get { return _acceleration; } }

    private float _wheelDirection;
    private bool _isReverse;
    private float _acceleration;

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
            handler(sender, args);
        }
    }

    public void OnGearChanged(object sender, GearChangedEventArgs args)
    {
        _isReverse = args.IsReverse;
        var handler = GearChanged;
        if (handler != null)
        {
            handler(sender, args);
        }
    }

}
