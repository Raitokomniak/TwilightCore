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
	float autoScrollTime = 2.5f;
	bool advanceDialogTrigger;
	

	void LateUpdate () {
		if(handlingDialog){
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
		Game.control.ui.UpdateAutoScrollInfo(autoScroll);
	}

	public void Init(){
		lineIndex = 0;
		handlingDialog = false;
		ToggleAutoScroll(true); //MAKE THIS AN OPTION
	}


	public void StartDialog(string _phase)
	{	
		Game.control.ui.ToggleDialog(true);
		if(_phase.Contains("Boss")) Game.control.ui.InitSpeakers ("Soma", _phase);
		else Game.control.ui.InitSpeakers ("Soma", "");

		handlingDialog = true;

		if (_phase == "Boss1") {
			Game.control.ui.UpdateBossInfo ("Silvi", "Friendly Huldra");
		}

		TextAsset dialogueText = Resources.Load<TextAsset> ("DialogText/" + _phase);
		lineList = new ArrayList ();
		lineList.InsertRange(0, dialogueText.text.Split("\n" [0]));
		lineIndex = -1;

		dialogRoutine = DialogRoutine();
		StartCoroutine(dialogRoutine);
	}
		
	public void GetDialog(){
		lineIndex++;
		ArrayList parsedLines = new ArrayList ();
		parsedLines.AddRange (lineList [lineIndex].ToString ().Split ("\t" [0]));

		Game.control.ui.UpdateDialog (parsedLines [0].ToString (), parsedLines [1].ToString ());

		if ((lineIndex + 1) == lineList.Count) {
			endOfDialogueChain = true;
			lineIndex = 0;
		}
	}

	IEnumerator DialogRoutine(){
		while(!endOfDialogueChain){
			GetDialog ();
			Game.control.ui.ToggleDialog (true);
			handlingDialog = true;
			yield return new WaitUntil(() => advanceDialogTrigger == true);
			advanceDialogTrigger = false;
		}

		EndDialog ();
	}

	public void EndDialog()
	{
		Game.control.ui.ToggleDialog(false);
		handlingDialog = false;
		endOfDialogueChain = false;
	}

}
