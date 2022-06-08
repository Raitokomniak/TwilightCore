using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLib : MonoBehaviour
{   

	public float OOBTop;
	public float OOBBot;
	public float OOBRight;
	public float OOBLeft;
			

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


    public void InitVectorLib(){
		OOBTop = 27;
		OOBBot = 0;
		OOBRight = 32f;
		OOBLeft = 0;

		centerTopOOB = GetVector("X1");
		rightWallTopSide = GetVector("J3");
		leftWallTopSide = GetVector("A3");

		centerTop = GetVector("X3");
		topWallLeftSide = GetVector("C1");
		topWallRightSide = GetVector("H1");
		botWallLeftSide = GetVector("C10");
		botWallRightSide = GetVector("H10");

		//DEFAULTS FOR ENTER/LEAVE
		//enterRight = new Vector3(3, 6, 0);
		enterRight = GetVector("H3");
		enterCenter = GetVector("X3");
		enterCenterBoss = GetVector("X3");

		enterLeft = GetVector("C3");
		enterRightWallBotSide = GetVector("H8");
		enterLeftWallBotSide = GetVector("C8");

		leaveRight = GetVector("R3");
		leaveCenter = GetVector("XY");
		leaveLeft = GetVector("L3");
	}

	public Vector3 GetVector(string coordinate){
		if		(coordinate == "CenterVer") return new Vector3(12.87f,0,0);
		else if (coordinate == "CenterHor") return new Vector3(17.3f,0,0);
		
		float ver = 0;
		float hor = 0;

		if(coordinate[0] == 'X') hor = 17.3f; 	//DEAD CENTER
		if(coordinate[1] == 'Y') ver = 12.87f; 	//DEAD CENTER
		if(coordinate[1] == 'U') ver = OOBTop; 	//OUT UP
		if(coordinate[1] == 'D') ver = OOBBot; 	// OUT DOWN
		if(coordinate[0] == 'L') hor = 0f; 	//LEFT OUT
		if(coordinate[0] == 'R') hor = 33; 		//RIGHT OUT
		
		if(coordinate[0] == 'A') hor = 3.8f;
		if(coordinate[0] == 'B') hor = 7.5f;
		if(coordinate[0] == 'C') hor = 10.5f;
		if(coordinate[0] == 'D') hor = 13.4f;
		if(coordinate[0] == 'E') hor = 16.3f;
		if(coordinate[0] == 'F') hor = 19.3f;
		if(coordinate[0] == 'G') hor = 22.3f;
		if(coordinate[0] == 'H') hor = 24.9f;
		if(coordinate[0] == 'I') hor = 27.9f;
		if(coordinate[0] == 'J') hor = 30.9f;

		if(coordinate[1] == '1') ver = 26;
		if(coordinate[1] == '2') ver = 23;
		if(coordinate[1] == '3') ver = 20.2f;
		if(coordinate[1] == '4') ver = 17.3f;
		if(coordinate[1] == '5') ver = 14.3f;
		if(coordinate[1] == '6') ver = 12.5f;
		if(coordinate[1] == '7') ver = 9.4f;
		if(coordinate[1] == '8') ver = 6.2f;
		if(coordinate[1] == '9') ver = 3.1f;
		if(coordinate.Length > 2) //IF 10
			if(coordinate[2] == '0') ver = 0;

		return new Vector3(hor, ver, 0);
	}
}
