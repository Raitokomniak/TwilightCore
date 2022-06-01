using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : MonoBehaviour {
	public EnemyShoot enemy;
    public EnemyMovement enemyMove;
	public VectorLib vectorLib;

	public List<EnemyMovementPattern> movementPatterns;
	public List<Pattern> patterns;


	public float bossIndex;
	public bool ignoreDialog;
	public int difficultyMultiplier;


	int bossPhase;
	public bool superPhase;
	IEnumerator nextPhaseRoutine;
	public IEnumerator phaseExecuteRoutine;
	bool start = true;
	public bool routineOver = true;
	public bool endOfPhase;


	public bool timerOn;
	public bool timerDone;
	public float phaseTimer;
	public float phaseTimeCap;


	public void Init(){
		enemy = GetComponent<EnemyShoot>();
        enemyMove = GetComponent<EnemyMovement>();

		//lib = Game.control.enemyLib;
		vectorLib = Game.control.vectorLib;
		movementPatterns = new List<EnemyMovementPattern> ();
		patterns = new List<Pattern> ();
		bossPhase = -1;
	}

	public void ResetLists(){
		patterns = new List<Pattern>();
		movementPatterns = new List<EnemyMovementPattern>();
	//	lib = Game.control.enemyLib;
	}

	void LateUpdate(){
		if(timerOn) timerDone = PhaseTimer();
	}

	public bool PhaseTimer(){
		if(endOfPhase) {
			timerDone = true;
			StopTimer();
			return true;
		}
		if(phaseTimer < phaseTimeCap){
			phaseTimer+=Time.deltaTime;
			return false;
		}
		else {
			StopTimer();
			return true;
		}
	}

	void StopTimer(){
		timerOn = false;
	}
	
	public virtual void ExecutePhase(int phase, Phaser phaser){}

	public virtual void StopCoro(){
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		routineOver = true;
	}

	public void StopPats(){
		foreach(Pattern p in patterns){
			p.StopPattern();
			if(p.routine != null) StopCoroutine(p.routine);
			if(p.animation){
				if(!p.animation.GetComponent<BulletAnimationController>().dontDestroy) 
					Destroy(p.animation);//
				else p.animation.GetComponent<BulletAnimationController>().stop = true;
			}
		}
	}

	public void InterruptPreviousPhase(){
		endOfPhase = true;
		StopPats();
		StopCoro();
		foreach(Pattern p in patterns){ p.StopPattern(); }
		routineOver = true;
	}

	public void NextPhase() {
		
		nextPhaseRoutine = PhasingTime();
		StartCoroutine(nextPhaseRoutine);
	}

	IEnumerator PhasingTime() {
		if(!start) InterruptPreviousPhase();
		else start = false;
		GetComponent<EnemyLife>().SetInvulnerable(true);
		if(!ignoreDialog) yield return new WaitUntil (() => Game.control.dialog.handlingDialog == false);
		yield return new WaitUntil (() => routineOver == true);
		yield return new WaitForSeconds(.5f);
		bossPhase++;
		routineOver = false;
		endOfPhase = false;
		GetComponent<EnemyLife>().SetInvulnerable(false);
		ExecutePhase (bossPhase, this);
		Game.control.ui.WORLD.UpdateTopPlayer ("Boss" + bossIndex + "_" + (bossPhase));
	}
}
