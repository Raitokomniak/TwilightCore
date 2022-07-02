using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{

    public EnemySpawner spawner = Game.control.enemySpawner;
    public DialogController dialog = Game.control.dialog;
    public StageHandler stage = Game.control.stageHandler;

    public string stageName;
    public string bgmName;
    public int stageindex;

	
    public EnvironmentHandler scene;
	public IEnumerator stageHandlerRoutine;
    public Wave boss;

    public Pattern p;
	public EnemyMovementPattern mp;

    public VectorLib lib;

    public void UpdateStageInfoToUI(){
        Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateStage(stageName);
		Game.control.stageUI.STAGETOAST.UpdateStageToastText (stageindex, stageName, bgmName);
    }
    public virtual void StopStage(){
       if(stageHandlerRoutine != null) StopCoroutine(stageHandlerRoutine);
       Destroy(this);
    }

    public virtual void StartStageHandler(){}
    public virtual void InitWaves(float difficultyMultiplier) {}
}
