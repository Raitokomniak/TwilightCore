using UnityEngine;

using System.Collections;

public class MenuController : MonoBehaviour
{
	public ArrayList mainMenuItems;
	public ArrayList pauseMenuItems;
	public ArrayList charSelectItems;

	ArrayList selectedList;


	int selectedIndex;
	string selection;
	string context;

	public bool menuOn = false;

	
	// Update is called once per frame
	void Update ()
	{
		if (menuOn) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				if(context == "MainMenu") Game.control.mainMenuUI.UpdateMenuSelection (MoveUp ());
				else if(context == "PauseMenu") Game.control.ui.UpdateMenuSelection ("PauseMenu", MoveUp ());
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				if(context == "MainMenu") Game.control.mainMenuUI.UpdateMenuSelection (MoveDown ());
				else if(context == "PauseMenu") Game.control.ui.UpdateMenuSelection ("PauseMenu", MoveDown ());
			} else if (Input.GetKeyDown (KeyCode.Z)) {
				CheckSelection ();	
			} else if (Input.GetKeyDown (KeyCode.X)) {
				CheckPreviousState ();
			}
		}
	}

	void CheckMenu ()
	{
		if (Game.control.GetCurrentScene () == "MainMenu") {
			context = "MainMenu";
			selectedList = mainMenuItems;
			Game.control.mainMenuUI.UpdateMenuSelection (0);
		}
		if (Game.control.GetCurrentScene () == "Level1") {
			if (Game.control.pause.paused) {
				context = "PauseMenu";
				selectedList = pauseMenuItems;
				Game.control.ui.UpdateMenuSelection ("PauseMenu", 0);
			}
		}
	}

	public void InitMenu(){
		mainMenuItems = new ArrayList ();
		mainMenuItems.Add ("Start Game");
		mainMenuItems.Add ("Quit Game");

		charSelectItems = new ArrayList ();
		charSelectItems.Add ("1");

		pauseMenuItems = new ArrayList ();
		pauseMenuItems.Add ("Resume");
		pauseMenuItems.Add ("Restart");
		pauseMenuItems.Add ("MainMenu");

		selectedList = new ArrayList();
	}

	public void ToggleMenu(bool value)
	{
		selectedIndex = 0;
		menuOn = value;
		CheckMenu ();
	}

	int MoveUp ()
	{
		if (selectedIndex == 0)
			selectedIndex = selectedList.Count - 1;
		else
			selectedIndex--;

		return selectedIndex;
	}

	int MoveDown ()
	{
		if (selectedIndex == selectedList.Count - 1)
			selectedIndex = 0;
		else
			selectedIndex++;

		return selectedIndex;
	}

	void CheckPreviousState ()
	{
		switch (Game.control.GetCurrentScene ()) {
		case "CharSelect":
			Game.control.MainMenu ();
			break;
		}
	}

	void CheckSelection ()
	{
		menuOn = false;
		selection = (string)selectedList [selectedIndex];

		switch (context) {
		case "MainMenu":
			switch (selection) {
			case "Start Game":
				Game.control.StartGame ();
				break;
			case "Quit Game":
				Application.Quit();
				break;
			}
			break;
		case "PauseMenu":
			switch (selection) {
			case "Resume":
				Game.control.pause.HandlePause();
				break;
			case "Restart":
				Game.control.stage.StartStage (true, "");
				break;
			case "Quit":
				Application.Quit();
				break;
			}
			break;
		}
	}

}
