using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_StageToastPanel : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI stageSubTitle;
    public TextMeshProUGUI stageBGM;

    public void UpdateStageToastText(int stageID, string stageName, string stageSubtitle, string BGMtext)
	{
		stageText.text = stageName;
        stageSubTitle.text = stageSubtitle;
        stageBGM.text = BGMtext;
	}

	public void Play(){
		IEnumerator stageText = StageText();
		StartCoroutine(stageText);
	}

	IEnumerator StageText()
	{
		yield return new WaitForSeconds(4);
		gameObject.SetActive(false);
	}

}

