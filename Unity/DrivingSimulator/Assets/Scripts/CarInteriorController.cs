﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteriorController : MonoBehaviour {
	public Transform Target;
	// Use this for initialization
	void Start () {		
	}
	void FixedUpdate () {
		transform.position = Target.position;
		transform.rotation = Target.rotation;
	}
}
