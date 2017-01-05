/*
 * Michael Barker
 * 13/06/16
 * Script to update the time text
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeUpdater : MonoBehaviour {

	public float time = 0;
	public Text timeText;

	// Use this for initialization
	void Start () {
		//Initializes text to null
		timeText.text = time.ToString("F4");
	}

	// Update is called once per frame
	void Update () {
		time = float.Parse (timeText.text);		//Get current time value
		time += Time.deltaTime;					//Add time between last and current frame
		timeText.text = time.ToString("F4");	//Displace new time value
	}
}
