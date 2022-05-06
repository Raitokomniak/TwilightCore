using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
	public StageHandler stageHandler;
	public IEnumerator stageHandlerRoutine;
    public Wave boss;

    void Awake(){
        stageHandler = Game.control.stageHandler;
        InitWaves(stageHandler.difficultyMultiplier);
		
    }
    public virtual void StopStage(){
       StopCoroutine(stageHandlerRoutine);
    }

    public virtual void StartStageHandler(){}
    public virtual void InitWaves(float difficultyMultiplier) {}
}
