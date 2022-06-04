using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : Stage
{
	void Awake(){
		stageName = "Asura's Path";
		bgmName = "Asura who remain Asura";
		stageindex = 1;

		scene = Game.control.scene;
		//stageHandler = Game.control.stageHandler;
		UpdateStageInfoToUI();
		InitWaves(stageHandler.difficultyMultiplier);
	}

	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}


	IEnumerator StageHandlerRoutine(){
		scene.SetPlaneSpeed(50f);
		while (dialog.handlingDialog) yield return null;
		dialog.StartDialog ("Stage1_0");

		while (stageHandler.stageTimer < 8f) yield return null;
		scene.SetPlaneSpeed (10f);
		scene.e_camera.Rotate (new Vector3(65, 0, 0));

		while (stageHandler.stageTimer < 14f) yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, -5));
		scene.SetPlaneSpeed (1f);
		while (stageHandler.stageTimer < 24f) yield return null;
		Game.control.ui.PlayStageToast();

		scene.e_camera.Move (new Vector3(50, 45, 72));
		scene.e_camera.Rotate (new Vector3(65, 0, 5));

		scene.SetPlaneSpeed (50f);
		while(stageHandler.stageTimer < 52f) yield return null;
		dialog.StartDialog ("Stage1_1");
		while (stageHandler.stageTimer < 55f) yield return null;
		
		while (!spawner.MidBossDead()) yield return null;
		yield return new WaitForSeconds (.5f);
		dialog.StartDialog ("Stage1_2");
		while (dialog.handlingDialog) yield return null;
		scene.SetPlaneSpeed (35f);

		while (stageHandler.stageTimer < boss.spawnTime - 1) yield return null;

		
		dialog.StartDialog ("Boss1");
		scene.SetPlaneSpeed (3f);

		while(dialog.handlingDialog) {
			if(stageHandler.stageTimer > 116.7f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 1);

		//USING THIS DISABLES SOME DEBUGGING BECAUSE STAGE DOESNT END IF BOSS DIES BEFORE THIS POINT
		//MAYBE FOR DEVVING RUN A SIMULTANEOUS ENDCHECKROUTINE THAT BYPASSES ALL THE TIMER CHECKS
		while(!spawner.bossWave.dead) yield return null;
		stageHandler.ToggleTimer(false);

		yield return new WaitUntil(() => stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		dialog.StartDialog ("Boss1_1");
		while (dialog.handlingDialog) yield return null;
		
		stageHandler.EndHandler ("StageComplete");
	}

	public override void InitWaves(float difficultyMultiplier) {
        VectorLib lib = Game.control.vectorLib;
		StageHandler stage = Game.control.stageHandler;
		stage.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;


		//PHASE 1

		mp = new EMP_EnterLeave(lib.topWallRightSide, .5f);
		mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveLeft);
		p = new P_SingleHoming();
		p.bulletMovement = new BMP_Explode(p, 0.5f, true);
		p.SetSprite ("Circle", "Big", "Red", "Big");
		stage.NewWave (new Wave (3f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, .5f);
		mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveLeft);
		p = new P_SingleHoming();
		p.bulletMovement = new BMP_Explode(p, 0.5f, true);
		p.SetSprite ("Circle", "Big", "Red", "Big");
		stage.NewWave (new Wave (5f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));
				
		
		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveRight);
		p = new P_SingleHoming();
		p.bulletMovement = new BMP_Explode(p, 0.5f, true);
		p.SetSprite ("Circle", "Big", "Red", "Big");
		stage.NewWave (new Wave (7f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));
																				// .6f
			
		//2ND PHASE

		if(difficultyMultiplier > 3){
			mp = new EMP_EnterLeave(lib.topWallRightSide, .5f);
			mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveRight);
			p = new P_SingleHoming();
			p.bulletMovement = new BMP_Explode(p, 0.5f, true);
			p.SetSprite ("Circle", "Big", "Red", "Big");	
			stage.NewWave (new Wave (8f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"));
				
			mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
			mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveLeft);
			p = new P_SingleHoming();
			p.bulletMovement = new BMP_Explode(p, 0.5f, true);
			p.SetSprite ("Circle", "Big", "Red", "Big");	
			stage.NewWave (new Wave (14f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"));
		}

		
		mp = new EMP_ZigZag(lib.topWallRightSide, 0, 1);
		mp.SetEnterLeaveDirection(new Vector3(0f, 8f, 0f), lib.leaveLeft);
		p = new P_Circle();
		p.bulletCount = Mathf.CeilToInt(2.8f * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	
		stage.NewWave (new Wave (16f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"));


		mp = new EMP_EnterLeave(lib.leftWallTopSide, 1);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (17f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"));

			
		//INTERLUDE -> PHASE3

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_Circle();
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (28f, mp, p, 5, false, 0,  3f / difficultyMultiplier,  "default"));
		
		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (31f, mp, p, 5, false, 0,  3f / difficultyMultiplier, "default"));
												 //31f
		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_Circle();
		p.bulletCount =  10;
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (33f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));


		mp = new EMP_EnterLeave(lib.leftWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.bulletCount =  10;
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (36f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));


		mp = new EMP_EnterLeave(lib.topWallRightSide, 4f);
		mp.speed = 2f;
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveRight);
		p = new P_Circle();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 11f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");
		stage.NewWave (new Wave (41f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"));


		mp = new EMP_EnterLeave(lib.topWallLeftSide, 4);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 11f, false);
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
		stage.NewWave (new Wave (41.5f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"));

		//MID-BOSS
		mp = new EMP_EnterFromTop();
		mp.SetEnterLeaveDirection(lib.enterCenterBoss, lib.GetVector("XY"));
		mp.stayTime = 23f;
		Wave midBoss = new Wave(mp, 55f, 150, false, 1, "boss0.5"); //55f
		midBoss.SetUpBoss (0.5f, "Asura", true);
		stage.NewWave (midBoss);



		//CONTD
		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (83f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (87f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveRight);
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_Explode(p, 7f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (90f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallRightSide, 4);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 11f, false);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (90f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 4);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 11f, false);
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");	 
		stage.NewWave (new Wave (90.5f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"));

/*
		//debug BOSS 1
		mp = new EMP_EnterFromTop();
		mp.SetEnterLeaveDirection(lib.enterCenterBoss, Vector3.zero);
		boss = new Wave(mp, 1f, 10 * Mathf.CeilToInt(difficultyMultiplier), true, 2, "boss1");
		boss.SetUpBoss (1, "Maaya, Forest Guardian", false);
		stage.NewWave (boss);
*/
	
		//BOSS 1
		mp = new EMP_EnterFromTop();
		mp.SetEnterLeaveDirection(lib.enterCenterBoss, Vector3.zero);
		float health = 0;
		if(difficultyMultiplier < 5) health = 120 * difficultyMultiplier;
		if(difficultyMultiplier >= 5) health = 70 * difficultyMultiplier;

		boss = new Wave(mp, 102f, Mathf.CeilToInt(health), true, 2, "boss1");
		boss.SetUpBoss (1, "Maaya, Forest Guardian", false);
		stage.NewWave (boss);
		
	}
}
