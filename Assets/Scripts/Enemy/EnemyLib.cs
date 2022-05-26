using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLib : MonoBehaviour
{
	SpriteLibrary spriteLib;
	public ArrayList stageWaves;

	public float centerX;
	public float centerY;
	public float topCenterY;
	public float rightOut;
	public float leftOut;

	public Vector3 centerTopOOB;
	public Vector3 center;
	public Vector3 rightWallTopSide;
	public Vector3 topWallLeftSide;
	public Vector3 topWallRightSide;
	public Vector3 leftWallTopSide;

	//SOME DEFAULT VALUES FOR ENTER/LEAVE DIRS
	public Vector3 enterRight;
	public Vector3 enterCenter;
	public Vector3 enterCenterBoss;
	public Vector3 enterLeft;

	public Vector3 leaveRight;
	public Vector3 leaveCenter;
	public Vector3 leaveLeft;



	public void InitEnemyLib(){
		centerX = -6f;
		centerY = 0f;
		topCenterY = 8f;
		leftOut = -22f;
		rightOut = 10f;

		centerTopOOB = new Vector3(centerX, 12f, 0f);
		rightWallTopSide = new Vector3(7f, 6f, 0f); //RIGHT WALL
		leftWallTopSide = new Vector3 (-18, 6f, 0f); //LEFT WALL
		center = new Vector3(centerX, 6f, 0f);
		topWallLeftSide = new Vector3 (-14, 12f, 0f); //TOP WALL LEFT  SIDE
		topWallRightSide = new Vector3 (3, 12f, 0f); //TOP WALL RIGHT SIDE


		//DEFAULTS FOR ENTER/LEAVE
		enterRight = new Vector3(3, 6, 0);
		enterCenter = new Vector3(Game.control.enemyLib.centerX, 6, 0);
		enterCenterBoss = new Vector3(Game.control.enemyLib.centerX, 8, 0);
		enterLeft = new Vector3(-14, 6, 0);

		leaveRight = new Vector3(Game.control.enemyLib.rightOut, 6f, 0);
		leaveCenter = new Vector3(Game.control.enemyLib.centerX, 18, 0);
		leaveLeft = new Vector3(Game.control.enemyLib.leftOut, 6f, 0);

		spriteLib = Game.control.spriteLib;

		stageWaves = new ArrayList();
	}

	public void NewWave(Wave w){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		stageWaves.Add(w);
	}

	public void NewWave(Wave w, List<Vector3> spawnPositions){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		w.spawnPositions = spawnPositions;
		w.FillPositionsArraysByEnemyCount();
		stageWaves.Add(w);
	}

	public void NewWave(Wave w, List<Vector3> spawnPositions, List<Vector3> enterDirections, List<Vector3> leaveDirections){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		w.spawnPositions = spawnPositions;
		w.enterDirections = enterDirections;
		w.leaveDirections = leaveDirections;
		w.FillPositionsArraysByEnemyCount();
		stageWaves.Add(w);
	}

}

