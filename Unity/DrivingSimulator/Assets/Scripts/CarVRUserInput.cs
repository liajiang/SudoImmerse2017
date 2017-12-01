using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarVRUserInput : MonoBehaviour {
    public CarDriver CarDriver;
    private CarController _car; // the car controller we want to use

    private void Awake()
    {
        // get the car controller
        _car = GetComponent<CarController>();
    }


    private void FixedUpdate()
    {
        // pass the input to the car!
        float steering = CarDriver.WheelDirection;
        float acceleration = CarDriver.NetAcceleration;
        Debug.Log(acceleration);
        if (CarDriver.IsReverse) {
            acceleration = -acceleration;
        }
        _car.MoveWithDirection(steering, acceleration, CarDriver.IsReverse);
    }
}
