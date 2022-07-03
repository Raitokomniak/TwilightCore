using UnityEngine;
using System.Collections;

public class EnemyLife : MonoBehaviour {
	public bool dead;
	public float maxHealth;
	public float currentHealth;
	public bool invulnerable = false;


	SpriteRenderer spriteRenderer;

	public virtual void Init(int setMaxHealth, int _healthBars, Phaser _bossScript){}

	public void SetHealth(int _maxHealth){
		maxHealth = _maxHealth;
		currentHealth = maxHealth;
	}

	public void SetInvulnerable(bool value){
		invulnerable = value;
		Game.control.stageUI.BOSS.ToggleInvulnerable(invulnerable);
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
            int lootAmount = Random.Range(1,3);
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
			if(!dead && !invulnerable) Die (false);
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		if (c.tag == "PlayerProjectile") {
				if(!dead && !invulnerable) Die (false);
				Destroy(c.gameObject);
		}
	}
}
