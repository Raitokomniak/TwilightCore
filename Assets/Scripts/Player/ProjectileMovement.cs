using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

	float movementSpeed;
	public bool piercing;
	public bool homing;
	public Vector3 targetPos;
	// Use this for initialization
	void Awake() {
		movementSpeed = .5f;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y < GameController.gameControl.ui.playAreaTopWall.transform.position.y && !GameController.gameControl.pause.paused) {
			if (homing) {
				if (targetPos != Vector3.up) {
					transform.position = Vector3.Lerp (transform.position, targetPos + new Vector3(0, 15,0), Time.deltaTime * movementSpeed * 10);
					//transform.Translate (targetPos);
				}
				else {
					transform.Translate (Vector3.up * (movementSpeed));
				}
				//if(targetPos != Vector3.up)
				//transform.rotation = Quaternion.FromToRotation (transform.position, targetPos);
			} else {
				transform.Translate (Vector3.up * (movementSpeed));
			}
		}
		else {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.GetComponent<EnemyLife>() != null) {
			c.gameObject.GetComponent<EnemyLife> ().TakeHit (GameController.gameControl.stats.damage);
			Destroy (this.gameObject);
		}
	}
}
