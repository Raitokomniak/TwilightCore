using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	EnemyShoot shooter;
	EnemyMovementPattern pat;
	SpriteRenderer spriteRenderer;

	public bool moving;
	public bool floating;
	public bool teleporting;
	public bool rotateOnAxis;

	void Awake () {
		shooter = GetComponent<EnemyShoot>();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		moving = false;
	}

	public void EnableSprite(bool toggle){
		spriteRenderer.enabled = toggle;
	}

	void Update () {
		rotateOnAxis = pat.rotateOnAxis;
		transform.rotation = pat.rotation;

		if (moving) {
			CheckDirection ();
			if (rotateOnAxis) {
				transform.RotateAround (pat.RotateOnAxis (), Vector3.back, (Time.deltaTime * pat.speed * pat.direction));
			} else {
				transform.position = Vector3.LerpUnclamped (this.transform.position, pat.targetPos, (pat.speed * Time.deltaTime));
			}

			if (transform.position.y < -12 || transform.position.x < -19 || transform.position.x > 9) {
				if(tag != "Boss")
					Destroy (this.gameObject);
			}
			if (tag == "Boss" || tag == "MidBoss") {
				
				Game.control.ui.UpdateBossXPos (transform.position.x, !teleporting);
			}

		}
	}

	public void SetUpPatternAndMove(EnemyMovementPattern p){
		pat = p;
		StartCoroutine (pat.Execute (this));
		moving = true;
	}

	public void SmoothAcceleration(){
		StartCoroutine (_SmoothAcceleration ());
	}

	IEnumerator _SmoothAcceleration(){
		float iniSpeed = pat.speed;
		pat.speed = 0;
		while (pat.speed != iniSpeed) {
			pat.speed += (iniSpeed / 10);
			yield return new WaitForSeconds (0.05f);
		}

	}

	void CheckDirection(){
		if (!pat.goingRight) {
			GetComponent<SpriteRenderer> ().flipX = false;
		} else {
			GetComponent<SpriteRenderer> ().flipX = true;
		}
	}

	public void Animate(string animation)
	{
		//StartCoroutine (_Animate (animation));
	}

	IEnumerator _Animate(string animation){
		Vector3 origin = pat.targetPos;
		switch (animation) {
		case "Float":
			floating = true;
			while (floating) {
				pat.targetPos = transform.position - new Vector3 (0, .8f, 0);
				SmoothAcceleration ();
				yield return new WaitForSeconds (1f);
				SmoothAcceleration ();
				pat.targetPos = transform.position + new Vector3 (0, .8f, 0);
				yield return new WaitForSeconds (1f);
			}
			SmoothAcceleration ();
			pat.targetPos = origin;
			break;
		}
	}
}
