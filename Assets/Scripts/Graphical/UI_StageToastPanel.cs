using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StageToastPanel : MonoBehaviour
{
    public TextMeshProUGUI stageText;

    public void UpdateStageToastText(int stageID, string stageName, string BGMtext)
	{
		stageText.text = "Stage " + stageID.ToString() + " - " + stageName + '\n' + "BGM: " + BGMtext;
	}

	public void Play(){
		IEnumerator stageText = StageText();
		StartCoroutine(stageText);
	}

	IEnumerator StageText()
	{
		//this.gameObject.SetActive (true);
		yield return new WaitForSeconds(3);
		this.gameObject.SetActive(false);
	}

}

