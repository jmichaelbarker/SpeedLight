using UnityEngine;
using System.Collections;

public class CarControl : MonoBehaviour {
	//creating a collider for each wheel using the specially premade var type "wheelcollider".
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;

	//public values that allow for change in the Unity developer
	public float motorForce;
	public float steerForce;
	public float brakeForce;


	//called in a fixed time interval instead of a per frame basis
	void Update () {
		float v = Input.GetAxis ("Vertical") * motorForce;
		float h = Input.GetAxis ("Horizontal") * steerForce;

		//simulates real word torque and forces rotation (rear wheel drive).
		wheelRL.motorTorque = v;
		wheelRR.motorTorque = v;

		//this forces the car to steer based on the horizontal input.
		wheelFL.steerAngle = h;
		wheelFR.steerAngle = h;
	}
}
