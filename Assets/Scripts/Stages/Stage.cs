using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
	public StageHandler stageHandler;
	public IEnumerator stageHandlerRoutine;
    public Wave boss;

    public Pattern p;
	public EnemyMovementPattern mp;

    public EnemyLib lib = Game.control.enemyLib;

    void Awake(){
        stageHandler = Game.control.stageHandler;
        InitWaves(stageHandler.difficultyMultiplier);
		
    }
    public virtual void StopStage(){
       if(stageHandlerRoutine != null) StopCoroutine(stageHandlerRoutine);
       Destroy(this);
    }

    public virtual void StartStageHandler(){}
    public virtual void InitWaves(float difficultyMultiplier) {}
}
