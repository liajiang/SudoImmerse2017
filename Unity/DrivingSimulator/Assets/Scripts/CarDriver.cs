using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CarDriver : MonoBehaviour
{
    public VRTK_ControllerEvents LeftController;
    public VRTK_ControllerEvents RightController;

    public event EventHandler<DirectionChangedEventArgs> DirectionChanged;
    public event EventHandler<GearChangedEventArgs> GearChanged;
    public event EventHandler<AccelerationChagnedEventArgs> AccelerationChanged;
    
    public bool IsReverse { get { return _isReverse; } }
    public float WheelDirection { get { return _wheelDirection; } }
    public float Acceleration { get { return _acceleration; } }

    private float _wheelDirection;
    private bool _isReverse;
    private float _acceleration;
    private float _brake;
    private float _netAcceleration;
    private float _lastNetAcceleration;

    private void Awake()
    {
        LeftController.TriggerPressed += Accelerate;
        RightController.TriggerPressed += Brake;
    }

    void Update ()
    {
        _lastNetAcceleration = _netAcceleration;
        _netAcceleration = _acceleration - _brake;
        if (_lastNetAcceleration != _netAcceleration)
        {
            var args = new AccelerationChagnedEventArgs(_netAcceleration);
            OnAccelerationChanged(this, args);
        }
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

    private void Accelerate(object sender, ControllerInteractionEventArgs args)
    {
        _acceleration = VRTK_ControllerReference.GetRealIndex(args.controllerReference);
    }

    private void Brake(object sender, ControllerInteractionEventArgs args)
    {
        _brake = VRTK_ControllerReference.GetRealIndex(args.controllerReference);
    }

    public void OnAccelerationChanged(object sender, AccelerationChagnedEventArgs args)
    {
        _acceleration = args.Acceleration;
        var handler = AccelerationChanged;
        if (handler != null)
        {
            handler(sender, args);
        }
    }

}
