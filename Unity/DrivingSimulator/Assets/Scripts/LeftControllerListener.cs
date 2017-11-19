using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LeftControllerListener : MonoBehaviour {
    public CarDriver CarDriver;

    private VRTK_ControllerEvents _controllerEvents;
	// Use this for initialization
	void Start () {
        _controllerEvents = GetComponent<VRTK_ControllerEvents>();
        _controllerEvents.TriggerAxisChanged += CarDriver.Accelerate;
        _controllerEvents.TriggerTouchStart += CarDriver.AccelerateStart;
        _controllerEvents.TriggerTouchEnd += CarDriver.AccelerateStop;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
