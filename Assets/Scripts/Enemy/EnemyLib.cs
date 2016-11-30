using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLib : MonoBehaviour
{

	SpriteLibrary spriteLib;

	public Pattern singleHoming;
	public Pattern circle;
	public Pattern maelStrom;
	public Pattern pacMan;
	public Pattern spiderWeb;
	public Pattern giantWeb;
	public Pattern laser;
	public Pattern spiral;
	public Pattern curtain;

	public EnemyMovementPattern leaving;
	public EnemyMovementPattern zigZag;
	public EnemyMovementPattern stopOnce;
	public EnemyMovementPattern enterFromTop;
	public EnemyMovementPattern enterLeave;

	public EnemyMovementPattern rocking;
	public EnemyMovementPattern centerHor;
	public EnemyMovementPattern centerVer;
	public EnemyMovementPattern swipeLeftToRight;

	public ArrayList stageWaves;

	public float centerX;
	public float centerY;
	public float farRight;
	public float farLeft;

	Vector3 middleTop;
	Vector3 topRight;
	public Vector3 leftTop;
	public Vector3 rightTop;
	public Vector3 topLeft;

	Wave bossMid1;
	Wave boss1;
	Wave boss2;


	void Awake(){
		centerX = -6f;
		centerY = 0f;
		farLeft = -22f;
		farRight = 8f;

		middleTop = new Vector3(centerX, 12f, 0f);
		topRight = new Vector3(7f, 6f, 0f);
		topLeft = new Vector3 (-18, 6f, 0f);
		leftTop = new Vector3 (-18, 12f, 0f);
		rightTop = new Vector3 (0, 12f, 0f);



	}

	void Update()
	{
		
	}

	public void InitEnemyLib(){
		spriteLib = GameController.gameControl.spriteLib;

		CreateMovementPatterns();
		CreatePatterns();
		CreateBosses();

		stageWaves = new ArrayList();
	}

	void CreateMovementPatterns()
	{
		leaving = 			new EnemyMovementPattern("Leaving", new Vector3(centerX, 14f, 0f), false, 0);
		zigZag = 			new EnemyMovementPattern("ZigZag", new Vector3(0f, 8f, 0f), false, 0);
		stopOnce = 	new EnemyMovementPattern("SnakeRightToLeft", new Vector3(0f, 6f, 0f), false, 0);
		enterFromTop = 		new EnemyMovementPattern("Enter", new Vector3(centerX, 8f, 0f), false, 0);
		enterLeave = 	new EnemyMovementPattern ("EnterLeave", new Vector3 (-7f, 8f, 0f), false, 18);
		rocking = 			new EnemyMovementPattern("Rocking", new Vector3(-2f, 8f, 0f), true, 0);
		centerHor = 		new EnemyMovementPattern("CenterHor", new Vector3(centerX, 8f, 0f), true, 0);
		centerVer = 		new EnemyMovementPattern("CenterVer", new Vector3(-7f, centerY, 0f), true, 0);
		swipeLeftToRight = new EnemyMovementPattern ("SwipeLeftToRight", new Vector3 (-15, 6f, 0f), true, 0);
	}

	void CreatePatterns()
	{
		singleHoming = new Pattern("SingleHoming", false, 1, 0f, 1f, 1, 100);
		circle = new Pattern("Circle", false, 7, 0f, 1f, 1, 100);
		maelStrom = new Pattern("Maelstrom", false, 10, 0f, 0.2f, 1, 100);
		pacMan = new Pattern("PacMan", false, 45, 6f, 1f, 1, 100);
		spiderWeb = new Pattern("SpiderWeb", true, 10, 0f, 3f, 1, 100);
		giantWeb = new Pattern("GiantWeb", true, 30, 0f, 1f, 2, 12);
		laser = new Pattern ("Laser", false, 1, 0, 10, 0, 15);
		spiral = new Pattern ("Spiral", false, 30, 0f, 0.001f, 1, 2);
		curtain = new Pattern ("Curtain", false, 8, 0f, .1f, 0, 10);
	}

	public void InitWaves(int stage)
	{
		stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

		switch(stage)
		{
		case 1:
			//											

			//1ST PHASE
			mp = new EnemyMovementPattern (enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", .5f);
			p = new Pattern (singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (1f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { rightTop });

			mp = new EnemyMovementPattern (enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (3f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { leftTop });

			//2ND PHASE
			mp = new EnemyMovementPattern (enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", .5f);
			p = new Pattern (singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (4f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { rightTop });

			mp = new EnemyMovementPattern (enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (15f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { leftTop });

			mp = new EnemyMovementPattern (zigZag);
			p = new Pattern (circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (16f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { rightTop });

			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Right");
			p = new Pattern (circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (17f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { topLeft });


			//PHASE3

			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (28f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { topLeft });

			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (31f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { topRight });

			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (33f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { topRight });

			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (36f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { topLeft });


			mp = new EnemyMovementPattern (enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (41f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { rightTop });

			mp = new EnemyMovementPattern (enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Yellow");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (41.5f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { leftTop });


			//MID-BOSS

			NewWave (stageWaves, bossMid1, new ArrayList { middleTop });


			//CONTD
			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (82f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { topRight });


			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (87f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { topRight });

			mp = new EnemyMovementPattern (stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (90f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { topLeft });

			mp = new EnemyMovementPattern (enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (90f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { rightTop });

			mp = new EnemyMovementPattern (enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Yellow");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			NewWave (stageWaves, new Wave (90.5f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { leftTop });


			NewWave (stageWaves, boss1, new ArrayList { middleTop });



			break;
		case 2:

		//															enmyCnt simul hlth 	isBoss, cd, hlthBars, spawnPositions
			p = new Pattern (singleHoming);
			p.Customize(new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite("Circle", "Big", "Red");
			NewWave (stageWaves, new Wave (2f, zigZag, p, 5, false, 0, false, 1f, 0), new ArrayList { middleTop });
			/*w = (Wave)stage1Waves [stage1Waves.Count - 1];
			w.shootPattern.Customize(new BulletMovementPattern (true, null, 0.5f, w.shootPattern, 0, 14));
			w.shootPattern.SetSprite("Circle", "Big", "Red");

*/
			p = new Pattern (circle);
			p.Customize(new BulletMovementPattern (true, "Explode", 2f, p, 0, 14));
			p.SetSprite("Circle", "Glow", "Green");
			NewWave (stageWaves, new Wave (3f, stopOnce, p, 6, false, 0, false, 1f, 0), new ArrayList { topRight });

			//NewWave (stage1Waves, new Wave (6f, zigZag, singleHoming, 6, false, 0, false, 1f,	0), new ArrayList { middleTop });
			//NewWave (stage1Waves, new Wave (10f, zigZag, circle, 6, false, 0, false, 1f,	0), new ArrayList { middleTop });
			p = new Pattern (circle);
			p.Customize(new BulletMovementPattern (true, "Explode", 6f, p, 0, 14));
			p.SetSprite("Circle", "Glow", "Yellow");
			NewWave (stageWaves, new Wave (10f, stopOnce, p, 6, false, 0, false, 1f, 0), new ArrayList { topRight });




			//p = new Pattern (laser);

			/*NewWave (stage1Waves, new Wave (25f, enterAndLeave, p, 2, true, 10, false,	1f, 0), new ArrayList {
				rightTop,
				leftTop
			});*/

			p = new Pattern (singleHoming);
			p.Customize(new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite("Circle", "Big", "Red");
			NewWave (stageWaves, new Wave (15f, zigZag, p, 5, false, 0, false, 1f, 0), new ArrayList { middleTop });
			NewWave (stageWaves, new Wave (20f, stopOnce, p, 6, false, 0, false, 2f,	0), new ArrayList { topRight });

			NewWave (stageWaves, new Wave (boss2), new ArrayList { middleTop });
			break;
		}
	}

	void NewWave(ArrayList stage, Wave w, ArrayList spawnPositions){
		w.SetSpawnPositions (spawnPositions);

		if (w.isBoss) {
//			Debug.Log ("create boss wave");
			w.sprite =spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		stage.Add (w);
	}


	void CreateBosses()
	{
		EnemyMovementPattern mp = new EnemyMovementPattern (enterLeave);
		mp.Customize ("StayTime", 27f);
		mp.Customize ("LeaveDir", "Up");
		bossMid1 = new Wave(55f, mp, null, 1,  false, 150, true, 3f, 1);
		bossMid1.SetUpBoss (0.5f, "Asura");

		mp = new EnemyMovementPattern (enterLeave);
		mp.Customize ("StayTime", 0);
		boss1 = new Wave(96f, mp, null, 1,  false, 200, true, 3f, 2);
		boss1.SetUpBoss (1, "Forest Guardian");

		boss2 = new Wave(30f, null, null, 1,  false, 200, true, 3f, 2);
		boss2.SetUpBoss (2, "Spider Queen");
		boss2.movementPattern = enterFromTop;
	}
}

