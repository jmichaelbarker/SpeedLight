using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Ahmed Mahmoud 16/06/16

//This script is utilized in the speedometer and will simply
//fill up a sprite as the speed of the car increases

public class FillBasedOnVelocity : MonoBehaviour {
	
	public float maxV = 10.0f;
	public Rigidbody objectToMeasure;

	private Image img;

	void Start () {
		img = GetComponent<Image> ();
	}


	void Update () {
		img.fillAmount = objectToMeasure.velocity.magnitude / maxV;
	}
}
