using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearStickManager : MonoBehaviour
{
    public CarDriver CarDriver;
    public Transform ReversePoint;
    public Transform ForwardPoint;

    private bool _isReverse;
    private bool _lastState;

	// Update is called once per frame
	void Update ()
	{
	    _lastState = _isReverse;
	    if (!_isReverse)
	    {
	        if (Vector3.Distance(transform.position, ReversePoint.position) < 1E-3)
	        {
	            _isReverse = true;
	        }
	    }
	    else
	    {
	        if (Vector3.Distance(transform.position, ForwardPoint.position) < 1E-3)
	        {
	            _isReverse = false;
	        }
        }
	    if (_lastState != _isReverse)
	    {
	        GearChangedEventArgs args = new GearChangedEventArgs(_isReverse);
	        CarDriver.OnGearChanged(this, args);
	    }
	}
}
