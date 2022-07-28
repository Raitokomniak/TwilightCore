using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage4 : Stage
{
	void Awake(){
		stageName = "Celestial Lotus Garden";
		bgmName = "????";
		stageindex = 4;

        lib = Game.control.vectorLib;
		scene = Game.control.scene;
		stage = Game.control.stageHandler;
        fadeFromWhite = false;
		UpdateStageInfoToUI();
        Game.control.enemySpawner.holdTimer = true;
		InitWaves(stage.difficultyMultiplier);
	}
	
	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}

	IEnumerator StageHandlerRoutine(){
      
       // Game.control.player.DebugFillCores();

        Game.control.stageUI.EffectOverlay("Black", false, 2.5f);

        scene.SetPlaneSpeed (2f);
        Game.control.stageUI.WORLD.SetTopLayerSpeed(5);
		Game.control.stageUI.PlayStageToast();

       // while (stage.stageTimer < Game.control.enemySpawner.midBossWave.spawnTime - 1) yield return null;
      //  while (stage.midBossOn) yield return null;
      
      //
      /*
       while(Game.control.dialog.handlingDialog) {
			yield return null;
		}*/

        //Game.control.sound.PlayMusic ("Stage", 4);




        // BOSS //
        
        while (stage.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
        Game.control.stageUI.WORLD.SetTopLayerSpeed(1);
        scene.SetPlaneSpeed (5f);
		Game.control.dialog.StartDialog ("Boss4_0");

        while(Game.control.dialog.handlingDialog) {
			//if(stage.stageTimer > 0) break;
			//else 
            yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 4);

        Game.control.stageHandler.ToggleTimer(true);

		
		while(!Game.control.enemySpawner.bossWave.dead) yield return null;

        Game.control.sound.FadeOutMusic();
        Game.control.sound.StopLoopingEffects();
        
		Game.control.stageHandler.ToggleTimer(false);

		yield return new WaitUntil(() => Game.control.stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		Game.control.dialog.StartDialog ("Boss4_1");
		while (Game.control.dialog.handlingDialog) yield return null;
		
		//FORCE MOVE PLAYER TO THE PORTAL HERE
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



		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 0f, 800, true, 3);
		boss.SetUpBoss (4, "Tridevi of the Lotus Garden", false);
		stage.NewWave (boss);

/*
        mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("L4")});
		p = new P_SingleHoming();
		p.SetSprite ("BigCircle", "Big", "Red", "Huge");
		stage.NewWave (new Wave (1f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));

        mp = new EnemyMovementPattern(lib.GetVector("A1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("D3", 1), new WayPoint("R4")});
		p = new P_SingleHoming();
		p.SetSprite ("BigCircle", "Big", "Red", "Huge");
		stage.NewWave (new Wave (3f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));

        mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 3), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Purple", "Small");	 
		stage.NewWave (new Wave (6f, mp, p, 1, false, 40, 3f / difficultyMultiplier, "gand_sitar"));

        mp = new EnemyMovementPattern(lib.GetVector("F1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 3), new WayPoint("L4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Yellow", "Small");	 
		stage.NewWave (new Wave (8f, mp, p, 1, false, 40, 3f / difficultyMultiplier, "gand_sitar"));

        mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 3), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Blue", "Small");	 
		stage.NewWave (new Wave (10f, mp, p, 1, false, 40, 3f / difficultyMultiplier, "gand_sitar"));
*/

	}
}
