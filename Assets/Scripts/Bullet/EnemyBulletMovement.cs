using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBulletMovement : MonoBehaviour {
	VectorLib lib;
	Vector3 playerPosition;
	Vector3 movementDirection;
	public BulletMovementPattern BMP;
	EnemyShoot shooter;
	List<GameObject> trails;
	Rigidbody2D rb;


	
	float trailSpawnCD = .07f;
	bool canSpawnTrail = true;

	void Awake() {
		playerPosition = Game.control.player.gameObject.transform.position;
		lib = Game.control.vectorLib;
		rb = GetComponent<Rigidbody2D>();
		foreach(GameObject o in GameObject.FindGameObjectsWithTag("EnemyProjectile")){
			Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), o.GetComponent<BoxCollider2D>(), true);
		}
	}


	void MakeTrail(){
		GameObject trailSprite = new GameObject();
		trailSprite.AddComponent<SpriteRenderer>();
		trailSprite.GetComponent<SpriteRenderer>().sprite = BMP.pattern.sprite;
		trailSprite.AddComponent<TrailMaker>();
		trailSprite.transform.SetParent(gameObject.transform);
		trailSprite.transform.position = gameObject.transform.position;

		GameObject instance = Instantiate(trailSprite, gameObject.transform.position, Quaternion.identity);

		IEnumerator wait = WaitForTrailCD();
		StartCoroutine(wait);
	}

	IEnumerator WaitForTrailCD(){
		canSpawnTrail = false;
		yield return new WaitForSeconds(trailSpawnCD);
		canSpawnTrail = true;
	}

	void FixedUpdate () {
		if(BMP.isMoving){
			float speed = Time.fixedDeltaTime * BMP.movementSpeed;
			if(BMP.trail){
				if(canSpawnTrail) MakeTrail();

			}

			if (BMP.rotateOnAxis)
				transform.RotateAround (BMP.centerPoint, Vector3.back, (speed));
			else {
				transform.rotation = BMP.rotation;

				if(BMP.moveWithForce) {
					rb.AddForce(movementDirection * (speed  * 100));
				}
				else{
					transform.Translate (movementDirection * (Time.deltaTime * BMP.movementSpeed));
				}

				CheckBounds();
			}
		}

		if(BMP.forceScale) transform.localScale = BMP.scale;
	}

	public bool CheckBounds(){
		float y = transform.position.y;
		float x = transform.position.x;
		float[] boundaries = Game.control.ui.WORLD.GetBoundaries();

		if (y < boundaries[0] - 2f || x < boundaries[1] || y > boundaries[2] ||  x > boundaries[3]){
			if (!BMP.dontDestroy) {
				Destroy (this.gameObject);
			} 
			return false;
		}
		else return true;
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
		float iniSpeed = BMP.accelMax;
		BMP.movementSpeed = 0;
		while (BMP.movementSpeed < iniSpeed) {
			BMP.movementSpeed += 0.6f;
			yield return new WaitForSeconds (0.1f / BMP.accelSpeed);
		}
	}

	public void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") Destroy (this.gameObject);
	}
	public void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "NullField") Destroy (this.gameObject);
	}

	public void OnCollisionStay2D(Collision2D c){
		if(c.gameObject.name == "PlayAreaBotWall"){
			if(GetComponent<BulletBouncer>()){
				GetComponent<BulletBouncer>().StopBounce();
			}
		}
	}
}
