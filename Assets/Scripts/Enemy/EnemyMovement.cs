using UnityEngine;
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
				if(movementPattern.smoothedMovement){

					//Vector3 targetPos = Vector3.Slerp(transform.position, movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
					transform.position = Vector3.MoveTowards(transform.position,  movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
				}
				else if(movementPattern.smoothArc) { //SLERPPI
					//Vector3 targetPos = Vector3.Slerp(transform.position, movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
					transform.position = Vector3.MoveTowards(transform.position,  movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
				}
				else transform.position = Vector3.LerpUnclamped (transform.position, movementPattern.targetPos, (movementPattern.speed * Time.deltaTime));
			}
			

			

			if (tag == "Boss" || tag == "MidBoss") {
				Game.control.ui.BOSS.UpdateBossXPos (transform.position.x, !teleporting);
			}
		}

		CheckOutOfBounds();

		
	/* 
		this doesnt work with hideSpriteOnSpawn. if boss1 sprite doesnt reappear after teleport, force it somewhere else

		if(!teleporting && !GetComponent<EnemyLife>().GetInvulnerableState())
			EnableSprite(true);*/
	}

	void CheckOutOfBounds(){
		if (transform.position.y > 15 || transform.position.y < -12 || transform.position.x < -22.5 || transform.position.x > 9)
			if(tag != "Boss") Destroy (this.gameObject);
	}

	public void SetUpPatternAndMove(EnemyMovementPattern p){
		movementPattern = p;
		StartCoroutine (movementPattern.Execute (this));
		moving = true;
	}

	//not used yet
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
