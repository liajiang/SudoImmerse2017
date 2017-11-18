using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriver : MonoBehaviour {
    public event EventHandler<DirectionChangedEventArgs> DirectionChanged;

	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
