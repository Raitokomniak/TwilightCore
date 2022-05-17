using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : Stage
{
	void Awake(){
		name = "Forest of Void Rituals";
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
		while (stageHandler.stageTimer < 24f) yield return null;
		Game.control.ui.PlayStageToast();
		while (Game.control.dialog.handlingDialog) yield return null;
		while (stageHandler.stageTimer < 112f) yield return null;
		
		Game.control.dialog.StartDialog ("Boss2");



		while(Game.control.dialog.handlingDialog) {
			if(stageHandler.stageTimer > 126f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 2);
	}

	

	public override void InitWaves(float difficultyMultiplier) {
		lib.stageWaves.Clear ();

/*
		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Center");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 10f);
		//p = new P_VoidPortal();
		//p.SetSprite ("Circle", "Glow", "Red");
		p = new P_VoidPortal();
		//p.bulletMovement = new BulletMovementPattern (false, "Explode", 0.5f, p, 0, 14);
		p.bulletMovement = new BMP_Explode(p, 0.5f, false);
		p.SetSprite ("Circle", "Glow", "Red");
		lib.NewWave (new Wave (1f, mp, p, 3, false, 5, 10f / difficultyMultiplier, "default"), new ArrayList { lib.middleTop, lib.rightTop, lib.leftTop });
*/


		//PHASE 1
		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Center");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (3f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "default"), new ArrayList { lib.middleTop });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Right");
		mp.Customize ("LeaveDir", "Right");
		mp.Customize("StayTime", 2f);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (5f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "default"), new ArrayList { lib.rightTop });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (7f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });


		//PHASE 2

		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Right");
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (13f, mp, p, 2, false, 0, 3f / difficultyMultiplier,  "default"), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Left");
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (15f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topLeft });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new P_Maelstrom();
		p.circleDelay= 1f;
		p.SetSprite ("Spider");
		p.bulletMovement = new BMP_SlowWaving(p, 9f);
		lib.NewWave (new Wave (17f, mp, p, 3, false, 0, 2f,  "default"), new ArrayList { lib.topLeft });




		//PHASE 3

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("EnterDir", "Right");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 1f);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red");	
		lib.NewWave (new Wave (27f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("LeaveDir", "Right");
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White");	
		lib.NewWave (new Wave (35f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("EnterDir", "Right");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 1f);
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "White");	
		lib.NewWave (new Wave (40f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("LeaveDir", "Left");
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (43f, mp, p, 2, false, 0,  3f / difficultyMultiplier, "default"), new ArrayList { lib.topLeft });


		mp = new EnemyMovementPattern (lib.enterLeave);
		//mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 3f);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "Red");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (47f, mp, p, 1, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });


		//PHASE 4

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 10f);
		p = new P_SpiderWebLaser();
		p.bulletCount = 5;
		lib.NewWave (new Wave (58f, mp, p, 1, false, 100, 5, "default"), new ArrayList { lib.leftTop });


		mp = new EnemyMovementPattern (lib.enterLeave);
		//mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new P_Maelstrom();
		p.bulletCount = 15;
		p.SetSprite ("Circle", "Glow", "White");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		lib.NewWave (new Wave (60f, mp, p, 4, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });


		//LOOPING WORK IN PROGRESS
		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Right");
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (65f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Left");
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (70f, mp, p, 10, false, 0,  3f / difficultyMultiplier, "default"), new ArrayList { lib.topLeft });


		boss = new Wave(mp, 113f, 150, true, 2, "boss2");
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		boss.movementPattern = lib.enterFromTop;
		lib.NewWave (boss, new ArrayList { lib.middleTop });
	}
}
