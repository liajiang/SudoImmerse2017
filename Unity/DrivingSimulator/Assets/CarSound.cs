using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour {

	public CarController CarDriver;

	private AudioSource audio;

	void Start()
	{
		audio = GetComponent<AudioSource>();
//		CarDriver.AccelerationChanged += OnAccelerationChanged;
	}
		
	void OnAccelerationChanged(object sender, AccelerationChagnedEventArgs args)
	{
		float vol = args.Acceleration;
		if (vol < 0.1f) {
			vol = 0.1f;
		}
		audio.volume = vol;
	}
}
