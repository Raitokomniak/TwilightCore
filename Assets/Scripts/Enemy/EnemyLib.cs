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

	public Vector3 centerTopOOB;
	public Vector3 centerTop;
	public Vector3 rightWallTopSide;
	public Vector3 enterRightWallBotSide;
	public Vector3 topWallLeftSide;
	public Vector3 topWallRightSide;
	public Vector3 botWallLeftSide;
	public Vector3 botWallRightSide;

	public Vector3 leftWallTopSide;
	public Vector3 enterLeftWallBotSide;

	//SOME DEFAULT VALUES FOR ENTER/LEAVE DIRS
	public Vector3 enterRight;
	public Vector3 enterCenter;
	public Vector3 enterCenterBoss;
	public Vector3 enterLeft;

	public Vector3 leaveRight;
	public Vector3 leaveCenter;
	public Vector3 leaveLeft;



	public void InitEnemyLib(){
		centerX = -5.75f;
		centerY = 0f;
		topCenterY = 8f;
		float topOut = 13;

		centerTopOOB = GetVector("X1");
		rightWallTopSide = GetVector("J3");
		//leftWallTopSide = new Vector3 (-18, 6f, 0f); //LEFT WALL
		leftWallTopSide = GetVector("A3");

		centerTop = new Vector3(centerX, 6f, 0f);
		topWallLeftSide = GetVector("C1");
		topWallRightSide = GetVector("H1");
		botWallLeftSide = GetVector("C10");
		botWallRightSide = GetVector("H10");

		//DEFAULTS FOR ENTER/LEAVE
		//enterRight = new Vector3(3, 6, 0);
		enterRight = GetVector("H3");
		enterCenter = new Vector3(centerX, 6, 0);
		enterCenterBoss = new Vector3(centerX, 8, 0);

		enterLeft = GetVector("C3");
		enterRightWallBotSide = GetVector("H8");
		enterLeftWallBotSide = GetVector("C8");

		leaveRight = GetVector("R3");
		leaveCenter = GetVector("XY");
		leaveLeft = GetVector("L3");

		spriteLib = Game.control.spriteLib;

		stageWaves = new ArrayList();
	}

	public Vector3 GetVector(string coordinate){
		if		(coordinate == "CenterVer") return new Vector3(0,0,0);
		else if (coordinate == "CenterHor") return new Vector3(-5.75f,0,0);
		
		float ver = 0;
		float hor = 0;
		
		if(coordinate[0] == 'X') hor = -5.75f; 	//DEAD CENTER
		if(coordinate[1] == 'Y') hor = 14f; 	//TOP OUT
		if(coordinate[0] == 'L') hor = -23f; 	//LEFT OUT
		if(coordinate[0] == 'R') hor = 10; 		//RIGHT OUT
		
		if(coordinate[0] == 'A') hor = -20.8f;
		if(coordinate[0] == 'B') hor = -16.5f;
		if(coordinate[0] == 'C') hor = -13.5f;
		if(coordinate[0] == 'D') hor = -10.4f;
		if(coordinate[0] == 'E') hor = -7.3f;
		if(coordinate[0] == 'F') hor = -4.3f;
		if(coordinate[0] == 'G') hor = -1.3f;
		if(coordinate[0] == 'H') hor = 1.9f;
		if(coordinate[0] == 'I') hor = 4.9f;
		if(coordinate[0] == 'J') hor = 7.9f;

		if(coordinate[1] == '1') ver = 13;
		if(coordinate[1] == '2') ver = 10;
		if(coordinate[1] == '3') ver = 7.2f;
		if(coordinate[1] == '4') ver = 4.3f;
		if(coordinate[1] == '5') ver = 1.3f;
		if(coordinate[1] == '6') ver = -1.5f;
		if(coordinate[1] == '7') ver = -4.4f;
		if(coordinate[1] == '8') ver = -7.2f;
		if(coordinate[1] == '9') ver = -10.1f;
		if(coordinate.Length > 2) //IF 10
			if(coordinate[2] == '0') ver = -13;

		return new Vector3(hor, ver, 0);
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

