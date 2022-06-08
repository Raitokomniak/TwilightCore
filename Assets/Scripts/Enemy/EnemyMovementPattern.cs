using UnityEngine;
using System.Collections;

public class EnemyMovementPattern
{
	public float speed;
	public float stayTime;

	public Vector3 spawnPosition;
	public Vector3 enterDir;
	public Vector3 leaveDir;

	public bool hideSpriteOnSpawn;
	public bool disableHitBox;
	public int movementDirection;
	public bool teleport;
	public bool rotateOnAxis;

	public Vector3 targetPos;
	public bool goingRight;
	public Vector3 centerPoint;

	public bool smoothedMovement;
	public bool smoothArc;

	public Vector3 previousPoint;

	public EnemyMovementPattern(){

	}

	public virtual EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern m){
		Debug.Log("not this one");
		return m;
	}

	public virtual IEnumerator ExecuteRoutine(EnemyMovement _m){
		yield return null;
	}

	public IEnumerator Execute(EnemyMovement enemy){
		enemy.teleporting = false; // NOT HERE
		IEnumerator executeRoutine = ExecuteRoutine(enemy);
		return executeRoutine;
	}

	public EnemyMovementPattern CopyValues(string empType, EnemyMovementPattern _emp){
		EnemyMovementPattern emp = null;
		if(empType == "EnterLeave") emp = new EMP_EnterLeave();
		if(empType == "EnterFromTop") emp = new EMP_EnterFromTop();
		if(empType == "ZigZag") emp = new EMP_ZigZag();
		if(empType == "Snake") emp = new EMP_Snake();
		if(empType == "Rock") emp = new EMP_Rock();
		if(empType == "Teleport") emp = new EMP_Teleport();
		if(empType == "Swing") emp = new EMP_Swing();
		if(empType == "TestPattern") emp = new EMP_TestPattern();
		emp.speed = _emp.speed;
		emp.stayTime = _emp.stayTime;
		emp.spawnPosition = _emp.spawnPosition;
		emp.enterDir = _emp.enterDir;
		emp.leaveDir = _emp.leaveDir;
		emp.movementDirection = _emp.movementDirection;
		emp.teleport = _emp.teleport;
		emp.rotateOnAxis = _emp.rotateOnAxis;
		emp.centerPoint = _emp.centerPoint;
		emp.hideSpriteOnSpawn = _emp.hideSpriteOnSpawn;
		emp.disableHitBox = _emp.disableHitBox;
		emp.smoothedMovement = _emp.smoothedMovement;
		emp.smoothArc = _emp.smoothArc;
		emp.previousPoint = _emp.previousPoint;
		return emp;
	}

	public void ForceLeave(){
		speed = 3f;
		UpdateDirection(leaveDir.x, leaveDir.y);
	}

	float Mirror (float pos)
	{
		float mirroredPos = Game.control.vectorLib.GetVector("X1").x + Mathf.Abs (pos - Game.control.vectorLib.GetVector("X1").x);
		return mirroredPos;
	}

	public void RotateOnAxis (float _speed)
	{
		speed = _speed * movementDirection;
		rotateOnAxis = true;
	}

	public void SetEnterLeaveDirection(Vector3 eDir, Vector3 lDir){
		enterDir = eDir;
		leaveDir = lDir;
	}

	public bool HasReachedDestination (EnemyMovement _m)
	{
		float x = _m.transform.position.x;
		float y = _m.transform.position.y;
		float threshold = 0.5f;

//		Debug.Log("threshX " + Mathf.Abs(x-targetPos.x) + ", threshY " +  Mathf.Abs(y-targetPos.y));
		
		if(Mathf.Abs(x-targetPos.x) <= threshold && Mathf.Abs(y-targetPos.y) <= threshold) {
			previousPoint = _m.transform.position;
			return true;
		} 
		else return false;
	}

	public bool HasReachedSlerp(EnemyMovement _m)
	{
		float x = _m.transform.position.x;
		float y = _m.transform.position.y;
		float threshold = 1f;

		if(Mathf.Abs(x-targetPos.x) <= threshold && Mathf.Abs(y-targetPos.y) <= threshold) {
			previousPoint = _m.transform.position;
			return true;
		} 
		else return false;
	}

	public void UpdateDirection (float h, float v)
	{
		//if(m.moving) m.SmoothAcceleration();
		if (h < targetPos.x) goingRight = false;
		else goingRight = true;

		targetPos = new Vector3 (h, v, 0f);
	}

	public void UpdateDirection (Vector3 dir)
	{	
		float h = dir.x;
		float v = dir.y;

		//if(m.moving) m.SmoothAcceleration();
		if (h < targetPos.x) goingRight = false;
		else goingRight = true;

		targetPos = new Vector3 (h, v, 0f);
	}

	public void UpdateDirection(string grid){
		Vector3 dir = Game.control.vectorLib.GetVector(grid);

		float h = dir.x;
		float v = dir.y;

		//if(m.moving) m.SmoothAcceleration();
		if (h < targetPos.x) goingRight = false;
		else goingRight = true;

		targetPos = new Vector3 (h, v, 0f);
	}

	public void UpdateSlerpDirection (Vector3 target, int dir)
	{	
		float h = target.x;
		float v = target.y;

		//if(m.moving) m.SmoothAcceleration();
		if (h < targetPos.x) goingRight = false;
		else goingRight = true;

		targetPos = new Vector3 (h, v, 0f);

		centerPoint.x = (previousPoint.x + targetPos.x) / 2;
        centerPoint.y = (previousPoint.y + targetPos.y) / 2;

		movementDirection = dir;
	}
}
