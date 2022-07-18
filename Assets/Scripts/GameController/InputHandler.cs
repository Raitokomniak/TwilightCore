using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    KeyCode CONFIRM = KeyCode.Z;
    KeyCode[] BACK = {KeyCode.X, KeyCode.Escape};
    KeyCode[] ARROW = {KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow};
    KeyCode[] WASD =  {KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S};

    KeyCode SHOOT = KeyCode.Z;
    KeyCode SPECIAL = KeyCode.X;
    KeyCode FOCUS = KeyCode.LeftShift;

    bool skipping;

    public string skipContext;

    float holdTimer;
    float holdTime = .5f;

    float skipTimer;
    float skipTime = .1f;

    bool keyUp = false;

    //move
    float hor;
    float ver;

        
    bool AllowgGeneralInput(){
		if(Game.control.loading) return false;
		return true;
	}

    void LateUpdate(){
        CheckDialog();
        CheckSkipping();
        CheckStageEnd();
        CheckIntro();
        CheckTutorial();
        CheckSaveScore();
        //combat
        if(!AllowCombatInput()) return;
        CheckMovement();
        CheckShoot();
        CheckSpecial();
    }

    /// COMBAT ///
    bool AllowCombatInput(){
        if(!Game.control.stageHandler.stageOn) return false;
        return true;
    }

    public void CheckMovement(){
        if(!AllowgGeneralInput()) return;
        if(!Game.control.player.movement.AllowMovement()) return;
        Game.control.player.movement.FocusMode(Input.GetKey(FOCUS));
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
    
        Game.control.player.movement.Move (hor, ver);
    }

    void CheckShoot(){
        if(!Game.control.player.combat.CanShoot()) return;
        if(Input.GetKey(SHOOT)) Game.control.player.combat.Shoot();
    }


    void CheckSpecial(){
        if(!Game.control.player.special.CanUseSpecial()) return;
        if(Input.GetKeyDown(SPECIAL)) Game.control.player.special.CheckUsedSpecial();
    }


    /// GAMEOVER ///
    void CheckSaveScore(){
        if(!Game.control.stageUI.GAMEOVER.saveScoreOn) return;
        if(!AllowgGeneralInput()) return;
		if(Input.GetKeyDown(CONFIRM)) Game.control.stageUI.GAMEOVER.SendScore();
    }

    /// MENU ///

    public bool AllowMenuInput(){
        if(Game.control.ui.tutorial.tutorialOn) return false;
		if(!Game.control.menu.menuOn) return false;
		if(Game.control.loading) return false;
		return true;
    }

    public string CheckMenuInput(string returnValue){
		string input = "";
        string iType = "";

		if(Input.GetKeyDown (ARROW[2])) {    input = "up";       iType = "cursor"; }
		if(Input.GetKeyDown (ARROW[3])) {   input = "down";     iType = "cursor"; }
		if(Input.GetKeyDown (ARROW[1])) {  input = "right";    iType = "cursor"; }
		if(Input.GetKeyDown (ARROW[0])) {  input = "left";     iType = "cursor"; }

		if(Input.GetKeyDown (CONFIRM)) input = "confirm"; iType = "selection";
		if(Input.GetKeyDown(BACK[0]) || Input.GetKeyDown(BACK[1]))  input = "back"; iType = "selection";
        if(Input.GetKeyDown(BACK[1])) input = "pause"; iType = "selection";

        if(returnValue == "value") return input;
        if(returnValue == "type")  return iType;	

        return "";
	}

    /// TUTORIAL ///

    void CheckTutorial(){
        if(!Game.control.ui.tutorial.tutorialOn) return;
        if(Input.GetKeyDown(CONFIRM)) Game.control.ui.tutorial.NextImage();
        if(Input.GetKeyDown(BACK[0]) || Input.GetKeyDown(BACK[1])) Game.control.ui.tutorial.Abort();
    }

    /// INTRO ///

    void CheckIntro(){
        if(!Game.control.stageHandler.intro.introOn) return;
        if(Input.GetKeyDown(CONFIRM)) Game.control.stageHandler.intro.NextPara();
    }

    /// STAGE ///

    
    void CheckStageEnd(){
       if(!AllowStageAdvanceInput()) return;
       if(Input.GetKeyDown(CONFIRM)) Game.control.stageHandler.CheckStageEnd();	
    }
    public bool AllowStageAdvanceInput(){
		if(Game.control.loading) return false;
        if(!Game.control.stageHandler.CanAdvanceStage()) return false;
		return true;
	}

   
    /// DIALOG ///

    void CheckDialog(){
        if(!AllowDialogInput()) return;
			
		if (Input.GetKeyDown (CONFIRM)) {
			Game.control.sound.PlayMenuSound("Cursor");
			Game.control.dialog.AdvanceDialog();
		}
		
    }

    bool AllowDialogInput(){
		if(Game.control.loading) return false;
		if(Game.control.pause.paused) return false;
		if(!Game.control.dialog.handlingDialog) return false; 
		return true;
	}


    
    
    /// SKIPPING TEXT ///
     void CheckSkipping(){
        if(!SkipContext()) {
            keyUp = false;
            return;
        }
        if(!Input.GetKey(CONFIRM)) keyUp = true;
        if(Input.GetKeyUp(CONFIRM)) keyUp = true;

        if(!AllowSkip()) return;

        if(skipping){
            if(skipTimer<= skipTime) skipTimer+= Time.deltaTime;
            else {
                skipTimer = 0;
                GoNext();
            }
        }

        if(Input.GetKey(CONFIRM)){
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

    bool SkipContext(){
        if(Game.control.stageHandler.intro.introOn) return true;
        if(Game.control.dialog.handlingDialog) return true;
        if(Game.control.stageHandler.countingStageEndBonuses) return true;
        return false;
    }

    bool AllowSkip(){
        if(!keyUp) return false;
        return true;
    }
}
