using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public GameObject hitBox;
	public MagneticRange magneticRange;
	float movementSpeed;
	public bool focusMode;

	void Update ()
	{
		if(CanMove()) {
			FocusMode (Input.GetKey (KeyCode.LeftShift));

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
		}
	}

	bool CanMove(){
		if(!Game.control.stageHandler.stageOn) return false;
		if(GetComponent<PlayerLife>().dead) return false;
		if(GetComponent<PlayerHandler>() == null) return false;
		return true;
	}

	public void FocusMode (bool focus)
	{
		focusMode = focus;

		if (focus) {
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed * Time.deltaTime / 2;
			magneticRange.Scale (1);
			magneticRange.GetComponent<AnimationController> ().rotating = true;
			GetComponent<PlayerShoot> ().FocusWeapons (1);
			if(Game.control.ui != null) Game.control.ui.LEFT_SIDE_PANEL.HighLightCoreInUse ("Night");
		} else if(Game.control.stageHandler.stats != null)  {
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed * Time.deltaTime;
			magneticRange.Scale (-1);
			magneticRange.GetComponent<AnimationController> ().rotating = false;
			GetComponent<PlayerShoot> ().FocusWeapons (-1);
			if(Game.control.ui != null) Game.control.ui.LEFT_SIDE_PANEL.HighLightCoreInUse ("Day");
		}
	}

	void Move (float x, float y)
	{
		if (!Game.control.pause.paused) {
			transform.position += new Vector3 (x * movementSpeed, y * movementSpeed, 0);
		}
	}
}
