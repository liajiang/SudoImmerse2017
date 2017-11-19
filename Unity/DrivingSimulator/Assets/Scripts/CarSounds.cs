using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour {
    public CarDriver CarDriver;
    private AudioSource _audio;

	// Use this for initialization
	void Start () {
        _audio = GetComponent<AudioSource>();
        _audio.volume = 0.1f;
        CarDriver.AccelerationChanged += OnAccelerationChanged;
	}

    void OnAccelerationChanged(object sender, AccelerationChagnedEventArgs args)
    {
        float vol = args.Acceleration;
        if (vol < 0.1f)
        {
            vol = 0.1f;
        }
        _audio.volume = vol;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
