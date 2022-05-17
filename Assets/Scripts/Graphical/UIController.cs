using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIController : MonoBehaviour {
	public GameObject stageWorldUI;

	public UI_RightSidePanel RIGHT_SIDE_PANEL;
	public UI_LeftSidePanel LEFT_SIDE_PANEL;
	public UI_Dialog DIALOG;
	public UI_Boss BOSS;
	public UI_World WORLD;
	public UI_GameOver GAMEOVER;


	//stage toast panel
	public TextMeshProUGUI stageText;
	public GameObject stagePanel;


	public TextMeshProUGUI toast;


	//Special
	public GameObject playerSpecialPanel;
	public TextMeshProUGUI playerSpecialText;


	//Screens
	public GameObject gameOver;
	public GameObject pauseScreen;
	public GameObject pauseMenuPanel;
	public GameObject stageEndPanel;
	public GameObject loadingScreen;
	public GameObject optionsScreen;



	//OPTIONS
    public GameObject optionsContainer;
	public GameObject optionsValueContainer;

	//GAMEOVER


	void Awake(){
		ToggleLoadingScreen(false);
		ToggleOptionsScreen(false);
	}


	public void ToggleSaveScorePrompt(){
	}

	public void UpdateMenuSelection(string context, int index){
		TextMeshProUGUI[] allSelections = null;
		TextMeshProUGUI[] optionsValues = null;

		if (context == "PauseMenu") {
			allSelections = pauseMenuPanel.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		else if(context == "OptionsMenu"){
			allSelections = optionsContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
			optionsValues = optionsValueContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		else if(context == "GameOverMenu"){
			allSelections = GAMEOVER.gameOverOptionsContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		else if(context == "SaveScorePrompt"){
			allSelections = GAMEOVER.saveScoreContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		
		foreach (TextMeshProUGUI text in allSelections) {
			text.fontStyle = TMPro.FontStyles.Normal;
		}
		if(context == "OptionsMenu"){
			foreach (TextMeshProUGUI text in optionsValues) {
				text.fontStyle = TMPro.FontStyles.Normal;
			}
		}
		allSelections[index].fontStyle = TMPro.FontStyles.Bold;
		if(context == "OptionsMenu") optionsValues[index].fontStyle = TMPro.FontStyles.Bold;
	}

	public void UpdateOptionSelection(int index, string text){
		TextMeshProUGUI option = optionsValueContainer.transform.GetChild(index).GetComponent<TextMeshProUGUI>();
		option.text = text;
	}
	

	public void InitStage(){
		stageWorldUI.SetActive (true);

		WORLD.GetWalls();
		LEFT_SIDE_PANEL.UpdateCoreCharge ("Day", 0);
		LEFT_SIDE_PANEL.UpdateCoreCharge ("Night", 0);

		BOSS.bossHealthSlider.gameObject.SetActive(false);
		BOSS.bossTimer.gameObject.SetActive(false);
		BOSS.bossNamePanel.SetActive (false);
		stageEndPanel.SetActive(false);
		GAMEOVER.saveScoreScreen.SetActive(false);
		gameOver.SetActive(false);
		PauseScreen(false);
		DIALOG.dialogPanel.SetActive(false);

		RIGHT_SIDE_PANEL.UpdateDifficulty(Game.control.stageHandler.difficultyAsString);
		WORLD.InitParallaxes();
		WORLD.ResetTopLayer ();
	}






	////////////////
	//BOSS
	

	public void ShowActivatedPlayerPhase(string text)
	{
		StartCoroutine (_ShowActivatedPlayerPhase (text));
	}

	IEnumerator _ShowActivatedPlayerPhase(string text){
		playerSpecialPanel.SetActive (true);
		playerSpecialText.text = text;

		int dir = -1;

		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < 60; i+=1) {
				playerSpecialPanel.transform.position += new Vector3 (dir + (7 * dir), 0, 0);
				yield return new WaitForSeconds (0.005f);
			}
			dir = 1;
			yield return new WaitForSeconds (5f);
		}
		playerSpecialPanel.SetActive (false);
	}

	//////////////////////////////
	/// 
	/// 
	/// 

	

	


	public void UpdateStageText(int stageID, string stageName, string BGMtext)
	{
		stageText.text = "Stage " + stageID.ToString() + " - " + stageName + '\n' + "BGM: " + BGMtext;
		RIGHT_SIDE_PANEL.UpdateStage(stageName);
	}

	public void ShowStageText(){
		IEnumerator stageText = StageText();
		StartCoroutine(stageText);
	}

	IEnumerator StageText()
	{
		stagePanel.SetActive (true);
		yield return new WaitForSeconds(3);
		stagePanel.gameObject.SetActive(false);
	}


	public void StageCompleted(bool value)
	{
		BOSS.HideBossTimer();
		stageEndPanel.SetActive(value);
	}

	


	public void PauseScreen(bool value){
		pauseScreen.SetActive(value);
	}


	public void PlayToast(string text){
		toast.text = text;
	}

	IEnumerator PlayToast(){
		toast.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		toast.gameObject.SetActive (false);
	}
	
	public void ToggleLoadingScreen(bool toggle){
		loadingScreen.SetActive(toggle);
	}

	//OPTIONS
	public void ToggleOptionsScreen(bool toggle){
		pauseMenuPanel.SetActive(!toggle);
		optionsScreen.SetActive(toggle);
	}


}
