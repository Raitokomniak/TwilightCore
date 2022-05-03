using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
	public List<TextMeshProUGUI> mainMenuTexts;
	public List<TextMeshProUGUI> difficultyMenuTexts;

	public Transform mainMenuPanel;
	public Transform difficultyPanel;
	public GameObject optionsPanel;
    public GameObject optionsContainer;

   	public void InitMainMenu(){
        gameObject.SetActive(true);
		ToggleMainMenu(true);
	}

	public void UpdateMenuSelection(string context, int index){
		List<TextMeshProUGUI> menuTexts = new List<TextMeshProUGUI>();
		if(context == "MainMenu") menuTexts = mainMenuTexts;
		else if(context == "DifficultyMenu") menuTexts = difficultyMenuTexts;

		foreach (TextMeshProUGUI textObject in menuTexts) {
			textObject.fontStyle = FontStyles.Normal;
		}
		menuTexts[index].fontStyle = FontStyles.Bold;
	}

	public void ToggleMainMenu(bool toggle){
		difficultyPanel.gameObject.SetActive(false);
		mainMenuTexts[0].fontStyle = FontStyles.Bold;
		mainMenuPanel.gameObject.SetActive(toggle);
	}

	public void ToggleDifficultySelection(bool toggle){
		difficultyPanel.gameObject.SetActive(toggle);
		mainMenuPanel.gameObject.SetActive(false);
	}

	public void ToggleOptions(){
		mainMenuPanel.gameObject.SetActive(false);
		optionsPanel.SetActive(true);
	}
}
