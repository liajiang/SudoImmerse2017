using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    public CarDriver CarDriver;
	public Vector3 dragMultiplier = new Vector3(200f, 500f, 100f);
	public int numberOfGears = 2;


	public float topSpeed = 160;
	public int maximumTurn = 15;
	public int minimumTurn = 0;
	public float throttle = 0; 
	public float steer = 0;


	float currentEnginePower = 0.0f;
	float engineForceValues;

	bool  canSteer;
	bool  canDrive;

	bool backGear;

	private Rigidbody body;

	void ResetTensor() {
		GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
	}

	// Use this for initialization
	void Start () {
		//TODO: register event
		body = GetComponent<Rigidbody> ();
		ResetTensor();

		backGear = false;

		//wheels = new Wheel[frontWheels.Length + rearWheels.Length];
		canDrive = true;
		canSteer = true;

		//SetupGears ();
	}

	// Update is called once per frame
	void Update () {
		GetInput();

		Vector3 relativeVelocity = transform.InverseTransformDirection(body.velocity);
	}

	void  FixedUpdate (){	
		// The rigidbody velocity is always given in world space, but in order to work in local space of the car model we need to transform it first.
		Vector3 relativeVelocity = transform.InverseTransformDirection(body.velocity);

		if (relativeVelocity.y != 0) {
			relativeVelocity.y = 0;
		}

		Debug.Log ("relativeVelocity = " + relativeVelocity);

		//UpdateDrag(relativeVelocity);

		CalculateEnginePower(relativeVelocity);

		ApplyThrottle(canDrive, relativeVelocity);

		ApplySteering(canSteer, relativeVelocity);

	}


	/***********************/
	/* Called from Update  */
	/***********************/

	void  GetInput (){
        //throttle will be get from the "accelerator" and "gear" (-1 to 1)
        //steer get from the steering wheel
        bool keyboardControl = false;
        if (keyboardControl)
        {
            throttle = Input.GetAxis("Vertical");
            steer = Input.GetAxis("Horizontal");
            backGear = Input.GetKey("space");
        }
        else
        {
            throttle = CarDriver.NetAcceleration;
            steer = CarDriver.WheelDirection;
            backGear = CarDriver.IsReverse;
        }
    }

	//deleted: since only have one level gear (one forwards, one backward)
	//	void  SetupGears (){
	//		engineForceValues = new float[numberOfGears];
	//		gearSpeeds = new float[numberOfGears];
	//
	//		float tempTopSpeed = topSpeed;
	//
	//		for(int i= 0; i < numberOfGears; i++)
	//		{
	//			if(i > 0)
	//				gearSpeeds[i] = tempTopSpeed / 4 + gearSpeeds[i-1];
	//			else
	//				gearSpeeds[i] = tempTopSpeed / 4;
	//
	//			tempTopSpeed -= tempTopSpeed / 4;
	//		}
	//
	//		float engineFactor = topSpeed / gearSpeeds[gearSpeeds.Length - 1];
	//
	//		for(int i = 0; i < numberOfGears; i++)
	//		{
	//			float maxLinearDrag = gearSpeeds[i] * gearSpeeds[i];// * dragMultiplier.z;
	//			engineForceValues[i] = maxLinearDrag * engineFactor;
	//		}
	//	}

	/**************************************************/
	/* Functions called from FixedUpdate()            */
	/**************************************************/

	void  UpdateDrag ( Vector3 relativeVelocity  ){
		Vector3 relativeDrag = new Vector3(	-relativeVelocity.x * Mathf.Abs(relativeVelocity.x), 
			-relativeVelocity.y * Mathf.Abs(relativeVelocity.y), 
			-relativeVelocity.z * Mathf.Abs(relativeVelocity.z) );

		Vector3 drag= Vector3.Scale(dragMultiplier, relativeDrag);

		//		if (initialDragMultiplierX > dragMultiplier.x) // Handbrake code
		//		{
		//			if (relativeVelocity.magnitude > 0) {
		//				drag.x /= (relativeVelocity.magnitude / (topSpeed / ( 1 + 2 * handbrakeXDragFactor ) ) );
		//			}
		//			drag.z *= (1 + Mathf.Abs(Vector3.Dot(body.velocity.normalized, transform.forward)));
		//			drag += body.velocity * Mathf.Clamp01(body.velocity.magnitude / topSpeed);
		//		}
		//		else // No handbrake
		//		{
		//			if (relativeVelocity.magnitude > 0) {
		//				drag.x *= topSpeed / relativeVelocity.magnitude;
		//			}
		//		}
		//
		//		if(Mathf.Abs(relativeVelocity.x) < 5 && !handbrake)
		//			drag.x = -relativeVelocity.x * dragMultiplier.x;

		Debug.Log ("drag:" + drag);
		Debug.Log ("word drag:" + transform.TransformDirection (drag));
		body.AddForce(transform.TransformDirection(drag) * body.mass * Time.deltaTime);
	}

	void  CalculateEnginePower (Vector3 relativeVelocity){
		Debug.Log ("before: throttle:" + throttle + ", currentEnginePower:" + currentEnginePower);

		if(throttle == 0)
		{
			currentEnginePower -= Time.deltaTime * 1000;
		}
		else if(Mathf.Sign(relativeVelocity.z) == Mathf.Sign(throttle))
		{
			float normPower = currentEnginePower/topSpeed * 2;
			if(normPower < 1)
				normPower = 10 - normPower * 9;
			else
				normPower = 1.9f - normPower * 0.9f;
			currentEnginePower += Time.deltaTime * 5000 * throttle;
		}
		else
		{
			currentEnginePower += Time.deltaTime * 5000 * throttle;
		}

		//if(backGear == true)
		//	currentEnginePower = - Mathf.Clamp(currentEnginePower, 0, topSpeed);
		//else
        currentEnginePower = Mathf.Clamp(currentEnginePower, 0, topSpeed);

		if (Mathf.Abs(currentEnginePower) < 1) {
			currentEnginePower = 0;
		}

		Debug.Log ("after: throttle:" + throttle + ", currentEnginePower:" + currentEnginePower);
	}

	void  ApplyThrottle ( bool canDrive , Vector3 relativeVelocity){
		if(canDrive)
		{
			float throttleForce = 0;
			float brakeForce = 0;

            if (throttle < 0)
            {
				brakeForce = -body.mass;
            }
            else
            {
				throttleForce = currentEnginePower * body.mass;
			}

			Debug.Log("throttleForce:" + throttleForce);
			Debug.Log ("brakeForce:" + brakeForce);
            Vector3 force = transform.forward * Time.deltaTime * (throttleForce + brakeForce);
            if (backGear)
            {
                force = -force;
            }
			body.AddForce(force);
		}
	}

	//TODO: set the rotating axis between the back wheels
	void  ApplySteering ( bool canSteer ,  Vector3 relativeVelocity  ){
		if(canSteer)
		{
			float turnRadius = 3.0f / Mathf.Sin((90 - (steer * 30)) * Mathf.Deg2Rad);
			float minMaxTurn = EvaluateSpeedToTurn(body.velocity.magnitude);
			float turnSpeed = Mathf.Clamp(relativeVelocity.z / turnRadius, -minMaxTurn / 10, minMaxTurn / 10);

			transform.RotateAround(	transform.position + transform.right * turnRadius * steer, 
				transform.up, 
				turnSpeed * Mathf.Rad2Deg * Time.deltaTime * steer);

			Vector3 debugStartPoint= transform.position + transform.right * turnRadius * steer;
			Vector3 debugEndPoint= debugStartPoint + Vector3.up * 5;

			float rotationDirection = Mathf.Sign(steer); // rotationDirection is -1 or 1 by default, depending on steering
			if(steer == 0)
			{
				//if(body.angularVelocity.y < 1) // If we are not steering and we are handbraking and not rotating fast, we apply a random rotationDirection
				//	rotationDirection = Random.Range(-1.0f, 1.0f);
				//else
				rotationDirection = body.angularVelocity.y; // If we are rotating fast we are applying that rotation to the car
			}
			// -- Finally we apply this rotation around a point between the cars front wheels.
			//transform.RotateAround( transform.TransformPoint( (	frontWheels[0].localPosition + frontWheels[1].localPosition) * 0.5f), 
			//	transform.up, 
			//body.velocity.magnitude * Mathf.Clamp01(1 - body.velocity.magnitude / topSpeed) * rotationDirection * Time.deltaTime * 2);
			transform.RotateAround(transform.TransformPoint(body.position), 
				transform.up, 
				body.velocity.magnitude * Mathf.Clamp01(1 - body.velocity.magnitude / topSpeed) * rotationDirection * Time.deltaTime * 2);
		}
	}


	/**************************************************/
	/*               Utility Functions                */
	/**************************************************/

	float  EvaluateSpeedToTurn ( float speed  ){
		if(speed > topSpeed / 2)
			return minimumTurn;

		float speedIndex = 1 - (speed / (topSpeed / 2));
		return minimumTurn + speedIndex * (maximumTurn - minimumTurn);
	}
}

