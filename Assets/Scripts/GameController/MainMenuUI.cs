using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
	public Transform mainMenuPanel;
	public Transform difficultyPanel;
	public GameObject optionsPanel;
    public GameObject optionsContainer;
	public GameObject optionsValueContainer;

   	public void InitMainMenu(){
        gameObject.SetActive(true);
		ToggleMainMenu(true);
	}

	public void UpdateMenuSelection(string context, int index){
		TextMeshProUGUI[] menuTexts = null;

		if(context == "MainMenu") menuTexts = mainMenuPanel.GetComponentsInChildren<TextMeshProUGUI>();
		else if(context == "DifficultyMenu") menuTexts = difficultyPanel.GetComponentsInChildren<TextMeshProUGUI>();
		else if(context == "OptionsMenu") menuTexts = optionsContainer.GetComponentsInChildren<TextMeshProUGUI>();

		foreach (TextMeshProUGUI textObject in menuTexts) {
			textObject.fontStyle = FontStyles.Normal;
		}
		menuTexts[index].fontStyle = FontStyles.Bold;
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
