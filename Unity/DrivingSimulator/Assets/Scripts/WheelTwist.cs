using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTwist : MonoBehaviour {

	public CarDriver CarDriver;

	void Start () {
		CarDriver.DirectionChanged += OnDirectionChanged;
	}

	void OnDirectionChanged(object sender, DirectionChangedEventArgs e) {
		float twist = e.Direction * 45;
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, twist, transform.localEulerAngles.z);
	}
}
