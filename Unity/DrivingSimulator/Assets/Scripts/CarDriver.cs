using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CarDriver : MonoBehaviour
{
    public event EventHandler<DirectionChangedEventArgs> DirectionChanged;
    public event EventHandler<GearChangedEventArgs> GearChanged;
    public event EventHandler<AccelerationChagnedEventArgs> AccelerationChanged;
    
    public bool IsReverse { get { return _isReverse; } }
    public float WheelDirection { get { return _wheelDirection; } }
    public float NetAcceleration { get { return _netAcceleration; } }

    private float _wheelDirection;
    private bool _isReverse;
    private float _acceleration;
    private float _brake;
    private float _netAcceleration;
    private float _lastNetAcceleration;
    private bool _braking;
    private bool _accelerating;

    private void Awake()
    {
    }

    void Update ()
    {
        _lastNetAcceleration = _netAcceleration;
        if (!_braking)
        {
            _brake = 0;
        }

        if (!_accelerating)
        {
            _acceleration = 0f;
        }
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

    public void AccelerateStart(object sender, ControllerInteractionEventArgs args)
    {
        _accelerating = true;
    }

    public void AccelerateStop(object sender, ControllerInteractionEventArgs args)
    {
        _accelerating = false;
    }

    public void BrakeStart(object sender, ControllerInteractionEventArgs args)
    {
        _braking = true;
    }

    public void BrakeStop(object sender, ControllerInteractionEventArgs args)
    {
        _braking = false;
    }

    public void Accelerate(object sender, ControllerInteractionEventArgs args)
    {
        _acceleration = args.buttonPressure;
    }

    public void Brake(object sender, ControllerInteractionEventArgs args)
    {
        _brake = args.buttonPressure;
    }

    public void OnAccelerationChanged(object sender, AccelerationChagnedEventArgs args)
    {
        var handler = AccelerationChanged;
        if (handler != null)
        {
            handler(sender, args);
        }
    }

}
