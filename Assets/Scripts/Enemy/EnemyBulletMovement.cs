using UnityEngine;
using System.Collections;


public class EnemyBulletMovement : MonoBehaviour {
	Vector3 playerPosition;
	Vector3 movementDirection;
	Vector3 initialPosition;

	BulletMovementPattern movement;

	public bool isLaser;

	float bWallPos;
	float lWallPos;
	float rWallPos;
	float tWallPos;

	void Awake() {
		initialPosition = transform.position;
		playerPosition = Game.control.player.gameObject.transform.position - initialPosition;

			bWallPos = Game.control.ui.GetBoundaries()[0] - 2f;
			lWallPos = Game.control.ui.GetBoundaries()[1] - 2f;
			tWallPos = Game.control.ui.GetBoundaries()[2] + 2f;
			rWallPos = Game.control.ui.GetBoundaries()[3] + 2f;		
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
