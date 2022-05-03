using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
	public List<string> mainMenuItems;
	public List<string> pauseMenuItems;
	public List<string> difficultyMenuItems;
	public List<string> optionsMenuItems;
	List<string> selectedList;


	int selectedIndex;
	string selection;
	string context;

	public bool menuOn = false;

	
	// Update is called once per frame
	void Update ()
	{
		if (menuOn) {
			if(context == "MainMenu" || context == "DifficultyMenu"){
				if (Input.GetKeyDown (KeyCode.UpArrow)) { 	Game.control.mainMenuUI.UpdateMenuSelection (context, MoveUp ()); };
				if (Input.GetKeyDown (KeyCode.DownArrow)) { Game.control.mainMenuUI.UpdateMenuSelection (context, MoveDown ()); };
			}
			
			if(context == "PauseMenu" || context == "OptionsMenu") {
				if (Input.GetKeyDown (KeyCode.UpArrow)) 	Game.control.ui.UpdateMenuSelection (context, MoveUp ());
				if (Input.GetKeyDown (KeyCode.DownArrow)) 	Game.control.ui.UpdateMenuSelection (context, MoveDown ());
			}

			if(context == "OptionsMenu"){
				//if (Input.GetKeyDown (KeyCode.Z)) CheckSelection ();	
				if (Input.GetKeyDown (KeyCode.RightArrow)) 	Game.control.options.UpdateOption(true, selectedIndex);
				if (Input.GetKeyDown (KeyCode.LeftArrow)) 	Game.control.options.UpdateOption(false, selectedIndex);
			}
			if (Input.GetKeyDown (KeyCode.Z)) CheckSelection ();	


			if(Input.GetKeyDown(KeyCode.Escape)){
				if(context == "PauseMenu"){
					ClosePauseMenu();
				}
				else if(context == "OptionsMenu"){
					Menu("PauseMenu");
				}
			}
		}

		else { //IF MENU NOT ON, TOGGLE ON
			if(Game.control.stageHandler.stageOn && Input.GetKeyDown(KeyCode.Escape)){
				Menu("PauseMenu");
				Game.control.pause.HandlePause();
			}
		}
		
	}

	void ClosePauseMenu(){
		Game.control.pause.HandlePause();
		menuOn = false;
	}

	public void Menu(string _context){
		context = _context;
		selectedIndex = 0;
		menuOn = true;

		if(context == "MainMenu"){
			selectedList = mainMenuItems;
			Game.control.mainMenuUI.UpdateMenuSelection ("MainMenu", 0);
		}
		else if(context == "PauseMenu") {
			selectedList = pauseMenuItems;
			Game.control.ui.UpdateMenuSelection ("PauseMenu", 0);
			Game.control.ui.ToggleOptionsScreen(false);
		}
		else if(context == "DifficultyMenu"){
			selectedList = difficultyMenuItems;
			Game.control.mainMenuUI.ToggleDifficultySelection(true);
			Game.control.mainMenuUI.UpdateMenuSelection ("DifficultyMenu", 0);
		}
		else if(context == "OptionsMenu"){
			selectedList = optionsMenuItems;
			Game.control.ui.ToggleOptionsScreen(true);
			Game.control.options.UpdateAllValues();
		}

	}

	public void InitMenu(){
		//INSTEAD OF ITEMS, JUST FOLLOW INDEX
		mainMenuItems = new List<string>();
		mainMenuItems.Add ("Start Game");
		mainMenuItems.Add ("Quit Game");

		difficultyMenuItems = new List<string>();
		difficultyMenuItems.Add("Very Easy");
		difficultyMenuItems.Add("Easy");
		difficultyMenuItems.Add("Normal");
		difficultyMenuItems.Add("Nightmare");

		pauseMenuItems = new List<string>();
		pauseMenuItems.Add ("Resume");
		pauseMenuItems.Add ("Restart");
		pauseMenuItems.Add ("Options");
		pauseMenuItems.Add ("Quit");

		optionsMenuItems = new List<string>();
		optionsMenuItems.Add("AutoScroll");
		optionsMenuItems.Add("BGM Volume");
		optionsMenuItems.Add("SFX Volume");

		selectedList = new List<string>();

		menuOn = false;
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

	void CheckSelection ()
	{
		if(context == "MainMenu"){
			switch (selectedIndex) {
			case 0: //start
				Menu("DifficultyMenu");
				break;
			case 1: //quit
				Application.Quit();
				break;
			}
		}
		else if(context == "DifficultyMenu"){
			
			switch (selectedIndex) {
			case 0:
				Game.control.stageHandler.SetDifficulty(1);
				break;
			case 1:
				Game.control.stageHandler.SetDifficulty(3);
				break;
			case 2:
				Game.control.stageHandler.SetDifficulty(5);
				break;
			case 3:
				Game.control.stageHandler.SetDifficulty(10);
				break;
			}
			menuOn = false;
			Game.control.StartGame ();
		}
		else if(context == "PauseMenu"){
			switch (selectedIndex) {
			case 0: //resume
				Game.control.pause.HandlePause();
				menuOn = false;
				break;
			case 1: //restart
				Game.control.stageHandler.RestartStage (Game.control.stageHandler.currentStage);
				break;
			case 2: //options
				Menu("OptionsMenu");
				break;
			case 3: //quit
				Application.Quit();
				break;
			}

			Game.control.ui.UpdateMenuSelection (context, 0);
		}
		/*
		else if(context == "Options"){
			switch (selectedIndex) {
			case 0: //autoscroll
				Debug.Log("Edit autoscroll value");
				break;
			}

		}*/

		
	}

}
