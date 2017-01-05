using UnityEngine;
using System.Collections;

//Ahmed Mahmoud 01/06/2016

/*This script has been created in order to allow the main camera
 * inside a scene to follow a gameObject (in this case the car).
 * The camera does this by interpolating from the current height/position
 * to the desired camera position (by using a vector). The camera will also
 * flip as the script detects backwards movement.*/


public class CameraControl : MonoBehaviour {

	//creating the gameobject var car (that will hold the car)
	public GameObject car;

	//the distance between the camera and the car (horizontal offset)
	public float distance = 6.0f; 

	//the distance between the camera and the car (vertical offset)
	public float height = 1.3f;


	public float rotationDamping = 3.0f; //determines the speed of the rotation from going from forwards to backwards or vice versa
	public float heightDamping = 2.0f;
	public float zoomRatio = 0.3f; 
	public float DefaultFOV = 60.0f;

	private Vector3 rotationVector;
	private	Rigidbody rb; 
	private Camera cam;
	private float accel;

	void Start () {
		//rigidbody is set to the car's
		rb = car.GetComponent<Rigidbody>();
		//camera is set to the main camera
		cam = GetComponent<Camera>(); 
	}

	void FixedUpdate () {
		//angle/height that the camera wants
		float wantedAngle = rotationVector.y;
		float wantedHeight = car.transform.position.y + height;

		float myAngle = transform.eulerAngles.y; //current angle
		float myHeight = transform.position.y; //current position (differs by the y axis)

		//the method Lerp interpolates between the current angle/height to the desired ones.
		myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime); 
		myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping* Time.deltaTime); 
		Quaternion currentRotation = Quaternion.Euler(0,myAngle,0);

		transform.position = car.transform.position; //moves the camera with the car
		transform.position -= currentRotation* Vector3.forward * distance; //horizontal offset
		transform.position = new Vector3 (transform.position.x, myHeight, transform.position.z); //vertical offset of the camera
		transform.LookAt(car.transform); //causes the camera to look at the current position by rotating transforming the camera on to the car

		Vector3 localVelocity = car.transform.InverseTransformDirection(rb.velocity); //velocity relative to the car's current movement

		if (localVelocity.z < -1.5f) { //if car is moving backwards relative to the local velocity
			rotationVector.y = car.transform.eulerAngles.y + 180; //rotate the camera 180 + the transform angle
		} else { 
			rotationVector.y = car.transform.eulerAngles.y; //rotate the camera according to the the transform angle
		} 

		accel = rb.velocity.magnitude; 
		cam.fieldOfView = DefaultFOV + accel * zoomRatio; //sets the FOV
	} 
}﻿