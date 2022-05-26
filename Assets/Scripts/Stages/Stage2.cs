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
		
		scene.e_camera.SetPosition (new Vector3(50,20,72));
		scene.e_camera.SetRotation (new Vector3(50, 0, 5));

		while (stageHandler.stageTimer < 0.1f) yield return null;

		//scene.SetPlaneSpeed (10f);

		while (stageHandler.stageTimer < 24f) yield return null;
		Game.control.ui.PlayStageToast();
		scene.e_camera.Move (new Vector3(50, 0, 72));
		scene.e_camera.Rotate (new Vector3(25, 0, 5));
		scene.SetPlaneSpeed (10f);

		while (Game.control.dialog.handlingDialog) yield return null;
		while (stageHandler.stageTimer < 112f) yield return null;
		
		Game.control.dialog.StartDialog ("Boss2");

		while(Game.control.dialog.handlingDialog) {
			if(stageHandler.stageTimer > 125.9f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 2);

		while(!Game.control.enemySpawner.bossWave.dead) yield return null;
		
		Game.control.stageHandler.EndHandler ("StageComplete");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
		lib.stageWaves.Clear ();


/*
VOID PORTAL DEBUG
		mp = new EnemyMovementPattern (lib.enterLeave);
		mp = new EMP_EnterLeave();
		mp.SetEnterLeaveDirection(lib.enterCenter, lib.leaveLeft);
		mp.stayTime = 10f;
		mp.Customize("EnterDir", "Center");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 10f);
		//p = new P_VoidPortal();
		//p.SetSprite ("Circle", "Glow", "Red");
		p = new P_VoidPortal();
		//p.bulletMovement = new BulletMovementPattern (false, "Explode", 0.5f, p, 0, 14);
		p.bulletMovement = new BMP_Explode(p, 0.5f, false);
		p.SetSprite ("Circle", "Glow", "Red");
		lib.NewWave (new Wave (1f, mp, p, 3, false, 5, 10f / difficultyMultiplier, "default"), new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide });
*/


		//PHASE 1

		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.bulletMovement = new BMP_Explode(p, 8f, false);
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = .5f;
		lib.NewWave (	new Wave (3f, mp, p, 2, false, 0, 10f / difficultyMultiplier, "default"), 
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		mp = new EMP_EnterLeave(lib.topWallRightSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveRight);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (5f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (7f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "default"));


		//PHASE 2

		mp = new EMP_Snake(lib.leftWallTopSide, 1, 1);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (13f, mp, p, 2, false, 0, 3f / difficultyMultiplier,  "default"));

		mp = new EMP_Snake(lib.rightWallTopSide, 1, -1);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (15f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.circleDelay= 1f;
		p.SetSprite ("Spider");
		p.bulletMovement = new BMP_SlowWaving(p, 9f);
		lib.NewWave (new Wave (17f, mp, p, 3, false, 0, 2f,  "default"));


		//PHASE 3

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 1f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red");	
		lib.NewWave (new Wave (27f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White");	
		lib.NewWave (new Wave (35f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 1f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "White");	
		lib.NewWave (new Wave (40f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (43f, mp, p, 2, false, 0,  3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 3f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (47f, mp, p, 1, false, 0, 3f / difficultyMultiplier, "default"));


		//PHASE 4
		// LASERS

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 10f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_SpiderWebLaser();
		p.bulletCount = 5;
		lib.NewWave (new Wave (56f, mp, p, 1, false, 100, 5, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "White");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (60f, mp, p, 4, false, 0, 3f / difficultyMultiplier, "default"));


/*
		//debug boss
		mp = new EMP_EnterFromTop();
		boss = new Wave(mp, 1f, 10, true, 2, "boss2");
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		lib.NewWave (boss);
*/

		mp = new EMP_EnterFromTop();
		boss = new Wave(mp, 114f, 150, true, 2, "boss2");
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		lib.NewWave (boss);
		
	}
}
