﻿using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {
	public bool paused;
    public bool playerHitTimerOn = false;
    float playerHitTimer = 0f;
    float playerHitTime = 1f;

	void Awake () {
		paused = false;
	}

	public void HandlePause()
	{
        if(!Game.control.stageHandler.stageOn) return;

		if(!paused) Pause();
		else Unpause (true);
	}

    void Update(){
        if(playerHitTimerOn) {
            playerHitTimer += Time.unscaledDeltaTime;
            if(playerHitTimer > playerHitTime) {
                EndPlayerHitPause();
            }
        }
    }

    public bool AllowPause(){
        if(playerHitTimerOn) return false;
		if(Game.control.menu.menuOn) return false;
		if(Game.control.mainMenuUI != null) return false;
		if(!Game.control.stageHandler.stageOn) return false;
		return true;
    }

    public void PlayerHitPause(){
        Game.control.player.movement.ShowHitBox(true);
        playerHitTimer = 0;
        playerHitTimerOn = true;
        SetTimeScale(false);
    }

    public void EndPlayerHitPause(){
        if(paused) return;
        
        SetTimeScale(true);
        Game.control.player.movement.ShowHitBox(false);
        Game.control.enemySpawner.DestroyAllProjectiles ();
        //Game.control.enemySpawner.DestroyAllPickUpPoints(); // DON'T KNOW IF SHOULD KEEP THIS OR NOT
        if(!Game.control.stageHandler.bossOn && !Game.control.stageHandler.midBossOn) 
            Game.control.enemySpawner.DestroyAllEnemies();
        playerHitTimerOn = false;
    }

	
	void Pause(){
		paused = true;
		Game.control.sound.PauseMusic();
        Game.control.sound.PauseEffects();
		Game.control.stageUI.TogglePauseScreen(true);
		SetTimeScale (false);
	}

	public void Unpause(bool resumeMusic){
		if(resumeMusic) {
            Game.control.sound.ResumeMusic();
            Game.control.sound.ResumeEffects();
        }
       
		paused = false;
		Game.control.stageUI.TogglePauseScreen(false);
		SetTimeScale (true);
	}

	public void SetTimeScale(bool normal)
	{
		if (normal) Time.timeScale = 1;
		else        Time.timeScale = 0;
	}
}
