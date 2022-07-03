using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletMovement : MonoBehaviour {
    HomingWarningLine homingWarningLine;

    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;
    BulletBouncer bulletBouncer;

	public SpriteRenderer spriteR;
    public SpriteRenderer glowRend;
	VectorLib lib;
	Vector3 movementDirection;
	public BulletMovementPattern BMP = null;
	EnemyShoot shooter;
	public Rigidbody2D rb;
	public bool active;
    bool hitBoxEnabled;
	float trailSpawnCD = .07f;
	bool canSpawnTrail = true;



    Vector3 findPlayer;
    GameObject nightCoreField;
    GameObject dayCoreField;
    Collider2D bulletCollider;
    Vector3 stageBot;
    bool canEnable;

	void Awake() {
		active = false;
		lib = Game.control.vectorLib;
		rb = GetComponent<Rigidbody2D>();
		spriteR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        glowRend = transform.GetChild(1).GetComponent<SpriteRenderer>();
        homingWarningLine = GetComponent<HomingWarningLine>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        bulletBouncer = GetComponent<BulletBouncer>();



        

	}

	public void Init(BulletMovementPattern _BMP, EnemyShoot enemyShoot){
		rb.simulated = true;
		BMP = _BMP;
		shooter = enemyShoot;
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
    // TRAIL
	void MakeTrail(){
		GameObject trailSprite = new GameObject();
		trailSprite.AddComponent<SpriteRenderer>();
		trailSprite.GetComponent<SpriteRenderer>().sprite = BMP.pattern.sprite;
		trailSprite.AddComponent<TrailMaker>();
		trailSprite.transform.SetParent(gameObject.transform);
		trailSprite.transform.position = gameObject.transform.position;

		GameObject instance = Instantiate(trailSprite, gameObject.transform.position, Quaternion.identity);

		IEnumerator wait = WaitForTrailCD();
		StartCoroutine(wait);
	}

	IEnumerator WaitForTrailCD(){
		canSpawnTrail = false;
		yield return new WaitForSeconds(trailSpawnCD);
		canSpawnTrail = true;
	}
    

    //////////////////////////////
    // UPDATE CHECKS

	void Update(){
        if(BMP == null) return;

        CheckCollider();
		CheckScale(); 

		if(BMP!=null) if(BMP.trail && canSpawnTrail) MakeTrail();
            
        if(BMP.isMoving){
            if(BMP.accelerating){
                if(BMP.movementSpeed < BMP.accelIniSpeed)
                    BMP.movementSpeed += BMP.accelSpeed * Time.deltaTime;
                else if(BMP.movementSpeed > BMP.accelIniSpeed)
                    BMP.accelerating = false;
            }

            if(BMP.rotateOnAxis)  AxisRotation();
            else {
                UpdateRotations();
                CheckMovementType();
            }
        }

        CheckBounds();
	}
    
	void CheckCollider(){
		canEnable = false;
		findPlayer = Game.control.player.gameObject.transform.position;

		if((transform.position - findPlayer).magnitude < 1f) canEnable = true; // IF NEAR PLAYER
		if(Game.control.stageUI.WORLD.GetBoundaries() != null) 
            if(bulletBouncer) if ((transform.position - stageBot).magnitude < 1f) canEnable = true; //IF BOUNCER && NEAR BOT WALL
		if(nightCoreField.activeSelf)   if((transform.position - nightCoreField.transform.position).magnitude < 6f)     canEnable = true;
		if(dayCoreField.activeSelf)     if((transform.position - dayCoreField.transform.position).magnitude < 13f)      canEnable = true;
        
		if(!hitBoxEnabled && canEnable) {
            hitBoxEnabled = true;
            bulletCollider.enabled = true;
            if(!bulletBouncer) Physics2D.IgnoreCollision(boxCollider, bulletCollider, true);
        }
		if(hitBoxEnabled && !canEnable) {
            bulletCollider.enabled = false;
            hitBoxEnabled =  false;
        }
	}

    public void DisableHitBoxes(){
        boxCollider.enabled = false;
       circleCollider.enabled = false;
    }


    void AxisRotation(){
        transform.RotateAround (BMP.centerPoint, Vector3.back, (Time.deltaTime * BMP.movementSpeed));
    }

    void CheckMovementType(){
        float speed = Time.fixedDeltaTime * BMP.movementSpeed;

        if(BMP.moveWithForce) {
			rb.isKinematic = false;
			rb.AddForce(movementDirection * (speed  * 100));
		}
		else {
			rb.isKinematic = true;
			movementDirection = -transform.up;
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

	public void OnCollisionStay2D(Collision2D c){
		if(c.gameObject.name == "PlayAreaBotWall"){
			if(GetComponent<BulletBouncer>()){
				GetComponent<BulletBouncer>().StopBounce();
			}
		}
	}
}
