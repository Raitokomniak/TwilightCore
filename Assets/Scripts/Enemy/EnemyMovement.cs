﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public EnemyMovementPattern movementPattern;
	SpriteRenderer spriteRenderer;
	public bool moving;
	public bool teleporting;
	public bool rotateOnAxis;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		moving = false;
	}

	public void EnableSprite(bool toggle){
		spriteRenderer.enabled = toggle;
	}

	void Update () {
		rotateOnAxis = movementPattern.rotateOnAxis;
	//	transform.rotation = movementPattern.rotation;

		if (moving) {
			CheckSpriteDirection ();

			if (rotateOnAxis) {
				transform.RotateAround (movementPattern.centerPoint, Vector3.back, (Time.deltaTime * movementPattern.speed * movementPattern.movementDirection));
				transform.rotation = Quaternion.Euler(0,0,0);
			} else {
				transform.position = Vector3.LerpUnclamped (this.transform.position, movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
			}

			CheckOutOfBounds();

			if (tag == "Boss" || tag == "MidBoss") {
				Game.control.ui.BOSS.UpdateBossXPos (transform.position.x, !teleporting);
			}
		}

		if(!teleporting && !GetComponent<EnemyLife>().GetInvulnerableState())
			EnableSprite(true);
	}

	void CheckOutOfBounds(){
		if (transform.position.y < -12 || transform.position.x < -19 || transform.position.x > 9)
			if(tag != "Boss") Destroy (this.gameObject);
	}

	public void SetUpPatternAndMove(EnemyMovementPattern p){
		movementPattern = p;
		StartCoroutine (movementPattern.Execute (this));
		moving = true;
	}

	public void SmoothAcceleration(){
		StartCoroutine (_SmoothAcceleration ());
	}

	IEnumerator _SmoothAcceleration(){
		float iniSpeed = movementPattern.speed;
		movementPattern.speed = 0;
		while (movementPattern.speed != iniSpeed) {
			movementPattern.speed += (iniSpeed / 10);
			yield return new WaitForSeconds (0.05f);
		}
	}

	void CheckSpriteDirection(){
		if (!movementPattern.goingRight) {
			GetComponent<SpriteRenderer> ().flipX = false;
		} else {
			GetComponent<SpriteRenderer> ().flipX = true;
		}
	}
}
