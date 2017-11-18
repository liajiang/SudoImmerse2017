using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigiController : MonoBehaviour {

	public float speed = 5;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter (Collision col)
	{
		Debug.Log ("CCCCCollision!");
	}

	void FixedUpdate ()
	{
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.AddForce (movement * 15);	
	}
}
