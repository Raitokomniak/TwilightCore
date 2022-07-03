using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_MAINMENU : UI
{
	public Transform mainMenuPanel;
	public Transform difficultyPanel;
	public GameObject difficultyContainer;
	public GameObject scorePanel;


   	public override void InitMainMenu(){
        gameObject.SetActive(true);
		ToggleMainMenu(true);
	}

    public override void UpdateMenu(string context, List<string> items){
        if(context == "MainMenu") allMenuSelections = mainMenuPanel.GetComponentsInChildren<TextMeshProUGUI>();
		else if(context == "DifficultyMenu") allMenuSelections = difficultyContainer.GetComponentsInChildren<TextMeshProUGUI>();
		else if(context == "OptionsMenu") {
            allMenuSelections = optionsLabels.GetComponentsInChildren<TextMeshProUGUI>();
            optionsValuesTexts = optionsValues.transform.GetComponentsInChildren<TextMeshProUGUI> ();
			foreach (TextMeshProUGUI text in optionsValuesTexts) text.fontStyle = TMPro.FontStyles.Normal;
        }  

        InitMenuTexts(context, items);
    }

	public void ToggleMainMenu(bool toggle){
        mainMenuPanel.gameObject.SetActive(toggle);

        if(toggle){
            difficultyPanel.gameObject.SetActive(false);
            ToggleOptions(false);
            scorePanel.SetActive(false);
            tutorial.gameObject.SetActive(false);
            mainMenuPanel.GetComponentsInChildren<TextMeshProUGUI>()[0].fontStyle = FontStyles.Bold;
            mainMenuPanel.gameObject.SetActive(true);
        }
	}


	public void ToggleDifficultySelection(bool toggle){
		difficultyPanel.gameObject.SetActive(toggle);
		mainMenuPanel.gameObject.SetActive(!toggle);
	}

	public void ToggleScorePanel(bool toggle){
        Game.control.io.LoadScore();
        scorePanel.GetComponent<ScorePanel>().Activate();
		mainMenuPanel.gameObject.SetActive(!toggle);
		scorePanel.SetActive(toggle);
	}

}
