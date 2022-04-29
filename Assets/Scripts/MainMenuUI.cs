using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
	public TextMeshProUGUI[] menuTexts;

   	public void InitMainMenu(){
        gameObject.SetActive(true);
        menuTexts[0].fontStyle = FontStyles.Bold;
	}

    public void UpdateMenuSelection(int index){
		foreach (TextMeshProUGUI textObject in menuTexts) {
			textObject.fontStyle = FontStyles.Normal;
		}
		menuTexts[index].fontStyle = FontStyles.Bold;
	}
}
