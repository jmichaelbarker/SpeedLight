/*
 * Michael Barker
 * 14/06/16 -> 17/06/16
 * Script to provide AI to any car entity within SpeedLight
 */

using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent (typeof(CarController))]
public class CustomAIControl : MonoBehaviour {
	/*
	 * This script acts as an AI input for the CarControl script (essentially sending input the way a user would).
	 * In this sense, the car is "actually" driving, following all physics setup in CarControl with no tricks/hacks
	 * to behave in the intended way.
	 */

	public enum BrakeCondition
	{
		/*
		 * Enumeration of different brakeConditions to determine when the AI should brake, if ever.
		 */

		NeverBrake,					//AI will never break, but continue driving in the direction it was moving after colliding with the target.
		TargetDirectionDifference,	//AI will break as it approaches an upcoming change in target - useful for true route-based AI (TODO: implement this).
		TargetDistance				//AI will brake as it approaches the target, coming to a complete stop exactly at the target.
	}

	//Fields to display in the Unity editor window

	[SerializeField] [Range (0, 1)] private float CautiousSpeedFactor = 0.05f;				//% of max speed while braking cautiously
	[SerializeField] [Range (0, 180)] private float CautiousMaxAngle = 50f;					//Approaching corner angle to warrant max braking caution
	[SerializeField] private float CautiousMaxDistance = 100f;								//Distance at which cautious braking should start
	[SerializeField] private float CautiousAngularVelocityFactor = 30f;						//Level of caution in regard to AI's angular velocity (i.e. to ease out of a spin)
	[SerializeField] private float SteerSensitivity = 0.05f;								//Steering sensitivity
	[SerializeField] private float AccelSensitivity = 0.04f;								//Acceleration sensitivity
	[SerializeField] private float BrakeSensitivity = 1f;									//Brake sensitivity
	[SerializeField] private float LateralWanderDistance = 3f;								//How far to wander (laterally) towards target
	[SerializeField] private float LateralWanderSpeed = 0.1f;								//How quickly to fluctuate lateral wandering
	[SerializeField] [Range (0, 1)] private float AccelWanderAmount = 0.1f;					//How much to wander acceleration
	[SerializeField] private float AccelWanderSpeed = 0.1f;									//How quickly to fluctuate acceleration wandering
	[SerializeField] private BrakeCondition ChosenBrakeCondition = BrakeCondition.TargetDistance;	//How the AI should brake (see BrakeCondition{})
	[SerializeField] private bool Driving;													//Whether AI is driving or stopped
	[SerializeField] private Transform Target;												//Target at which to drive towards
	[SerializeField] private bool StopWhenTargetReached;									//Whether AI should stop at target
	[SerializeField] private float ReachTargetThreshold = 2;								//Proximity at which to consider AI has reached target

	//Private fields

	private float RandomPerlin;				//Random value upon which to base wander (so as to avoid all AIs following the same wandering pattern)
	private CarController CarController;	//Reference to current CarController
	private float AvoidOtherCarTime;		//Time in which to avoid other car with which a collision occurred
	private float AvoidOtherCarSlowdown;	//Amount to slow down due to collision, while also avoiding other car
	private float AvoidPathOffset;			//Direction (-1 || 1) in which to offset current path to avoid other car
	private Rigidbody Rigidbody;			//AI's Rigidbody

	private void Awake ()
	{
		/*
		 * Code to run as soon as any AI is instantiated, i.e. AI initialization
		 */

		CarController = GetComponent<CarController> ();	//Get reference to the AI's CarController
		Rigidbody = GetComponent<Rigidbody> ();			//Get reference to the AI's Rigidbody
		RandomPerlin = Random.value * 100;				//Generate random wander value
	}

	private void FixedUpdate ()
	{
		/*
		 * Code to run every frame.
		 */

		if (Target == null || !Driving) {
			//Car shouldn't be moving, stop using the handbrake.
			//Note that, currently, the handbrake will completely break all movement until backwards acceleration occurs.
			CarController.Move (0, 0, -1f, -1f);
		} else {
			float desiredSpeed = CarController.MaxSpeed;	//Get the predefined maximum speed of the car.

			Vector3 fwd = transform.forward;				//Get the forward Vector3 of the AI (from Unity Scripting Manual: blue axis in Unity Editor).

			if (Rigidbody.velocity.magnitude > desiredSpeed * 0.1f) {
				fwd = Rigidbody.velocity;					//If the AI is going to be travelling faster than the predefined maximum speed, set fwd Vector3 to current velocity.
			}

			/*------------------------------------------------------------------------------------------------------*/
			/* Determine whether car should be slowing, as per predefined BrakeCondition enum						*/
			/*------------------------------------------------------------------------------------------------------*/

			switch (ChosenBrakeCondition) {
			case BrakeCondition.TargetDirectionDifference:
				{
					/*
					 * Car will brake according to an upcoming change in direction, for example in a route-based AI system while approaching a corner.
					 * Not currently implemented in actual gameplay due to time constraints, though all code is (or should be) fully functional.
					 */

					float cornerAngle = Vector3.Angle (Target.forward, fwd);																//Determine angle between current direction vector and target location
					float spinningAngle = Rigidbody.angularVelocity.magnitude * CautiousAngularVelocityFactor;								//Determine current spin amount, multiplied by caution factor
					float cautionRequired = Mathf.InverseLerp (0, CautiousMaxAngle, Mathf.Max (spinningAngle, cornerAngle));				//Determine whether to be cautious (i.e. slow down), and what extent to slow down
					desiredSpeed = Mathf.Lerp (CarController.MaxSpeed, CarController.MaxSpeed * CautiousSpeedFactor, cautionRequired);		//Set desired speed value according to above calculations
					break;
				}
			case BrakeCondition.TargetDistance:
				{
					/*
					 * Car will brake as it approaches the target, so that it stops as soon as it reaches the target.
					 * Not currently implemented in actual gameplay due to time constraints, though all code is (or should be) fully functional.
					 * Likely to not implemented in main navigational AI, due to the fact that the car is expected to maintain mostly constant speed through all waypoints.
					 */

					Vector3 distance = Target.position - transform.position;																//Determine distance to target
					float distanceCautiousFactor = Mathf.InverseLerp (CautiousMaxDistance, 0, distance.magnitude);							//Determine caution factor
					float spinningAngle = Rigidbody.angularVelocity.magnitude * CautiousAngularVelocityFactor;								//Take into account current spin amount, multiplied as in upcoming corner angle calculation
					float cautionRequired = Mathf.Max (Mathf.InverseLerp (0, CautiousMaxAngle, spinningAngle), distanceCautiousFactor);		//Determine whether caution is required (i.e. braking threshold has been entered)
					desiredSpeed = Mathf.Lerp (CarController.MaxSpeed, CarController.MaxSpeed * CautiousSpeedFactor, cautionRequired);		//Determine desired speed to which to slow down
					break;
				}
			case BrakeCondition.NeverBrake:
				{
					/*
					 * Car will never brake, therefore this case is empty.
					 */
					break;
				}
			}

			/*------------------------------------------------------------------------------------------------------*/
			/*Handle avoiding other cars (triggered by a collision)													*/
			/*------------------------------------------------------------------------------------------------------*/

			Vector3 offsetTargetPos = Target.position;				//Set target position to 'actual' target position
			if (Time.time < AvoidOtherCarTime) {					//If evasive action is currently being taken
				desiredSpeed *= AvoidOtherCarSlowdown;				//Slow down (if required)
				offsetTargetPos += Target.right * AvoidPathOffset;	//Wander away from other car (while still following true target)
			} else {	//Block is entered if no evasive action is necessary; therefore, the car wanders slightly so as to appear slightly more natural.
				offsetTargetPos += Target.right * (Mathf.PerlinNoise (Time.time * LateralWanderSpeed, RandomPerlin) * 2 - 1) * LateralWanderDistance;
			}

			/*------------------------------------------------------------------------------------------------------*/
			/*Calculate required values for target pathfinding, including random wander								*/
			/*------------------------------------------------------------------------------------------------------*/

			float accelBrakeSensitivity = (desiredSpeed < CarController.CurrentSpeed) ? BrakeSensitivity : AccelSensitivity;				//Different sensitivity for braking/accelerating
			float accel = Mathf.Clamp ((desiredSpeed - CarController.CurrentSpeed) * accelBrakeSensitivity, -1, 1);							//Determine amount of acceleration/braking so as to achieve desired speed
			accel *= (1 - AccelWanderAmount) + (Mathf.PerlinNoise (Time.time * AccelWanderSpeed, RandomPerlin) * AccelWanderAmount);		//Add wander to acceleration - this allows the AI to seem less robotic and more natural. Increasing the wander will create more jostling between AIs during a race, for instance.
			Vector3 localTarget = transform.InverseTransformPoint (offsetTargetPos);														//Calculate position of target, relative to AI, to steer towards
			float targetAngle = Mathf.Atan2 (localTarget.x, localTarget.z) * Mathf.Rad2Deg;													//Determine angle towards target, relative to AI, using basic trigonometry (SOHCAHTOA)
			float steer = Mathf.Clamp (targetAngle * SteerSensitivity, -1, 1) * Mathf.Sign (CarController.CurrentSpeed);					//Determine amount of steering required so as to position the car towards the target
			CarController.Move (steer, accel, accel, 0f);																					//Pass movement data to CarController, so as to reposition the car in the calculated direction with the calculated acceleration

			if(StopWhenTargetReached && localTarget.magnitude < ReachTargetThreshold){
				Driving = false;																											//Stop driving, if necessary, when the AI gets is within the predetermined stopping threshold
			}
		}
	}

	private void OnCollisionStay (Collision col)
	{
		/*
		 * Triggers on collision, and handles collisions with other cars (taking evasive action, etc.)
		 * This trigger could likely be made more efficient, if not for time constraints.
		 */

		if(col.rigidbody != null){
			var otherAI = col.rigidbody.GetComponent<CustomAIControl> ();										//Get reference to other AI involved in collision
			if(otherAI != null){																				//Only continue if AI is not null
				AvoidOtherCarTime = Time.time + 1;																//Take evasive action for 1 second (TODO: make editable within Unity editor)
				if (Vector3.Angle (transform.forward, otherAI.transform.position - transform.position) < 90) {	//Determine which car is further ahead
					AvoidOtherCarSlowdown = 0.5f;																//Other car is ahead, therefore break
				} else{
					AvoidOtherCarSlowdown = 1;																	//Other car is behind, therefore continue at same acceleration
				}

				var otherCarLocalDelta = transform.InverseTransformPoint (otherAI.transform.position);			//Determine difference between local position and other AI's position
				float otherCarAngle = Mathf.Atan2 (otherCarLocalDelta.x, otherCarLocalDelta.z);					//Determine angle between local and other AI, using basic trigonometry (SOHCAHTOA)
				AvoidPathOffset = LateralWanderDistance * -Mathf.Sign (otherCarAngle);							//Set path offset using calculated values and random lateral wander
			}
		}
	}

	public void SetTarget (Transform target)
	{
		/*
		 * Public method to externally change target. Useful for simple pathfinding AI, where a
		 * different target is passed after the previous target has been reached.
		 */

		Target = target;
		Driving = true;
	}

	public void SetStopWhenTargetReached (Boolean b)
	{
		//Adjust whether to stop when target has been reached.
		StopWhenTargetReached = b;
	}
}