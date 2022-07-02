using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {
	float movementSpeed = 40f;
	//public Vector3 targetPos;
   
	void Update () {
        if(Game.control.stageUI.WORLD.GetBoundaries() == null) return;
        
		if(transform.position.y < Game.control.stageUI.WORLD.GetBoundaries()[2] && !Game.control.pause.paused) {
			/*if (homing) {
				if (targetPos != Vector3.up) 
					 transform.position = Vector3.Lerp (transform.position, targetPos + new Vector3(0, 15,0), Time.deltaTime * movementSpeed * 10);
				else transform.Translate (Vector3.up * (movementSpeed) * Time.deltaTime);
			} 
			else */
			transform.Translate (Vector3.up * (movementSpeed) * Time.deltaTime);
		}
		else Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D c){
		if(c.tag == "EnvironmentalHazard"){
			Destroy(this.gameObject);
		}
	}
}
