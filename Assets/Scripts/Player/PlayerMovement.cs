using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	PlayerHandler player;
	public GameObject hitBox;
	public MagneticRange magneticRange;

	float movementSpeed;
	public bool focusMode;


	void Awake ()
	{
		hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		player = GetComponent<PlayerHandler>();
	}

	void Update ()
	{
		if(CanMove()) {
			FocusMode (Input.GetKey (KeyCode.LeftShift));

			float hor = Input.GetAxisRaw ("Horizontal");
			float ver = Input.GetAxisRaw ("Vertical");

			if (ver < 0) //BOTTOM
				if (transform.position.y >= Game.control.ui.GetBoundaries()[0] + .5f)
					Move (0, ver);

			if (hor < 0) //LEFT
				if (transform.position.x > Game.control.ui.GetBoundaries()[1] + .5f) 
					Move (hor, 0);

			if (ver > 0) //TOP 
				if (transform.position.y <= Game.control.ui.GetBoundaries()[2] - 2f) 
					Move (0, ver);
				
			if (hor > 0) //RIGHT 
				if (transform.position.x <= Game.control.ui.GetBoundaries()[3] - .5f) 
					Move (hor, 0);
		}
	}

	bool CanMove(){
		if(!Game.control.stageHandler.stageOn) return false;
		if(GetComponent<PlayerLife>().dead) return false;
		if(player == null) return false;
		return true;
	}

	public void FocusMode (bool focus)
	{
		if (focus) {
			focusMode = true;
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed * Time.deltaTime / 2;
			magneticRange.Scale (1);
			magneticRange.GetComponent<AnimationController> ().rotating = true;
			GetComponent<PlayerShoot> ().FocusWeapons (1);
			if(Game.control.ui != null) Game.control.ui.CoreInUse ("Night");
		} else if(Game.control.stageHandler.stats != null)  {
			focusMode = false;
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed * Time.deltaTime;
			magneticRange.Scale (-1);
			magneticRange.GetComponent<AnimationController> ().rotating = false;
			GetComponent<PlayerShoot> ().FocusWeapons (-1);
			if(Game.control.ui != null) Game.control.ui.CoreInUse ("Day");
		}
	}

	void Move (float x, float y)
	{
		float hor = x * movementSpeed;
		float ver = y * movementSpeed;

		if (!Game.control.pause.paused) {
			transform.position += new Vector3 (hor, ver, 0);
		}
	}


}
