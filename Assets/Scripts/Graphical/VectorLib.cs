using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLib : MonoBehaviour
{   
	//out of bounds values
	public float OOBTop;
	public float OOBBot;
	public float OOBRight;
	public float OOBLeft;

    void Start(){
        Init();
    }

    public void Init(){
		OOBTop = 27;
		OOBBot = 0;
		OOBRight = 33f;
		OOBLeft = 0;
	}

	public Vector3 GetVector(string coordinate){
		if		(coordinate == "CenterVer") return new Vector3(12.87f,0,0);
		else if (coordinate == "CenterHor") return new Vector3(17.3f,0,0);
		
		float ver = 0;
		float hor = 0;

		if(coordinate[0] == 'X') hor = 17.3f; 	//DEAD CENTER
		if(coordinate[1] == 'Y') ver = 12.87f; 	//DEAD CENTER
		if(coordinate[1] == 'U') ver = OOBTop + 6; 		//OUT UP
		if(coordinate[1] == 'D') ver = OOBBot - 1; 		// OUT DOWN
		if(coordinate[0] == 'L') hor = OOBLeft - 1; 	//LEFT OUT
		if(coordinate[0] == 'R') hor = OOBRight + 1; //RIGHT OUT
		
		
		if(coordinate[0] == 'A') hor = 1.8f;
		if(coordinate[0] == 'B') hor = 5.95f;
		if(coordinate[0] == 'C') hor = 8.85f;
		if(coordinate[0] == 'D') hor = 11.4f;
		if(coordinate[0] == 'E') hor = 12.3f;
		if(coordinate[0] == 'F') hor = 17.75f;
		if(coordinate[0] == 'G') hor = 20.45f;
		if(coordinate[0] == 'H') hor = 23.45f;
		if(coordinate[0] == 'I') hor = 26.3f;
		if(coordinate[0] == 'J') hor = 29.3f;
		if(coordinate[0] == 'K') hor = 32.2f;

		if(coordinate[1] == '1') ver = 26;
		if(coordinate[1] == '2') ver = 23;
		if(coordinate[1] == '3') ver = 20.2f;
		if(coordinate[1] == '4') ver = 17.3f;
		if(coordinate[1] == '5') ver = 14.3f;
		if(coordinate[1] == '6') ver = 11.5f;
		if(coordinate[1] == '7') ver = 8.5f;
		if(coordinate[1] == '8') ver = 5.6f;
		if(coordinate[1] == '9') ver = 2.74f;
		if(coordinate.Length > 2) //IF 10
			if(coordinate[2] == '0') ver = 0;

		return new Vector3(hor, ver, 0);
	}
}
