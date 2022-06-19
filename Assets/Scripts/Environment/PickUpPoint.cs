using UnityEngine;
using System.Collections;

public class PickUpPoint : MonoBehaviour {
	bool onMagneticRange = false;
	float accelSpeed;
    float movementSpeed = 5f;
	
	void Update () {
		if(transform.position.y <= Game.control.vectorLib.OOBBot){
			Destroy(this.gameObject);
		}
		if(Game.control.player.movement.atPickUpThreshold){
			onMagneticRange = true;
		}
		if(onMagneticRange){
			accelSpeed += Time.deltaTime;
			transform.position = Vector3.LerpUnclamped (transform.position, Game.control.player.transform.position, (Time.deltaTime * (20 * accelSpeed)));
		}
		else {
			accelSpeed += Time.deltaTime;
			transform.position += Vector3.down * (Time.deltaTime * (movementSpeed * accelSpeed));
		}
	}

	void OnTriggerStay2D(Collider2D c)
	{
		if (c.tag == "MagneticRange") onMagneticRange = true;
	}

}
