using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour {

	public CarDriver CarDriver;

	private AudioSource audio;

	void Start()
	{
		audio = GetComponent<AudioSource>();
		CarDriver.AccelerationChanged += OnAccelerationChanged;
	}
		
	void OnAccelerationChanged(object sender, AccelerationChagnedEventArgs args)
	{
		float vol = args.Acceleration;
		if (vol < 0.1) {
			vol = 0.1;
		}
		audio.volume = vol;
	}
}
