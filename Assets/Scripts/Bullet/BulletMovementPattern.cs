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
    public bool moveWhileRotates;
    public float axisRotateSpeed;
    public bool holdShape;

    public int randomForcedXDir;


    public bool yAxisRotation;
    public bool xAxisRotation;

    public bool unFold;

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
    public float laserMaxScale = 50;

    public float laserDelay = 2;

    public bool retainSpriteRotation;

    public string hitBoxType = "Circle"; //DEFAULT  

    public bool forceSprite = true;

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
    
	public virtual BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
		Debug.Log("not this getnew");
		return null;
	}
    
    //DEEP COPY
    public BulletMovementPattern CopyValues(BulletMovementPattern _bmp, BulletMovementPattern newInstance){
		newInstance.bullet = _bmp.bullet;
		newInstance.centerPoint = _bmp.centerPoint;
		newInstance.targetMagnitude = _bmp.targetMagnitude;
		newInstance.isHoming = _bmp.isHoming;
		newInstance.startHoming = _bmp.startHoming;
		newInstance.isMoving = _bmp.isMoving;
		newInstance.randomDirs = _bmp.randomDirs;
		newInstance.rotateOnAxis = _bmp.rotateOnAxis;
		newInstance.dontDestroy = _bmp.dontDestroy;
		newInstance.movementSpeed = _bmp.movementSpeed;
		newInstance.accelMax = _bmp.accelMax;
		newInstance.accelSpeed = _bmp.accelSpeed;
        newInstance.accelIniSpeed = _bmp.accelIniSpeed;
        newInstance.accelerating = _bmp.accelerating;
		newInstance.rotation = _bmp.rotation;
		newInstance.scale = _bmp.scale;
		newInstance.forceScale = _bmp.forceScale;
		newInstance.pattern = _bmp.pattern;
		newInstance.layer = _bmp.layer;
		newInstance.trail = _bmp.trail;
		newInstance.moveWithForce = _bmp.moveWithForce;
		newInstance.forceRotation = _bmp.forceRotation;
		newInstance.rotationDir = _bmp.rotationDir;
		newInstance.waitAndExplodeWaitTime = _bmp.waitAndExplodeWaitTime;
        newInstance.hitBoxType = _bmp.hitBoxType;
        newInstance.name = _bmp.name;
        newInstance.moveWhileRotates = _bmp.moveWhileRotates;
        newInstance.axisRotateSpeed = _bmp.axisRotateSpeed;
        newInstance.holdShape = _bmp.holdShape;
        newInstance.unFold = _bmp.unFold;
        newInstance.randomForcedXDir = _bmp.randomForcedXDir;
        newInstance.xAxisRotation = _bmp.xAxisRotation;
        newInstance.yAxisRotation = _bmp.yAxisRotation;
        newInstance.laserMaxScale = _bmp.laserMaxScale;
        newInstance.laserDelay = _bmp.laserDelay;
        newInstance.retainSpriteRotation = _bmp.retainSpriteRotation;
        newInstance.forceSprite = _bmp.forceSprite;

		return newInstance;
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
		//movementSpeed = 
        axisRotateSpeed = (speed * 10) * dir;
		rotateOnAxis = true;
		isMoving = true;
	}

	public void SetSpriteRotation(Vector3 eulers){
		spriteRotation.eulerAngles = eulers;
	}
}