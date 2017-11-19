using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPosition : MonoBehaviour {

    private Vector3 localPosition;

    public GameObject HeadPosition;
	
	void FixedUpdate () {
        transform.position = HeadPosition.transform.position;
	}
}
