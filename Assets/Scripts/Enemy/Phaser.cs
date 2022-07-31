using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : MonoBehaviour {
	public EnemyShoot shooter;
	public BossLife life;
    public EnemyMovement movement;
	public VectorLib vectorLib;

	public List<EnemyMovementPattern> movementPatterns;
	public List<Pattern> patterns;


	public float bossIndex;
	public bool ignoreDialog;
	public int difficulty;


	int bossPhase;
	public bool superPhase;
	IEnumerator nextPhaseRoutine;
	public IEnumerator phaseExecuteRoutine;
	bool start = true;
	public bool routineOver = true;
	public bool endOfPhase;

    public bool bossSurvivalBonus = true;
    public bool bossTimeBonus = true;

	public bool timerOn;
	public float phaseTimer;
	public float phaseTime;
    float phaseCountDownTick;
	public int numberOfPhases;

    public float topLayerWaitTime = 0;

	public void Init(){
		shooter = GetComponent<EnemyShoot>();
        movement = GetComponent<EnemyMovement>();
		vectorLib = Game.control.vectorLib;
		movementPatterns = new List<EnemyMovementPattern> ();
		patterns = new List<Pattern> ();
		life = GetComponent<BossLife>();
		bossPhase = -1;
	}

	public void ResetLists(){
		patterns = new List<Pattern>();
		movementPatterns = new List<EnemyMovementPattern>();
	}

	void LateUpdate(){
		if(timerOn && Game.control.stageHandler.stageOn) PhaseTimer();
	}

    public virtual void KeepTrackOfDamage(float d){}

    void CheckTimerSoundTick(){
        if(phaseTimer <= 6) {
            for(int i = 0; i < 6; i++){
                if(phaseCountDownTick != i){
                    if(Mathf.Approximately(i, Mathf.RoundToInt(phaseTimer))){
                        phaseCountDownTick = i;
                        Game.control.sound.PlaySound("Enemy", "CountDown", false);
                    }
                }
            }
        }
    }

	public void PhaseTimer(){
		if(phaseTimer > 0){
			if(!Game.control.pause.paused) 
                 phaseTimer-=Time.unscaledDeltaTime;
            else phaseTimer-=Time.deltaTime;
			Game.control.stageUI.BOSS.UpdateBossTimer(phaseTimer);
            CheckTimerSoundTick();
		}
		else {
            TimeOut();
		}
	}

    void TimeOut(){
        if(!Game.control.stageHandler.midBossOn && bossIndex != 4) 
                Game.control.stageHandler.DenyBossTimeBonus();
		if(!superPhase && bossIndex != 4)  life.SetHealthToThreshold();
		StopPhaseTimer();
		NextPhase();
    }

	public void StartPhaseTimer(float time){
        phaseCountDownTick = 0;
		endOfPhase = false;
		phaseTime = time;
		phaseTimer = phaseTime;
		Game.control.stageUI.BOSS.StartBossTimer(time);
		timerOn = true;
	}

	public void StopPhaseTimer(){
		Game.control.stageUI.BOSS.HideBossTimer();
		endOfPhase = true;
		timerOn = false;
	}
	
	public virtual void ExecutePhase(int phase, Phaser phaser){}

	public virtual void StopCoro(){
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		routineOver = true;
	}

	public void StopPats(){
		foreach(Pattern p in patterns){
			p.Stop();
			if(p.routine != null) StopCoroutine(p.routine);
			if(p.animation){
				if(!p.animation.GetComponent<SpriteAnimationController>().dontDestroy) 
					p.animation.gameObject.SetActive(false);
				else p.animation.GetComponent<SpriteAnimationController>().stop = true;
			}
		}
	}

	public void InterruptPreviousPhase(){
		endOfPhase = true;
		Game.control.stageUI.BOSS.HideSpell();
		StopPats();
		StopCoro();
		foreach(Pattern p in patterns){ p.Stop(); }
		routineOver = true;
	}

	public void NextPhase() {
		StopPhaseTimer();
		if(superPhase) life.NextHealthBar();
		if(bossPhase == numberOfPhases - 1) life.Die(false);
		else {
			life.DropLoot("Core"); //
			life.DropLoot("Core");
            if(bossPhase >=0 && bossTimeBonus) {
                Game.control.stageUI.PlayToast("Time Bonus!");
                Game.control.player.GainScore(3000);
                life.DropLoot("ExpPoint");
            }
            if(bossSurvivalBonus) {
                Game.control.stageUI.PlayToast("Survival Bonus!");
                Game.control.player.GainScore(3000);
                life.DropLoot("ExpPoint");
            }
			bossSurvivalBonus = true;
			nextPhaseRoutine = PhasingTime();
			StartCoroutine(nextPhaseRoutine);
		}
	}

	IEnumerator PhasingTime() {
		if(!start) InterruptPreviousPhase();
		else start = false;
		life.SetInvulnerable(true);
		if(!ignoreDialog) yield return new WaitUntil (() => Game.control.dialog.handlingDialog == false);
		yield return new WaitUntil (() => routineOver == true);
		yield return new WaitForSeconds(.5f);
		
		bossPhase++;
		routineOver = false;
		endOfPhase = false;
		
		life.SetInvulnerable(false);
		if(bossPhase > 0 && bossPhase % 2 != 0) superPhase = true;
		ExecutePhase (bossPhase, this);
        yield return new WaitForSeconds(topLayerWaitTime);
		Game.control.stageUI.WORLD.UpdateTopPlayer ("Boss" + bossIndex + "_" + (bossPhase));
	}
}
