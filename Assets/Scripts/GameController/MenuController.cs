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

	// Use this for initialization
	void Awake ()
	{
		

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (menuOn) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				MoveUp ();
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
				MoveDown ();
			} else if (Input.GetKeyDown (KeyCode.Z)) {
				CheckSelection ();	
			} else if (Input.GetKeyDown (KeyCode.X)) {
				CheckPreviousState ();
			}
		}
	}

	void CheckMenu ()
	{
		if (GameController.gameControl.scene.GetCurrentScene () == "MainMenu") {
			context = "MainMenu";
			selectedList = mainMenuItems;
		} else if (GameController.gameControl.scene.GetCurrentScene () == "CharSelect") {
			context = "CharSelect";
			selectedList = charSelectItems;
		}

		if (GameController.gameControl.scene.GetCurrentScene () == "Level1") {
			if (GameController.gameControl.pause.paused) {
				context = "PauseMenu";
				selectedList = pauseMenuItems;
			}
		}
		if (context != "") {
			GameController.gameControl.ui.UpdateMenuSelection (context, selectedIndex);
		}
	}

	public void InitMenu(){
		mainMenuItems = new ArrayList ();
		mainMenuItems.Add ("Start Game");
		mainMenuItems.Add ("Quit Game");

		charSelectItems = new ArrayList ();
		charSelectItems.Add ("1");

		pauseMenuItems = new ArrayList ();
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

	void MoveUp ()
	{
		if (selectedIndex == 0)
			selectedIndex = selectedList.Count - 1;
		else
			selectedIndex--;

		GameController.gameControl.ui.UpdateMenuSelection (context, selectedIndex);
	}

	void MoveDown ()
	{
		if (selectedIndex == selectedList.Count - 1)
			selectedIndex = 0;
		else
			selectedIndex++;

		GameController.gameControl.ui.UpdateMenuSelection (context, selectedIndex);
		Debug.Log ("menuselect " + selectedIndex);
	}

	void CheckPreviousState ()
	{
		switch (GameController.gameControl.scene.GetCurrentScene ()) {
		case "CharSelect":
			GameController.gameControl.scene.MainMenu ();
			break;
		}
	}

	void CheckSelection ()
	{
		menuOn = false;
		selection = (string)selectedList [selectedIndex];
//		Debug.Log(context);
//		Debug.Log (selection);
		switch (context) {
		case "MainMenu":
			switch (selection) {
			case "Start Game":
				GameController.gameControl.scene.CharSelect ();
				break;
			case "Quit Game":
				GameController.gameControl.QuitGame ();
				break;
			}
			break;
		case "CharSelect":
			selection = (string)charSelectItems [selectedIndex];

			switch (selection) {
			case "1":
				StartCoroutine(GameController.gameControl.scene.StartGame ());
				break;
			}
			break;
		case "PauseMenu":
			switch (selection) {
			case "Restart":
				GameController.gameControl.gameEnd.Restart ();
				break;
			case "MainMenu":
				GameController.gameControl.scene.MainMenu ();
				break;
			}
			break;
		}
	}

}
