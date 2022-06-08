using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public EnemyMovementPattern movementPattern;
	VectorLib lib;
	SpriteRenderer spriteRenderer;
	public bool moving;
	public bool teleporting;
	public bool rotateOnAxis;

	void Awake () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		lib = Game.control.vectorLib;
		moving = false;
	}

	public void EnableSprite(bool toggle){
		spriteRenderer.enabled = toggle;
	}

	void Update () {
		rotateOnAxis = movementPattern.rotateOnAxis;

		if (moving) {
			CheckSpriteDirection ();

			if (rotateOnAxis) {
				transform.RotateAround (movementPattern.centerPoint, Vector3.back, (Time.deltaTime * movementPattern.speed * 10 * movementPattern.movementDirection));
				transform.GetChild(1).Rotate(Vector3.forward * (Time.deltaTime * movementPattern.speed * 10 * movementPattern.movementDirection));
				//transform.rotation = Quaternion.Euler(0,0,0);
			} else {
				if(movementPattern.smoothedMovement){
					transform.position = Vector3.MoveTowards(transform.position,  movementPattern.targetPos, (movementPattern.speed * Time.deltaTime) * 3); //MULTIPLIER BECAUSE LERP IS SO MUCH FASTER COMPARED
				}
				else if(movementPattern.smoothArc) { //SLERPPI
					/*Vector3 targetPos = Vector3.Slerp(transform.position, movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
					transform.position = Vector3.MoveTowards(transform.position, targetPos, (movementPattern.speed * Time.deltaTime) * 3);*/
					//transform.RotateAround (movementPattern.centerPoint, Vector3.back, (Time.deltaTime * movementPattern.speed * 10 * movementPattern.movementDirection));
					transform.RotateAround (movementPattern.centerPoint, Vector3.back, (Time.deltaTime * movementPattern.speed * 10 * movementPattern.movementDirection));
					transform.GetChild(0).Rotate(Vector3.forward * (Time.deltaTime * movementPattern.speed * 10 * movementPattern.movementDirection));
					transform.GetChild(1).Rotate(Vector3.forward * (Time.deltaTime * movementPattern.speed * 10 * movementPattern.movementDirection));
				}
				else transform.position = Vector3.LerpUnclamped (transform.position, movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
			}
			
			if (tag == "Boss" || tag == "MidBoss") {
				Game.control.ui.BOSS.ToggleBossXPos(!teleporting);
				Game.control.ui.BOSS.UpdateBossXPos (transform.position.x);
			}
		}

		CheckOutOfBounds();
	}

	void CheckOutOfBounds(){
		if (transform.position.y > lib.OOBTop || transform.position.y < lib.OOBBot  || transform.position.x < lib.OOBLeft || transform.position.x > lib.OOBRight)
			if(tag != "Boss" && tag != "MidBoss") Destroy (this.gameObject);
	}

	public void SetUpPatternAndMove(EnemyMovementPattern p){
		movementPattern = p;
		StartCoroutine (movementPattern.Execute (this));
		moving = true;
	}

	//not used yet
	public void SmoothAcceleration(float accelTime){
		StartCoroutine (_SmoothAcceleration (accelTime));
	}

	IEnumerator _SmoothAcceleration(float accelTime){
		float iniSpeed = movementPattern.speed;
		movementPattern.speed = 0;
		while (movementPattern.speed != iniSpeed) {
			movementPattern.speed += (iniSpeed / 10);
			yield return new WaitForSeconds (0.01f * accelTime);
		}
	}

	void CheckSpriteDirection(){
		if (!movementPattern.goingRight) {
			GetComponentInChildren<SpriteRenderer> ().flipX = false;
		} else {
			GetComponentInChildren<SpriteRenderer> ().flipX = true;
		}
	}
}
