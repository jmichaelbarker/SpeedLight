using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

//Ahmed Mahmoud (10/06/16)  - Finalized on 14/06/16

/*This script will be active during the duration of the user playing the game.
 * One the users opts to pause the menu and chooses an action this script's functions
 * will be called. */

public class PauseGame : MonoBehaviour
{
	public Transform pauseMenu;
	
	// checks for the user to open/close pause menu
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) { //if the user presses the escape key
			pause ();
		}
	}

	public void quit ()
	{
		Application.Quit (); //quits the application
	}

	public void restart (int levelID)
	{
		SceneManager.LoadScene (levelID);
		AudioListener.pause = false; //resuming sound
	}

	public void quitToMenu ()
	{
		SceneManager.LoadScene (0); //quits the application

		Time.timeScale = 1; //unpausing the world.
		AudioListener.pause = false; //resuming sound
	}

	public void pause ()
	{			
		if (pauseMenu.gameObject.activeInHierarchy == false) { //if the pause menu is not active
			pauseMenu.gameObject.SetActive (true); //open it (set to active)

			Time.timeScale = 0; //setting the time scale to 0, effectively stopping the world.
			AudioListener.pause = true; //pausing sound
		} else { //if it is active
			pauseMenu.gameObject.SetActive (false); //close it

			Time.timeScale = 1; //unpausing the world.
			AudioListener.pause = false; //resuming sound
		}
	}
}
