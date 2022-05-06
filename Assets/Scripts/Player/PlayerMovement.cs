using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	PlayerHandler player;
	bool init;
	float movementSpeed;
	public GameObject hitBox;
	public GameObject magRange;
	public MagneticRange magneticRange;

	public bool focusMode;

	float leftWallX;
	float rightWallX;
	float bottomWallY;
	float topWallY;

	void Awake ()
	{
		hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
		player = GetComponent<PlayerHandler>();
	}

	public void Init(){
		init = true;
	}


	public void SetUpBoundaries(float left, float right, float bottom, float top){
		leftWallX = left + .5f;
		rightWallX = right - .5f;
		bottomWallY = bottom + .5f;
		topWallY = top - 2f;
	}

	void Update ()
	{
		if(init &&  !GetComponent<PlayerLife>().dead) {
			FocusMode (Input.GetKey (KeyCode.LeftShift));

			float hor = Input.GetAxisRaw ("Horizontal");
			float ver = Input.GetAxisRaw ("Vertical");

			if (hor < 0) 
				if (transform.position.x > leftWallX) 
					Move (hor, 0);
				
			if (hor > 0) 
				if (transform.position.x <= rightWallX) 
					Move (hor, 0);
				
			if (ver > 0) 
				if (transform.position.y <= topWallY) 
					Move (0, ver);
				
			if (ver < 0)
				if (transform.position.y >= bottomWallY)
					Move (0, ver);
				
		}
	}

	public void FocusMode (bool focus)
	{
		if (focus) {
			focusMode = true;
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			movementSpeed = player.stats.movementSpeed * Time.deltaTime / 2;
			magneticRange.Scale (1);
			magneticRange.GetComponent<AnimationController> ().rotating = true;
			GetComponent<PlayerShoot> ().FocusWeapons (1);
			Game.control.ui.CoreInUse ("Night");
		} else {
			focusMode = false;
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			movementSpeed = player.stats.movementSpeed * Time.deltaTime;
			magneticRange.Scale (-1);
			magneticRange.GetComponent<AnimationController> ().rotating = false;
			GetComponent<PlayerShoot> ().FocusWeapons (-1);
			Game.control.ui.CoreInUse ("Day");
		}
	}

	public Vector3 GetLocalPosition()
	{
		return transform.position;
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
