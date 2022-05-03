using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
	public Transform mainMenuPanel;
	public Transform difficultyPanel;

	public GameObject difficultyContainer;

	public GameObject optionsPanel;
    public GameObject optionsContainer;
	public GameObject optionsValueContainer;

	TextMeshProUGUI[] menuTexts = null;

   	public void InitMainMenu(){
        gameObject.SetActive(true);
		ToggleMainMenu(true);
	}

	public void UpdateMenuSelection(string context, int index){

		if(context == "MainMenu") menuTexts = mainMenuPanel.GetComponentsInChildren<TextMeshProUGUI>();
		else if(context == "DifficultyMenu") menuTexts = difficultyContainer.GetComponentsInChildren<TextMeshProUGUI>();
		else if(context == "OptionsMenu") menuTexts = optionsContainer.GetComponentsInChildren<TextMeshProUGUI>();

		foreach (TextMeshProUGUI textObject in menuTexts) {
			textObject.fontStyle = FontStyles.Normal;
		}
		menuTexts[index].fontStyle = FontStyles.Bold;
	}

	public void UpdateAutoScrollInfo(bool autoScroll){
		if(autoScroll) menuTexts[0].text = "Autoscroll: ON";
		else menuTexts[0].text = "Autoscroll: OFF";
	}

	public void ToggleMainMenu(bool toggle){
		difficultyPanel.gameObject.SetActive(false);
		optionsPanel.SetActive(false);
		mainMenuPanel.GetComponentsInChildren<TextMeshProUGUI>()[0].fontStyle = FontStyles.Bold;
		mainMenuPanel.gameObject.SetActive(toggle);
	}

	public void ToggleDifficultySelection(bool toggle){
		difficultyPanel.gameObject.SetActive(toggle);
		mainMenuPanel.gameObject.SetActive(false);
	}

	public void ToggleOptions(bool toggle){
		mainMenuPanel.gameObject.SetActive(!toggle);
		optionsPanel.SetActive(toggle);
		//if(toggle) Game.control.menu.Menu("OptionsMenuMain");
	}

	
	public void UpdateOptionSelection(int index, string text){
		TextMeshProUGUI option = optionsValueContainer.transform.GetChild(index).GetComponent<TextMeshProUGUI>();
		option.text = text;
	}
}
