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
	string context;

	public bool menuOn = false;


	string CheckInput(){
		string input = "";

		if(Input.GetKeyDown (KeyCode.UpArrow)) input = "up";
		if(Input.GetKeyDown (KeyCode.DownArrow)) input = "down";
		if(Input.GetKeyDown (KeyCode.RightArrow)) input = "right";
		if(Input.GetKeyDown (KeyCode.LeftArrow)) input = "left";

		if(Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Return)) input = "confirm";
		if(Input.GetKeyDown (KeyCode.Escape)) input = "back";
		
		return input;		
	}

	bool MainMenuContext(){
		if(context == "MainMenu" || context == "DifficultyMenu") return true;
		else return false;
	}
	
	bool OtherMenuContext(){
		if(context == "SaveScoreScreen") return false;
		if(context == "Hiscores") return false;
		return true;
	}

	// Update is called once per frame
	void Update ()
	{
		if (AllowInput()) {

			if (CheckInput() == "up") { 
				Game.control.sound.PlayMenuSound("Cursor");
				if(MainMenuContext()) Game.control.mainMenuUI.UpdateMenuSelection (context, MoveUp ()); 
				else if(context == "OptionsMenu" && Game.control.mainMenuUI == null) Game.control.ui.UpdateMenuSelection (context, MoveUp ());
				else if(context == "OptionsMenu" && Game.control.mainMenuUI != null) Game.control.mainMenuUI.UpdateMenuSelection (context, MoveUp ());
				else if(OtherMenuContext()) Game.control.ui.UpdateMenuSelection (context, MoveUp ());
			}
			if (CheckInput() == "down") {
				Game.control.sound.PlayMenuSound("Cursor");
				if(MainMenuContext()) Game.control.mainMenuUI.UpdateMenuSelection (context, MoveDown ()); 
				else if(context == "OptionsMenu" && Game.control.mainMenuUI == null) Game.control.ui.UpdateMenuSelection (context, MoveDown ());
				else if(context == "OptionsMenu" && Game.control.mainMenuUI != null) Game.control.mainMenuUI.UpdateMenuSelection (context, MoveDown ());
				else if(OtherMenuContext()) Game.control.ui.UpdateMenuSelection (context, MoveDown ());
			}
			if (CheckInput() == "right") 	{
				if(context == "OptionsMenu"){
					Game.control.sound.PlayMenuSound("Cursor");
					Game.control.options.UpdateOption(true, selectedIndex);
				}
			}
			if (CheckInput() == "left") 	{
				if(context == "OptionsMenu"){
					Game.control.sound.PlayMenuSound("Cursor");
					Game.control.options.UpdateOption(false, selectedIndex);
				}
			}
			if (CheckInput() == "confirm") CheckSelection ();	

			if(CheckInput() == "back"){
				if(context == "PauseMenu") Game.control.sound.PlayMenuSound("Confirm");
				else Game.control.sound.PlayMenuSound("Cancel");

				if(context == "PauseMenu") ClosePauseMenu();
				else if(context == "DifficultyMenu") Menu("MainMenu");
				
				else if(context == "OptionsMenu"){
					if(Game.control.mainMenuUI == null) 
						 Menu("PauseMenu");
					else Menu("MainMenu");
				}
				else if(context == "Hiscores"){
					if(Game.control.mainMenuUI != null) Menu("MainMenu");
				}
				else if(context == "SaveScoreScreen") Menu("GameOverMenu");
			}


		}

		else if(AllowPause() && CheckInput() == "back"){
				Menu("PauseMenu");
				Game.control.sound.PlayMenuSound("Pause");
				Game.control.pause.HandlePause();
			}
	}
	
	bool AllowInput(){
		if(!menuOn) return false;
		if(Game.control.stageHandler.loading) return false;
		return true;
	}

	bool AllowPause(){
		if(menuOn) return false;
		if(Game.control.mainMenuUI != null) return false;
		if(!Game.control.stageHandler.stageOn) return false;
		return true;
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
			Game.control.ui.GAMEOVER.GameOverSelections(true);
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
		pauseMenuItems.Add ("Main Menu");
		pauseMenuItems.Add ("Quit");

		optionsMenuItems = new List<string>();
		optionsMenuItems.Add("Dialog AutoScroll");
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
		else selectedIndex--;
		return selectedIndex;
	}

	int MoveDown ()
	{
		if (selectedIndex == selectedList.Count - 1) 
			selectedIndex = 0;
		else selectedIndex++;
		return selectedIndex;
	}

	void CheckSelection ()
	{
	
		if(context == "MainMenu"){
			Game.control.sound.PlayMenuSound("Selection");
			if(selectedIndex == 0) Menu("DifficultyMenu");
			if(selectedIndex == 1) {Game.control.mainMenuUI.ToggleScorePanel(true); context = "Hiscores"; }
			if(selectedIndex == 2) Menu("OptionsMenuMain");
			if(selectedIndex == 3) Game.control.QuitGame();
		}
		else if(context == "DifficultyMenu"){
			Game.control.sound.PlayMenuSound("Selection");
			if(selectedIndex == 0) Game.control.stageHandler.SetDifficulty(1);
			if(selectedIndex == 1) Game.control.stageHandler.SetDifficulty(3);
			if(selectedIndex == 2) Game.control.stageHandler.SetDifficulty(5);
			if(selectedIndex == 3) Game.control.stageHandler.SetDifficulty(10);
			menuOn = false;
			Game.control.StartGame ();
		}
		else if(context == "PauseMenu"){
			Game.control.sound.PlayMenuSound("Selection");
			if(selectedIndex == 0) {
				Game.control.pause.HandlePause();
				menuOn = false;
			}
			if(selectedIndex == 1) Game.control.stageHandler.RestartStage ();
			if(selectedIndex == 2) Menu("OptionsMenu");
			if(selectedIndex == 3) Game.control.MainMenu();
			if(selectedIndex == 4) Game.control.QuitGame();
			Game.control.ui.UpdateMenuSelection (context, 0);
		}
		else if(context == "GameOverMenu"){
			Game.control.sound.PlayMenuSound("Selection");
			if(selectedIndex == 0) Game.control.stageHandler.StartGame();
			if(selectedIndex == 1) Game.control.MainMenu();
			if(selectedIndex == 2) Game.control.QuitGame();
		}
		else if(context == "SaveScorePrompt"){
			Game.control.sound.PlayMenuSound("Selection");
			if(selectedIndex == 0) {Game.control.ui.GAMEOVER.SaveScoreScreen(true); context = "SaveScoreScreen";}
			if(selectedIndex == 1) Menu("GameOverMenu");
		}
	}
}
