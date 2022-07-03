using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public UI_LoadingScreen LOADING_SCREEN;

    public TextMeshProUGUI[] allMenuSelections;

	public GameObject optionsScreen;
    public GameObject optionsLabels;
	public GameObject optionsValues;
    public TextMeshProUGUI[] optionsValuesTexts = null;


    public TutorialScreen tutorial;

   	public void ToggleLoadingScreen(bool toggle){
		LOADING_SCREEN.gameObject.SetActive(toggle);
        if(toggle) LOADING_SCREEN.ShowLoadingScreen();
	}

    
    public void ToggleTutorial(bool toggle){
        tutorial.gameObject.SetActive(toggle);
        tutorial.tutorialOn = toggle;
    }

    public void ToggleOptions(bool toggle){
		optionsScreen.SetActive(toggle);
	}

    public virtual void UpdateMenu(string context, List<string> items){}
    public void UpdateMenuSelection(string context, int index){
        foreach (TextMeshProUGUI textObject in allMenuSelections) textObject.fontStyle = FontStyles.Normal;
        if(context == "OptionsMenu") foreach (TextMeshProUGUI text in optionsValuesTexts) text.fontStyle = TMPro.FontStyles.Normal;

        allMenuSelections[index].fontStyle = TMPro.FontStyles.Bold;
		if(context == "OptionsMenu") optionsValuesTexts[index].fontStyle = TMPro.FontStyles.Bold;
    }

    public void InitMenuTexts(string context, List<string> items){
        for(int i = 0; i < items.Count; i++){
            allMenuSelections[i].fontStyle = FontStyles.Normal;
            allMenuSelections[i].text = items[i];
        }
        UpdateMenuSelection(context, 0);
    }
    public virtual void InitMainMenu(){}

	public void UpdateOptionSelection(int index, string text){
		TextMeshProUGUI option = optionsValues.transform.GetChild(index).GetComponent<TextMeshProUGUI>();
		option.text = text;
	}
}
