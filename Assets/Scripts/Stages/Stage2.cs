using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : Stage
{
	void Awake(){
		stageName = "Void Ritual";
        stageSubtitle = "~Ancient Forest~";
		bgmName = "Spelunker";
		stageindex = 2;
        fadeFrom = "Black";

        LateStageInit();
	}
	

	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}

	IEnumerator StageHandlerRoutine(){
		Game.control.sound.PlayMusic ("Stage", 2);
		scene.e_camera.SetPosition (new Vector3(80,45,72));
		scene.e_camera.SetRotation (new Vector3(65, 0, 5));

		while (stage.stageTimer < 0.1f) yield return null;
		
		scene.SetPlaneSpeed (3f);

		while (stage.stageTimer < 26.5f) yield return null;
		Game.control.stageUI.PlayStageToast();
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

        //////////////////////////////
        // BOSS //

		while (Game.control.dialog.handlingDialog) yield return null;
		while (stage.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
		
		Game.control.dialog.StartDialog ("Boss2");

		while(Game.control.dialog.handlingDialog) {
			if(stage.stageTimer > 124.1f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 2);

		while(!Game.control.enemySpawner.bossWave.dead) yield return null;

        Game.control.sound.FadeOutMusic();
		Game.control.stageHandler.ToggleTimer(false);

		yield return new WaitUntil(() => Game.control.stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		Game.control.dialog.StartDialog ("Boss2_1");
		while (Game.control.dialog.handlingDialog) yield return null;
		
		//FORCE MOVE PLAYER TO THE PORTAL HERE
		Game.control.player.movement.ForceMove(lib.GetVector("X3"));
		Game.control.stageUI.FadeTo("White", 2.5f);
		yield return new WaitForSeconds(4f);
		Game.control.stageHandler.EndHandler ("StageComplete");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
		
		StageHandler stage = Game.control.stageHandler;
		stage.waves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;
		lib = Game.control.vectorLib;



		//PHASE 1

		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 8f);
		p.SetSprite ("Circle", "Glow", "Red", "Medium");
		p.executeDelay= 1f;
		p.coolDown = .5f;
		stage.NewWave (new Wave (3f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 8f);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.executeDelay= 1f;
		p.coolDown = .5f;
		stage.NewWave (	new Wave (8f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "gand_flute"));

		//PHASE 2

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("R3")});
		mp.force = true;
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, true);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	
		stage.NewWave (new Wave (13f, mp, p, 3, false, 0, 3f / difficultyMultiplier,  "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, true);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (15f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (16f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (18f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "gand_flute"));

		//STAGE INTERLUDE
		//PHASE 3

		mp = new EnemyMovementPattern(lib.GetVector("K8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3"), new WayPoint("L3")});
		mp.force = true;
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red", "Medium");	
		stage.NewWave (new Wave (30f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "gand_flute"));
		
		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3"), new WayPoint("R3")});
		mp.force = true;
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "Red", "Medium");	
		stage.NewWave (new Wave (30f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("K8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3"), new WayPoint("L3")});
		mp.force = true;
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, true);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (32f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("K8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3"), new WayPoint("L3")});
		mp.force = true;
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, true);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (35f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3"), new WayPoint("L3")});
		mp.force = true;
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, true);
		p.SetSprite ("Arrow", "Glow", "White", "Small");	
		stage.NewWave (new Wave (40f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3"), new WayPoint("L3")});
		mp.force = true;
		p = new P_RepeatedHoming();
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, true);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (43f, mp, p, 1, false, 0,  3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3"), new WayPoint("L3")});
		mp.force = true;
		p = new P_Circle();
		p.rotationDirection = -1;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.SetSprite ("Circle", "Glow", "White", "Medium");	
		stage.NewWave (new Wave (45f, mp, p, 10, false, 0, 3f / difficultyMultiplier, "gand_flute"));


		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.force = true;
		//mp.smoothedMovement = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "Red", "Medium");
		p.executeDelay= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (47f, mp, p, 1, false, 0, 3f / difficultyMultiplier, "gand_flute"));


		//PHASE 4
		// LASERS, SPIDERS, MAELSTROMS

		mp = new EnemyMovementPattern(lib.GetVector("B8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.disableHitBox = true;
		mp.hideSpriteOnSpawn = true;
		p = new P_SpiderWebLaser();
        p.SetSprite ("Laser", "Glow", "Red", "Small");
		p.bulletCount = 4 * Mathf.CeilToInt(difficultyMultiplier);
		stage.NewWave (new Wave (55.5f, mp, p, 1, false, 100, 5, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.infinite = false;
		p.circleDelay= 1f;
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (60f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("B8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 4 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.executeDelay= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (64f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});

		mp = new EnemyMovementPattern(lib.GetVector("B8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.disableHitBox = true;
		mp.hideSpriteOnSpawn = true;
		p = new P_SpiderWebLaser();
        p.SetSprite ("Laser", "Glow", "Red", "Small");
		p.bulletCount = 4 * Mathf.CeilToInt(difficultyMultiplier);
		stage.NewWave (new Wave (67.5f, mp, p, 1, false, 100, 5, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		p.executeDelay= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (68f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});

	    mp = new EnemyMovementPattern(lib.GetVector("A8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (71f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "gand_flute"));

		 mp = new EnemyMovementPattern(lib.GetVector("A8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (73f, mp, p, 3, false, 0, 2f / difficultyMultiplier,  "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Diamond", "Glow", "Red", "Small");
		p.executeDelay= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (75f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});

		mp = new EnemyMovementPattern(lib.GetVector("B1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.bulletCount = 3 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Diamond", "Glow", "Red", "Small");
		p.executeDelay= 1f;
		p.coolDown = 1f;
		stage.NewWave (new Wave (78f, mp, p, 2, false, 0, 3f / difficultyMultiplier, "gand_flute"),
						new List<Vector3> {lib.GetVector("H1"), lib.GetVector("C1")},
						new List<Vector3>{lib.GetVector("H3"), lib.GetVector("C3")},
						new List<Vector3>{lib.GetVector("L3"), lib.GetVector("R3")});



		////////////////////////////////////////////////////////////////
		//VOID PORTALS
		// 1 portal 1 asura

		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 10), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(3f, 5, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (80f, mp, p, 1, false, 5, 100, "default"));

		
		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 2), new WayPoint("R3")});
        mp.disableHitBox = true;
		p = new P_Spiral(6 * (int)difficultyMultiplier, 1);
		p.loopCircles =  288 * 4;
		p.executeDelay = 1f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 3f);
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		Wave w = new Wave(85f, mp, p, 5, false, 1, 5 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w); ///// 85f

        // EXTRA SPIDERS
        mp = new EnemyMovementPattern(lib.GetVector("A8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (86f, mp, p, 5, false, 0, 2f / difficultyMultiplier,  "gand_flute"));

        

		////////////////////////////////////////////////////////
		//3 PORTALS 9 ASURAS
		
		
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("X3", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(9f, 5, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (90f, mp, p, 1, false, 5, 100, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("C3", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(9f, 5, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (90.5f, mp, p, 1, false, 5, 100, "default"));

		mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("I3", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(9f, 5, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (91f, mp, p, 1, false, 5, 100, "default"));

		///

		mp = new EnemyMovementPattern(lib.GetVector("X3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 2), new WayPoint("L5")});
		mp.disableHitBox = true;
		p = new P_Spiral(3 * (int)difficultyMultiplier, 1);
		p.loopCircles =  288 * 1;
		p.BMP = new BMP_WaitAndExplode(p, 11f, 0);
		p.BMP.movementSpeed = 1f;
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		w = new Wave(95f, mp, p, 3, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);

		mp = new EnemyMovementPattern(lib.GetVector("I3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 2), new WayPoint("R5")});
		mp.disableHitBox = true;
		p = new P_Spiral(3 * (int)difficultyMultiplier, 1);
		p.loopCircles =  288 * 1;
		p.BMP = new BMP_WaitAndExplode(p, 11f, 0);
		p.BMP.movementSpeed = 1f;
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		w = new Wave(96f, mp, p, 3, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);

		mp = new EnemyMovementPattern(lib.GetVector("C3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 2), new WayPoint("L5")});
		mp.disableHitBox = true;
		p = new P_Spiral(3 * (int)difficultyMultiplier, 1);
		p.loopCircles =  288 * 1;
		p.BMP = new BMP_WaitAndExplode(p, 11f, 0);
		p.BMP.movementSpeed = 1f;
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		w = new Wave(97f, mp, p, 3, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);


        // EXTRA SPIDERS
		mp = new EnemyMovementPattern(lib.GetVector("A8"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("L3")});
		p = new P_Maelstrom();
		p.infinite = false;
		p.circleDelay= 1f;
		p.bulletCount = 1 * Mathf.CeilToInt(difficultyMultiplier);
		p.SetSprite ("Spider_Glow", "Small");
		p.BMP = new BMP_SlowWaving(p, 9f, true);
		stage.NewWave (new Wave (97f, mp, p, 5, false, 0, 2f / difficultyMultiplier,  "gand_flute"));



		//TRAPPING VOIDS
		mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("I3", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(13f, 7, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (105f, mp, p, 1, false, 5, 100, "default")); //105

		mp = new EnemyMovementPattern(lib.GetVector("I10"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("I8", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(13f, 7, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (106f, mp, p, 1, false, 5, 100, "default")); //105
		
		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("C3", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(13f, 7, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (107f, mp, p, 1, false, 5, 100, "default")); //105

		mp = new EnemyMovementPattern(lib.GetVector("C10"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("C8", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(13f, 7, 6);
		p.executeDelay = 2f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (108f, mp, p, 1, false, 5, 100, "default")); //105




		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 114f, 600, true, 2);
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		stage.NewWave (boss);


         /*

		//debug boss
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 1f, 50, true, 2);
		boss.SetUpBoss (2, "Joanette, Queen of Spiders", false);
		stage.NewWave (boss);
    */




	}
}
