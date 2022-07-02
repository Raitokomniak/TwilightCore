using UnityEngine;
using System.Collections;
using System.IO;

public class DialogController : MonoBehaviour {

	public ArrayList lineList;
	public bool handlingDialog;
	int lineIndex;
	bool endOfDialogueChain;
	public bool autoScroll;

	IEnumerator dialogRoutine;
	float autoScrollTimer = 0;
	float autoScrollTime = 3.5f;
	bool advanceDialogTrigger;
	

	void LateUpdate () {
		if(AllowInput()){
			if(autoScroll){

				if(autoScrollTimer < autoScrollTime)
					autoScrollTimer += Time.deltaTime;
				else
					AdvanceDialog();
			}

			if (Input.GetKeyDown (KeyCode.Z)) {
				Game.control.sound.PlayMenuSound("Cursor");
				AdvanceDialog();
			}
		}
	}

	bool AllowInput(){
		if(Game.control.loading) return false;
		if(Game.control.pause.paused) return false;
		if(!handlingDialog) return false; 
		return true;
	}
	void AdvanceDialog(){
		advanceDialogTrigger = true;
		autoScrollTimer = 0;
	}

	public void ToggleAutoScroll(bool toggle){
		autoScroll = toggle;
		if(Game.control.mainMenuUI == null) Game.control.stageUI.DIALOG.UpdateAutoScrollInfo(autoScroll);
	}

	public void Init(){
		lineIndex = 0;
		handlingDialog = false;
		ToggleAutoScroll(true); //MAKE THIS AN OPTION
	}


	public void StartDialog(string _phase)
	{	

		Game.control.stageUI.DIALOG.ToggleDialog(true);
		
		if(!_phase.Contains("Boss")) Game.control.stageUI.DIALOG.InitPlayerSpeaker();
		if (_phase.Contains("Boss1")) { Game.control.stageUI.DIALOG.InitBossSpeaker("Boss1"); }
		if (_phase.Contains("Boss2")) { Game.control.stageUI.DIALOG.InitBossSpeaker("Boss2"); }

		TextAsset dialogueText = Resources.Load<TextAsset> ("DialogText/" + _phase);
		lineList = new ArrayList ();
		lineList.InsertRange(0, dialogueText.text.Split("\n" [0]));
		lineIndex = -1;

		dialogRoutine = DialogRoutine();
		StartCoroutine(dialogRoutine);
	}
		

	IEnumerator DialogRoutine(){
		bool waitStart = true;
		while(!endOfDialogueChain){
			GetDialog ();
			handlingDialog = true;
			if(waitStart) { 
				yield return new WaitForSeconds(.5f); 
				waitStart = false;
			}

			yield return new WaitUntil(() => advanceDialogTrigger == true);
			advanceDialogTrigger = false;
		}

		EndDialog ();
	}

	public void GetDialog(){
		lineIndex++;
		ArrayList parsedLines = new ArrayList ();
		parsedLines.AddRange (lineList [lineIndex].ToString ().Split ("\t" [0]));

		Game.control.stageUI.DIALOG.UpdateDialog (parsedLines [0].ToString (), parsedLines [1].ToString ());

		if ((lineIndex + 1) == lineList.Count) {
			endOfDialogueChain = true;
			lineIndex = 0;
		}
	}

	public void EndDialog()
	{
		Game.control.stageUI.DIALOG.ToggleDialog(false);
		handlingDialog = false;
		endOfDialogueChain = false;
	}

}
