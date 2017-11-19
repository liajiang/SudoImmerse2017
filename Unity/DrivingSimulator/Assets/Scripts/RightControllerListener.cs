using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class RightControllerListener : MonoBehaviour {
    public CarDriver CarDriver;

    private VRTK_ControllerEvents _controllerEvents;
	// Use this for initialization
	void Start () {
        _controllerEvents = GetComponent<VRTK_ControllerEvents>();
        _controllerEvents.TriggerAxisChanged += CarDriver.Brake;
        _controllerEvents.TriggerTouchStart += CarDriver.BrakeStart;
        _controllerEvents.TriggerTouchEnd += CarDriver.BrakeStop;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
