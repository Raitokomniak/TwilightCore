using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : Stage
{

	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}


	IEnumerator StageHandlerRoutine(){
		SceneHandler scene = Game.control.scene;
		Game.control.ui.UpdateStageText (1, "Asura's Path", "Asura who remain Asura");
		while (Game.control.dialog.handlingDialog) yield return null;
		
		while (stageHandler.stageTimer < 4f) yield return null;

		while (stageHandler.stageTimer < 8f) yield return null;
		scene.SetPlaneSpeed (10f);
		scene.RotateCamera (35, 0, 0);

		while (stageHandler.stageTimer < 14f) yield return null;
		scene.RotateCamera (35, 0, -5);
		scene.SetPlaneSpeed (1f);
		while (stageHandler.stageTimer < 24f) yield return null;
		Game.control.ui.ShowStageText();

		scene.MoveCamera (50, 0, 72);
		scene.RotateCamera (25, 0, 5);

		scene.SetPlaneSpeed (10f);
		while(stageHandler.stageTimer < 50f) yield return null;
		Game.control.dialog.StartDialog ("Stage1_1");
		while (stageHandler.stageTimer < 55f) yield return null;
		
		while (!Game.control.enemySpawner.midBossWave.dead) yield return null;
		yield return new WaitForSeconds (.5f);
		Game.control.dialog.StartDialog ("Stage1_2");
		while (Game.control.dialog.handlingDialog) yield return null;
		scene.SetPlaneSpeed (15f);

		while (stageHandler.stageTimer < boss.spawnTime - 1) yield return null;
		Game.control.dialog.StartDialog ("Boss1");
		scene.SetPlaneSpeed (3f);
	}

	public override void InitWaves(float difficultyMultiplier) {
        EnemyLib lib = Game.control.enemyLib;
		lib.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;
		
		
		
			//1st PHASE
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", .5f);
			p = new P_SingleHoming();
			p.bulletMovement = new BMP_Explode(p, 0.5f, true);
			p.SetSprite ("Circle", "Big", "Red");
			lib.NewWave (new Wave (3.5f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.rightTop });
									
			
			
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new P_SingleHoming();
			p.bulletMovement = new BMP_Explode(p, 0.5f, true);
			p.SetSprite ("Circle", "Big", "Red");
			lib.NewWave (new Wave (7f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });
																				// .6f
			

			//2ND PHASE
			if(difficultyMultiplier > 3){
				
				mp = new EnemyMovementPattern (lib.enterLeave);
				mp.Customize ("LeaveDir", "Right");
				mp.Customize ("StayTime", .5f);
				p = new P_SingleHoming();
				p.bulletMovement = new BMP_Explode(p, 0.5f, true);
				p.SetSprite ("Circle", "Big", "Red");	
				lib.NewWave (new Wave (8f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.rightTop });

				
				mp = new EnemyMovementPattern (lib.enterLeave);
				mp.Customize ("LeaveDir", "Left");
				mp.Customize ("StayTime", 2f);
				p = new P_SingleHoming();
				p.bulletMovement = new BMP_Explode(p, 0.5f, true);
				p.SetSprite ("Circle", "Big", "Red");	
				lib.NewWave (new Wave (15f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });
		
			}
		
	
			mp = new EnemyMovementPattern (lib.zigZag);
			mp.Customize("Direction", 1);
			p = new P_Circle();
			p.bulletCount = Mathf.CeilToInt(2.8f * (difficultyMultiplier / 2));
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	
			lib.NewWave (new Wave (16f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.rightTop });


			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Right");
			p = new P_Circle();
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (17f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topLeft });

		
			//PHASE3

			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new P_Circle();
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (28f, mp, p, 5, false, 0,  3f / difficultyMultiplier,  "default"), new ArrayList { lib.topLeft });
												 //28f
			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new P_Circle();
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (31f, mp, p, 5, false, 0,  3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });
												 //31f

			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new P_Circle();
			p.bulletCount =  10;
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (33f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });

			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new P_Circle();
			p.bulletCount =  10;
			//p.bulletMovement = new BulletMovementPattern (false, "Explode", 7f, p, 0, 14);
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (36f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topLeft });


			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 4f);
			p = new P_Circle();
			p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
			p.bulletMovement = new BMP_Explode(p, 11f, false);
			p.SetSprite ("Circle", "Glow", "Green");
			lib.NewWave (new Wave (41f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 4f);
			p = new P_Circle();
			p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
			p.bulletMovement = new BMP_Explode(p, 11f, false);
			p.SetSprite ("Circle", "Glow", "Yellow");
			lib.NewWave (new Wave (41.5f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });


			//MID-BOSS

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("StayTime", 24f);
			mp.Customize ("LeaveDir", "Up");
			Wave bossMid1 = new Wave(mp, 55f, 150, false, 1, "boss0.5"); //55f
			bossMid1.SetUpBoss (0.5f, "Asura", true);

			lib.NewWave (bossMid1, new ArrayList { lib.middleTop });



			//CONTD
			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new P_Circle();
			//p.bulletMovement = new BulletMovementPattern (false, "Explode", 7f, p, 0, 14);
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (83f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });


			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Left");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new P_Circle();
			p.bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
			//p.bulletMovement = new BulletMovementPattern (false, "Explode", 7f, p, 0, 14);
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (87f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topRight });

			mp = new EnemyMovementPattern (lib.snake);
			mp.Customize ("LeaveDir", "Right");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new P_Circle();
			p.bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
			//p.bulletMovement = new BulletMovementPattern (false, "Explode", 7f, p, 0, 14);
			p.bulletMovement = new BMP_Explode(p, 7f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (90f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"), new ArrayList { lib.topLeft });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 4f);
			p = new P_Circle();
			p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
			//.bulletMovement = new BulletMovementPattern (false, "Explode", 11f, p, 0, 14);
			p.bulletMovement = new BMP_Explode(p, 11f, false);
			p.SetSprite ("Circle", "Glow", "Green");	 
			lib.NewWave (new Wave (90f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 4f);
			p = new P_Circle();
			p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
			//p.bulletMovement = new BulletMovementPattern (false, "Explode", 11f, p, 0, 14);
			p.bulletMovement = new BMP_Explode(p, 11f, false);
			p.SetSprite ("Circle", "Glow", "Yellow");	 
			lib.NewWave (new Wave (90.5f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "default"), new ArrayList { lib.leftTop });


			//*/
			


			//DEBUG
			/*
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("StayTime", 0);
			boss = new Wave(1f, mp, null, 1,  false, 10 * Mathf.CeilToInt(difficultyMultiplier / 2) , true, 3f, 2);
			*/

			// BIG BOSS
			///*
			mp = new EnemyMovementPattern (lib.enterFromTop);
			mp.Customize ("StayTime", 0);
			boss = new Wave(mp, 100f, 60 * Mathf.CeilToInt(difficultyMultiplier), true, 2, "boss1");
			boss.movementPattern = lib.enterFromTop;
			//*/

			boss.SetUpBoss (1, "Maaya, Forest Guardian", false);
			lib.NewWave (boss, new ArrayList { lib.middleTop });
	}
}
