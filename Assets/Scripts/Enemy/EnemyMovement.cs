using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	public EnemyMovementPattern pattern;
	public Rigidbody2D rb;
    public SpriteRenderer enemySprite;
    
	VectorLib lib;
	public bool moving;
	public bool teleporting;

	public bool normalizedForce;

    public bool staticSpriteDir;

	float rotateAngle;

	void Awake () {
		lib = Game.control.vectorLib;
		rb = GetComponent<Rigidbody2D>();
		moving = false;
	}

	void FixedUpdate () {

		if (moving) {
			CheckSpriteDirection ();

			if (pattern.rotateOnAxis) {
				rotateAngle = Time.deltaTime * pattern.speed * 25 * pattern.rotationDirection;
				transform.RotateAround (pattern.centerPoint, Vector3.back, rotateAngle);
				transform.GetChild(0).Rotate(Vector3.forward * rotateAngle);
				transform.GetChild(1).Rotate(Vector3.forward * rotateAngle);
				rb.drag = 10f;
				rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -20, 20), Mathf.Clamp(rb.velocity.y, -20, 20));
			} 
			else {
				if(pattern.force) MoveWithForce();
				else MoveWithLerp();
			}
			
			if (tag == "Boss" || tag == "MidBoss") Game.control.stageUI.BOSS.UpdateBossXPos (transform.position.x, teleporting);
		}
		if(tag != "Boss" && tag != "MidBoss") CheckBounds();
	}

	void MoveWithLerp(){
		transform.position = Vector3.LerpUnclamped (transform.position, pattern.targetPosition, (pattern.speed * Time.deltaTime));
	}
    
	
	void MoveWithForce(){
		Vector2 tpos = new Vector2(pattern.targetPosition.x, pattern.targetPosition.y);
		Vector2 cpos = new Vector2(transform.position.x, transform.position.y);
		Vector2 dir = tpos - cpos;
		
		if(normalizedForce) 
				rb.AddForce(dir.normalized * pattern.speed * 20);
		else	rb.AddForce(dir * pattern.speed * 5);

		rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -20, 20), Mathf.Clamp(rb.velocity.y, -20, 20));
	}

	void CheckBounds(){
		float y = transform.position.y;
		float x = transform.position.x;
		float[] walls = Game.control.stageUI.WORLD.GetBoundaries();
        if(walls == null) return;
        
		if (y < walls[0] || x < walls[1] || y > walls[2] || x > walls[3]){
            //out of bounds
			GetComponent<EnemyLife>().invulnerable = true;
        }
		else 
			GetComponent<EnemyLife>().invulnerable = false;

		if (y > lib.OOBTop || y < lib.OOBBot || x < lib.OOBLeft || x > lib.OOBRight)
			Destroy (this.gameObject);
	}

	public void SetUpPatternAndMove(EnemyMovementPattern p){
		pattern = p;
		StartCoroutine (pattern.Execute (this));
		moving = true;
	}

    public void WaitForFloat(){
        IEnumerator WaitFloat = WaitFloatRoutine();
        StartCoroutine(WaitFloat);
    }

    IEnumerator WaitFloatRoutine(){
        yield return new WaitUntil(() => pattern.HasReachedDestination(this) == true);
        BossFloat();
    }

	public void SmoothAcceleration(float accelTime){
		StartCoroutine (_SmoothAcceleration (accelTime));
	}

	IEnumerator _SmoothAcceleration(float accelTime){
		float iniSpeed = pattern.speed;
		pattern.speed = 0;
		while (pattern.speed < iniSpeed) {
			pattern.speed += (iniSpeed / 10);
			yield return new WaitForSeconds (0.01f * accelTime);
		}
	}

    public void BossFloat(){
        Debug.Log("float");
        IEnumerator routine = BossFloatRoutine();
        StartCoroutine(routine);
    }

    IEnumerator BossFloatRoutine(){

        Transform sprite = enemySprite.gameObject.transform;
        Vector3 spriteOriginPos = sprite.position;

        while(Game.control.dialog.handlingDialog){
            bool floating = Game.control.dialog.handlingDialog;
            for(float i = .1f; i > 0.05; i-=Time.deltaTime){
                if(!floating) break;
                sprite.position = new Vector3(sprite.position.x, sprite.position.y - (i * 0.2f), 0);
                yield return new WaitForSeconds(Time.deltaTime * 20);
                
            }
            for(float i = .1f; i > 0.05f; i-=Time.deltaTime){
                if(!floating) break;
                sprite.position = new Vector3(sprite.position.x, sprite.position.y + (i * 0.2f), 0);
                yield return new WaitForSeconds(Time.deltaTime * 20);
            }

            yield return null;
        }

        sprite.localPosition = new Vector3(0,0,0);
        
    }

	// sprite 

	public void EnableSprite(bool toggle){
		enemySprite.enabled = toggle;
	}

	void CheckSpriteDirection(){
        if(staticSpriteDir) return;
		enemySprite.flipX = !pattern.goingRight;
	}

    
    public void CorrectRotation(){
        transform.rotation = Quaternion.Euler(0,0,0);
        enemySprite.transform.rotation = Quaternion.Euler(0,0,0);
    }

}
