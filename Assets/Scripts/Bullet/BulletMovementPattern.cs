﻿using UnityEngine;
using System.Collections;

public class BulletMovementPattern
{
	private ArrayList properties;
	public Vector3 centerPoint;

	public float targetMagnitude;
	public bool isHoming;
	public bool isMoving;

	public bool rotateOnAxis;
	public bool dontDestroy;
	public string property;

	public float movementSpeed;
	public float accelSpeed = 6f;
	public Quaternion rotation;
	public Vector3 scale;

	private Pattern p;
	public int layer;
	public int laserIndex;

	public BulletMovementPattern(bool _isHoming, string _property, float _movementSpeed, Pattern _p, int _layer, float magnitude)
	{
		isHoming = _isHoming;
		property = _property;
		movementSpeed = _movementSpeed;
		isMoving = true;
		layer = _layer;
		p = _p;
		rotateOnAxis = false;
		properties = new ArrayList();
		targetMagnitude = (float)magnitude;
		scale = new Vector3 (2,2,2);
	}

	public BulletMovementPattern(BulletMovementPattern b)
	{
		isHoming = b.isHoming;
		property = b.property;
		movementSpeed = b.movementSpeed;
		isMoving = true;
		layer = b.layer;
		p = b.p;
		rotateOnAxis = false;
		properties = new ArrayList();
		targetMagnitude = (float)b.targetMagnitude;
		scale = new Vector3 (2,2,2);
	}

	public IEnumerator Execute(GameObject bullet)
	{
		isMoving = true;

		if(property == "DownAndExplode"){
			movementSpeed = 10f;
			yield return new WaitForSeconds (1f);
			
			CorrectRotation ();
			Explode (true, bullet, 2.6f, 1);
			yield return new WaitUntil (() => bullet.GetComponent<EnemyBulletMovement> ().CheckDistance () > targetMagnitude);

			Stop (bullet);

			yield return new WaitForSeconds (0.3f);
			_RotateOnAxis (bullet, 1, 100f);

			yield return new WaitForSeconds (1f);
			CancelAxisRotation (10f);
		}
		if(property == "WaitAndExplode"){
			movementSpeed = 0;
			//
			rotation = bullet.transform.rotation;
			Explode (false, bullet, 14, 1);
			yield return new WaitUntil (() => p.allBulletsSpawned);
			yield return new WaitForSeconds (.2f);
			movementSpeed = accelSpeed;
			bullet.GetComponent<EnemyBulletMovement> ().SmoothAcceleration ();
			Explode (false, bullet, 14, 1);

			rotation = bullet.transform.rotation;
			//rotation = bullet.transform.rotation;
			//CorrectRotation ();

		}
		if(property == "TurnToSpears"){
			movementSpeed = 0;
			yield return new WaitForSeconds (1f);
			bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Spear", "Bevel", "Lilac");
			bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (.14f, 4);
			yield return new WaitForSeconds (1f);
			FindPlayer(bullet);
			yield return new WaitForSeconds (.3f);
			movementSpeed = 10f;
			bullet.GetComponent<EnemyBulletMovement> ().SmoothAcceleration ();

		}
		if(property == "WaitToHome"){
			movementSpeed = 0;
			yield return new WaitForSeconds (1f);
			FindPlayer(bullet);
			yield return new WaitForSeconds (.3f);
			movementSpeed = 10f;
			bullet.GetComponent<EnemyBulletMovement> ().SmoothAcceleration ();
		}
		if(property == "Aurora"){
			Explode (false, bullet, 14, 1);
			rotation = bullet.transform.rotation;
			yield return new WaitForSeconds (.1f);
			movementSpeed = .5f;
			yield return new WaitForSeconds (2f);
			movementSpeed = 0f;
			yield return new WaitUntil (() => p.allBulletsSpawned);
			movementSpeed = 8f;
			for (int i = 0; i < 2; i++) {
				Quaternion newRotation = Quaternion.Euler (0, 0, 150 * Random.Range (-1, 1));
				rotation = Quaternion.RotateTowards (bullet.transform.rotation, newRotation, Time.deltaTime);
				yield return new WaitForSeconds (1f);
				rotation = Quaternion.Euler (0, 0, 90 * Random.Range (-1, 1));
				yield return new WaitForSeconds (.5f);
			}
		}
		if(property == "Explode"){
			Explode (false, bullet, 14, 1);
			rotation = bullet.transform.rotation;
		}
		if(property == "Stop"){
				Explode (false, bullet, 14, 1);
			rotation = bullet.transform.rotation;
			yield return new WaitForSeconds (.1f);
			movementSpeed = 1f;
			yield return new WaitForSeconds (2f);
			movementSpeed = 7f;
		}
		if(property == "StopAndRotate"){
			dontDestroy = true;
			rotation = bullet.transform.rotation;
			Explode (true, bullet, targetMagnitude, 4.5f);
			yield return new WaitUntil (() => bullet.GetComponent<EnemyBulletMovement> ().CheckDistance () > targetMagnitude);

			Stop (bullet);
			yield return new WaitForSeconds (1f);

			if (layer == 0) {
				_RotateOnAxis (bullet, -1, 80f);
			} else if (layer == 1) {
				_RotateOnAxis (bullet, 1, 80f);
			}

			yield return new WaitForSeconds (1f);

			isMoving = true;
			movementSpeed = 10f;
			rotateOnAxis = false;

			yield return new WaitForSeconds (1f);

			dontDestroy = false;
		}
		if(property == "PendulumLaser"){
			dontDestroy = true;
			centerPoint = bullet.transform.position;

			bullet.GetComponent<EnemyBulletMovement> ().isLaser = true;
			Stop (bullet);
			scale = new Vector3 (0, 0, 0);
			for (float i = 0; i < 50; i++) {
				scale = new Vector3 (i * 0.1f, i, 1);
				bullet.transform.position -= new Vector3(0, 0.25f, 0);
				//_RotateOnAxis (bullet, 1, 100f);
				yield return new WaitForSeconds (.01f);
			}
		
			int dir = 1;
			if (laserIndex == 0)
				dir = 1;
			else
				dir = -1;
			yield return new WaitForSeconds (2f);
			_RotateOnAxis (bullet, dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, -dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, -dir, 10f);

			for (float i = 50; i > 0; i--) {
				scale = new Vector3 (i * 0.1f, scale.y, 1);
				yield return new WaitForSeconds (.01f);
			}
			GameObject.Destroy (bullet);
		}
		if(property == "LaserExpand"){				
			Collider2D coll = bullet.GetComponent<BoxCollider2D>();
			coll.enabled = false;
			dontDestroy = true;
			centerPoint = bullet.transform.position;
			bullet.GetComponent<EnemyBulletMovement> ().isLaser = true;
			Explode (true, bullet, 1, 6f);
			Stop (bullet);
			yield return new WaitForSeconds (.5f);
			bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Laser", "Glow", "Purple");
			bullet.GetComponent<SpriteRenderer> ().sortingOrder = -1;
			Vector2 origColliderSize = bullet.GetComponent<BoxCollider2D>().size;
			scale = new Vector3 (0, 0, 0);
			for (float i = 0; i < 50; i++) {
				scale = new Vector3 (i * 0.02f, i, 1);
				bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (i * 0.002f, i);
				//bullet.transform.position -= new Vector3(0, 0.25f, 0);
				//_RotateOnAxis (bullet, 1, 100f);
				yield return new WaitForSeconds (.02f);
			}
				
			int dir = laserIndex;

			if (laserIndex == 0)
				dir = 1;
			else
				dir = -1;

			yield return new WaitForSeconds (2f);

			coll.enabled = true;
			for (float i = 50; i > 0; i--) {
				scale = new Vector3 (i * 0.1f, scale.y, 1);
				yield return new WaitForSeconds (.01f);
			}
			GameObject.Destroy (bullet);
		}
		if(property == "LaserRotate"){
			dontDestroy = true;
			centerPoint = bullet.transform.position;
			bullet.GetComponent<EnemyBulletMovement> ().isLaser = true;
			Explode (true, bullet, 1, 6f);
			Stop (bullet);
			yield return new WaitForSeconds (.5f);
			bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Laser", "Glow", "Purple");
			bullet.GetComponent<SpriteRenderer> ().sortingOrder = -1;
			Vector3 origColliderSize = bullet.GetComponent<BoxCollider2D>().size;
			scale = new Vector3 (0, 0, 0);
			for (float i = 0; i < 50; i++) {
				scale = new Vector3 (i * 0.02f, i, 1);
				bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (i * 0.002f, i);
				//bullet.transform.position -= new Vector3(0, 0.25f, 0);
				//_RotateOnAxis (bullet, 1, 100f);
				yield return new WaitForSeconds (.02f);
			}
				
			int dir = laserIndex;

			if (laserIndex == 0)
				dir = 1;
			else
				dir = -1;
			_RotateOnAxis (bullet, dir, 10f);
			yield return new WaitForSeconds (4f);
			/*_RotateOnAxis (bullet, -dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, -dir, 10f);
			*/
			for (float i = 50; i > 0; i--) {
				scale = new Vector3 (i * 0.1f, scale.y, 1);
				yield return new WaitForSeconds (.01f);
			}
			GameObject.Destroy (bullet);

		}
		if(property == "SlowWaving"){
			Explode (false, bullet, 14, 1);
			isMoving = true;
			while(bullet.activeSelf){
				
				for(float i = 0; i < 70; i+=5)
				{
					rotation = Quaternion.Euler(0, 0, rotation.z + i);
					yield return new WaitForSeconds(.02f);
				}
				Quaternion tempRot = rotation;
				for(float i = 70; i > 0; i-=5)
				{
					rotation = Quaternion.Euler(0, 0, tempRot.z + i);
					yield return new WaitForSeconds(.02f);
				}
			}
		}
	}
		
	void FindPlayer(GameObject _bullet){
		Vector3 player = Game.control.player.gameObject.transform.position;
		Vector3 vectorToTarget = player - _bullet.transform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		rotation = Quaternion.Slerp(rotation, q, Time.deltaTime * 10f);
	}

	private void Stop(GameObject bullet){
		isMoving = false;
	}

	void CorrectRotation(){
		if (p.bullets.Count != 0)
			p.bullets.RemoveAt (p.bullets.Count - 1);
		rotation = Quaternion.Euler (0f, 0f, (p.bullets.Count - 1) * (360 / p.bulletCount));
	}

	private void Explode(bool animate, GameObject bullet, float _targetMagnitude, float targetScale)
	{
		centerPoint = bullet.transform.position;
		if (animate){
			p.Animate(targetScale, centerPoint);
		}
		isMoving = true;
		isHoming = false;
		targetMagnitude = _targetMagnitude;
	}

	void CancelAxisRotation(float speed)
	{
		rotateOnAxis = false;
		movementSpeed = speed;
		targetMagnitude = 20f;
	}
	private void _RotateOnAxis(GameObject bullet, int dir, float speed)
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