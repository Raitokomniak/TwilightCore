﻿using System;
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

	public Vector3 middleTop;
	public Vector3 topRight;
	public Vector3 leftTop;
	public Vector3 rightTop;
	public Vector3 topLeft;

	public Wave bossMid1;
	public Wave boss1;
	public Wave boss2;


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

	public void InitEnemyLib(){
		spriteLib = Game.control.spriteLib;

		CreateMovementPatterns();
		CreatePatterns();
		CreateBosses();

		stageWaves = new ArrayList();
	}

	void CreateMovementPatterns()
	{
		leaving = 			new EnemyMovementPattern("Leaving", new Vector3(centerX, 14f, 0f), false, 0);
		zigZag = 			new EnemyMovementPattern("ZigZag", new Vector3(0f, 8f, 0f), false, 0);
		stopOnce = 			new EnemyMovementPattern("SnakeRightToLeft", new Vector3(0f, 6f, 0f), false, 0);
		enterFromTop = 		new EnemyMovementPattern("Enter", new Vector3(centerX, 8f, 0f), false, 0);
		enterLeave = 		new EnemyMovementPattern ("EnterLeave", new Vector3 (-7f, 8f, 0f), false, 18);
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

	public void InitWaves(int stage){
		if(stage == 1){
			gameObject.AddComponent<Stage1>();
		}
		else if(stage == 2){
			gameObject.AddComponent<Stage2>();
		} 
	}

	public void NewWave(ArrayList stage, Wave w, ArrayList spawnPositions){
		w.SetSpawnPositions (spawnPositions);

		if (w.isBoss || w.isMidBoss) {
			w.sprite =spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		stage.Add (w);
	}


	void CreateBosses()
	{
		EnemyMovementPattern mp = new EnemyMovementPattern (enterLeave);

		mp = new EnemyMovementPattern (enterLeave);
		mp.Customize ("StayTime", 27f);
		mp.Customize ("LeaveDir", "Up");
		bossMid1 = new Wave(55f, mp, null, 1,  false, 150, false, 3f, 1);
		bossMid1.SetUpBoss (0.5f, "Asura");
		bossMid1.isMidBoss = true;
	
		mp = new EnemyMovementPattern (enterLeave);
		mp.Customize ("StayTime", 0);
		boss1 = new Wave(96f, mp, null, 1,  false, 100, true, 3f, 2);
		boss1.SetUpBoss (1, "Forest Guardian");
		
		boss2 = new Wave(30f, null, null, 1,  false, 200, true, 3f, 2);
		boss2.SetUpBoss (2, "Spider Queen");
		boss2.movementPattern = enterFromTop;
	}
}

