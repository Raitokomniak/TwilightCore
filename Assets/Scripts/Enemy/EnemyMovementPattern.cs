using UnityEngine;
using System.Collections;

public class EnemyMovementPattern
{
	EnemyMovement m;
	EnemyLib lib;

	public float speed;
	public string name;
	public Vector3 targetPos;
	public bool infinite;
	public float stayTime;

	public Vector3 enterDir;
	//public string enterDir;
	//public string leaveDir;
	public Vector3 leaveDir;

	public int direction;
	public bool goingRight;
	public bool teleport;

	public bool rotateOnAxis;
	public Vector3 centerPoint;
	public Quaternion rotation;

	public EnemyMovementPattern (string _name, Vector3 _targetPos, bool _infinite, float _stayTime)
	{
		name = _name;
		targetPos = _targetPos;
		infinite = _infinite;
		stayTime = _stayTime;
		speed = 3f;
		teleport = false;
		rotateOnAxis = false;
	}

	public EnemyMovementPattern (EnemyMovementPattern p)
	{
		name = p.name;
		speed = p.speed;
		targetPos = p.targetPos;
		infinite = p.infinite;
		stayTime = p.stayTime;
		teleport = false;
		rotateOnAxis = false;
	}

	public IEnumerator Execute (EnemyMovement _m)
	{
		m = _m;
		Vector3 vector = new Vector3 (targetPos.x, targetPos.y, 0f);
		centerPoint = m.transform.position;
		lib = Game.control.enemyLib;

		m.teleporting = false;

		if (teleport) {
			m.teleporting = true;
			m.EnableSprite(false);
		} 

		if (name == "CenterHor")
			vector.y = _m.transform.position.y;
		else if (name == "CenterVer")
			vector.x = _m.transform.position.x;

		UpdateDirection (vector.x, vector.y);

		if (teleport) {
			yield return new WaitUntil (() => CheckIfReachedDestination (m) == true);
			m.EnableSprite(true);
			m.teleporting = false;
		}



		switch (name) { 
		case "ZigZag":	//Repeatable
			if(direction == 1){
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 6f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 2f);
			}
			else if(direction == -1){
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 6f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 2f);
			}
			else if(direction == 2){
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 6f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 2f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 2f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 6f);
			}
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			UpdateDirection (-20f, 6f);
			break;
		case "ZigZagBoss":{
			if(direction == 1){
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 6f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 2f);
			}
			else if(direction == -1){
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 6f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 2f);
			}
			else if(direction == 2){
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 6f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 2f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (-14f, 2f);
				yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
				UpdateDirection (4f, 6f);
			}
			break;
		}
		case "SnakeRightToLeft": //Repeatable
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);

			UpdateDirection (leaveDir.x, 6f);
			break;
		case "DoubleSnake": //Repeatable
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			if (leaveDir.x > 0){
				UpdateDirection (-12f, 6f);
			}
			else {
				UpdateDirection (2f, 6f);
			}
			
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			if (leaveDir.x > 0)
				UpdateDirection (lib.rightOut, 6f);
			else
				UpdateDirection (lib.leftOut, 6f);
			break;
		case "EnterLeave":
			if(enterDir != Vector3.zero) UpdateDirection(enterDir.x, enterDir.y);
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			//_m.Animate ("Float");
			
			if (stayTime > 0) {
				yield return new WaitForSeconds (stayTime);
				//if (leaveDir == "Left")
					
				//else if (leaveDir == "Right")
				//	UpdateDirection (10f, 6f);
				//else if (leaveDir == "Up")
				//	UpdateDirection (lib.centerX, 13f);
			}
			
			UpdateDirection (leaveDir.x, leaveDir.y);
			/*
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			//_m.Animate ("Float");
			if (stayTime <= 0) stayTime = 1f;
			yield return new WaitForSeconds (stayTime);

			if (leaveDir == "Left") UpdateDirection (lib.leftOut, _m.transform.position.y);
			else if (leaveDir == "Right") UpdateDirection (lib.rightOut, _m.transform.position.y);
			else if (leaveDir == "Up") UpdateDirection (_m.transform.position.x, 13f);

			//UpdateDirection (-20f, 6f);
			break;
			*/
			break;
		case "Enter":
			
			//UpdateDirection (lib.centerX, 13f);
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			//_m.Animate ("Float");
			if (stayTime > 0) yield return new WaitForSeconds (stayTime);
			//UpdateDirection (leaveDir.x, leaveDir.y);
			break;
		case "Rocking":
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			UpdateDirection (-11f, 8f);
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			//yield return new WaitForSeconds(3);
			//UpdateDirection (0f, 8f);
			break;
		case "Swing":
			centerPoint = new Vector3 (-6, 13, 0);
			yield return new WaitForSeconds (1f);
			rotation = m.transform.rotation;
			_RotateOnAxis (50f * speed);
			yield return new WaitForSeconds (4f);
			rotateOnAxis = false;
			m.moving = false;
			break;
		case "SwipeLeftToRight":
			yield return new WaitForSeconds (2f);
			while (!_m.GetComponent<Phaser> ().endOfPhase) {
				UpdateDirection (1f, 6f);
				yield return new WaitForSeconds (5f);
				UpdateDirection (vector.x, vector.y);
				yield return new WaitForSeconds (5f);
			}
			break;

		case "Leaving":
			yield return new WaitForSeconds (4f);
			//GameObject.Destroy (_m.gameObject);
			break;
		default:
			break;
		}
	}

	float Mirror (float pos)
	{
		float mirroredPos = lib.centerX + Mathf.Abs (pos - lib.centerX);
		return mirroredPos;
	}

	public Vector3 RotateOnAxis ()
	{
		return centerPoint;
	}

	void _RotateOnAxis (float _speed)
	{
		speed = _speed * direction;
		rotateOnAxis = true;
	}

	public void Customize (string key, float value)
	{
		switch (key) {
		case "Teleport":
			if (value == 0) {
				teleport = false;
			} else
				teleport = true;
			break;
		case "Speed":
			speed = value;
			break;
		case "Direction":
			direction = Mathf.RoundToInt (value);
			break;
		case "StayTime":
			stayTime = value;
			break;
		}
	}

	public void Customize(string key, Vector3 value){
		switch(key){
			case "EnterDir":
			targetPos = value;
			break;
		}
	}

	public void Customize (string key, string value)
	{
		switch (key) {

		case "LeaveDir":
			if(value == "Right") leaveDir = new Vector3(Game.control.enemyLib.rightOut, 6f, 0);
			else if(value == "Left") leaveDir = new Vector3(Game.control.enemyLib.leftOut, 6f, 0);
			else if(value == "Center") leaveDir = new Vector3(Game.control.enemyLib.centerX, 18, 0);
			break;
		case "EnterDir":
			if(value == "Right") enterDir = new Vector3(3, 6, 0);
			else if(value == "Left") enterDir = new Vector3(-14, 6, 0);
			else if(value == "Center") enterDir = new Vector3(Game.control.enemyLib.centerX, 6, 0);
			break;
		}
	}


	public bool CheckIfReachedDestination (EnemyMovement _m)
	{
		float x = _m.transform.position.x;
		float y = _m.transform.position.y;
		float treshhold = 0.5f;
		if(Mathf.Abs(x-targetPos.x) <= 0.5f && Mathf.Abs(y-targetPos.y) <= treshhold) return true;
		else return false;
	}

	private void UpdateDirection (float h, float v)
	{
		//if(m.moving) m.SmoothAcceleration();
		if (h < targetPos.x) {
			goingRight = false;
		} else {
			goingRight = true;
		}

		targetPos = new Vector3 (h, v, 0f);
	}
}
