using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : Stage
{
	void Awake(){
		stageName = "Forest of Void Rituals";
		bgmName = "?";
		stageindex = 2;

		scene = Game.control.scene;
		stageHandler = Game.control.stageHandler;
		UpdateStageInfoToUI();
		InitWaves(stageHandler.difficultyMultiplier);
	}
	

	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}

	IEnumerator StageHandlerRoutine(){
		
		scene.e_camera.SetPosition (new Vector3(80,45,72));
		scene.e_camera.SetRotation (new Vector3(65, 0, 5));

		while (stageHandler.stageTimer < 0.1f) yield return null;
		
		scene.SetPlaneSpeed (3f);

		while (stageHandler.stageTimer < 26.5f) yield return null;
		Game.control.ui.PlayStageToast();
		scene.e_camera.Move (new Vector3(65, 45, 72));
		scene.e_camera.Rotate (new Vector3(65, 0, 5));
		scene.SetPlaneSpeed (10f);


		while (stageHandler.stageTimer < 46f) yield return null;
		scene.SetPlaneSpeed (5f);
		scene.e_camera.Rotate (new Vector3(35, 0, 15));

		while (stageHandler.stageTimer < 50f) yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, -15));

		while (stageHandler.stageTimer < 52f)  yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, 15));

		while (stageHandler.stageTimer < 56f) yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, 5));
		
		scene.SetPlaneSpeed (10f);

		while (stageHandler.stageTimer < 83f) yield return null;
		scene.SetPlaneSpeed (1f);

		while (Game.control.dialog.handlingDialog) yield return null;
		while (stageHandler.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
		
		Game.control.dialog.StartDialog ("Boss2");

		while(Game.control.dialog.handlingDialog) {
			if(stageHandler.stageTimer > 125.9f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 2);

		while(!Game.control.enemySpawner.bossWave.dead) yield return null;
		Game.control.stageHandler.ToggleTimer(false);

		yield return new WaitUntil(() => Game.control.stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		Game.control.dialog.StartDialog ("Boss2_1");
		while (Game.control.dialog.handlingDialog) yield return null;
		
		//FORCE MOVE PLAYER TO THE PORTAL HERE
		Game.control.player.movement.ForceMove(lib.GetVector("X3"));
		Game.control.ui.EffectOverlay("White", true, 2.5f);
		yield return new WaitForSeconds(4f);
		Game.control.stageHandler.EndHandler ("StageComplete");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
		VectorLib lib = Game.control.vectorLib;
		StageHandler stage = Game.control.stageHandler;
		stage.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

/*
	SLERP TEST
		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0f);
		mp.SetEnterLeaveDirection(lib.rightWallTopSide, lib.leaveLeft);
		mp.smoothedMovement = true;
		mp.speed = 4f;
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red");	
		stage.NewWave (new Wave (1f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

*/

/*
		//PHASE 1

		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 8f, false);
		p.SetSprite ("Circle", "Glow", "Red", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = .5f;
		stage.NewWave (	new Wave (3f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "default"), 
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 8f, false);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = .5f;
		stage.NewWave (	new Wave (8f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "default"), 
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		//PHASE 2

		mp = new EMP_Snake(lib.leftWallTopSide, 0, 1);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	
		stage.NewWave (new Wave (13f, mp, p, 3, false, 0, 3f / difficultyMultiplier,  "default"));

		mp = new EMP_Snake(lib.rightWallTopSide, 0, -1);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (15f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.bulletMovement = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (18f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		//STAGE INTERLUDE
		//PHASE 3

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red", "Medium");	
		stage.NewWave (new Wave (30f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (35f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (40f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		mp.smoothedMovement = true;
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "White", "Medium");	
		stage.NewWave (new Wave (45f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (43f, mp, p, 1, false, 0,  3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 3f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		mp.smoothedMovement = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "Red", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (47f, mp, p, 1, false, 0, 3f / difficultyMultiplier, "default"));


		//PHASE 4
		// LASERS, SPIDERS, MAELSTROMS

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 10f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		mp.disableHitBox = true;
		mp.hideSpriteOnSpawn = true;
		p = new P_SpiderWebLaser();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		stage.NewWave (new Wave (56f, mp, p, 1, false, 100, 5, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.bulletMovement = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (60f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (64f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveLeft, lib.leaveRight});

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (68f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveLeft, lib.leaveRight});

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.smoothedMovement = true;
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Spider_Glow", "Small");
		p.bulletMovement = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (71f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Diamond", "Glow", "Red", "Small");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (75f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveLeft, lib.leaveRight});

		////////////////////////////////////////////////////////////////
		//VOID PORTALS
		// 1 portal 1 asura

		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(3f, 5, 6);
		p.delayBeforeAttack = 2f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 0, false);
		stage.NewWave (new Wave (80f, mp, p, 1, false, 5, 100, "default"), ///// 80f
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide}, 
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		
		mp = new EMP_EnterLeave(lib.enterCenter, 5);
		mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveRight); //REMEMBER SMOOTH CURVE AWAY????
		p = new P_Spiral(6 * (int)difficultyMultiplier);
		p.loopCircles =  288 * 4;
		p.rotationDirection = 1;
		p.delayBeforeAttack = 1f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 3f, false);
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");

		stage.NewWave(new Wave(85f, mp, p, 1, false, 3, 5 / difficultyMultiplier, "default")); ///// 85f


		////////////////////////////////////////////////////////
		//3 PORTALS 9 ASURAS
		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(9f, 5, 6);
		p.delayBeforeAttack = 2f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 0, false); 
		stage.NewWave (new Wave (90f, mp, p, 3, false, 5, 100, "default"),  ///// 93f
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		mp = new EMP_EnterLeave(lib.enterRight, 1);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveRight); //REMEMBER SMOOTH CURVE AWAY????
		p = new P_Spiral(3 * (int)difficultyMultiplier);
		p.loopCircles =  288 * 1;
		p.bulletMovement = new BMP_WaitAndExplode(p, 11f);
		p.bulletMovement.movementSpeed = 1f;
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");

		stage.NewWave(new Wave(95f, mp, p, 9, false, 3, 6 / difficultyMultiplier, "default"),  ///// 100
						new List<Vector3> { lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight + new Vector3(0, -3, 0), lib.leaveRight + new Vector3(0, -3, 0), lib.leaveLeft + new Vector3(0, -3, 0)});
		

		//TRAPPING VOIDS

		mp = new EMP_EnterLeave(lib.enterRight, 5f);
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(13f, 7, 6);
		p.delayBeforeAttack = 2f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 0, false);
		//p.SetSprite ("Circle", "Glow", "White");
		stage.NewWave (new Wave (105f, mp, p, 4, false, 5, 100, "default"), //SPAWNTIME 105
						new List<Vector3> {lib.enterRight, lib.enterLeft, lib.enterRightWallBotSide, lib.enterLeftWallBotSide},
						new List<Vector3>{lib.enterRight, lib.enterLeft, lib.enterRightWallBotSide, lib.enterLeftWallBotSide},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter, lib.leaveCenter});

*/

		//debug boss
		mp = new EMP_EnterFromTop();
		boss = new Wave(mp, 1f, 30, true, 2, "boss2");
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		stage.NewWave (boss);

		float health = 0;
		if(difficultyMultiplier < 5) health = 100 * difficultyMultiplier;
		if(difficultyMultiplier >= 5) health = 70 * difficultyMultiplier;


		mp = new EMP_EnterFromTop();
		boss = new Wave(mp, 114f, Mathf.CeilToInt(health), true, 2, "boss2");
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		stage.NewWave (boss);

	}
}
