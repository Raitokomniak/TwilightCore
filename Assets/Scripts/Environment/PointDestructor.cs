using UnityEngine;
using System.Collections;

public class PointDestructor : MonoBehaviour {
	bool onMagneticRange = false;

	float accelSpeed;
	
	void Update () {
		if(transform.position.y <= Game.control.ui.WORLD.playAreaBottomWall.transform.position.y){
			Destroy(this.gameObject);
		}
		if (onMagneticRange) {
			accelSpeed += Time.deltaTime;
			Vector3 newPosition = Game.control.player.movement.hitBox.transform.position + new Vector3(0, 1, 0);
			GetComponent<Rigidbody2D> ().isKinematic = true;
			transform.position = Vector3.LerpUnclamped (transform.position, newPosition, Time.deltaTime * 9f + accelSpeed);
		}
	}

	void OnTriggerStay2D(Collider2D c)
	{
		if (c.tag == "MagneticRange" && !Game.control.player.special.specialAttack) {
			onMagneticRange = true;
		}

	}

}
