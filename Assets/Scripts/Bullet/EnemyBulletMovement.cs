using UnityEngine;
using System.Collections;

public class EnemyBulletMovement : MonoBehaviour {
	VectorLib lib;
	Vector3 playerPosition;
	Vector3 movementDirection;
	BulletMovementPattern BMP;
	EnemyShoot shooter;

	void Awake() {
		playerPosition = Game.control.player.gameObject.transform.position;
		lib = Game.control.vectorLib;
	}


	void Update () {
		
		if(BMP.isMoving){
			if (BMP.rotateOnAxis)
				transform.RotateAround (BMP.RotateOnAxis (), Vector3.back, (Time.deltaTime * BMP.movementSpeed));
			else {
				transform.rotation = BMP.rotation;
				transform.Translate (movementDirection * (Time.deltaTime * BMP.movementSpeed));
				CheckBounds();
			}
		}

		if(BMP.forceScale) transform.localScale = BMP.scale;
	}

	void CheckBounds(){
		float y = transform.position.y;
		float x = transform.position.x;
		float[] boundaries = Game.control.ui.WORLD.GetBoundaries();

		if (y < boundaries[0] || x < boundaries[1] || y > boundaries[2] ||  x > boundaries[3])
			if (!BMP.dontDestroy) Destroy (this.gameObject);
	}

	public float GetRemainingDistance(){
		return (transform.position - BMP.centerPoint).magnitude;
	}

	public void SetUpBulletMovement(BulletMovementPattern b, EnemyShoot enemy)
	{
		shooter = enemy;
		BMP = b;
		
		if (BMP.isHoming){ 
			 movementDirection = playerPosition - shooter.transform.position;
		}
		else movementDirection = Vector3.down;

		StartCoroutine(b.Execute(this.gameObject));
	}

	public void SmoothAcceleration(){
		StartCoroutine (_SmoothAcceleration ());
	}

	IEnumerator _SmoothAcceleration(){
		float iniSpeed = BMP.accelSpeed;
		BMP.movementSpeed = 0;
		while (BMP.movementSpeed < iniSpeed) {
			BMP.movementSpeed += 0.6f;
			yield return new WaitForSeconds (0.01f);
		}
	}

	public void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") Destroy (this.gameObject);
	}
	public void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "NullField") Destroy (this.gameObject);
	}
}
