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
	public float accelSpeed = 6f;
	public Quaternion rotation;
	public Vector3 scale;
	public bool forceScale = false;

	public Pattern pattern;
	public int layer;
	public int laserIndex;

	public BulletMovementPattern(){}

	public virtual BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
		Debug.Log("not this getnew");
		return null;
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