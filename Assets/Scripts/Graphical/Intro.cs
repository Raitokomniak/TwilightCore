using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    TextAsset introText;
    List<string> lineList;
    int lineIndex;

    bool introOn;
    public bool introDone;

    bool skipping;

    float holdTimer;
    float holdTime = .5f;

    float skipTimer;
    float skipTime = .1f;

    private void LateUpdate() {

        if(!introOn) return;

        if(skipping){
            if(skipTimer<= skipTime) skipTimer+= Time.deltaTime;
            else {
                skipTimer = 0;
                NextPara();
            }
        }

        if(Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Return)){
            if(holdTimer>=holdTime) skipping = true;
            else holdTimer+=Time.deltaTime;
        }
        else {
            holdTimer = 0;
            skipping = false;
        }
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            NextPara();
        
    }
    
    public void Run(){
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
