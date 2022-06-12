using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : Stage
{
	void Awake(){
		stageName = "Forest of Void Rituals";
		bgmName = "Spelunker";
		stageindex = 2;

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
		
		scene.e_camera.SetPosition (new Vector3(80,45,72));
		scene.e_camera.SetRotation (new Vector3(65, 0, 5));

		while (stage.stageTimer < 0.1f) yield return null;
		
		scene.SetPlaneSpeed (3f);

		while (stage.stageTimer < 26.5f) yield return null;
		Game.control.ui.PlayStageToast();
		scene.e_camera.Move (new Vector3(65, 45, 72));
		scene.e_camera.Rotate (new Vector3(65, 0, 5));
		scene.SetPlaneSpeed (10f);


		while (stage.stageTimer < 46f) yield return null;
		scene.SetPlaneSpeed (5f);
		scene.e_camera.Rotate (new Vector3(35, 0, 15));

		while (stage.stageTimer < 50f) yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, -15));

		while (stage.stageTimer < 52f)  yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, 15));

		while (stage.stageTimer < 56f) yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, 5));
		
		scene.SetPlaneSpeed (10f);

		while (stage.stageTimer < 83f) yield return null;
		scene.SetPlaneSpeed (1f);

		while (Game.control.dialog.handlingDialog) yield return null;
		while (stage.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
		
		Game.control.dialog.StartDialog ("Boss2");

		while(Game.control.dialog.handlingDialog) {
			if(stage.stageTimer > 125.9f) break;
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
		stage.waves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;


		//PHASE 1

		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 8f, false, false);
		p.SetSprite ("Circle", "Glow", "Red", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = .5f;
		stage.NewWave (	new Wave (3f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "default"), 
						new List<Vector3> { lib.GetVector("X1"), lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("X3"), lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("R3"), lib.GetVector("L3"), lib.GetVector("XU")});

		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.bulletMovement = new BMP_Explode(p, 8f, false, false);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = .5f;
		stage.NewWave (	new Wave (8f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "default"), 
						new List<Vector3> { lib.GetVector("X1"), lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("X3"), lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("R3"), lib.GetVector("L3"), lib.GetVector("XU")});

		//PHASE 2

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	
		stage.NewWave (new Wave (13f, mp, p, 3, false, 0, 3f / difficultyMultiplier,  "default"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("R3")});
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (15f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.bulletMovement = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (18f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		//STAGE INTERLUDE
		//PHASE 3

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3"), new WayPoint("L3")});
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red", "Medium");	
		stage.NewWave (new Wave (30f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3"), new WayPoint("L3")});
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (35f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3"), new WayPoint("L3")});
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (40f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3"), new WayPoint("L3")});
	//mp.smoothedMovement = true;
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "White", "Medium");	
		stage.NewWave (new Wave (45f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3"), new WayPoint("L3")});
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.bulletMovement = new BMP_WaitToHome(p, 9f);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (43f, mp, p, 1, false, 0,  3f / difficultyMultiplier, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		//mp.smoothedMovement = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "Red", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (47f, mp, p, 1, false, 0, 3f / difficultyMultiplier, "default"));


		//PHASE 4
		// LASERS, SPIDERS, MAELSTROMS

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.disableHitBox = true;
		mp.hideSpriteOnSpawn = true;
		p = new P_SpiderWebLaser();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		stage.NewWave (new Wave (55f, mp, p, 1, false, 100, 5, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.bulletMovement = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (60f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (64f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (68f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});

	   mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Spider_Glow", "Small");
		p.bulletMovement = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (71f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "default"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Diamond", "Glow", "Red", "Small");
		p.delayBeforeAttack= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (75f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "default"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});

		////////////////////////////////////////////////////////////////
		//VOID PORTALS
		// 1 portal 1 asura

		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 1), new WayPoint("R3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(3f, 5, 6);
		p.delayBeforeAttack = 2f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 0, false, false);
		stage.NewWave (new Wave (80f, mp, p, 1, false, 5, 100, "default"), ///// 80f
						new List<Vector3> { lib.GetVector("X1"), lib.GetVector("H1"), lib.GetVector("C1")}, 
						new List<Vector3>{lib.GetVector("X3"), lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("R3"), lib.GetVector("L3"), lib.GetVector("XU")});

		
		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 2), new WayPoint("R3")});
		p = new P_Spiral(6 * (int)difficultyMultiplier);
		p.loopCircles =  288 * 4;
		p.rotationDirection = 1;
		p.delayBeforeAttack = 1f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 3f, false, false);
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");

		stage.NewWave(new Wave(85f, mp, p, 5, false, 3, 5 / difficultyMultiplier, "default")); ///// 85f


		////////////////////////////////////////////////////////
		//3 PORTALS 9 ASURAS
		
		// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		// REMOVE ENTER/LEAVE DIRS AND MAKE MULTIPLE WAVES THAT SPAWN AT DIFFERENT INTERVALS //////////////////////////////////////////////////////
		// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("X3", 1f), new WayPoint("R3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(9f, 5, 6);
		p.delayBeforeAttack = 2f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 0, false, false); 
		stage.NewWave (new Wave (90f, mp, p, 3, false, 5, 100, "default"),  ///// 93f
						new List<Vector3> { lib.GetVector("X1"), lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("X3"), lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("R3"), lib.GetVector("L3"), lib.GetVector("XU")});

		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 2), new WayPoint("R3")});
		p = new P_Spiral(3 * (int)difficultyMultiplier);
		p.loopCircles =  288 * 1;
		p.bulletMovement = new BMP_WaitAndExplode(p, 11f);
		p.bulletMovement.movementSpeed = 1f;
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");

		stage.NewWave(new Wave(95f, mp, p, 9, false, 3, 6 / difficultyMultiplier, "default"),  ///// 100
						new List<Vector3> {lib.GetVector("X3"), lib.GetVector("I3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("X3"), lib.GetVector("I3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L5"), lib.GetVector("R5"), lib.GetVector("L5")});
		

		//TRAPPING VOIDS

		//mp = new EMP_EnterLeave(lib.GetVector("H3"), 5f);
		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 2f), new WayPoint("R3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(13f, 7, 6);
		p.delayBeforeAttack = 2f;
		p.infinite = false;
		p.bulletMovement = new BMP_Explode(p, 0, false, false);
		//p.SetSprite ("Circle", "Glow", "White");
		stage.NewWave (new Wave (105f, mp, p, 4, false, 5, 100, "default"), //SPAWNTIME 105
						new List<Vector3> {lib.GetVector("I3"), lib.GetVector("C3"), lib.GetVector("I8"), lib.GetVector("C8")},
						new List<Vector3>{lib.GetVector("I3"), lib.GetVector("C3"), lib.GetVector("I8"), lib.GetVector("C8")},
						new List<Vector3>{lib.GetVector("R3"), lib.GetVector("L3"),  lib.GetVector("XU"),  lib.GetVector("XU")});


/*
		//debug boss
		mp = new EMP_EnterFromTop();
		boss = new Wave(mp, 1f, 30, true, 2, "boss2");
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		stage.NewWave (boss);
*/
		float health = 0;
		if(difficultyMultiplier < 5) health = 100 * difficultyMultiplier;
		if(difficultyMultiplier >= 5) health = 70 * difficultyMultiplier;


		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 114f, Mathf.CeilToInt(health), true, 2);
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		stage.NewWave (boss);

	}
}
