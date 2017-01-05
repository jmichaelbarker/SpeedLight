using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


//Ahmed Mahmoud 02/06/2016

/*This script has been created to manage the introduction menu
 * in SpeedLight. This menu requires a quitMenu canvas, start button
 * and quit button. This script has many premade functions that are then
 * attached to the different buttons within the menu via the editor.*/


public class MenuScript : MonoBehaviour {

	public Button startButton;
	public Canvas levelMenu;
	public Button levelButton;
	public Canvas quitMenu;
	public Button quitButton;
	public Canvas helpMenu;
	public Button helpButton;


	//getting the different components of the menu
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
		quitButton = quitButton.GetComponent<Button> ();

		startButton = startButton.GetComponent<Button> ();

		helpMenu = helpMenu.GetComponent<Canvas> ();
		helpButton = helpButton.GetComponent<Button> ();

		levelMenu = levelMenu.GetComponent<Canvas> ();
		levelButton = levelButton.GetComponent<Button> ();


		levelMenu.enabled = false;
		helpMenu.enabled = false;
		quitMenu.enabled = false; //disabling these menus at the start (till user clicks them)
	}

	public void pressQuit() {
		quitMenu.enabled = true; //enabling the quit menu (after user clicks quit)

		//setting the buttons to their respective state
		startButton.enabled = false; //these buttons are disabled to prevent the user from clicking them after they press quit
		helpButton.enabled = false;
		quitButton.enabled = false;
		levelButton.enabled = false;
	}

	public void quitExit() {
		quitMenu.enabled = false; //disabling the quit menu (after user clicks no in the quit menu)

		startButton.enabled = true; //setting the buttons to their respective state
		quitButton.enabled = true;
		helpButton.enabled = true;
		levelButton.enabled = true;
	}

	public void pressHelp() {
		helpMenu.enabled = true; //enabling the help menu (after user clicks help)

		//setting the buttons to their respective state
		startButton.enabled = false; //these buttons are disabled to prevent the user from clicking them after they press help
		helpButton.enabled = false;
		quitButton.enabled = false;
		levelButton.enabled = false;
	}

	public void helpExit() {
		helpMenu.enabled = false; //disabling the help menu

		startButton.enabled = true; //setting the buttons to their respective state
		quitButton.enabled = true;
		helpButton.enabled = true;
		levelButton.enabled = true;
	}

	public void presslvlSelect () {
		levelMenu.enabled = true; //enabling the level selector menu

		//setting the buttons to their respective state
		startButton.enabled = false; //these buttons are disabled to prevent the user from clicking them after they press level selector
		helpButton.enabled = false;
		quitButton.enabled = false;
		levelButton.enabled = false;
	}

	public void lvlSelectExit () {
		levelMenu.enabled = false; //disabling the level selector menu

		startButton.enabled = true; //setting the buttons to their respective state
		quitButton.enabled = true;
		helpButton.enabled = true;
		levelButton.enabled = true;
	}


	public void LoadScene (int lvl) {
		SceneManager.LoadScene (lvl);
	}

	public void start() {
		SceneManager.LoadScene (1); //loads the first level

	}

	public void quit() {
		Application.Quit(); //quits the application
	}
}

