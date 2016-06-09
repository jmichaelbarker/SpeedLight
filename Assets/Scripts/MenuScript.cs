using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startButton;
	public Button quitButton;


	//getting the different components of the menu
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startButton = startButton.GetComponent<Button> ();
		quitButton = quitButton.GetComponent<Button> ();

		quitMenu.enabled = false; //disabling the quit menu at the start
	}

	public void pressExit() {
		quitMenu.enabled = true; //enabling the quit menu (after user clicks quit)

		startButton.enabled = false; //setting the buttons to their respective state
		quitButton.enabled = true;
	}

	public void noQuit() {
		quitMenu.enabled = false; //disabling the quit menu (after user clicks no in the quit menu)

		startButton.enabled = true; //setting the buttons to their respective state
		quitButton.enabled = true;
	}

	public void start() {
		Application.LoadLevel (1); //loads the first level
	
	}

	public void quit() {
		Application.Quit();
	}

}

