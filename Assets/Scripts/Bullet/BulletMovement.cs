﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletMovement : MonoBehaviour {
    HomingWarningLine homingWarningLine;

    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;

	public SpriteRenderer spriteR;
    public SpriteRenderer glowRend;
	VectorLib lib;
	Vector3 movementDirection;
	public BulletMovementPattern BMP = null;
	public Rigidbody2D rb;
	public bool active;
    public bool hitBoxEnabled;

    Vector3 findPlayer;
    GameObject nightCoreField;
    GameObject dayCoreField;
    Collider2D bulletCollider;
    Vector3 stageBot;
    public bool canEnable;

	void Awake() {
		active = false;
		lib = Game.control.vectorLib;
		rb = GetComponent<Rigidbody2D>();
		spriteR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        glowRend = transform.GetChild(1).GetComponent<SpriteRenderer>();
        homingWarningLine = GetComponent<HomingWarningLine>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
	}

	public void Init(BulletMovementPattern _BMP, EnemyShoot enemyShoot){
		rb.simulated = true;
		BMP = _BMP;
		SetUpBulletMovement ();
		active = true;
        
        bulletCollider = null; 
        if     (BMP.hitBoxType == "Box")    bulletCollider = boxCollider;
        else if(BMP.hitBoxType == "Circle") bulletCollider = circleCollider;

        nightCoreField = Game.control.player.special.nightSpecial;
		dayCoreField = Game.control.player.special.daySpecial;

        stageBot = new Vector3(0, Game.control.stageUI.WORLD.GetBoundaries()[0],0);
	}

    public void SetUpBulletMovement()
	{
		if (BMP.startHoming) BMP.FindPlayer(gameObject);
		else movementDirection = Vector3.down;
        
		StartCoroutine(BMP.Execute(this.gameObject));
	}

	public void Pool(){
        DisableHitBoxes();
        spriteR.sortingOrder = 4;
        glowRend.sortingOrder = 0;
        spriteR.sprite = null;
        glowRend.sprite = null;
        transform.localScale = new Vector3 (0, 0, 0);
        if(BMP != null) BMP.accelerating = false;
		BMP = null;
		active = false;
		rb.simulated = false;
        if(homingWarningLine) homingWarningLine.gameObject.SetActive(false);
		Stop();
        this.enabled = false;
        
        //Destroy(this.gameObject);
	}


    

    //////////////////////////////
    // UPDATE CHECKS

	void Update(){
        if(BMP == null) return;
        if(!BMP.rotateOnAxis) UpdateRotations();

        CheckCollider();
		CheckScale();
        CheckSprite();

        if(BMP == null) return;   
        if(BMP.isMoving){
            if(BMP.accelerating){
                if(BMP.movementSpeed < BMP.accelIniSpeed)
                    BMP.movementSpeed += BMP.accelSpeed * Time.deltaTime;
                else if(BMP.movementSpeed > BMP.accelIniSpeed)
                    BMP.accelerating = false;
            }

            if(!BMP.rotateOnAxis) CheckMovementType();
            else if(BMP.rotateOnAxis && !BMP.moveWhileRotates)  AxisRotation();
            else if(BMP.rotateOnAxis && BMP.moveWhileRotates) {
                AxisRotation();
                CheckMovementType();
            }
        }

        CheckBounds(); ////////////////////////////////////////////////////////////
	}

    void CheckSprite(){
        if(BMP == null) return;
        if(!BMP.forceSprite) return;
        if(spriteR.sprite != BMP.pattern.sprite) spriteR.sprite = BMP.pattern.sprite;
    }

	void CheckCollider(){

		findPlayer = Game.control.player.gameObject.transform.position;

        // IF NEAR PLAYER
		if((transform.position - findPlayer).magnitude < 1f) canEnable = true; 
        
        if(nightCoreField != null && nightCoreField.activeSelf)
            if((transform.position - nightCoreField.transform.position).magnitude < 2f + Game.control.player.special.specialScale)
                Game.control.bulletPool.StoreBulletToPool(this);
            else canEnable = false;
        if(dayCoreField != null && dayCoreField.activeSelf)
            if((transform.position - dayCoreField.transform.position).magnitude < 5f + Game.control.player.special.specialScale)
                Game.control.bulletPool.StoreBulletToPool(this);
            else canEnable = false;
        
        
		ToggleHitBox();
	}

    void ToggleHitBox(){
        if(bulletCollider==null) return;
        if(!hitBoxEnabled){
            if(canEnable) {
                bulletCollider.enabled = true;
                hitBoxEnabled = true;
            }
            else {
                bulletCollider.enabled = false;
                hitBoxEnabled =  false;
            }
        }
        else {
            if(canEnable) {
                bulletCollider.enabled = true;
                hitBoxEnabled = true;
            }
            else {
                bulletCollider.enabled = false;
                hitBoxEnabled =  false;
            }
        }
    }

    public void DisableHitBoxes(){
        boxCollider.enabled = false;
        circleCollider.enabled = false;
    }


    void AxisRotation(){
        transform.RotateAround (BMP.centerPoint, Vector3.back, (Time.deltaTime * BMP.axisRotateSpeed));
    }

    void CheckMovementType(){
        float speed = Time.fixedDeltaTime * BMP.movementSpeed;

        if(BMP.moveWithForce) {
			rb.isKinematic = false;
			rb.AddForce(movementDirection * (speed  * 100));
		}
		else {
			rb.isKinematic = true;
			if(BMP.holdShape) movementDirection = new Vector3(BMP.randomForcedXDir, -1, 0);
            else if(BMP.unFold) movementDirection = -Vector3.up * (BMP.rotation.z + .5f);
            else movementDirection = -transform.up;
			rb.velocity = movementDirection * BMP.movementSpeed;
		}
    }

    void CheckScale(){
        if(BMP != null) if(BMP.forceScale && transform.localScale != BMP.scale) transform.localScale = BMP.scale;
    }

    
	public bool CheckBounds(){
		float y = transform.position.y;
		float x = transform.position.x;

		if(y < lib.OOBBot || x < lib.OOBLeft || y > lib.OOBTop || x > lib.OOBRight){
			if (active && !BMP.dontDestroy){
				Game.control.bulletPool.StoreBulletToPool(this);
            }
			return false;
		}
		else return true;
	}
	
	void UpdateRotations(){
		if(transform.rotation != BMP.rotation) transform.rotation = BMP.rotation;
		if(transform.GetChild(0).rotation != BMP.spriteRotation) transform.GetChild(0).rotation = BMP.spriteRotation;
		if(transform.GetChild(1).rotation != BMP.spriteRotation) transform.GetChild(1).rotation = BMP.spriteRotation;
	}

    //////////////////////////////
    // MOVEMENT MODS

	public float GetRemainingDistance(Vector3 point){
		return (transform.position - point).magnitude;
	}

	public void Stop(){
		if(rb != null) rb.velocity = Vector2.zero;
	}


    //////////////////////////////
    // COLLIDER

	public void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") Game.control.bulletPool.StoreBulletToPool(this);
	}

    public void OnTriggerEnter2D(Collider2D c){
		if (c.tag == "NullField") Game.control.bulletPool.StoreBulletToPool(this);
	}
}
