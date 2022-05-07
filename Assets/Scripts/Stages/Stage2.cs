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
		while (stageHandler.stageTimer < 1f) yield return null;
		Game.control.sound.PlayMusic ("Boss", 2);
		Game.control.dialog.StartDialog ("Boss2");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
        EnemyLib lib = Game.control.enemyLib;
		lib.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

/*
		//															enmyCnt simul hlth 	isBoss, cd, hlthBars, spawnPositions
			p = new Pattern (lib.singleHoming);
			p.Customize(new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite("Circle", "Big", "Red");
			lib.NewWave (lib.stageWaves, new Wave (2f, lib.zigZag, p, 5, false, 0, false, 1f, 0), new ArrayList { lib.middleTop });
			Wave w = (Wave)lib.stageWaves[0];
			w.shootPattern.Customize(new BulletMovementPattern (true, null, 0.5f, w.shootPattern, 0, 14));
			w.shootPattern.SetSprite("Circle", "Big", "Red");

			p = new Pattern (lib.circle);
			p.Customize(new BulletMovementPattern (true, "Explode", 2f, p, 0, 14));
			p.SetSprite("Circle", "Glow", "Green");
			lib.NewWave (lib.stageWaves, new Wave (3f, lib.stopOnce, p, 6, false, 0, false, 1f, 0), new ArrayList { lib.topRight });

			//NewWave (stage1Waves, new Wave (6f, zigZag, singleHoming, 6, false, 0, false, 1f,	0), new ArrayList { middleTop });
			//NewWave (stage1Waves, new Wave (10f, zigZag, circle, 6, false, 0, false, 1f,	0), new ArrayList { middleTop });
			p = new Pattern (lib.circle);
			p.Customize(new BulletMovementPattern (true, "Explode", 6f, p, 0, 14));
			p.SetSprite("Circle", "Glow", "Yellow");
			lib.NewWave (lib.stageWaves, new Wave (10f, lib.stopOnce, p, 6, false, 0, false, 1f, 0), new ArrayList { lib.topRight });


			//p = new Pattern (laser);

			lib.NewWave (lib.stageWaves, new Wave (25f, lib.enterLeave, p, 2, true, 10, false,	1f, 0), new ArrayList {
				lib.rightTop,
				lib.leftTop
			});

			p = new Pattern (lib.singleHoming);
			p.Customize(new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite("Circle", "Big", "Red");
			lib.NewWave (lib.stageWaves, new Wave (15f, lib.zigZag, p, 5, false, 0, false, 1f, 0), new ArrayList { lib.middleTop });
			lib.NewWave (lib.stageWaves, new Wave (20f, lib.stopOnce, p, 6, false, 0, false, 2f,	0), new ArrayList { lib.topRight });

			
*/		

			Wave boss2 = new Wave(2f, null, null, 1,  false, 150, true, 3f, 2);
			boss2.SetUpBoss (2, "Spider Queen", false);
			boss2.movementPattern = lib.enterFromTop;
			lib.NewWave (lib.stageWaves, boss2, new ArrayList { lib.middleTop });
	}
}
