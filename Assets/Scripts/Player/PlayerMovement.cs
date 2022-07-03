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
        if(CanForceMove()) transform.position = Vector3.Lerp(transform.position, forceMoveTarget, Time.deltaTime);
        if(!CanMove()) return;
		if(AllowInput()) FocusMode (Input.GetKey (KeyCode.LeftShift));

		float hor = Input.GetAxisRaw ("Horizontal");
		float ver = Input.GetAxisRaw ("Vertical");
        Move (hor, ver);
		CheckPickUpThreshold();
	}

	bool AllowInput(){
		if(Game.control.loading) return false;
		return true;
	}

	public void CheckPickUpThreshold(){
		if(Game.control.stageUI){
			if(transform.position.y >= Game.control.stageUI.WORLD.pickUpThreshold.transform.position.y)
				atPickUpThreshold = true;
			else atPickUpThreshold = false;
		}
	}

	public void ForceMove(Vector3 targetPos){
		GetComponent<SpriteRenderer>().sortingOrder = 5;
		GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
		forceMoveTarget = targetPos;
		forceMoving = true;
        Debug.Log("player forcemove");
	}

	bool CanForceMove(){
		if(!forceMoving) return false;
		if(GetComponent<PlayerLife>().dead) return false;
		if(GetComponent<PlayerHandler>() == null) return false;
		return true;
	}

	bool CanMove(){
		if(!Game.control.stageHandler.stageOn && !Game.control.stageHandler.onBonusScreen) return false;
		if(GetComponent<PlayerLife>().dead) return false;
		if(forceMoving) return false;
		if(GetComponent<PlayerHandler>() == null) return false;
        if(Game.control.pause.paused) return false;
        if(Game.control.stageHandler.gameOver) return false;
		return true;
	}

    public void ShowHitBox(bool toggle){
        float alpha = 0;
        if(toggle) alpha = 1;
        hitBox.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, alpha);
    }

	public void FocusMode (bool toggle)
	{
        if(Game.control.stageHandler.stats == null) return;

		focusMode = toggle;

		if (focusMode) {
			ShowHitBox(true);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed / 3.8f;
			magneticRange.Toggle(true);
			GetComponent<PlayerShoot> ().FocusWeapons (toggle);
			Game.control.stageUI.LEFT_SIDE_PANEL.HighLightCoreInUse ("Night");
			Game.control.stageUI.WORLD.TogglePickUpThreshold(true);
		} else {
			ShowHitBox(false);
			movementSpeed = Game.control.stageHandler.stats.movementSpeed;
			magneticRange.Toggle(false);
			GetComponent<PlayerShoot> ().FocusWeapons (toggle);
			Game.control.stageUI.LEFT_SIDE_PANEL.HighLightCoreInUse ("Day");
			Game.control.stageUI.WORLD.TogglePickUpThreshold(false);
		}
	}

	void Move (float x, float y)
	{
        float[] walls = Game.control.stageUI.WORLD.GetBoundaries();
        if(walls == null) return;
        
        if (transform.position.y < walls[0] + .5f && y < 0)  y = 0;
        if (transform.position.x < walls[1] + .5f && x < 0)  x = 0;
        if (transform.position.y > walls[2] - 2f && y > 0)   y = 0;
        if (transform.position.x > walls[3] - .5f && x > 0)  x = 0;

        Vector3 normalizedVector = new Vector3 (x, y, 0).normalized;
		transform.position += normalizedVector * Time.deltaTime * movementSpeed;
	}
}
