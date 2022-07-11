using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : Stage
{
	void Awake(){
		stageName = "River of Tears";
		bgmName = "????";
		stageindex = 3;

        lib = Game.control.vectorLib;
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
      
      /*
        Game.control.stageUI.EffectOverlay("White", false, 2.5f);

        scene.SetPlaneSpeed (2f);
        Game.control.stageUI.WORLD.SetTopLayerSpeed(5);

        Game.control.dialog.StartDialog("Stage3_0");

        while (stage.stageTimer < 12f) yield return null;
		Game.control.stageUI.PlayStageToast();

        while (stage.stageTimer < 14f) yield return null;
        scene.SetPlaneSpeed (20f);

        while (stage.stageTimer < 40f) yield return null;
        scene.SetPlaneSpeed (5f);

        scene.e_camera.Rotate (new Vector3(65, 0, -5));

        while (stage.stageTimer < 45f) yield return null;

        scene.e_camera.Rotate (new Vector3(65, 0, 5));

        while (stage.stageTimer < 50f) yield return null;

        scene.e_camera.Rotate (new Vector3(65, 0, 0));
*/

        // BOSS //
        Game.control.stageUI.WORLD.SetTopLayerSpeed(1);
        while (stage.stageTimer < Game.control.enemySpawner.bossWave.spawnTime - 1) yield return null;
		Game.control.dialog.StartDialog ("Boss3");

        while(Game.control.dialog.handlingDialog) {
			if(stage.stageTimer > 123.9f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 3);

		
		while(!Game.control.enemySpawner.bossWave.dead) yield return null;
		Game.control.stageHandler.ToggleTimer(false);

		yield return new WaitUntil(() => Game.control.stageHandler.CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds(1f);

		Game.control.dialog.StartDialog ("Boss3_1");
		while (Game.control.dialog.handlingDialog) yield return null;
		
		//FORCE MOVE PLAYER TO THE PORTAL HERE
		yield return new WaitForSeconds(1f);
		Game.control.stageHandler.EndHandler ("StageComplete");
	}

	

	public override void InitWaves(float difficultyMultiplier) {
		VectorLib lib = Game.control.vectorLib;
		//StageHandler stage = Game.control.stageHandler;
		stage.waves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;



        //DEBUG TO TEST NEW PATTERNS
/*
        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("C5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Shape(10, "ThreeCircles", 2);
		p.coolDown = 2;
		p.infinite = false;
        p.executeDelay = 1f;
        p.BMP = new BMP_Explode(p, 10);
        p.BMP.unFold = true;
        p.SetSprite ("Circle", "Glow", "BlackOrange", "Small");	 
		stage.NewWave (new Wave (1f, mp, p, 1, false, 0, 10f / difficultyMultiplier, "asura"));
*/



/*

        //PORTAL
        mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("C3", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(2f, 5, 6);
		p.executeDelay = 1f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (1f, mp, p, 1, false, 5, 100, "default"));

        //ASURA
        mp = new EnemyMovementPattern(lib.GetVector("C3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("B1")});
		mp.disableHitBox = true;
		Wave w = new Wave(3f, mp, p, 1, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);

        //portal
        mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("I4", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(2f, 5, 6);
		p.executeDelay = 1f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (2f, mp, p, 1, false, 5, 100, "default"));

        //asura
        mp = new EnemyMovementPattern(lib.GetVector("I4"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("J1")});
		mp.disableHitBox = true;
		w = new Wave(5f, mp, p, 1, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);

        //portal
        mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("D5", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(2f, 5, 6);
		p.executeDelay = 1f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (3f, mp, p, 1, false, 5, 100, "default"));

        //asura
        mp = new EnemyMovementPattern(lib.GetVector("D5"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("B1")});
		mp.disableHitBox = true;
		w = new Wave(7f, mp, p, 1, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);

        //portal
        mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){ new WayPoint("X4", 4f), new WayPoint("L3")});
		mp.hideSpriteOnSpawn = true;
		mp.disableHitBox = true;
		p = new P_VoidPortal(2f, 5, 6);
		p.executeDelay = 1f;
		p.infinite = false;
		p.BMP = new BMP_Explode(p, 0);
		stage.NewWave (new Wave (4f, mp, p, 1, false, 5, 100, "default"));

        //asura
        mp = new EnemyMovementPattern(lib.GetVector("X4"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("J1")});
		mp.disableHitBox = true;
		w = new Wave(9f, mp, p, 1, false, 3, 6 / difficultyMultiplier, "asura");
		w.invulnerable = true;
		stage.NewWave(w);

        /// INTERLUDE //



        




        
        

        //BLUE STARS BURSTING INTO CIRCLES
        mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("I5", 5, -1), new WayPoint("R3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Blue", "Small");	 
		stage.NewWave (new Wave (15f, mp, p, 3, false, 0, 5f / difficultyMultiplier, "asura"));


        mp = new EnemyMovementPattern(lib.GetVector("H1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("D5", 5, -1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Turquoise", "Small");	 
		stage.NewWave (new Wave (16f, mp, p, 3, false, 0, 5f / difficultyMultiplier, "asura"));



        //COOL 3D CIRCLE ARROW PATTERN
        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("C5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, false);
		p.SetSprite ("Arrow", "Glow", "Blue", "Small");	 
		stage.NewWave (new Wave (20f, mp, p, 5, false, 0, 20f / difficultyMultiplier, "asura"));

        mp = new EnemyMovementPattern(lib.GetVector("H1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("D5", 5, -1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Turquoise", "Small");	 
		stage.NewWave (new Wave (22f, mp, p, 3, false, 0, 5f / difficultyMultiplier, "asura"));

        mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 1), new WayPoint("E4", 1, -1), new WayPoint("B1")});
		mp.force = true;
		p = new P_Maelstrom();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, false);
		p.SetSprite ("Arrow", "Glow", "Turquoise", "Small");	 
		stage.NewWave (new Wave (25f, mp, p, 5, false, 0, 20f / difficultyMultiplier, "asura"));

        //SMALL CIRCLE CLUSTER
        mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C4", 1), new WayPoint("D5", 1, -1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Shape(Mathf.CeilToInt(2 * (difficultyMultiplier)), "ThreeCircles", 1);
		p.coolDown = 2;
		p.infinite = false;
        p.BMP = new BMP_Explode(p, 7, true, true, true);
        p.BMP.axisRotateSpeed = 4;
		p.SetSprite ("Circle", "Glow", "BlackOrange", "Tiny");	 
		stage.NewWave (new Wave (28f, mp, p, 10, false, 0, 10f / difficultyMultiplier, "asura"));


        mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("I5", 5, -1), new WayPoint("R3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (30f, mp, p, 3, false, 0, 5f / difficultyMultiplier, "asura"));


        //ROTATING CIRCLES
        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("C5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Shape(Mathf.CeilToInt(4 * (difficultyMultiplier)), "TwoCircles", 2);
		p.coolDown = 2;
		p.infinite = false;
        p.BMP = new BMP_Explode(p, 5, true, true, false);
        p.BMP.axisRotateSpeed = 2;
		p.SetSprite ("Circle", "Glow", "BlackRed", "Small");	 
		stage.NewWave (new Wave (40f, mp, p, 10, false, 0, 10f / difficultyMultiplier, "asura"));

        //SMALL CIRCLE CLUSTER
        mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C4", 1), new WayPoint("D5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Shape(Mathf.CeilToInt(4 * (difficultyMultiplier)), "ThreeCircles", 1);
		p.coolDown = 2;
		p.infinite = false;
        p.BMP = new BMP_Explode(p, 7, true, true, true);
        p.BMP.axisRotateSpeed = 4;
        p.BMP.rotationDir = 1;
		p.SetSprite ("Circle", "Glow", "BlackOrange", "Tiny");	 
		stage.NewWave (new Wave (45f, mp, p, 10, false, 0, 10f / difficultyMultiplier, "asura"));

        mp = new EnemyMovementPattern(lib.GetVector("A6"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Shape(Mathf.CeilToInt(5 * (difficultyMultiplier)), "TwoCircles", 2);
		p.coolDown = 2;
		p.infinite = false;
        p.BMP = new BMP_Explode(p, 5, true, true, false);
        p.BMP.axisRotateSpeed = 2;
		p.SetSprite ("Circle", "Glow", "BlackRed", "Small");	 
		stage.NewWave (new Wave (46f, mp, p, 3, false, 0, 10f / difficultyMultiplier, "asura"));

         mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("I5", 5, -1), new WayPoint("R3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (55f, mp, p, 3, false, 0, 5f / difficultyMultiplier, "asura"));








        //mid boss

        mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.stayTime = 20f;
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		Wave midBoss = new Wave(mp, 65f, 200, false, 1);
		midBoss.SetUpBoss (2.5f, "????", true);
		stage.NewWave (midBoss);






        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("C5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, false);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (89f, mp, p, 5, false, 0, 20f / difficultyMultiplier, "asura"));

        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("C5", 1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Maelstrom();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitToHome(p, 9f, false);
		p.SetSprite ("Arrow", "Glow", "Red", "Small");	 
		stage.NewWave (new Wave (90f, mp, p, 5, false, 0, 20f / difficultyMultiplier, "asura"));

        mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 2), new WayPoint("L5")});
		p = new P_Spiral(3 * (int)difficultyMultiplier, 1);
		p.loopCircles =  288 * 2;
		p.BMP = new BMP_WaitAndExplode(p, 11f, 0);
		p.BMP.movementSpeed = 1f;
		p.SetSprite ("Circle", "Glow", "White", "Medium");
		w = new Wave(93f, mp, p, 7, false, 3, 6 / difficultyMultiplier, "asura");
		stage.NewWave(w);

        mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("I5", 5, -1), new WayPoint("R3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Blue", "Small");	 
		stage.NewWave (new Wave (95f, mp, p, 10, false, 0, 5f / difficultyMultiplier, "asura"));


        mp = new EnemyMovementPattern(lib.GetVector("H1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("D5", 5, -1), new WayPoint("L3")});
		mp.force = true;
		p = new P_Circle();
		p.coolDown = 2;
		p.infinite = false;
		p.bulletCount = Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
		p.SetSprite ("Diamond", "Glow", "Turquoise", "Small");	 
		stage.NewWave (new Wave (100f, mp, p, 5, false, 0, 5f / difficultyMultiplier, "asura"));

*/

        /// BOSS DEBUG ///

        
        mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 1f, Mathf.CeilToInt(100), true, 2);
		boss.SetUpBoss (3, "Danu, Mother of the Asura", false);
		stage.NewWave (boss);

/*
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		boss = new Wave(mp, 115f, Mathf.CeilToInt(400), true, 2);
		boss.SetUpBoss (3, "Danu, Mother of the Asura", false);
		stage.NewWave (boss);

        */
	}
}
