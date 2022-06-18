using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletMovement : MonoBehaviour {
	public SpriteRenderer spriteR;
	VectorLib lib;
	Vector3 movementDirection;
	public BulletMovementPattern BMP;
	EnemyShoot shooter;
	Rigidbody2D rb;

	public bool active;

	float accelIniSpeed;
	bool accelerating;


	
	float trailSpawnCD = .07f;
	bool canSpawnTrail = true;

	void Awake() {
		active = false;
		lib = Game.control.vectorLib;
		rb = GetComponent<Rigidbody2D>();
		spriteR = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	public void Init(BulletMovementPattern _BMP, EnemyShoot enemyShoot){
		rb.simulated = true;
		BMP = _BMP;
		shooter = enemyShoot;
		SetUpBulletMovement ();
		active = true;
	}

    public void SetUpBulletMovement()
	{
		if (BMP.startHoming) BMP.FindPlayer(gameObject);
		else movementDirection = Vector3.down;

		StartCoroutine(BMP.Execute(this.gameObject));
	}

	public void Pool(){
		BMP = null;
		active = false;
		accelerating = false;
		rb.simulated = false;
		Stop();
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

	void FixedUpdate () {
		if(active && BMP != null){
            
			CheckCollider();

			if(BMP.isMoving){
                
                CheckTrail();

                if(BMP.rotateOnAxis) 
                    AxisRotation();
                else {
                    //if(!BMP.startHoming) 
                        
                    UpdateRotations();
					CheckMovementType();
                }
			}
			CheckScale();
            CheckBounds();
		}
	}

	void Update(){
		if(accelerating && BMP != null){
			if(BMP.movementSpeed < accelIniSpeed)
				BMP.movementSpeed += 4f * BMP.accelSpeed * Time.deltaTime;
			else if(BMP.movementSpeed > accelIniSpeed)
				accelerating = false;
		}
	}
    
	void CheckCollider(){
		bool canEnable = false;
		Vector3 findPlayer = Game.control.player.gameObject.transform.position;
		GameObject nightCoreField = Game.control.player.special.nightSpecial;
		GameObject dayCoreField = Game.control.player.special.daySpecial;

		if((transform.position - findPlayer).magnitude < 1f) canEnable = true; // IF NEAR PLAYER
		if(GetComponent<BulletBouncer>() && (transform.position - new Vector3(0, Game.control.ui.WORLD.GetBoundaries()[0],0)).magnitude < 1f) canEnable = true; //IF BOUNCER && NEAR BOT WALL
		if(nightCoreField.activeSelf && (transform.position - nightCoreField.transform.position).magnitude < 6f) canEnable = true;
		if(dayCoreField.activeSelf && (transform.position - dayCoreField.transform.position).magnitude < 13f) canEnable = true;
		
		if(!GetComponent<BoxCollider2D>().enabled && canEnable) {
            GetComponent<BoxCollider2D>().enabled = true;
            if(!GetComponent<BulletBouncer>()) Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), Game.control.ui.WORLD.playAreaBottomWall.GetComponent<BoxCollider2D>(), true);
        }
		if(GetComponent<BoxCollider2D>().enabled && !canEnable) GetComponent<BoxCollider2D>().enabled = false;
	}

    void CheckTrail(){
        if(BMP != null) if(BMP.trail && canSpawnTrail)  MakeTrail();
    }

    void AxisRotation(){
        BMP.centerPoint = transform.position;
		BMP.movementSpeed = 100;
		Quaternion q = Quaternion.AngleAxis (BMP.movementSpeed, Vector3.up);
		rb.MovePosition (q * (rb.transform.position - BMP.centerPoint) + BMP.centerPoint);
		rb.MoveRotation (rb.transform.rotation * q);
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
        if(BMP != null) if(BMP.forceScale) transform.localScale = BMP.scale;
    }

    
	public bool CheckBounds(){
		float y = transform.position.y;
		float x = transform.position.x;

		if(y < lib.OOBBot || x < lib.OOBLeft || y > lib.OOBTop || x > lib.OOBRight){
			if (active && !BMP.dontDestroy)
				Game.control.bulletPool.StoreBulletToPool(gameObject);
			return false;
		}
		else return true;
	}
	
	void UpdateRotations(){
		transform.rotation = BMP.rotation;
		transform.GetChild(0).rotation = BMP.spriteRotation;
		transform.GetChild(1).rotation = BMP.spriteRotation;
	}

    //////////////////////////////
    // MOVEMENT MODS

	public float GetRemainingDistance(Vector3 point){
		return (transform.position - point).magnitude;
	}

	public void Stop(){
		if(rb != null) rb.velocity = Vector2.zero;
	}

	public void SmoothAcceleration(){
		if(BMP != null) {
			accelerating = true;
			accelIniSpeed = BMP.accelMax;
			BMP.movementSpeed = 0;
		}
	}

    //////////////////////////////
    // COLLIDER

	public void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") Game.control.bulletPool.StoreBulletToPool(this.gameObject);
	}

	public void OnCollisionStay2D(Collision2D c){
		if(c.gameObject.name == "PlayAreaBotWall"){
			if(GetComponent<BulletBouncer>()){
				GetComponent<BulletBouncer>().StopBounce();
			}
		}
	}
}
