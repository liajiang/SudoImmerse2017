using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.RightArrow)) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 1);
		}
	}

	void OnCollisionEnter (Collision col) {
		if(col.gameObject.name == "parking_lot") {
			return;
		}
		Debug.Log ("collide");
	}
}
