using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public GameObject hitBox;
	public MagneticRange magneticRange;
	float movementSpeed;
	public bool focusMode;
	public bool forceMoving;

	Vector3 forceMoveTarget;

	public bool atPickUpThreshold;

	void Update ()
	{
		if(CanMove()) {
			if(AllowInput()) FocusMode (Input.GetKey (KeyCode.LeftShift));

			float hor = Input.GetAxisRaw ("Horizontal");
			float ver = Input.GetAxisRaw ("Vertical");

			if (ver < 0) //BOTTOM
				if (transform.position.y >= Game.control.ui.WORLD.GetBoundaries()[0] + .5f)
					Move (0, ver);

			if (hor < 0) //LEFT
				if (transform.position.x > Game.control.ui.WORLD.GetBoundaries()[1] + .5f) 
					Move (hor, 0);

			if (ver > 0) //TOP 
				if (transform.position.y <= Game.control.ui.WORLD.GetBoundaries()[2] - 2f) 
					Move (0, ver);
				
			if (hor > 0) //RIGHT 
				if (transform.position.x <= Game.control.ui.WORLD.GetBoundaries()[3] - .5f) 
					Move (hor, 0);

			CheckPickUpThreshold();
		}
		
		if(CanForceMove()){
			transform.position = Vector3.Lerp(transform.position, forceMoveTarget, movementSpeed / 10);
			//transform.position = Vector3.MoveTowards(transform.position, forceMoveTarget, (movementSpeed * Time.deltaTime) * 4);
		}
	}
	
	bool AllowInput(){
		if(Game.control.loading) return false;
		return true;
	}

	public void CheckPickUpThreshold(){
		if(Game.control.ui){
			if(transform.position.y >= Game.control.ui.WORLD.pickUpThreshold.transform.position.y)
				atPickUpThreshold = true;
			else atPickUpThreshold = false;
		}
	}

	public void ForceMove(Vector3 targetPos){
		GetComponent<SpriteRenderer>().sortingOrder = 5;
		GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
		forceMoveTarget = targetPos;
		forceMoving = true;
	}

	bool CanForceMove(){
		if(!forceMoving) return false;
		if(GetComponent<PlayerLife>().dead) return false;
		if(GetComponent<PlayerHandler>() == null) return false;
		return true;
	}

	bool CanMove(){
		if(!Game.control.stageHandler.stageOn) return false;
		if(GetComponent<PlayerLife>().dead) return false;
		if(forceMoving) return false;
		if(GetComponent<PlayerHandler>() == null) return false;
		return true;
	}

	public void FocusMode (bool toggle)
	{
		focusMode = toggle;

		if (focusMode) {
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed * Time.deltaTime / 3;
			magneticRange.Scale (1);
			magneticRange.GetComponent<AnimationController> ().StartRotating(4f);
			GetComponent<PlayerShoot> ().FocusWeapons (toggle);
			//if(Game.control.ui != null) 
			Game.control.ui.LEFT_SIDE_PANEL.HighLightCoreInUse ("Night");
			Game.control.ui.WORLD.TogglePickUpThreshold(true);
		} else if(Game.control.stageHandler.stats != null)  {
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed * Time.deltaTime;
			magneticRange.Scale (-1);
			magneticRange.GetComponent<AnimationController> ().StopRotating();
			GetComponent<PlayerShoot> ().FocusWeapons (toggle);
			//if(Game.control.ui != null) 
			Game.control.ui.LEFT_SIDE_PANEL.HighLightCoreInUse ("Day");
			Game.control.ui.WORLD.TogglePickUpThreshold(false);
		}
	}

	void Move (float x, float y)
	{
		if (!Game.control.pause.paused) {
			transform.position += new Vector3 (x * movementSpeed, y * movementSpeed, 0);
		}
	}
}
