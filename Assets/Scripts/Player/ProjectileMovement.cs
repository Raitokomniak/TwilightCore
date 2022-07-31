using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {
	float movementSpeed = 40f;
	//public Vector3 targetPos;  // DONT DELET IN CASE YOU WANT TO MAKE A HOMING BULLET
   
	void Update () {
        if(Game.control.stageUI == null) return;
        if(Game.control.stageUI.WORLD.GetBoundaries() == null) return;
        
		if(transform.position.y < Game.control.stageUI.WORLD.GetBoundaries()[2] && !Game.control.pause.paused) { //see why this needs pausecheck if affected by timescale
            // DONT DELET IN CASE YOU WANT TO MAKE A HOMING BULLET
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
