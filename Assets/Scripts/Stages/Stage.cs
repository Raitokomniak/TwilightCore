using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public string stageName;
    public string bgmName;
    public int stageindex;

	public StageHandler stageHandler;
    public SceneHandler scene;
	public IEnumerator stageHandlerRoutine;
    public Wave boss;

    public Pattern p;
	public EnemyMovementPattern mp;

    public EnemyLib lib = Game.control.enemyLib;

    public void UpdateStageInfoToUI(){
        Game.control.ui.RIGHT_SIDE_PANEL.UpdateStage(name);
		Game.control.ui.STAGETOAST.UpdateStageToastText (stageindex, name, bgmName);
    }
    public virtual void StopStage(){
       if(stageHandlerRoutine != null) StopCoroutine(stageHandlerRoutine);
       Destroy(this);
    }

    public virtual void StartStageHandler(){}
    public virtual void InitWaves(float difficultyMultiplier) {}
}
