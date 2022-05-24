using UnityEngine;
using System.Collections;
using System.IO;

public class DialogController : MonoBehaviour {

	public ArrayList lineList;
	public bool handlingDialog;
	int lineIndex;
	bool endOfDialogueChain;
	public bool autoScroll;

	public bool playerActiveSpeaker;
	IEnumerator dialogRoutine;
	float autoScrollTimer = 0;
	float autoScrollTime = 3.5f;
	bool advanceDialogTrigger;
	

	void LateUpdate () {
		if(!Game.control.pause.paused && handlingDialog){
			if(autoScroll){

				if(autoScrollTimer < autoScrollTime)
					autoScrollTimer += Time.deltaTime;
				else
					AdvanceDialog();
			}

			if (Input.GetKeyDown (KeyCode.Z)){
				AdvanceDialog();
			}
		}
	}

	void AdvanceDialog(){
		advanceDialogTrigger = true;
		autoScrollTimer = 0;
	}

	public void ToggleAutoScroll(bool toggle){
		autoScroll = toggle;
		if(Game.control.mainMenuUI == null) Game.control.ui.DIALOG.UpdateAutoScrollInfo(autoScroll);
	}

	public void Init(){
		lineIndex = 0;
		handlingDialog = false;
		ToggleAutoScroll(true); //MAKE THIS AN OPTION
	}


	public void StartDialog(string _phase)
	{	
		Game.control.ui.DIALOG.ToggleDialog(true);
		
		if(!_phase.Contains("Boss")) Game.control.ui.DIALOG.InitPlayerSpeaker();
		if (_phase.Contains("Boss1")) { Game.control.ui.DIALOG.InitBossSpeaker("Boss1"); }
		if (_phase.Contains("Boss2")) { Game.control.ui.DIALOG.InitBossSpeaker("Boss2"); }

		TextAsset dialogueText = Resources.Load<TextAsset> ("DialogText/" + _phase);
		lineList = new ArrayList ();
		lineList.InsertRange(0, dialogueText.text.Split("\n" [0]));
		lineIndex = -1;

		handlingDialog = true;
		dialogRoutine = DialogRoutine();
		StartCoroutine(dialogRoutine);
	}
		

	IEnumerator DialogRoutine(){
		while(!endOfDialogueChain){
			GetDialog ();
			handlingDialog = true;
			yield return new WaitUntil(() => advanceDialogTrigger == true);
			advanceDialogTrigger = false;
		}

		EndDialog ();
	}

	public void GetDialog(){
		lineIndex++;
		ArrayList parsedLines = new ArrayList ();
		parsedLines.AddRange (lineList [lineIndex].ToString ().Split ("\t" [0]));

		Game.control.ui.DIALOG.UpdateDialog (parsedLines [0].ToString (), parsedLines [1].ToString ());

		if ((lineIndex + 1) == lineList.Count) {
			endOfDialogueChain = true;
			lineIndex = 0;
		}
	}

	public void EndDialog()
	{
		Game.control.ui.DIALOG.ToggleDialog(false);
		handlingDialog = false;
		endOfDialogueChain = false;
	}

}
