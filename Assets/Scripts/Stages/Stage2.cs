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
		Game.control.ui.UpdateStageText (stageHandler.currentStage);
	
		while (Game.control.dialog.handlingDialog) yield return null;
		while (stageHandler.stageTimer < 50f) yield return null;
		Game.control.sound.PlayMusic ("Boss", 2);
		Game.control.dialog.StartDialog ("Boss2");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
        EnemyLib lib = Game.control.enemyLib;
		lib.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("LeaveDir", "Right");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	
		lib.NewWave (new Wave (2f, mp, p, 1, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

		mp = new EnemyMovementPattern (lib.snake);
		mp.Customize ("LeaveDir", "Left");
		p = new Pattern (lib.repeatedHoming);
		p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
		p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		p.SetSprite ("Arrow", "Glow", "Red");	 
		lib.NewWave (new Wave (4f, mp, p, 1, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });

		mp = new EnemyMovementPattern (lib.enterLeave);
		mp.Customize ("LeaveDir", "Left");
		mp.Customize("StayTime", 2f);
		p = new Pattern (lib.singleHoming);
		p.SetSprite ("Circle", "Glow", "Red");
		//p.Customize (new BulletMovementPattern (false, "WaitToHome", 9f, p, 0, 14));
		lib.NewWave (new Wave (8f, mp, p, 1, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });

		Wave boss2 = new Wave(50f, null, null, 1,  false, 150, true, 3f, 2);
		boss2.SetUpBoss (2, "Spider Queen", false);
		boss2.movementPattern = lib.enterFromTop;
		lib.NewWave (boss2, new ArrayList { lib.middleTop });
	}
}
