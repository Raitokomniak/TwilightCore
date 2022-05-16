using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : MonoBehaviour {
	public EnemyShoot enemy;
    public EnemyMovement enemyMove;
	public EnemyLib lib;

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

		lib = Game.control.enemyLib;
		movementPatterns = new List<EnemyMovementPattern> ();
		patterns = new List<Pattern> ();
		bossPhase = -1;
	}

	public void ResetLists(){
		patterns = new List<Pattern>();
		movementPatterns = new List<EnemyMovementPattern>();
		lib = Game.control.enemyLib;
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

/*
	public void WaitForSecondsFloat(float _phaseTimeCap){
		timerOn = false;
		if(!endOfPhase) timerDone = false;
		phaseTimeCap = _phaseTimeCap;
		phaseTimer = 0;
		timerOn = true;
	}
	*/
	
	public virtual void ExecutePhase(int phase, Phaser phaser){}

	public virtual void StopCoro(){
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		routineOver = true;
	}

	public void InterruptPreviousPhase(){
		endOfPhase = true;
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
		Game.control.ui.UpdateTopPlayer ("Boss" + bossIndex + "_" + (bossPhase));
	}
}
