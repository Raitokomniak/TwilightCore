using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public DialogController dialog = Game.control.dialog;
    public StageHandler stage = Game.control.stageHandler;

    public string stageName;
    public string stageSubtitle;

    public string bgmName;
    public int stageindex;
    public string fadeFrom;

    public EnvironmentHandler scene;
	public IEnumerator stageHandlerRoutine;
    public Wave boss;

    public VectorLib lib;

    public void UpdateStageInfoToUI(){
        Game.control.stageUI.fadeFrom = fadeFrom;
        Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateStage(stageName);
		Game.control.stageUI.STAGETOAST.UpdateStageToastText (stageindex, stageName, stageSubtitle, bgmName);
    }
    
    public virtual void StopStage(){
       if(stageHandlerRoutine != null) StopCoroutine(stageHandlerRoutine);
       Destroy(this);
    }

    public void LateStageInit(){
        lib = Game.control.vectorLib;
		scene = Game.control.scene;
		stage = Game.control.stageHandler;
		UpdateStageInfoToUI();
    }

    public virtual void StartStageHandler(){}
    public virtual void InitWaves(float difficultyMultiplier) {}
}
