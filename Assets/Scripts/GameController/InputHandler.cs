using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    bool skipping;

    public string skipContext;

    float holdTimer;
    float holdTime = .5f;

    float skipTimer;
    float skipTime = .1f;


    void LateUpdate(){
        if(!AllowSkip()) return;

        if(skipping){
            if(skipTimer<= skipTime) skipTimer+= Time.deltaTime;
            else {
                skipTimer = 0;
                GoNext();
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
    }

    void GoNext(){
        if(skipContext == "Intro") Game.control.stageHandler.intro.NextPara();
        if(skipContext == "Dialog") Game.control.dialog.AdvanceDialog(); 
        if(skipContext == "Bonus") Game.control.stageUI.STAGEEND.waitTime = .1f;
    }

    bool AllowSkip(){
        if(Game.control.stageHandler.intro.introOn) return true;
        if(Game.control.dialog.handlingDialog) return true;
        if(Game.control.stageHandler.countingStageEndBonuses) return true;
        return false;
    }
}
