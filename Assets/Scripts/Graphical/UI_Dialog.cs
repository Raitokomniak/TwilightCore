using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Dialog : MonoBehaviour
{
    
	public GameObject dialogPanel;
	public Image dialogBG;
	public TextMeshProUGUI dialogName;
	public TextMeshProUGUI dialogContent;
	public Image dialogRightChar;
	public Image dialogLeftChar;
	public TextMeshProUGUI bossDialogName;
	public TextMeshProUGUI bossDialogDescription;

	public TextMeshProUGUI autoScrollInfo;

    public void ToggleDialog(bool value){
		dialogPanel.SetActive(value);
		bossDialogName.transform.parent.gameObject.SetActive (false);
	}

	public void InitPlayerSpeaker(){
		dialogRightChar.gameObject.SetActive (false);
		dialogLeftChar.gameObject.SetActive (false);
	
		dialogRightChar.gameObject.SetActive (true);	
		dialogRightChar.sprite = Resources.Load<Sprite> ("Images/DialogCharacters/mainchar");
	}

	public void InitBossSpeaker(string boss){
		bossDialogName.transform.parent.gameObject.SetActive (true);
		dialogLeftChar.gameObject.SetActive (true);
		dialogLeftChar.sprite = Resources.Load<Sprite> ("Images/DialogCharacters/" + boss);
		if(boss == "Boss1") Game.control.stageUI.DIALOG.UpdateBossInfo ("Maaya", "Friendly Huldra");
		if(boss == "Boss2") Game.control.stageUI.DIALOG.UpdateBossInfo ("Joanette", "Void Spinner");
	}

	public void UpdateAutoScrollInfo(bool autoScroll){
		if(autoScroll) autoScrollInfo.text = "Autoscroll: ON";
		else autoScrollInfo.text = "Autoscroll: OFF";
	}
	public void UpdateDialog(string speaker, string content){
		dialogContent.text = content;

		if (speaker == "Boss") {

			dialogBG.transform.localScale = new Vector3 (-1, 1, 1);

			if (dialogRightChar.GetComponent<Image>().color.a != .3f) {
				dialogRightChar.transform.position += new Vector3 (10, 0, 0);
				dialogLeftChar.transform.position += new Vector3 (10, 0, 0);
			}
			dialogLeftChar.GetComponent<Image> ().color = new Color (1, 1, 1, 1);
			dialogRightChar.GetComponent<Image> ().color = new Color (1, 1, 1, .3f);
		}
		else 
		{
			dialogBG.transform.localScale = new Vector3 (1, 1, 1);
			
			if (dialogLeftChar.GetComponent<Image>().color.a != .3f) {
				dialogLeftChar.transform.position -= new Vector3 (10, 0, 0);
				dialogRightChar.transform.position -= new Vector3 (10, 0, 0);
			}
			dialogLeftChar.GetComponent<Image> ().color = new Color (1, 1, 1, .3f);
			dialogRightChar.GetComponent<Image> ().color = new Color (1, 1, 1, 1f);
		}


	}

	public void UpdateBossInfo(string name, string description){
		bossDialogName.transform.parent.gameObject.SetActive (true);
		bossDialogName.text = name;
		bossDialogDescription.text = description;
	}
}
