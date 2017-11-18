using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour {

	public float rotationMultiplier;

	private Rigidbody rb;

	void Start() {
		rb = GetComponent<Rigidbody>();
	}
	void FixedUpdate() {
		float forwardVelocity = rb.velocity.z;
		transform.Rotate(Vector3.right * Time.deltaTime * rotationMultiplier * forwardVelocity);
	}
}
