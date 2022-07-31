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
    public string fadeFrom;

    //TOAST
	public TextMeshProUGUI toast;
    public List<string> toastQue;
    bool toastPlaying;

    //PAUSE SCREEN
	public GameObject pauseScreen;
	public GameObject pauseMenuOptions;


	public void InitStage(){
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
        FadeFrom(fadeFrom, 1.5f);
	}

    public void PrepareForFade(){
         EffectOverlay("Black");
    }

	void Awake(){
		ToggleLoadingScreen(false);
        TogglePauseMenu(false);
		ToggleOptions(false);
	}

    void Update(){
        if(toastQue.Count > 0 && !toastPlaying) {
            ExecuteToast();
        }
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

    public void FadeTo(string color, float fadeTime){
        fadeOver = false;
		EFFECT_OVERLAY.gameObject.SetActive(true);
		EFFECT_OVERLAY.color = new Color(1,1,1,1);
        IEnumerator animateRoutine = AnimateOverlay(true, fadeTime);
		StartCoroutine(animateRoutine);
    }

    public void FadeFrom(string color, float fadeTime){
        fadeOver = false;
		EFFECT_OVERLAY.gameObject.SetActive(true);
		if(color == "White") EFFECT_OVERLAY.color = new Color(1,1,1,1);
		else if(color == "Black") EFFECT_OVERLAY.color = new Color(0,0,0,1);
        IEnumerator animateRoutine = AnimateOverlay(false, fadeTime);
		StartCoroutine(animateRoutine);
    }

	public void EffectOverlay(string color){
		if(color == "White") EFFECT_OVERLAY.color = new Color(1,1,1,1);
		else if(color == "Black") EFFECT_OVERLAY.color = new Color(0,0,0,1);
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


    //for public reference only
	public void PlayToast(string text){
        if(toastQue == null) toastQue = new List<string>();
        if(toastQue.Count > 0 && toastQue[toastQue.Count - 1] == text) return;
        toastQue.Add(text);
	}

    void ExecuteToast(){
        
        toastQue.Reverse();
        string text = toastQue[toastQue.Count - 1];
        toastQue.RemoveAt(toastQue.Count - 1);
        toastQue.Reverse();
        IEnumerator toastRoutine = ToastRoutine(text);
        StartCoroutine(toastRoutine);
    }

	IEnumerator ToastRoutine(string text){
        toast.text = text;
        toastPlaying = true;
        toast.gameObject.SetActive (true);
        yield return new WaitForSeconds (1f);
        toast.gameObject.SetActive (false);
        toastPlaying = false;
	}

}
