using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIController : MonoBehaviour {

	public UI_RightSidePanel RIGHT_SIDE_PANEL;
	public UI_LeftSidePanel LEFT_SIDE_PANEL;
	public UI_Dialog DIALOG;
	public UI_Boss BOSS;
	public UI_World WORLD;
	public UI_GameOver GAMEOVER;
	public UI_StageToastPanel STAGETOAST;
	public UI_StageComplete STAGEEND;

	public Image EFFECT_OVERLAY;


	public TextMeshProUGUI toast;


	//Special
	public GameObject playerSpecialPanel;
	public TextMeshProUGUI playerSpecialText;


	//Screens
	public GameObject pauseScreen;
	public GameObject loadingScreen;
	public GameObject optionsScreen;


	//OPTIONS
	public GameObject pauseMenuOptions;
    public GameObject optionsContainer;
	public GameObject optionsValueContainer;

	public void InitStage(){
		EffectOverlay("Black");
		WORLD.GetWalls();
		LEFT_SIDE_PANEL.EmptyCores();
		BOSS.HideUI();
		

		STAGEEND.gameObject.SetActive(false);
		GAMEOVER.saveScoreScreen.SetActive(false);
		GAMEOVER.gameObject.SetActive(false);
		EFFECT_OVERLAY.gameObject.SetActive(false);
		TogglePauseScreen(false);
		DIALOG.dialogPanel.SetActive(false);
		
		WORLD.InitParallaxes();
		WORLD.ResetTopLayer ();

		LEFT_SIDE_PANEL.SetSliderMaxValues();

		RIGHT_SIDE_PANEL.UpdateDifficulty(Game.control.stageHandler.difficultyAsString);
		EffectOverlay("Black", false, 1);
	}

	void Awake(){
		ToggleLoadingScreen(false);
		ToggleOptionsScreen(false);
	}

	public void TogglePauseScreen(bool value){
		pauseScreen.SetActive(value);
	}
	
	public void ToggleLoadingScreen(bool toggle){
		loadingScreen.SetActive(toggle);
	}

	public void ToggleOptionsScreen(bool toggle){
		pauseMenuOptions.SetActive(!toggle);
		optionsScreen.SetActive(toggle);
	}

	public void HideStageCompletedScreen(){
		BOSS.HideBossTimer();
		STAGEEND.gameObject.SetActive(false);
	}
	
	public void EffectOverlay(string color, bool fadeIn, float fadeTime){
		EFFECT_OVERLAY.gameObject.SetActive(true);
		if(color == "White") EFFECT_OVERLAY.color = new Color(1,1,1,1);
		if(color == "Black") EFFECT_OVERLAY.color = new Color(0,0,0,1);
		//if(color == "NightCore") EFFECT_OVERLAY.color = new Color(0.06f,0.04f,0.01f,0.47f);
		IEnumerator animateRoutine = AnimateOverlay(fadeIn, fadeTime);
		StartCoroutine(animateRoutine);
	}

	public void EffectOverlay(string color){
		if(color == "White") EFFECT_OVERLAY.color = new Color(1,1,1,1);
		if(color == "Black") EFFECT_OVERLAY.color = new Color(0,0,0,1);
	}

	IEnumerator AnimateOverlay(bool fadeIn, float fadeTime){
		float r = EFFECT_OVERLAY.color.r;
		float g = EFFECT_OVERLAY.color.g;
		float b = EFFECT_OVERLAY.color.b;
		float a = EFFECT_OVERLAY.color.a;

		if(fadeIn){
			EFFECT_OVERLAY.color = new Color(EFFECT_OVERLAY.color.r,EFFECT_OVERLAY.color.g,EFFECT_OVERLAY.color.b,0);
			for(float i = 0; i < a; i+= Time.deltaTime){
				EFFECT_OVERLAY.color = new Color(r,g,b,i);
				yield return new WaitForSeconds(fadeTime * Time.deltaTime);
			}
		}
		else {
			EFFECT_OVERLAY.color = new Color(r,g,b,a);
			for(float i = 1; i > 0; i-= Time.deltaTime){
				EFFECT_OVERLAY.color = new Color(r,g,b,i);
				yield return new WaitForSeconds(fadeTime * Time.deltaTime);
			}
		}
		yield return null;
	}

	public void ShowStageCompletedScreen(){
		BOSS.HideUI();
		STAGEEND.gameObject.SetActive(true);
		STAGEEND.UpdateScoreBreakDown();
	}


	public void UpdateMenuSelection(string context, int index){
		TextMeshProUGUI[] allSelections = null;
		TextMeshProUGUI[] optionsValues = null;

		if (context == "PauseMenu") {
			allSelections = pauseMenuOptions.transform.GetComponentsInChildren<TextMeshProUGUI> ();
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
	
	public void ShowActivatedPlayerPhase(string text){
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

	public void PlayStageToast(){
		STAGETOAST.gameObject.SetActive(true);
		STAGETOAST.Play();
	}

	public void PlayToast(string text){
		toast.text = text;
	}

	IEnumerator PlayToast(){
		toast.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		toast.gameObject.SetActive (false);
	}

}
