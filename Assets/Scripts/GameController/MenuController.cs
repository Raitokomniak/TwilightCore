using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
	public List<string> mainMenuItems;
	public List<string> pauseMenuItems;
	public List<string> yesNoItems;
	public List<string> difficultyMenuItems;
	public List<string> optionsMenuItems;
	public List<string> gameOverMenuItems;

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
			else if (context == "PauseMenu" || context == "GameOverMenu" || context == "SaveScorePrompt") {
				if (Input.GetKeyDown (KeyCode.UpArrow)) 	Game.control.ui.UpdateMenuSelection (context, MoveUp ());
				if (Input.GetKeyDown (KeyCode.DownArrow)) 	Game.control.ui.UpdateMenuSelection (context, MoveDown ());
			}

			if(context == "OptionsMenu"){
				//if (Input.GetKeyDown (KeyCode.Z)) CheckSelection ();
				if (Input.GetKeyDown (KeyCode.UpArrow)) 	
					if(Game.control.mainMenuUI == null) 
						Game.control.ui.UpdateMenuSelection (context, MoveUp ());
					else 
						Game.control.mainMenuUI.UpdateMenuSelection (context, MoveUp ());

				if (Input.GetKeyDown (KeyCode.DownArrow)) 	
					if(Game.control.mainMenuUI == null) 
						Game.control.ui.UpdateMenuSelection (context, MoveDown ());
					else
						Game.control.mainMenuUI.UpdateMenuSelection (context, MoveDown ());

				if (Input.GetKeyDown (KeyCode.RightArrow)) 	Game.control.options.UpdateOption(true, selectedIndex);
				if (Input.GetKeyDown (KeyCode.LeftArrow)) 	Game.control.options.UpdateOption(false, selectedIndex);
			}
			if (Input.GetKeyDown (KeyCode.Z)) CheckSelection ();	


			if(Input.GetKeyDown(KeyCode.Escape)){
				if(context == "PauseMenu"){
					ClosePauseMenu();
				}
				else if(context == "DifficultyMenu"){
					Menu("MainMenu");
				}
				else if(context == "OptionsMenu"){
					if(Game.control.mainMenuUI == null) Menu("PauseMenu");
					else {
						Menu("MainMenu");
					}
				}
				else if(context == "Hiscores"){
					if(Game.control.mainMenuUI != null)
						Menu("MainMenu");
				}
				else if(context == "SaveScoreScreen"){
					Menu("GameOverMenu");
				}
			}
		}

		else { //IF MENU NOT ON, TOGGLE ON
			if(Game.control.mainMenuUI == null && Game.control.stageHandler.stageOn && Input.GetKeyDown(KeyCode.Escape)){
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
			Game.control.mainMenuUI.ToggleMainMenu(true);
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
		else if(context == "SaveScorePrompt"){
			selectedList = yesNoItems;
			Game.control.ui.UpdateMenuSelection ("SaveScorePrompt", 0);
			//Game.control.ui.togglesavescoreprompt
		}
		else if(context == "OptionsMenuMain"){
			selectedList = optionsMenuItems;
			Game.control.mainMenuUI.ToggleOptions(true);
			Game.control.mainMenuUI.UpdateMenuSelection ("OptionsMenu", 0);
			Game.control.options.UpdateAllValues();
			context = "OptionsMenu";
		}
		else if(context == "GameOverMenu"){
			selectedList = gameOverMenuItems;
			Game.control.ui.GameOverSelections(true);
			Game.control.ui.UpdateMenuSelection ("GameOverMenu", 0);
		}
	}

	public void InitMenu(){
		yesNoItems = new List<string>();
		yesNoItems.Add("Yes");
		yesNoItems.Add("No");

		//INSTEAD OF ITEMS, JUST FOLLOW INDEX
		mainMenuItems = new List<string>();
		mainMenuItems.Add ("Start Game");
		mainMenuItems.Add ("Hiscores");
		mainMenuItems.Add ("Options");
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

		gameOverMenuItems = new List<string>();
		gameOverMenuItems.Add("Restart Stage");
		gameOverMenuItems.Add("Main Menu");
		gameOverMenuItems.Add("Quit");

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
			if(selectedIndex == 0) Menu("DifficultyMenu");
			if(selectedIndex == 1) {Game.control.mainMenuUI.ToggleScorePanel(true); context = "Hiscores"; }
			if(selectedIndex == 2) Menu("OptionsMenuMain");
			if(selectedIndex == 3) Game.control.QuitGame();
		}
		else if(context == "DifficultyMenu"){
			if(selectedIndex == 0) Game.control.stageHandler.SetDifficulty(1);
			if(selectedIndex == 1) Game.control.stageHandler.SetDifficulty(3);
			if(selectedIndex == 2) Game.control.stageHandler.SetDifficulty(5);
			if(selectedIndex == 3) Game.control.stageHandler.SetDifficulty(10);
			menuOn = false;
			Game.control.StartGame ();
		}
		else if(context == "PauseMenu"){
			if(selectedIndex == 0) {
				Game.control.pause.HandlePause();
				menuOn = false;
			}
			if(selectedIndex == 1) Game.control.stageHandler.RestartStage (Game.control.stageHandler.currentStage);
			if(selectedIndex == 2) Menu("OptionsMenu");
			if(selectedIndex == 3) Game.control.QuitGame();
			Game.control.ui.UpdateMenuSelection (context, 0);
		}
		else if(context == "GameOverMenu"){
			if(selectedIndex == 0) Game.control.stageHandler.RestartStage (Game.control.stageHandler.currentStage);
			if(selectedIndex == 1) Game.control.MainMenu();
			if(selectedIndex == 2) Game.control.QuitGame();
		}
		else if(context == "SaveScorePrompt"){
			if(selectedIndex == 0) {Game.control.ui.SaveScoreScreen(true); context = "SaveScoreScreen";}
			if(selectedIndex == 1) Menu("GameOverMenu");
		}
	}
}
