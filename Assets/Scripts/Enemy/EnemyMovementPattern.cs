using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint {
    public string coordinate;
    public bool rotateAround;
    public float rotateTime;
    public int rotateDir;
    public float stayTimeAtWayPoint = 0;

    public WayPoint(string _coordinate){
        coordinate = _coordinate;
    }

	public WayPoint(string _coordinate, float _stayTime){
		coordinate = _coordinate;
		stayTimeAtWayPoint = _stayTime;
	}
    public WayPoint(string _coordinate, float _rotateTime, int _rotateDir){
        coordinate = _coordinate;
        rotateAround = true;
        rotateTime = _rotateTime;
        rotateDir = _rotateDir;
    }
}

public class EnemyMovementPattern
{
	public VectorLib lib;
	
	//spawn pos, waypoints, speed between points, staytime at points
	public Vector3 spawnPosition;
	public List<WayPoint> wayPoints;
	public float speed = 5f; //DEFAULT
	public float stayTime;

	//some modifiers
	public bool teleports;
	public bool infinite;
	public bool hideSpriteOnSpawn;
	public bool disableHitBox;
	public bool force; //if false, lerps

	//make loopdiloops
	public bool rotateOnAxis;
	public int rotationDirection;
	public Vector3 centerPoint;
	public Vector3 previousPoint;


	//updated
	public Vector3 targetPosition;
	public bool goingRight;
	
	public EnemyMovementPattern(){}
	
	//THIS PREVENTS MULTIPLE ENEMIES USING THE SAME INSTANCE OF EMP
	public EnemyMovementPattern(EnemyMovementPattern _emp){
		speed = _emp.speed;
		stayTime = _emp.stayTime;
		spawnPosition = _emp.spawnPosition;
		rotationDirection = _emp.rotationDirection;
		teleports = _emp.teleports;
		rotateOnAxis = _emp.rotateOnAxis;
		centerPoint = _emp.centerPoint;
		hideSpriteOnSpawn = _emp.hideSpriteOnSpawn;
		disableHitBox = _emp.disableHitBox;
		force = _emp.force;
		previousPoint = _emp.previousPoint;
		wayPoints = _emp.wayPoints;
		infinite = _emp.infinite;
	}

    public EnemyMovementPattern(Vector3 _spawnPosition){
        wayPoints = new List<WayPoint>();
        spawnPosition = _spawnPosition;
        force = true;
    }


	IEnumerator ExecuteRoutine(EnemyMovement m){
		if(wayPoints == null || wayPoints.Count  == 0) {
//			Debug.Log("waypoints is null");
			yield break;
		}
		
		foreach(WayPoint w in wayPoints){
                if(w.rotateAround){
                    rotateOnAxis = true;
                    UpdateRotateAxisTarget(lib.GetVector(w.coordinate), w.rotateDir, m);
                    yield return new WaitForSeconds(w.rotateTime);
                    rotateOnAxis = false;
                }
                else {
                    if(teleports){
                        m.teleporting = true;
	                    m.EnableSprite(false);
                    }
                    //m.SmoothAcceleration(3);
                    UpdateDirection(w.coordinate);
                    yield return new WaitUntil (() => HasReachedDestination (m) == true);
                    if(teleports){
                        m.EnableSprite(true);
		                m.teleporting = false;
                    }
                    yield return new WaitForSeconds(w.stayTimeAtWayPoint);
                    
                }
            }
         yield return null;
	}

	public IEnumerator Execute(EnemyMovement enemy){
		lib = Game.control.vectorLib;
		enemy.teleporting = false; // NOT HERE
		IEnumerator executeRoutine = ExecuteRoutine(enemy);
		return executeRoutine;
	}

	public  void SetWayPoints(ICollection _wayPoints){
		wayPoints = _wayPoints as List<WayPoint>;
	}

	public bool HasReachedDestination (EnemyMovement _m)
	{
		float x = _m.transform.position.x;
		float y = _m.transform.position.y;
		float threshold = 1f;

		if(Mathf.Abs(x-targetPosition.x) <= threshold && Mathf.Abs(y-targetPosition.y) <= threshold) {
			previousPoint = _m.transform.position;
			_m.rb.drag = 8f;
			return true;
		} 
		else {
			_m.rb.drag = 5f;
			return false;
		} 
	}


	//with custom threshold for leeway
	public bool HasReachedDestination(EnemyMovement _m, float threshold)
	{
		float x = _m.transform.position.x;
		float y = _m.transform.position.y;

		if(Mathf.Abs(x-targetPosition.x) <= threshold && Mathf.Abs(y-targetPosition.y) <= threshold) {
			previousPoint = _m.transform.position;
			_m.rb.drag = 2f;
			return true;
		} 
		else {
			_m.rb.drag = 6f;
			return false;
		}
	}

	public void UpdateDirection(string grid){
		Vector3 dir = Game.control.vectorLib.GetVector(grid);

		float h = dir.x;
		float v = dir.y;

		//if(m.moving) m.SmoothAcceleration();
		if (h < targetPosition.x) goingRight = false;
		else goingRight = true;

		targetPosition = new Vector3 (h, v, 0f);
	}

	public void UpdateRotateAxisTarget (Vector3 target, int dir, EnemyMovement m)
	{	
		float h = target.x;
		float v = target.y;

		previousPoint = m.transform.position;

		if (h < targetPosition.x) goingRight = false;
		else goingRight = true;

		targetPosition = new Vector3 (h, v, 0f);

		centerPoint.x = (previousPoint.x + targetPosition.x) / 2;
        centerPoint.y = (previousPoint.y + targetPosition.y) / 2;

		rotationDirection = dir;
	}
}
