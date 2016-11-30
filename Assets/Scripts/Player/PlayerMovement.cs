using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
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
	}

	void Start ()
	{
		leftWallX = GameController.gameControl.ui.playAreaLeftWall.transform.position.x + .5f;
		rightWallX = GameController.gameControl.ui.playAreaRightWall.transform.position.x - .5f;
		bottomWallY = GameController.gameControl.ui.playAreaBottomWall.transform.position.y + .5f;
		topWallY = GameController.gameControl.ui.playAreaTopWall.transform.position.y - 2f;
	}

	void Update ()
	{
		FocusMode (Input.GetKey (KeyCode.LeftShift));

		float hor = Input.GetAxisRaw ("Horizontal");
		float ver = Input.GetAxisRaw ("Vertical");

		if (hor < 0) {
			if (transform.position.x > leftWallX) {
				Move (hor, 0);
			}
		}
		if (hor > 0) {
			if (transform.position.x <= rightWallX) {
				Move (hor, 0);
			}
		}
		if (ver > 0) {
			if (transform.position.y <= topWallY) {
				Move (0, ver);
			}
		}

		if (ver < 0) {
			if (transform.position.y >= bottomWallY) {
				Move (0, ver);
				}
			}
	}

	public void FocusMode (bool focus)
	{
		if (focus) {
			focusMode = true;
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
			movementSpeed = GameController.gameControl.stats.movementSpeed / 2;
			magneticRange.Scale (1);
			magneticRange.GetComponent<AnimationController> ().rotating = true;
			GetComponent<PlayerShoot> ().FocusWeapons (1);
			GameController.gameControl.ui.CoreInUse ("Night");
		} else {
			focusMode = false;
			hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);
			movementSpeed = GameController.gameControl.stats.movementSpeed;
			magneticRange.Scale (-1);
			magneticRange.GetComponent<AnimationController> ().rotating = false;
			GetComponent<PlayerShoot> ().FocusWeapons (-1);
			GameController.gameControl.ui.CoreInUse ("Day");
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

		if (!GameController.gameControl.pause.paused) {
			transform.position += new Vector3 (hor, ver, 0);
		}
	}


}
