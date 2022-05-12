using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : Stage
{
	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}

	IEnumerator StageHandlerRoutine(){
		SceneHandler scene = Game.control.scene;
		Game.control.ui.UpdateStageText (2, "Forest of Void Rituals", "Stage2");
		while (stageHandler.stageTimer < 24f) yield return null;
		Game.control.ui.ShowStageText();
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
		
		//PHASE 1
		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Center");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new Pattern (lib.maelStrom);
		p.Customize("BulletCount", 15);
		p.SetSprite ("Circle", "Glow", "Red");
		p.Customize("Delay", 1f);
		p.Customize("CoolDown", 1f);
		lib.NewWave (new Wave (3f, mp, p, 1, false, 5, false, 10f / difficultyMultiplier, 0), new ArrayList { lib.middleTop });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Right");
		mp.Customize ("LeaveDir", "Right");
		mp.Customize("StayTime", 2f);
		p = new Pattern (lib.maelStrom);
		p.Customize("BulletCount", 15);
		p.SetSprite ("Circle", "Glow", "Red");
		p.Customize("Delay", 1f);
		p.Customize("CoolDown", 1f);
		lib.NewWave (new Wave (5f, mp, p, 1, false, 5, false, 10f / difficultyMultiplier, 0), new ArrayList { lib.rightTop });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new Pattern (lib.maelStrom);
		p.Customize("BulletCount", 15);
		p.SetSprite ("Circle", "Glow", "Red");
		p.Customize("Delay", 1f);
		p.Customize("CoolDown", 1f);
		lib.NewWave (new Wave (7f, mp, p, 1, false, 5, false, 10f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });


		//PHASE 2

		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Right");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (13f, mp, p, 2, false, 2, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Left");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (15f, mp, p, 2, false, 2, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new Pattern (lib.maelStrom);
		p.Customize("CircleDelay", 1f);
		p.SetSprite ("Spider");
		p.Customize (new BulletMovementPattern (false, "SlowWaving", 5f, p, 0, 14));
		lib.NewWave (new Wave (17f, mp, p, 3, false, 0, false, 2f, 0), new ArrayList { lib.topLeft });




		//PHASE 3

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("EnterDir", "Right");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 1f);
		p = new Pattern (lib.circle);
		p.Customize("RotationDirection", -1);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.SetSprite ("Circle", "Glow", "Red");	
		lib.NewWave (new Wave (27f, mp, p, 10, false, 5, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("LeaveDir", "Right");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "White");	
		lib.NewWave (new Wave (35f, mp, p, 2, false, 5, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("EnterDir", "Right");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 1f);
		p = new Pattern (lib.circle);
		p.Customize("RotationDirection", -1);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.SetSprite ("Circle", "Glow", "White");	
		lib.NewWave (new Wave (40f, mp, p, 10, false, 5, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("LeaveDir", "Left");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (43f, mp, p, 2, false, 5, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });


		mp = new EnemyMovementPattern (lib.enterLeave);
		//mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 3f);
		p = new Pattern (lib.maelStrom);
		p.Customize("BulletCount", 15);
		p.SetSprite ("Circle", "Glow", "Red");
		p.Customize("Delay", 1f);
		p.Customize("CoolDown", 1f);
		lib.NewWave (new Wave (47f, mp, p, 1, false, 5, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });


		//PHASE 4

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 10f);
		p = new Pattern(lib.spiderWebLaser);
		lib.NewWave (new Wave (55f, mp, p, 1, false, 100, false, 5, 0), new ArrayList { lib.leftTop });


		mp = new EnemyMovementPattern (lib.enterLeave);
		//mp.Customize("EnterDir", "Left");
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new Pattern (lib.maelStrom);
		p.Customize("BulletCount", 15);
		p.SetSprite ("Circle", "Glow", "White");
		p.Customize("Delay", 1f);
		p.Customize("CoolDown", 1f);
		lib.NewWave (new Wave (60f, mp, p, 4, false, 5, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });


		//LOOPING WORK IN PROGRESS
		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Right");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (65f, mp, p, 10, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.doubleSnake);
		mp.Customize ("LeaveDir", "Left");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (70f, mp, p, 10, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });



		boss = new Wave(113f, null, null, 1,  false, 150, true, 3f, 2);
		boss.SetUpBoss (2, "Spider Queen", false);
		boss.movementPattern = lib.enterFromTop;
		lib.NewWave (boss, new ArrayList { lib.middleTop });
	}
}
