using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage5 : Stage
{
	void Awake(){
		stageName = "BRAHMA TEMPLE";
		bgmName = "????";
		stageindex = 5;
        fadeFrom = "Black";
        LateStageInit();
	}
	
	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}

	IEnumerator StageHandlerRoutine(){
      
       // Game.control.player.DebugFillCores();


        scene.SetPlaneSpeed (2f);
        Game.control.stageUI.WORLD.SetTopLayerSpeed(5);
		Game.control.stageUI.PlayStageToast();

/*
        // BOSS //
        
        while (stage.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
        Game.control.stageUI.WORLD.SetTopLayerSpeed(1);
        scene.SetPlaneSpeed (5f);
		Game.control.dialog.StartDialog ("Boss5_0");

        while(Game.control.dialog.handlingDialog) {
			//if(stage.stageTimer > 0) break;
			//else 
            yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 5);

        Game.control.stageHandler.ToggleTimer(true);

		
		while(!Game.control.enemySpawner.bossWave.dead) yield return null;

        Game.control.sound.FadeOutMusic();
        Game.control.sound.StopLoopingEffects();
        
		Game.control.stageHandler.ToggleTimer(false);
        

        */


		yield return new WaitUntil(() => Game.control.stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		Game.control.dialog.StartDialog ("Boss5_1");
		while (Game.control.dialog.handlingDialog) yield return null;
		


		yield return new WaitForSeconds(1f);
		Game.control.stageHandler.EndHandler ("StageComplete");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
		VectorLib lib = Game.control.vectorLib;
		//StageHandler stage = Game.control.stageHandler;
		stage.waves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;



        //DEBUG TO TEST NEW PATTERNS
/*
        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("C5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Shape(10, "ThreeCircles", 2);
		p.coolDown = 2;
		p.infinite = false;
        p.executeDelay = 1f;
        p.BMP = new BMP_Explode(p, 10);
        p.BMP.unFold = true;
        p.SetSprite ("Circle", "Glow", "BlackOrange", "Small");	 
		stage.NewWave (new Wave (1f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "asura"));
*/


	}
}
