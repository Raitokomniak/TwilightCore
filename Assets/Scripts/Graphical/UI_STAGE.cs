using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UI_STAGE : UI {

	public UI_RightSidePanel RIGHT_SIDE_PANEL;
	public UI_LeftSidePanel LEFT_SIDE_PANEL;
	public UI_Dialog DIALOG;
	public UI_Boss BOSS;
	public UI_World WORLD;
	public UI_GameOver GAMEOVER;
	public UI_StageToastPanel STAGETOAST;
	public UI_StageComplete STAGEEND;
    public UI_INTRO INTRO;

	//EFFECT OVERLAY
	public Image EFFECT_OVERLAY;
    public bool fadeOver;
    public bool fadeFromBlack;

    //TOAST
	public TextMeshProUGUI toast;
    bool toastPlaying;

    //PAUSE SCREEN
	public GameObject pauseScreen;
	public GameObject pauseMenuOptions;


	public void InitStage(){
		if(fadeFromBlack) EffectOverlay("Black");
        else EffectOverlay("White");
		//LEFT_SIDE_PANEL.EmptyCores();
		BOSS.HideUI();
		
		STAGEEND.Hide();
		GAMEOVER.saveScoreScreen.SetActive(false);
		GAMEOVER.gameObject.SetActive(false);
		EFFECT_OVERLAY.gameObject.SetActive(false);
		TogglePauseScreen(false);
		DIALOG.dialogPanel.SetActive(false);
		
		WORLD.InitParallaxes();
		WORLD.ResetTopLayer ();

		LEFT_SIDE_PANEL.SetSliderMaxValues();

		RIGHT_SIDE_PANEL.UpdateDifficulty(Game.control.stageHandler.difficultyAsString);
		if(fadeFromBlack) EffectOverlay("Black", false, 1);
        else EffectOverlay("White", false, 1);
	}

	void Awake(){
		ToggleLoadingScreen(false);
        TogglePauseMenu(false);
		ToggleOptions(false);
	}

    /////////////////////////////////////////
    // MENUS 
    /////////////////////////////////////////
	public void TogglePauseScreen(bool toggle){
		pauseScreen.SetActive(toggle);
        TogglePauseMenu(toggle);
	}

    public void TogglePauseMenu(bool toggle){
        pauseMenuOptions.SetActive(toggle);
    }

    public override void UpdateMenu(string context, List<string> items){
        if(context == "PauseMenu")       allMenuSelections = pauseMenuOptions.transform.GetComponentsInChildren<TextMeshProUGUI> ();
        if(context == "OptionsMenu")    {
            allMenuSelections = optionsLabels.transform.GetComponentsInChildren<TextMeshProUGUI> ();
            optionsValuesTexts = optionsValues.transform.GetComponentsInChildren<TextMeshProUGUI> ();
			foreach (TextMeshProUGUI text in optionsValuesTexts) text.fontStyle = TMPro.FontStyles.Normal;
        } 
        if(context == "GameOverMenu"){   allMenuSelections = GAMEOVER.gameOverOptionsContainer.transform.GetComponentsInChildren<TextMeshProUGUI> (); }
        if(context == "SaveScorePrompt") allMenuSelections = GAMEOVER.saveScoreContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();

        InitMenuTexts(context, items);
    }
	
    /////////////////////////////////////////
    // FX OVERLAY
    /////////////////////////////////////////

	public void EffectOverlay(string color, bool fadeIn, float fadeTime){
        fadeOver = false;
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
        fadeOver = true;
		yield return null;
	}


    /////////////////////////////////////////
    // TOAST
    /////////////////////////////////////////

	public void PlayStageToast(){
		STAGETOAST.gameObject.SetActive(true);
		STAGETOAST.Play();
	}

	public void PlayToast(string text){
        toast.text = text;
        IEnumerator toastRoutine = PlayToast();
        StartCoroutine(toastRoutine);
	}

	IEnumerator PlayToast(){
        toastPlaying = true;
        toast.gameObject.SetActive (true);
        yield return new WaitForSeconds (1f);
        toast.gameObject.SetActive (false);
        toastPlaying = false;
	}

}
