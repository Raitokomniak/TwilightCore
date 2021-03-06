﻿using UnityEngine;
using System.Collections;


public class EnemyBulletMovement : MonoBehaviour {

	GameObject Player;

	Quaternion originalRot;

	Vector3 playerPosition;
	Vector3 movementDirection;
	Vector3 initialPosition;
	public float targetMagnitude;

	Quaternion rotation;
	Sprite sprite;
	BulletMovementPattern movement;
	public bool isHoming;
	public bool isMoving;
	bool isRotating;
	bool dontDestroy;
	public bool isLaser;

	float bWallPos;
	float lWallPos;
	float rWallPos;
	float tWallPos;

	void Awake() {
		targetMagnitude = 100f;
		isMoving = true;
		Player = GameController.gameControl.stage.Player;

		initialPosition = transform.position;
		playerPosition = Player.transform.position - initialPosition;

		bWallPos = GameController.gameControl.ui.playAreaBottomWall.transform.position.y - 2f;
		lWallPos = GameController.gameControl.ui.playAreaLeftWall.transform.position.x - 2f;
		tWallPos = GameController.gameControl.ui.playAreaTopWall.transform.position.y + 2f;
		rWallPos = GameController.gameControl.ui.playAreaRightWall.transform.position.x + 2f;
		originalRot = transform.rotation;

	}


	void Update () {
		float y = transform.position.y;
		float x = transform.position.x;

		transform.localScale = movement.scale;

		if(movement.isMoving){
			if (movement.isHoming)
				movementDirection = playerPosition;
			else if (movement.findPlayer) {
				//Debug.Log ("rotate");
				//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPosition), 10 * Time.deltaTime);
			}
			else {
				//movementDirection = new Vector3 (-7, -12, 0);
				movementDirection = Vector3.down;
			}


			if (movement.rotateOnAxis) {
				transform.RotateAround (movement.RotateOnAxis (), Vector3.back, (Time.deltaTime * movement.movementSpeed));
			} 

			else {
					transform.rotation = movement.rotation;
					transform.Translate (movementDirection * (Time.deltaTime * movement.movementSpeed));

					if(y <= bWallPos || x <= lWallPos || x >= rWallPos || y >= tWallPos){
						if (!movement.dontDestroy) {
							Destroy (this.gameObject);
						}
					}
			}
		}
			
	}

	public float CheckDistance(){
		
		Vector3 heading = transform.position - movement.centerPoint;
		float distance = heading.magnitude;
		//Debug.Log ("magnitude" + distance + " vs target " + targetMagnitude + "rot " + isRotating);
		return distance;
	}

	public void SetUpBulletMovement(BulletMovementPattern b)
	{
		movement = b;
		StartCoroutine(b.Execute(this.gameObject));
	}

	public void SmoothAcceleration(){
		StartCoroutine (_SmoothAcceleration ());
	}

	IEnumerator _SmoothAcceleration(){
		float iniSpeed = movement.movementSpeed;
		movement.movementSpeed = 0;
		while (movement.movementSpeed != iniSpeed) {
			movement.movementSpeed += 0.6f;
			yield return new WaitForSeconds (0.01f);
		}

	}

	public void OnTriggerStay2D(Collider2D c)
	{
		if (c.tag == "NullField") {
			Destroy (this.gameObject);
		}
	}
}
