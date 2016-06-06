using UnityEngine;
using System.Collections;

using UnityEngine; 
using System.Collections;

public class CameraControl : MonoBehaviour {

	//creating the gameobject var car (to focus on)
	public GameObject car;

	//the distance between the camera and the car (horizontal offset)
	public float distance = 6.0f; 

	//the distance between the camera and the car (vertical offset)
	public float height = 1.3f;

	//determines the speed of the rotation from going from forwards to backwards or vice versa
	public float rotationDamping = 3.0f;
	public float heightDamping = 2.0f;

	public float zoomRatio = 0.3f; 
	public float DefaultFOV = 60.0f; 
	private Vector3 rotationVector; 
	Rigidbody rb; 
	Camera camera; 

	void Start () {
		//rigidbody is set to the car
		rb = car.GetComponent<Rigidbody>();
		//camera is set to the main camera
		camera = GetComponent<Camera>(); 
	} 

	void LateUpdate () {
		//angle/height that the camera wants
		float wantedAngle = rotationVector.y;
		float wantedHeight = car.transform.position.y + height;

		float myAngle = transform.eulerAngles.y; 
		float myHeight = transform.position.y;

		myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime); 
		myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping* Time.deltaTime); 
		Quaternion currentRotation = Quaternion.Euler(0,myAngle,0);

		transform.position = car.transform.position; 
		transform.position -= currentRotation* Vector3.forward * distance; 
		transform.position = new Vector3 (transform.position.x, myHeight, transform.position.z); transform.LookAt(car.transform); } 

	void FixedUpdate (){ 
		Vector3 localVelocity = car.transform.InverseTransformDirection(rb.velocity); 
		if (localVelocity.z<-1.5f){ 
			rotationVector.y = car.transform.eulerAngles.y + 180; 
		} else { 
			rotationVector.y = car.transform.eulerAngles.y; 
		} 

		float acc = rb.velocity.magnitude; 
		camera.fieldOfView = DefaultFOV + acc * zoomRatio; 
	} 
}﻿