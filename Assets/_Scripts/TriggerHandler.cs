/*
 * Michael Barker
 * 09/06/16
 * Script to handle collision triggers between the user car and other entities
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TriggerHandler : MonoBehaviour
{

	public Text TimeText;
	public int NextLevel;
	private float time;
	private Rigidbody rb;

	void Start ()
	{
		//Get Rigidbody component of parent car (for adjusting car's position)
		rb = GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider other)
	{
		time = float.Parse (TimeText.text);											//Get current time

		if (TimeText != null) {														//Only enter if a Text object dedicated to time exists
			if (other.gameObject.CompareTag ("Time-2")) {							//If user collides with 2 second time bonus
				other.gameObject.SetActive (false);									//Disable the bonus (so as to avoid multiple collisions)
				time -= 2;															//Decrement time by 2 seconsds
				TimeText.text = time.ToString ("F4");								//Update time
			} else if (other.gameObject.CompareTag ("Time-5")) {					//If user col;ides with 5 second time bonus
				other.gameObject.SetActive (false);									//Disable the bonus (so as to avoid multiple collisions)
				time -= 5;															//Decrement time by 5 seconsds
				TimeText.text = time.ToString ("F4");								//Update time
			}

			if (other.gameObject.CompareTag ("FinishLine")) {						//If user collides with the finish line
				SceneManager.LoadScene (NextLevel);									//Load next level
			}

			if (other.gameObject.CompareTag ("Fallthrough")) {						//If user collides with fallthrough ground
				time += 2;															//Add 2-second time penalty
				TimeText.text = time.ToString ("F4");								//Update time
				transform.position = new Vector3 (169.7641f, 1, 189.8017f);			//Move car back to starting position
				transform.rotation = Quaternion.Euler (new Vector3 (0, 180, 0));	//Move car back to starting rotation
				rb.velocity = Vector3.zero;											//Reset velocity
				rb.angularVelocity = Vector3.zero;									//Reset angular momentum
			
				/****************PLEASE NOTE****************/
				/* This method will only work for Level 3, */
				/* as values position values are hardcoded */
				/* into the program.                       */
				/*******************************************/
			}
		}
	}
}