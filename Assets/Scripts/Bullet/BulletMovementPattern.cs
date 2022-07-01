using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletMovementPattern
{
    public WaitForSeconds Waitp1;
    public WaitForSeconds Wait1;
    public WaitForSeconds Wait2;


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

    public void ReceiveBulletData(GameObject _bullet){
       bullet = _bullet;
       bulletSprite = bullet.GetComponentInChildren<SpriteRenderer>().sprite;
       bulletMovement = bullet.GetComponent<BulletMovement>();
       bulletBoxCollider = bullet.GetComponent<BoxCollider2D>();
       bulletCircleCollider = bullet.GetComponent<CircleCollider2D>();
       bulletHomingWarningLine = bullet.GetComponentInChildren<HomingWarningLine>();
    }

	public BulletMovementPattern(){
        Waitp1 = new WaitForSeconds(.1f);
        Wait1 = new WaitForSeconds(1);
        Wait2 = new WaitForSeconds(2);
    }

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
		bmp.trail = _bmp.trail;
		bmp.moveWithForce = _bmp.moveWithForce;
		bmp.forceRotation = _bmp.forceRotation;
		bmp.rotationDir = _bmp.rotationDir;
		bmp.waitAndExplodeWaitTime = _bmp.waitAndExplodeWaitTime;
        bmp.hitBoxType = _bmp.hitBoxType;
        bmp.name = _bmp.name;

		return bmp;
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



	public void Explode(float magnitude){
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