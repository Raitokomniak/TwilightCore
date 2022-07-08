using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletMovementPattern
{
    public string name;

	public GameObject bullet;
	public Vector3 centerPoint;
	
	public float targetMagnitude;
	public bool isHoming;
	public bool startHoming;
	public bool isMoving;

	public bool randomDirs; //FOR SLOWWAVING ONLY BUT...

	public bool forceRotation;

	public bool rotateOnAxis = false; //DEFAULT
	public int rotationDir;
	public bool dontDestroy;

	public float movementSpeed;
	public float accelMax = 6f;
	public float accelSpeed = 1f;
	public Quaternion rotation;
	public Quaternion spriteRotation;

	public Vector3 scale;
	public bool forceScale = false;

	public float waitAndExplodeWaitTime;

    public bool waitToTrail = false;
	public bool trail;

	public Pattern pattern;
	public int layer;
	public bool moveWithForce = false;

	public float accelIniSpeed;
	public bool accelerating;


    public string hitBoxType = "Circle"; //DEFAULT  

//////////////////////////////////////////////////////////////////////////////

    public BulletMovement bulletMovement;
    public Sprite bulletSprite;

    public BoxCollider2D bulletBoxCollider;
    public CircleCollider2D bulletCircleCollider;

    public HomingWarningLine bulletHomingWarningLine;


    BulletMovementPattern copiedBMP;

    public void ReceiveBulletData(GameObject _bullet){
       bullet = _bullet;
       bulletSprite = bullet.GetComponentInChildren<SpriteRenderer>().sprite;
       bulletMovement = bullet.GetComponent<BulletMovement>();
       bulletBoxCollider = bullet.GetComponent<BoxCollider2D>();
       bulletCircleCollider = bullet.GetComponent<CircleCollider2D>();
       bulletHomingWarningLine = bullet.GetComponentInChildren<HomingWarningLine>();
    }
    
	public virtual BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
		Debug.Log("not this getnew");
		return null;
	}

	public BulletMovementPattern CopyValues(string bmpType, BulletMovementPattern _bmp){
		copiedBMP = new BulletMovementPattern();
		if(bmpType == "Aurora") copiedBMP = new BMP_Aurora();
		if(bmpType == "RainDrop") copiedBMP = new BMP_RainDrop();
		if(bmpType == "DownAndExplode") copiedBMP = new BMP_DownAndExplode();
		if(bmpType == "Explode") copiedBMP = new BMP_Explode();
        if(bmpType == "LaserExpand") copiedBMP = new BMP_LaserExpand();
		if(bmpType == "SlowWaving") copiedBMP = new BMP_SlowWaving();
		if(bmpType == "StopAndRotate") copiedBMP = new BMP_StopAndRotate();
		if(bmpType == "TurnToSpears") copiedBMP = new BMP_TurnToSpears();
		if(bmpType == "WaitAndExplode") copiedBMP = new BMP_WaitAndExplode();
		if(bmpType == "WaitToHome") copiedBMP = new BMP_WaitToHome();

		copiedBMP.bullet = _bmp.bullet;
		copiedBMP.centerPoint = _bmp.centerPoint;
		copiedBMP.targetMagnitude = _bmp.targetMagnitude;
		copiedBMP.isHoming = _bmp.isHoming;
		copiedBMP.startHoming = _bmp.startHoming;
		copiedBMP.isMoving = _bmp.isMoving;
		copiedBMP.randomDirs = _bmp.randomDirs;
		copiedBMP.rotateOnAxis = _bmp.rotateOnAxis;
		copiedBMP.dontDestroy = _bmp.dontDestroy;
		copiedBMP.movementSpeed = _bmp.movementSpeed;
		copiedBMP.accelMax = _bmp.accelMax;
		copiedBMP.accelSpeed = _bmp.accelSpeed;
        copiedBMP.accelIniSpeed = _bmp.accelIniSpeed;
        copiedBMP.accelerating = _bmp.accelerating;
		copiedBMP.rotation = _bmp.rotation;
		copiedBMP.scale = _bmp.scale;
		copiedBMP.forceScale = _bmp.forceScale;
		copiedBMP.pattern = _bmp.pattern;
		copiedBMP.layer = _bmp.layer;
		copiedBMP.trail = _bmp.trail;
		copiedBMP.moveWithForce = _bmp.moveWithForce;
		copiedBMP.forceRotation = _bmp.forceRotation;
		copiedBMP.rotationDir = _bmp.rotationDir;
		copiedBMP.waitAndExplodeWaitTime = _bmp.waitAndExplodeWaitTime;
        copiedBMP.hitBoxType = _bmp.hitBoxType;
        copiedBMP.name = _bmp.name;

		return copiedBMP;
	}


	public IEnumerator Execute(GameObject _bullet){
		bullet = _bullet;
        if(name == "aurora" && bullet.GetComponentInChildren<SpriteRenderer>().sprite.name != "Fireball_Glow_Orange") 
            Debug.Log(bullet.GetComponentInChildren<SpriteRenderer>().sprite.name);
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
		rotation = q;
	}

	public void Stop(GameObject bullet){
		isMoving = false;
		bulletMovement.Stop();
	}



	public void StartMoving(float magnitude){
        centerPoint = bullet.transform.position;
		targetMagnitude = magnitude;
		isMoving = true;
	}


	public bool HasReachedDestination(Vector3 targetPosition, BulletMovement _m){
		float x = _m.transform.position.x;
		float y = _m.transform.position.y;
		float threshold = 1f;

		if(Mathf.Abs(x-targetPosition.x) <= threshold && Mathf.Abs(y-targetPosition.y) <= threshold) {
			return true;
		} 
		else {
			return false;
		} 
	}

    public void SmoothAcceleration(){
		accelerating = true;
		accelIniSpeed = accelMax;
		movementSpeed = 0;
        isMoving = true;
	}

	public void CancelAxisRotation(float speed)
	{
		rotateOnAxis = false;
		movementSpeed = speed;
		targetMagnitude = 20f;
	}

	public void RotateOnAxis(int dir, float speed)
	{
		movementSpeed = speed * dir;
		rotateOnAxis = true;
		isMoving = true;
	}

	public void SetSpriteRotation(Vector3 eulers){
		spriteRotation.eulerAngles = eulers;
	}
}