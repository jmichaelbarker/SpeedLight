/*
 * Michael Barker
 * 13/06/16
 * Script to handle waypoint transitions
 */

using UnityEngine;
using System.Collections;
using System.IO;

public class WaypointTrackHandler : MonoBehaviour
{
	public int startNode;
	public int endNode;
	private int curNode;
	public WaypointManager waypointManager;
	private CustomAIControl customAIControl;

	public int carID;

	private void Start ()
	{
		curNode = startNode;																								//Set the curNode counter to the first node

		waypointManager = GameObject.FindGameObjectWithTag ("WaypointManager-" + carID).GetComponent<WaypointManager> ();	//Get waypointManager (script) pointer
		customAIControl = GameObject.FindGameObjectWithTag ("CarAI-" + carID).GetComponent<CustomAIControl> ();				//Get carAI (script) pointer

		customAIControl.SetTarget (waypointManager.waypointNodes [startNode].getTransform ());								//Set car's target to the first node
	}

	void OnTriggerEnter (Collider other)																//When colliding with an object
	{
		if (other.gameObject.CompareTag ("Node-" + carID)) {											//If the object is a node belonging to this AI
			curNode++;																					//Increment current node counter

			if (curNode == endNode+1) {																	//If outside of node range
				customAIControl.SetStopWhenTargetReached (true);										//Ensure car will stop
				curNode--;																				//Decrement current node counter
			}

			customAIControl.SetTarget (waypointManager.waypointNodes [curNode].getTransform ());		//Set target to next node
			other.gameObject.SetActive (false);															//Deactivate current node
		}
	}
}
