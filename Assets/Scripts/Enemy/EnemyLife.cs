using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {
	public bool dead;
    public bool hasHealth;
	public float maxHealth;
	public float currentHealth;
	public bool invulnerable = false;

    public ParticleSystem hitFXparticles;


	SpriteRenderer spriteRenderer;

    void Awake(){
        hitFXparticles = GetComponentInChildren<ParticleSystem>();
        hitFXparticles.Stop();
    }
	public virtual void Init(int setMaxHealth, int _healthBars, Phaser _bossScript){}

	public void SetHealth(int _maxHealth){
		maxHealth = _maxHealth;
		currentHealth = maxHealth;
	}

	public void SetInvulnerable(bool value){
		invulnerable = value;
		Game.control.stageUI.BOSS.ToggleInvulnerable(invulnerable);
	}
    
    void CheckHit(){
        if(dead || invulnerable) return;
        
        if(hasHealth) TakeHit();
        else Die (false);
    }

    void TakeHit(){
        currentHealth -= 1;
        if(currentHealth < 0) Die(false);
        PlayFX("Hit");
        IEnumerator animateHit = AnimateHit();
        StartCoroutine(animateHit);
    }

    public void PlayFX(string type){
        var shape = hitFXparticles.shape;
        var main = hitFXparticles.main;
        var emitter = hitFXparticles.emission;

        if(type == "Hit"){
            shape.shapeType = ParticleSystemShapeType.SingleSidedEdge;
            main.startSpeed = 8;
            hitFXparticles.Emit(1);
        }
        if(type == "Death"){
            shape.shapeType = ParticleSystemShapeType.Sphere;
            emitter.rateOverTime = 30;
            main.startSpeed = 20;
            hitFXparticles.Play();
        }
    }


    public IEnumerator AnimateHit(){
        GetComponent<EnemyMovement>().enemySprite.color = new Color(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(0.1f);
        GetComponent<EnemyMovement>().enemySprite.color = new Color(1, 1, 1, 1);
    }



	public virtual void Die(bool silent) {
		dead = true;
		Game.control.sound.PlaySound ("Enemy", "Die", true);
		IEnumerator animateDeathRoutine = AnimateDeath(silent);
		StartCoroutine(animateDeathRoutine);
	}

	public virtual IEnumerator AnimateDeath(bool silent){
		DropLoot("Core");
		DisableEnemy();
		yield return new WaitForSeconds(3f);
		Destroy(this.gameObject);
	}

	public virtual void DisableEnemy(){
		spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
		GetComponent<EnemyShoot>().StopPattern();
		transform.GetChild(2).gameObject.SetActive(false);
		spriteRenderer.enabled = false;
		GetComponent<EnemyMovement>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<EnemyShoot>().enabled = false;
	}

	public void DropLoot(string type){
        GameObject pickUpPointPrefab = Resources.Load("Prefabs/pickUpPoint") as GameObject;
        string pointType = "";
    
		if(type == "Core"){
            int lootAmount = Random.Range(1,2);
			for(int i = 0; i < lootAmount; i++){ ////////// MANAGE AMOUNT OF LOOT
                Vector3 spawnPoint = transform.position + new Vector3(Random.Range(-5, 5), 2f, 0);
                GameObject pickUpPoint = Instantiate(pickUpPointPrefab, spawnPoint, Quaternion.Euler(0,0,0));

                    if(Random.Range(0, 2) == 0) 
                         pointType = "NightCorePoint";
                    else pointType = "DayCorePoint";

                    pickUpPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + pointType);
                    pickUpPoint.tag = pointType;
			}
		}
		if(type == "ExpPoint")
			for(int i = 0; i < 9; i++) {
                Vector3 spawnPoint = transform.position + new Vector3(Random.Range(-5, 5), 2f, 0);
                GameObject pickUpPoint = Instantiate(pickUpPointPrefab, spawnPoint, Quaternion.Euler(0,0,0));
				pickUpPoint.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + type);
                pickUpPoint.tag = type;
            }
	}

	void OnTriggerStay2D(Collider2D c){
		if (c.tag == "NullField") {
			CheckHit();
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "PlayerProjectile") {
			CheckHit();
			Destroy(c.gameObject);
		}
	}
}
