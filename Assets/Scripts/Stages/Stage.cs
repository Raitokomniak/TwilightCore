using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public DialogController dialog;
    public StageHandler stage;

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
        dialog = Game.control.dialog;
        stage = Game.control.stageHandler;
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
