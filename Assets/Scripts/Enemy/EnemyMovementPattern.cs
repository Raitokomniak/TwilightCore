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

	public string leaveDir;

	public int direction;

	public bool reached;
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

		m.floating = false;
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
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			UpdateDirection (-14f, 6f);
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			UpdateDirection (10f, 2f);
			break;
		case "SnakeRightToLeft": //Repeatable
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			if (leaveDir == "Right")
				UpdateDirection (2f, 6f);
			else
				UpdateDirection (-12f, 6f);
			
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			if (leaveDir == "Right")
				UpdateDirection (lib.farRight, 6f);
			else
				UpdateDirection (lib.farLeft, 6f);
			break;
		case "EnterLeave":
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			//_m.Animate ("Float");
			if (stayTime > 0) {
				yield return new WaitForSeconds (stayTime);
				//if (leaveDir == "Left")
					UpdateDirection (-20f, 6f);
				//else if (leaveDir == "Right")
				//	UpdateDirection (10f, 6f);
				//else if (leaveDir == "Up")
				//	UpdateDirection (lib.centerX, 13f);
			}
			break;
		case "Rocking":
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
			UpdateDirection (-11f, 8f);
			yield return new WaitUntil (() => CheckIfReachedDestination (_m) == true);
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
		case "LeaveDir":
			
			break;
		
		}
	}

	public void Customize (string key, string value)
	{
		switch (key) {

		case "LeaveDir":
			leaveDir = value;
			break;

		}
	}


	public bool CheckIfReachedDestination (EnemyMovement _m)
	{
		int x = Mathf.RoundToInt (_m.transform.position.x);
		int y = Mathf.RoundToInt (_m.transform.position.y);

		if (Mathf.Approximately (x, targetPos.x)
		    && Mathf.Approximately (y, targetPos.y))
			reached = true;
		else
			reached = false;

		return reached;
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
