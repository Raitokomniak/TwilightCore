using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : MonoBehaviour {
	public int difficultyMultiplier;
	public EnemyShoot enemy;
    public EnemyMovement enemyMove;
	public List<EnemyMovementPattern> movementPatterns;
	public List<Pattern> patterns;
	public bool endOfPhase;

	//MAKE ARRAY
	//public Pattern p1, p2, p3, p4, p5;
	//MAKE ARRAY
	//public EnemyMovementPattern mp1, mp2, mp3, mp4, mp5;
	public IEnumerator numerator;

	Phaser boss;
	
	public bool superPhase;

	Wave wave;

	int bossPhase;

	IEnumerator phasingTime;
	public EnemyLib lib;

	bool start = true;

	public bool routineOver = true;

	public bool timerOn;
	public bool timerDone;
	public float phaseTimer;
	public float phaseTimeCap;


	void Awake(){
		enemy = GetComponent<EnemyShoot>();
        enemyMove = GetComponent<EnemyMovement>();
	}
	public void Init(float bossIndex, Wave _wave){
		
		lib = Game.control.enemyLib;
		movementPatterns = new List<EnemyMovementPattern> ();
		patterns = new List<Pattern> ();
		bossPhase = -1;
//		Debug.Log("phaser awake");

		wave = _wave;
		if(bossIndex == 0.5f) boss = gameObject.AddComponent<Boss05>();
		if(bossIndex == 1) boss = gameObject.AddComponent<Boss1>();

		boss.difficultyMultiplier = Game.control.stage.difficultyMultiplier;
	}


	void LateUpdate(){
		if(timerOn) timerDone = PhaseTimer();
	}

	public bool PhaseTimer(){
	//	Debug.Log("COUNTING");
		if(endOfPhase) {
			Debug.Log("END OF PHASE INTERRUPT TIMER");
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

	public void WaitForSecondsFloat(float _phaseTimeCap){
		timerOn = false;
		if(!endOfPhase) timerDone = false;
		phaseTimeCap = _phaseTimeCap;
		phaseTimer = 0;
		timerOn = true;
	}
	
	public virtual void ExecutePhase(int phase, Phaser phaser){}

	public virtual void StopCoro(){
		
	}

	public void InterruptPreviousPhase(){
		endOfPhase = true;
		boss.StopCoro();

		foreach(Pattern p in boss.patterns){
			p.StopPattern();
		}

		routineOver = true;
	}

	public void NextBossPhase() {
		phasingTime = PhasingTime();
		StartCoroutine(phasingTime);
	}

	IEnumerator PhasingTime() {
		if(!start) InterruptPreviousPhase();
		else start = false;

		GetComponent<EnemyLife>().SetInvulnerable(true);
		yield return new WaitUntil (() => Game.control.dialog.handlingDialog == false);
		yield return new WaitUntil (() => routineOver == true);
		yield return new WaitForSeconds(.5f);
		bossPhase++;
		routineOver = false;
		endOfPhase = false;
		GetComponent<EnemyLife>().SetInvulnerable(false);
		boss.ExecutePhase (bossPhase, this);
		Game.control.ui.UpdateTopPlayer ("Boss" + wave.bossIndex + "_" + (bossPhase));
	}
}
