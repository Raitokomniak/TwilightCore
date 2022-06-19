using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : Stage
{
	void Awake(){
		stageName = "ASURA RIVERBANK";
		bgmName = "????";
		stageindex = 3;

        lib = Game.control.vectorLib;
		scene = Game.control.scene;
		stage = Game.control.stageHandler;
		UpdateStageInfoToUI();
		InitWaves(stage.difficultyMultiplier);
	}
	
	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}

	IEnumerator StageHandlerRoutine(){

		Game.control.player.DebugFillCores();

		while (stage.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;

        yield return null;

        Game.control.ui.WORLD.SetTopLayerSpeed(5);

		Game.control.dialog.StartDialog ("Boss3");
		Game.control.sound.PlayMusic ("Boss", 3);

		
		while(!Game.control.enemySpawner.bossWave.dead) yield return null;
		Game.control.stageHandler.ToggleTimer(false);

		yield return new WaitUntil(() => Game.control.stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		Game.control.dialog.StartDialog ("Boss3_1");
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

        //COOL ROTATING CIRCLE ARROW PATTERN
        mp = new EnemyMovementPattern(lib.GetVector("K8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (41f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "gand_flute"));


		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 2f, Mathf.CeilToInt(150 * difficultyMultiplier), true, 2);
		boss.SetUpBoss (3, "Danu, Mother of the Asura", false);
		stage.NewWave (boss);
	}
}
