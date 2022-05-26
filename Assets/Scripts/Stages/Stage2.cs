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

		scene.SetPlaneSpeed (3f);

		while (stageHandler.stageTimer < 26.5f) yield return null;
		Game.control.ui.PlayStageToast();
		scene.e_camera.Move (new Vector3(50, 0, 72));
		scene.e_camera.Rotate (new Vector3(25, 0, 5));
		scene.SetPlaneSpeed (10f);

		while (Game.control.dialog.handlingDialog) yield return null;
		while (stageHandler.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
		
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
		//PHASE 1

		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 15;
		p.bulletMovement = new BMP_Explode(p, 8f, false);
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = .5f;
		lib.NewWave (	new Wave (3f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "default"), 
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		//PHASE 2

		mp = new EMP_Snake(lib.leftWallTopSide, 0, 1);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (10f, mp, p, 3, false, 0, 3f / difficultyMultiplier,  "default"));

		mp = new EMP_Snake(lib.rightWallTopSide, 0, -1);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (13f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow");
		p.bulletMovement = new BMP_SlowWaving(p, 9f);
		lib.NewWave (new Wave (18f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		//STAGE INTERLUDE
		//PHASE 3

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red");	
		lib.NewWave (new Wave (30f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveRight);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White");	
		lib.NewWave (new Wave (40f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.rightWallTopSide, 0f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "White");	
		lib.NewWave (new Wave (43f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 0);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (45f, mp, p, 1, false, 0,  3f / difficultyMultiplier, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 3f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (48f, mp, p, 1, false, 0, 3f / difficultyMultiplier, "default"));


		//PHASE 4
		// LASERS, SPIDERS, MAELSTROMS

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 10f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_SpiderWebLaser();
		p.bulletCount = 5;
		lib.NewWave (new Wave (56f, mp, p, 1, false, 100, 5, "default"));

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow");
		p.bulletMovement = new BMP_SlowWaving(p, 9f);
		lib.NewWave (new Wave (60f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 10f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_SpiderWebLaser();
		p.bulletCount = 5;
		lib.NewWave (new Wave (65f, mp, p, 1, false, 100, 5, "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "White");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (68f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveLeft, lib.leaveRight});

		mp = new EMP_EnterLeave(lib.leftWallTopSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow");
		p.bulletMovement = new BMP_SlowWaving(p, 9f);
		lib.NewWave (new Wave (71f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		mp = new EMP_EnterLeave(lib.topWallLeftSide, 2f);
		mp.SetEnterLeaveDirection(lib.enterLeft, lib.leaveLeft);
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (75f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveLeft, lib.leaveRight});

*/
		//VOID PORTALS

		mp = new EMP_EnterLeave(lib.centerTopOOB, 2f);
		mp.hideSpriteOnSpawn = true;
		p = new P_VoidPortal(6f);
		p.delayBeforeAttack = 2f;
		p.bulletCount = 1;
		p.bulletMovement = new BMP_Explode(p, 0, false);
		//p.SetSprite ("Circle", "Glow", "White");
		lib.NewWave (new Wave (1f, mp, p, 3, false, 5, 100, "default"), //SPAWNTIME 85
						new List<Vector3> { lib.centerTopOOB, lib.topWallRightSide, lib.topWallLeftSide},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight, lib.leaveLeft, lib.leaveCenter});

		mp = new EMP_EnterLeave(lib.enterRight, 1);
		mp.SetEnterLeaveDirection(lib.enterRight, lib.leaveRight); //REMEMBER SMOOTH CURVE AWAY
		//no pattern yet
		p = new P_SingleHoming();
		p.bulletCount = 1;
		p.bulletMovement = new BMP_Explode(p, 0, false);
		lib.NewWave(new Wave(5f, mp, p, 9, false, 3, 3 / difficultyMultiplier, "default"),
						new List<Vector3> { lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.enterCenter, lib.enterRight, lib.enterLeft},
						new List<Vector3>{lib.leaveRight + new Vector3(0, -3, 0), lib.leaveRight + new Vector3(0, -3, 0), lib.leaveLeft + new Vector3(0, -3, 0)});
		
		
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
