using UnityEngine;
using System.Collections;
using System.IO;

public class DialogController : MonoBehaviour {
	public TextAsset dialogueText;
	public ArrayList lines;
	public ArrayList lineList;
	public ArrayList chains;

	char delimiterChar;

	string phase;

	public bool handlingDialog;
	StreamReader reader;
	int localStoryProgression;
	string speaker;
	string text;
	int lineIndex;
	bool endOfDialogueChain;
	bool auto;

	bool init;
	IEnumerator autoDialog;

	
	// Update is called once per frame
	void Update () {
		if(handlingDialog){
			if (Input.GetKeyDown (KeyCode.Z) && !auto){
				StopCoroutine (autoDialog);
				CheckForDialogue ();	
			}
		}
	}

	public void InitDialog(){
		//Debug.Log ("init dialog");
		speaker = "";
		text = "";

		localStoryProgression = 0;
		lineIndex = 0;
		handlingDialog = false;
		init = true;


	}


	public void StartDialog(string _phase, float index, bool _auto)
	{
		
		auto = _auto;
//		Debug.Log ("start dialog " + _phase);
		if (!init)
			InitDialog ();
		
		phase = _phase;
		TextAsset dialogueText = Resources.Load<TextAsset> ("DialogText/" + _phase + index + "Dialog");
		handlingDialog = true;
		Game.control.ui.ToggleDialog(true);

		Game.control.ui.InitSpeakers ("Morale", _phase, index);
		if (_phase == "Boss") {
			switch(Mathf.RoundToInt(index))
			{
			case 1:
				Game.control.ui.UpdateBossInfo ("Maya", "Friendly Huldra");
				break;
			}
		}



		chains = new ArrayList ();
		delimiterChar = '=';

		chains.AddRange(dialogueText.text.Split(delimiterChar));
		lineList = new ArrayList ();
		lineList.InsertRange(0, chains[localStoryProgression].ToString().Split("\n" [0]));
		lineIndex = -1;
		CheckForDialogue();

	}
		
	public void GetDialogue(){
		lineIndex++;
		ArrayList parsedLines = new ArrayList ();
		parsedLines.AddRange (lineList [lineIndex].ToString ().Split ("\t" [0]));
		speaker = parsedLines [0].ToString ();
		text = parsedLines [1].ToString ();

		Game.control.ui.UpdateDialog (speaker, text);

		if ((lineIndex + 1) == lineList.Count) {
			endOfDialogueChain = true;
			lineIndex = 0;
		}
		//EndDialog ();

		//CheckForDialogue ();
		autoDialog = AutoDialog ();
		StartCoroutine(autoDialog);
	}

	void CheckForDialogue(){
		//EndDialog ();
		//Debug.Log(endOfDialogueChain);
		if (!endOfDialogueChain) {
			GetDialogue ();
			Game.control.ui.ToggleDialog (true);
			handlingDialog = true;
		} else {
			Game.control.ui.ToggleDialog(false);
			EndDialog ();
		}
	}


	IEnumerator AutoDialog()
	{
//		Debug.Log ("autodialog " + auto);
		yield return new WaitForSeconds (1);
		if(!auto) yield return new WaitForSeconds (2);
		CheckForDialogue ();
	}

	public void EndDialog()
	{
		//reader.Close();
		Game.control.ui.ToggleDialog(false);
		handlingDialog = false;
		endOfDialogueChain = false;
	}

}
