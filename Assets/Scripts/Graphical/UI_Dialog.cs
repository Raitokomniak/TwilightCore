using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Dialog : MonoBehaviour
{
	public GameObject dialogPanel;
	public TextMeshProUGUI dialogContent;
	public Image dialogRightChar;
	public Image dialogLeftChar;

    public Transform bossInfoPanel;
	public TextMeshProUGUI bossDialogName;
	public TextMeshProUGUI bossDialogDescription;

	public TextMeshProUGUI autoScrollInfo;

    string content;
    IEnumerator rollText;
    public bool rollDone;


    public void ToggleDialog(bool value){
		dialogPanel.SetActive(value);
		bossDialogName.transform.parent.gameObject.SetActive (value);
	}

	public void InitPlayerSpeaker(){
		dialogRightChar.gameObject.SetActive (false);
		dialogLeftChar.gameObject.SetActive (false);

        bossInfoPanel.gameObject.SetActive(false);
	
		dialogRightChar.gameObject.SetActive (true);	
		dialogRightChar.sprite = Resources.Load<Sprite> ("Images/DialogCharacters/mainchar");
	}

    public void UpdateBossSpeaker(string speaker){
        bossInfoPanel.gameObject.SetActive(true);
        dialogLeftChar.gameObject.SetActive (true);

        UI_Dialog dui = Game.control.stageUI.DIALOG;
        string path = "Images/DialogCharacters/";
        string bossPath = "";

        if(speaker == "Maaya"){     bossPath = "Boss1";      dui.UpdateBossInfo (speaker, "Friendly Huldra"); }
        if(speaker == "Joanette"){  bossPath = "Boss2";      dui.UpdateBossInfo (speaker, "Void Spinner");  }
        if(speaker == "Danu"){      bossPath = "Boss3";      dui.UpdateBossInfo (speaker, "Daughter of Daksha");}
        if(speaker == "Tridevi"){   bossPath = "Boss4";      dui.UpdateBossInfo (speaker, "Emissary of Divinity");}
        if(speaker == "Saraswati"){ bossPath = "Boss4";      dui.UpdateBossInfo (speaker, "Emissary of Knowledge");}
        if(speaker == "Lakshmi"){   bossPath = "Boss4_1";    dui.UpdateBossInfo (speaker, "Emissary of Wealth");}
        if(speaker == "Parvati"){   bossPath = "Boss4_2";    dui.UpdateBossInfo (speaker, "Emissary of Harmony");}
        
        if(speaker == "???"){ Resources.Load<Sprite>     (path + "Boss4");      dui.UpdateBossInfo ("???", "");}

        if(bossPath != "") dialogLeftChar.sprite = Resources.Load<Sprite>       (path + bossPath);
    }

	public void UpdateAutoScrollInfo(bool autoScroll){
		if(autoScroll) autoScrollInfo.text = "Autoscroll: ON";
		else autoScrollInfo.text = "Autoscroll: OFF";
	}

	public void UpdateDialog(string speaker, string _content){
        rollDone = false;
        content = _content;
        StopAllCoroutines();
        rollText = RollDialogText(content);
        StartCoroutine(rollText);
		SetupSpeakers(speaker);
	}

    void SetupSpeakers(string speaker){
        if (speaker == "Soma") {
			if (dialogLeftChar.GetComponent<Image>().color.a != .3f) {
				dialogLeftChar.transform.position -= new Vector3 (10, 0, 0);
				dialogRightChar.transform.position -= new Vector3 (10, 0, 0);
			}
			dialogLeftChar.GetComponent<Image> ().color = new Color (1, 1, 1, .3f);
			dialogRightChar.GetComponent<Image> ().color = new Color (1, 1, 1, 1f);
		}
		else 
		{
            UpdateBossSpeaker(speaker);

			if (dialogRightChar.GetComponent<Image>().color.a != .3f) {
				dialogRightChar.transform.position += new Vector3 (10, 0, 0);
				dialogLeftChar.transform.position += new Vector3 (10, 0, 0);
			}
			dialogLeftChar.GetComponent<Image> ().color = new Color (1, 1, 1, 1);
			dialogRightChar.GetComponent<Image> ().color = new Color (1, 1, 1, .3f);

		}
    }

    IEnumerator RollDialogText(string _content){
        char[] contentChars = content.ToCharArray();
        WaitForSeconds wait = new WaitForSeconds(.03f);
        string contentString = "";

        for(int i = 0; i < contentChars.Length; i++){
            char c = contentChars[i];
            
            contentString += c;
            dialogContent.text = contentString;
            if(c != ' ' && i % 3 == 0) Game.control.sound.PlaySound("General", "Dialog", true);
            yield return wait;
        }

        rollDone = true;
        
        yield return null;
    }

    public void ForceContent(){
        StopAllCoroutines();
        dialogContent.text = content;
        rollDone = true;
    }

	public void UpdateBossInfo(string name, string description){
		bossDialogName.transform.parent.gameObject.SetActive (true);
		bossDialogName.text = name;
		bossDialogDescription.text = description;
	}
}
