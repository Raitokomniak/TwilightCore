using UnityEngine;

using System.Collections;

public class MenuController : MonoBehaviour
{
	public ArrayList mainMenuItems;
	public ArrayList pauseMenuItems;
	public ArrayList difficultyMenuItems;

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
				if(context == "MainMenu" || context == "DifficultyMenu") Game.control.mainMenuUI.UpdateMenuSelection (context, MoveUp ());
				else if(context == "PauseMenu") Game.control.ui.UpdateMenuSelection ("PauseMenu", MoveUp ());

			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				if(context == "MainMenu" || context == "DifficultyMenu") Game.control.mainMenuUI.UpdateMenuSelection (context, MoveDown ());
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
			Game.control.mainMenuUI.UpdateMenuSelection ("MainMenu", 0);
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
		//INSTEAD OF ITEMS, JUST FOLLOW INDEX
		mainMenuItems = new ArrayList ();
		mainMenuItems.Add ("Start Game");
		mainMenuItems.Add ("Quit Game");

		difficultyMenuItems = new ArrayList();
		difficultyMenuItems.Add("Easy");
		difficultyMenuItems.Add("Normal");
		difficultyMenuItems.Add("Hard");
		difficultyMenuItems.Add("Nightmare");

		charSelectItems = new ArrayList ();
		charSelectItems.Add ("1");

		pauseMenuItems = new ArrayList ();
		pauseMenuItems.Add ("Resume");
		pauseMenuItems.Add ("Restart");
		pauseMenuItems.Add ("Quit");

		

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
		selection = (string)selectedList [selectedIndex];

		if(context == "MainMenu"){
			switch (selection) {
			case "Start Game":
				Game.control.mainMenuUI.ToggleDifficultySelection(true);
				context = "DifficultyMenu";
				selectedList = difficultyMenuItems;
				Game.control.mainMenuUI.UpdateMenuSelection ("DifficultyMenu", 0);
				break;
			case "Quit Game":
				Application.Quit();
				break;
			}
		}
		else if(context == "DifficultyMenu"){
			
			switch (selection) {
			case "Easy":
				Game.control.stageHandler.difficultyMultiplier = 1;
				break;
			case "Normal":
				Game.control.stageHandler.difficultyMultiplier = 3;
				break;
			case "Hard":
				Game.control.stageHandler.difficultyMultiplier = 5;
				break;
			case "Nightmare":
				Game.control.stageHandler.difficultyMultiplier = 7;
				break;
			}
			menuOn = false;
			Game.control.StartGame ();
		}
		else if(context == "PauseMenu"){
			switch (selection) {
			case "Resume":
				Game.control.pause.HandlePause();
				break;
			case "Restart":
				Game.control.stageHandler.StartStage (true);
				break;
			case "Quit":
				Application.Quit();
				break;
			}
		}
	}

}
