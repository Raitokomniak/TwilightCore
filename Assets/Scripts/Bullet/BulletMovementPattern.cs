using UnityEngine;
using System.Collections;

public class BulletMovementPattern
{
	public GameObject bullet;
	public Vector3 centerPoint;
	
	public float targetMagnitude;
	public bool isHoming;
	public bool startHoming;
	public bool isMoving;

	public bool randomDirs; //FOR SLOWWAVING ONLY BUT...

	public bool rotateOnAxis = false; //DEFAULT
	public bool dontDestroy;

	public float movementSpeed;
	public float accelMax = 6f;
	public float accelSpeed = 1f;
	public Quaternion rotation;
	public Vector3 scale;
	public bool forceScale = false;

	public bool trail;

	public Pattern pattern;
	public int layer;
	public int laserIndex;

	public bool moveWithForce;

	public BulletMovementPattern(){}

	public virtual BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
		Debug.Log("not this getnew");
		return null;
	}

	public BulletMovementPattern CopyValues(string bmpType, BulletMovementPattern _bmp){
		BulletMovementPattern bmp = new BulletMovementPattern();
		if(bmpType == "Aurora") bmp = new BMP_Aurora();
		if(bmpType == "RainDrop") bmp = new BMP_RainDrop();
		if(bmpType == "DownAndExplode") bmp = new BMP_DownAndExplode();
		if(bmpType == "Explode") bmp = new BMP_Explode();
		if(bmpType == "LaserExpand") bmp = new BMP_LaserExpand();
		if(bmpType == "LaserPendulum") bmp = new BMP_LaserPendulum();
		if(bmpType == "LaserRotate") bmp = new BMP_LaserRotate();
		if(bmpType == "SlowWaving") bmp = new BMP_SlowWaving();
		if(bmpType == "Stop") bmp = new BMP_Stop();
		if(bmpType == "StopAndRotate") bmp = new BMP_StopAndRotate();
		if(bmpType == "TurnToSpears") bmp = new BMP_TurnToSpears();
		if(bmpType == "WaitAndExplode") bmp = new BMP_WaitAndExplode();
		if(bmpType == "WaitToHome") bmp = new BMP_WaitToHome();

		bmp.bullet = _bmp.bullet;
		bmp.centerPoint = _bmp.centerPoint;
		bmp.targetMagnitude = _bmp.targetMagnitude;
		bmp.isHoming = _bmp.isHoming;
		bmp.startHoming = _bmp.startHoming;
		bmp.isMoving = _bmp.isMoving;
		bmp.randomDirs = _bmp.randomDirs;
		bmp.rotateOnAxis = _bmp.rotateOnAxis;
		bmp.dontDestroy = _bmp.dontDestroy;
		bmp.movementSpeed = _bmp.movementSpeed;
		bmp.accelMax = _bmp.accelMax;
		bmp.accelSpeed = _bmp.accelSpeed;
		bmp.rotation = _bmp.rotation;
		bmp.scale = _bmp.scale;
		bmp.forceScale = _bmp.forceScale;
		bmp.pattern = _bmp.pattern;
		bmp.layer = _bmp.layer;
		bmp.laserIndex = _bmp.laserIndex;
		bmp.trail = _bmp.trail;
		bmp.moveWithForce = _bmp.moveWithForce;

		return bmp;
	}


	public IEnumerator Execute(GameObject _bullet){
		bullet = _bullet;
		isMoving = true;
		centerPoint = _bullet.transform.position;
		IEnumerator executeRoutine = ExecuteRoutine();
		return executeRoutine;
	}

 	public virtual IEnumerator ExecuteRoutine(){
	 	yield return null;
	}

	public void FindPlayer(GameObject _bullet){
		Vector3 player = Game.control.player.gameObject.transform.position;
		Vector3 vectorToTarget = player - _bullet.transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		rotation = Quaternion.Slerp(rotation, q, Time.deltaTime * 10f);
	}

	public void Stop(GameObject bullet){
		isMoving = false;
	}

	public void CorrectRotation(){
		if (pattern.bullets.Count != 0)
			pattern.bullets.RemoveAt (pattern.bullets.Count - 1);
		rotation = Quaternion.Euler (0f, 0f, (pattern.bullets.Count - 1) * (360 / pattern.bulletCount));
	}

	public void Explode(float magnitude){
		targetMagnitude = magnitude;
		isMoving = true;
	}

	public void Explode(bool animate, GameObject bullet, float _targetMagnitude, float targetScale)
	{
		centerPoint = bullet.transform.position;
		if (animate){
			pattern.Animate(targetScale, 1, centerPoint);
		}
		isMoving = true;
		targetMagnitude = _targetMagnitude;
	}

	public void CancelAxisRotation(float speed)
	{
		rotateOnAxis = false;
		movementSpeed = speed;
		targetMagnitude = 20f;
	}
	public void _RotateOnAxis(GameObject bullet, int dir, float speed)
	{
		movementSpeed = speed;
		movementSpeed = movementSpeed * dir;
		rotateOnAxis = true;
		isMoving = true;
	}

	public void RotateOnAxis(GameObject bullet, int dir, float speed)
	{
		movementSpeed = speed;
		movementSpeed = movementSpeed * dir;
		rotateOnAxis = true;
		isMoving = true;
	}

	public Vector3 RotateOnAxis()
	{
		return centerPoint;
	}

}