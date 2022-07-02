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

    void Start(){
        InitMenu();
    }

	string CheckInput(string returnValue){
		string input = "";
        string iType = "";

		if(Input.GetKeyDown (KeyCode.UpArrow)) {    input = "up";       iType = "cursor"; }
		if(Input.GetKeyDown (KeyCode.DownArrow)){   input = "down";     iType = "cursor"; }
		if(Input.GetKeyDown (KeyCode.RightArrow)){  input = "right";    iType = "cursor"; }
		if(Input.GetKeyDown (KeyCode.LeftArrow)) {  input = "left";     iType = "cursor"; }

		if(Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Return)) input = "confirm"; iType = "selection";
		if(Input.GetKeyDown (KeyCode.Escape)) input = "back"; iType = "selection";

        if(returnValue == "value") return input;
        if(returnValue == "type")  return iType;	

        return "";
	}

    void LateUpdate ()
	{
		if (AllowInput()) {
            
			if (CheckInput("value") == "up")    Game.control.ui.UpdateMenuSelection(context, Move ("up"));
			if (CheckInput("value") == "down")  Game.control.ui.UpdateMenuSelection(context, Move ("down"));

            if(context == "OptionsMenu") {
                if (CheckInput("value") == "right") Game.control.options.UpdateOption(true, selectedIndex);
                if (CheckInput("value") == "left") 	Game.control.options.UpdateOption(false, selectedIndex);
			}
        
			if (CheckInput("value") == "confirm")   CheckSelection ();	
			if (CheckInput("value") == "back")      CheckPrevious();

		}
		else if(AllowPause() && CheckInput("value") == "back"){
			Menu("PauseMenu");
			Game.control.sound.PlayMenuSound("Pause");
			Game.control.pause.HandlePause();
		}
	}

	
	bool AllowInput(){
		if(!menuOn) return false;
		if(Game.control.loading) return false;
		return true;
	}

	bool AllowPause(){
        if(Game.control.pause.playerHitTimerOn) return false;
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
		}
		else if(context == "PauseMenu") {
			selectedList = pauseMenuItems;
            Game.control.stageUI.TogglePauseMenu(true);
			Game.control.ui.ToggleOptions(false);
		}
		else if(context == "DifficultyMenu"){
			selectedList = difficultyMenuItems;
			Game.control.mainMenuUI.ToggleDifficultySelection(true);
		}
		else if(context == "OptionsMenu"){
			selectedList = optionsMenuItems;
			Game.control.ui.ToggleOptions(true);
			Game.control.options.UpdateAllValues();
		}
		else if(context == "SaveScorePrompt"){
			selectedList = yesNoItems;
		}
		else if(context == "OptionsMenuMain"){
			selectedList = optionsMenuItems;
            Game.control.mainMenuUI.ToggleMainMenu(false);
			Game.control.ui.ToggleOptions(true);
			Game.control.options.UpdateAllValues();
			context = "OptionsMenu";
		}
		else if(context == "GameOverMenu"){
			selectedList = gameOverMenuItems;
			Game.control.stageUI.GAMEOVER.GameOverSelections(true);
		}

        Game.control.ui.UpdateMenu(context, selectedList);
	}

	public void InitMenu(){
		yesNoItems = new List<string>();
		yesNoItems.Add("Yes");
		yesNoItems.Add("No");

		//INSTEAD OF ITEMS, JUST FOLLOW INDEX
		mainMenuItems = new List<string>();
		mainMenuItems.Add ("New Game");
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
		pauseMenuItems.Add ("Restart Stage");
		pauseMenuItems.Add ("Options");
		pauseMenuItems.Add ("Main Menu");
		pauseMenuItems.Add ("Quit");

		optionsMenuItems = new List<string>();
        optionsMenuItems.Add("Screen Mode");
        optionsMenuItems.Add("Resolution");
		optionsMenuItems.Add("Dialog AutoScroll");
		optionsMenuItems.Add("BGM Volume");
		optionsMenuItems.Add("SFX Volume");

		gameOverMenuItems = new List<string>();
		gameOverMenuItems.Add("Restart Game");
		gameOverMenuItems.Add("Main Menu");
		gameOverMenuItems.Add("Quit");

		selectedList = new List<string>();

		menuOn = false;
	}

    int Move(string dir){
        if(context == "Hiscores") return 0;
        Game.control.sound.PlayMenuSound("Cursor");

        if(dir == "up") {
            if (selectedIndex == 0) 
			selectedIndex = selectedList.Count - 1;
            else selectedIndex--;
            return selectedIndex;
        }
        else if (dir == "down") {
            if (selectedIndex == selectedList.Count - 1) 
			selectedIndex = 0;
            else selectedIndex++;
            return selectedIndex;
        }
        return 0;
    }


    void CheckPrevious(){
        Game.control.sound.PlayMenuSound("Cancel");
        
        switch(context){
            case "PauseMenu": ClosePauseMenu();
            break;
            case "DifficultyMenu": Menu("MainMenu");
            break;
            case "OptionsMenu":
                if(Game.control.mainMenuUI == null) Menu("PauseMenu");
                else Menu("MainMenu");
            break;
            case "Hiscores": Menu("MainMenu");
            break;
            case "SaveScoreScreen": Menu("GameOverMenu");
            break;
        }
    }
	void CheckSelection ()
	{
        Game.control.sound.PlayMenuSound("Selection");

		if(context == "MainMenu"){
			
			if(selectedIndex == 0) Menu("DifficultyMenu");
			if(selectedIndex == 1) {Game.control.mainMenuUI.ToggleScorePanel(true); context = "Hiscores"; }
			if(selectedIndex == 2) Menu("OptionsMenuMain");
			if(selectedIndex == 3) Game.control.QuitGame();
		}
		else if(context == "DifficultyMenu"){

			if(selectedIndex == 0) Game.control.stageHandler.SetDifficulty(2);
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
			if(selectedIndex == 1) Game.control.stageHandler.RestartStage ();
			if(selectedIndex == 2) Menu("OptionsMenu");
			if(selectedIndex == 3) Game.control.stageHandler.MainMenu();
			if(selectedIndex == 4) Game.control.QuitGame();
			Game.control.stageUI.UpdateMenuSelection (context, 0);
		}
		else if(context == "GameOverMenu"){

			if(selectedIndex == 0) Game.control.stageHandler.StartGame();
			if(selectedIndex == 1) Game.control.stageHandler.MainMenu();
			if(selectedIndex == 2) Game.control.QuitGame();
		}
		else if(context == "SaveScorePrompt"){

			if(selectedIndex == 0) {Game.control.stageUI.GAMEOVER.SaveScoreScreen(true); context = "SaveScoreScreen";}
			if(selectedIndex == 1) Menu("GameOverMenu");
		}
	}
}
