using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    TextAsset introText;
    List<string> lineList;
    int lineIndex;

    public bool introOn;
    public bool introDone;

    
    public void Run(){
        Game.control.inputHandler.skipContext = "Intro";
        introOn = true;
        introDone = false;
        introText = Resources.Load<TextAsset> ("DialogText/IntroText");
		lineList = new List<string>();
		lineList.InsertRange(0, introText.text.Split("\n" [0]));
        lineIndex = 0;
        Game.control.stageUI.INTRO.Init();
        NextPara();
    }

    public void EndIntro(){
        introDone = true;
        introOn = false;
    }
    public void NextPara(){
        Game.control.stageUI.INTRO.ForceFadeIn();
        if(lineIndex >= lineList.Count) {
            introOn = false;
            Game.control.stageUI.INTRO.Hide();
            return;
        }
        Game.control.stageUI.INTRO.UpdateParagraph(lineList[lineIndex]);
        lineIndex++;
    }
}
