using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : Stage
{
	void Awake(){
		stageName = "Asura's Path";
        stageSubtitle = "~Astria Fairgrounds~";
		bgmName = "Asura who remain Asura";
		stageindex = 1;
        fadeFrom = "Black";

        LateStageInit();
	}

	public override void StartStageHandler(){
		stageHandlerRoutine = StageHandlerRoutine();
		StartCoroutine(stageHandlerRoutine);
	}


	IEnumerator StageHandlerRoutine(){
        Game.control.sound.PlayMusic ("Stage", 1);
        

		scene.SetPlaneSpeed(50f);
		while (dialog.handlingDialog) yield return null;
		dialog.StartDialog ("Stage1_0");

		while (stage.stageTimer < 8f) yield return null;
		scene.SetPlaneSpeed (10f);
		scene.e_camera.Rotate (new Vector3(65, 0, 0));

		while (stage.stageTimer < 14f) yield return null;
		scene.e_camera.Rotate (new Vector3(65, 0, -5));
		scene.SetPlaneSpeed (1f);
		while (stage.stageTimer < 24f) yield return null;
		Game.control.stageUI.PlayStageToast();

		scene.e_camera.Move (new Vector3(50, 45, 72));
		scene.e_camera.Rotate (new Vector3(65, 0, 5));

		scene.SetPlaneSpeed (50f);
		while(stage.stageTimer < 52f) yield return null;
		dialog.StartDialog ("Stage1_1");
		while (stage.stageTimer < 55f) yield return null;
		  
		while (stage.midBossOn) yield return null;
		dialog.StartDialog ("Stage1_2");
		while (dialog.handlingDialog) yield return null;
		scene.SetPlaneSpeed (35f);
      
        //////////////////////////////////////////////////////////
        // BOSS //

        while (stage.stageTimer < boss.spawnTime - 5) yield return null;

        if(Game.control.enemySpawner.EnemiesAlive()) while (stage.stageTimer < boss.spawnTime - 1) yield return null;
        else boss.spawnTime = boss.spawnTime - 5;
		

		dialog.StartDialog ("Boss1");
		scene.SetPlaneSpeed (3f);
        yield return new WaitForSeconds(1f);

		while(dialog.handlingDialog) {
			if(stage.stageTimer > 116.7f) break;
			else yield return null;
		}
		Game.control.sound.PlayMusic ("Boss", 1);

        
        while(!stage.bossScript.life.dead) yield return null;

        Game.control.sound.FadeOutMusic();
        stage.ToggleTimer(false);
		yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => stage.CheckIfAllPickUpsGone() == true);

		dialog.StartDialog ("Boss1_1");
		while (dialog.handlingDialog) yield return null;

		yield return new WaitForSeconds(2f);

		stage.EndHandler ("StageComplete");
	}

	public override void InitWaves(float difficultyMultiplier) {
        VectorLib lib = Game.control.vectorLib;
		StageHandler stage = Game.control.stageHandler;
		stage.waves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;


		//PHASE 1
		mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("L4")});
		p = new P_SingleHoming();
		p.SetSprite ("BigCircle", "Big", "Red", "Huge");
		stage.NewWave (new Wave (3f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("R4")});
		p = new P_SingleHoming();
		p.SetSprite ("BigCircle", "Big", "Red", "Huge");
		stage.NewWave (new Wave (5f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_horn"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_MusicalNotes();
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (6f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_sitar"));

			
		//2ND PHASE

		if(difficultyMultiplier > 3){
			mp = new EnemyMovementPattern(lib.GetVector("I1"));
			mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("R4")});
			p = new P_SingleHoming();
			p.SetSprite ("BigCircle", "Big", "Red", "Huge");	
			stage.NewWave (new Wave (8f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_flute"));
				
			mp = new EnemyMovementPattern(lib.GetVector("C1"));
			mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("L4")});
			p = new P_SingleHoming();
			p.SetSprite ("BigCircle", "Big", "Red", "Huge");	
			stage.NewWave (new Wave (14f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_horn"));
		}

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("R4")});
		p = new P_MusicalNotes();
		p.bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (9f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));



		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("F3", 1), new WayPoint("L4")});
		p = new P_MusicalNotes();
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (17f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_flute"));

			
		//INTERLUDE -> PHASE3

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("R4")});
		p = new P_MusicalNotes();
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (28f, mp, p, 5, false, 0,  3f / difficultyMultiplier,  "gand_horn"));
		
		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 4), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Green", "Small");	 
		stage.NewWave (new Wave (30f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_sitar"));
		
		
		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("R4")});
		p = new P_MusicalNotes();
		p.bulletCount =  10;
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (33f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("R4")});
		p = new P_Circle();
		p.bulletCount =  10;
		p.BMP = new BMP_Explode(p, 7f);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (34f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));


		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_Circle();
		p.bulletCount =  10;
		p.BMP = new BMP_Explode(p, 7f);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (38f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_horn"));

		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 4), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Green", "Small");
		stage.NewWave (new Wave (41f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_sitar"));


		mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 4), new WayPoint("L4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
		stage.NewWave (new Wave (41.5f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_flute"));


		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_MusicalNotes();
		p.bulletCount =  10;
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (43f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_horn"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_Circle();
		p.bulletCount =  10;
		p.BMP = new BMP_Explode(p, 7f);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (45f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_horn"));



		//MID-BOSS
		//mp = new EMP_EnterFromTop();
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		//mp.leaveDir = lib.GetVector("XU");
		mp.stayTime = 23f;
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		Wave midBoss = new Wave(mp, 55f, 150, false, 1); //55f
		midBoss.SetUpBoss (0.5f, "Asura", true);
		stage.NewWave (midBoss);
        



		//CONTD
		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_Circle();
		p.BMP = new BMP_Explode(p, 7f);
		p.SetSprite ("Circle", "Glow", "Green", "Small");	 
		stage.NewWave (new Wave (83f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_horn"));

		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 4), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_MusicalNotes();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (85f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_sitar"));

		mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 4), new WayPoint("L4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Yellow", "Small");
		stage.NewWave (new Wave (86f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_MusicalNotes();
		p.BMP = new BMP_Explode(p, 7f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (87f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_sitar"));

		mp = new EnemyMovementPattern(lib.GetVector("K3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 1), new WayPoint("L4")});
		p = new P_Circle();
		p.BMP = new BMP_Explode(p, 7f);
		p.SetSprite ("Circle", "Glow", "Green", "Small");	 
		stage.NewWave (new Wave (89f, mp, p, 5, false, 0, 3f / difficultyMultiplier, "gand_horn"));

		mp = new EnemyMovementPattern(lib.GetVector("A3"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("R4")});
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * (difficultyMultiplier / 2));
		p.BMP = new BMP_Explode(p, 7f);
		p.SetSprite ("Circle", "Glow", "Green", "Small");	 
		stage.NewWave (new Wave (90f, mp, p, 3, false, 0, 3f / difficultyMultiplier, "gand_flute"));

		mp = new EnemyMovementPattern(lib.GetVector("I1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("I3", 4), new WayPoint("L4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Green", "Medium");	 
		stage.NewWave (new Wave (90f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_horn"));

		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 4), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_Circle();
		p.bulletCount =  Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
		p.SetSprite ("Circle", "Glow", "Yellow", "Small");	 
		stage.NewWave (new Wave (90.5f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_sitar"));

		mp = new EnemyMovementPattern(lib.GetVector("C1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 4), new WayPoint("R4")});
		mp.speed = 2f;
		p = new P_MusicalNotes();
		p.bulletCount = Mathf.CeilToInt(4 * difficultyMultiplier);
		p.BMP = new BMP_Explode(p, 11f);
        p.BMP.retainSpriteRotation = true;
		p.SetSprite ("Note", "NoStem", "Green", "Small");	 
		stage.NewWave (new Wave (93f, mp, p, 3, false, 40, 3f / difficultyMultiplier, "gand_sitar"));

		//BOSS 1
		//mp = new EMP_EnterFromTop();
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});

		boss = new Wave(mp, 105f, 600, true, 2);
		boss.SetUpBoss (1, "Maaya, Forest Guard", false);
		stage.NewWave (boss);

	}
/*



	//BOSDEBUG
		mp = new EnemyMovementPattern(lib.GetVector("X1"));
		mp.SetWayPoints(new List<WayPoint>(){new WayPoint("X3")});
		

		boss = new Wave(mp, 1f, 30, true, 2);
		boss.SetUpBoss (1, "Maaya, TEST SUBJECT", false);
		stage.NewWave (boss);

	}
    
    */
}