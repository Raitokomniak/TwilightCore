using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

	bool init;
	GameObject projectile;
	public float bulletScale;

	float coolDownTimer;
	float coolDown;

	public int shootLevel;
	public float damage;

	public GameObject weapon;
	public ArrayList weapons;
	public ArrayList weaponsInUse;

	//string[] shootTypes;
	//public float shootSpeed;
	void Awake () {
		projectile = Resources.Load("BulletSprites/playerProjectile") as GameObject;
		coolDownTimer = 0f;
		coolDown = .1f;
		bulletScale = 1f;
		shootLevel = 0;
	}

	public void Init(){
		if (weapons != null && weapons.Count > 0) {
			foreach (GameObject w in weapons) {
				Destroy (w);
			}
		}
		weapons = new ArrayList ();
		weaponsInUse = new ArrayList ();

		transform.position = new Vector3 (Game.control.enemyLib.centerX, -8, 0);
		weapon = Resources.Load<GameObject> ("CharacterSprites/maincharweapon");

		weapons.Add ((GameObject)Instantiate(weapon, transform.position + new Vector3(0, 1f, 0), transform.rotation));
		weapons.Add ((GameObject)Instantiate(weapon, transform.position + new Vector3 (-1f, 0f, 0), transform.rotation)); // 1 left
		weapons.Add ((GameObject)Instantiate(weapon, transform.position + new Vector3 (1f, 0f, 0), transform.rotation)); //1 right
		weapons.Add ((GameObject)Instantiate(weapon, transform.position + new Vector3(0, -1f, 0), transform.rotation)); //Homing

		foreach (GameObject w in weapons) {
			w.transform.SetParent (this.gameObject.transform);
		}

		for (int i = 0; i < weapons.Count; i++) {
			GameObject w = (GameObject)weapons [i];
			w.transform.localScale = Vector3.one;
			if (i == 3) {
				w.transform.localScale = new Vector3 (1.5f, 1.5f, 1);
			}
		}
		weaponsInUse.Add ((GameObject)weapons [0]);
		UpdateShootLevel ();

		init = true;
	}

	void Update () {
		if(init && !Game.control.menu.menuOn){
			if(Input.GetKey	(KeyCode.Z) 
			&& coolDownTimer <= 0 
			&& !Game.control.stageHandler.gameOver 
			&& !Game.control.pause.paused
				&& !Game.control.dialog.handlingDialog){
				Shoot();
			}
			//Shooting coolDownTimer
			if(coolDownTimer > 0)  coolDownTimer -= Time.deltaTime;
		}
	}

	public void DisableWeapons(){
		foreach(GameObject weapon in weapons){
			weapon.SetActive(false);
		}
	}

	void Shoot()
	{
		Vector3 newPosition;
		Quaternion newRotation;
		Game.control.sound.PlaySound("Player", "Shoot", true);

		for (int i = 0; i < weaponsInUse.Count; i++) {
			GameObject projectile = Resources.Load("BulletSprites/playerProjectile") as GameObject;

			GameObject weapon = (GameObject)weapons [i];
			newPosition = weapon.transform.position + new Vector3(0, 0.5f, 0);
			newRotation = weapon.transform.rotation;
			projectile.transform.localScale = new Vector3 (3, 3, 3);
			if (GetComponent<PlayerMovement> ().focusMode) {
				projectile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BulletSprites/playerProjectileSprite2");
			}
			else 
				projectile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("BulletSprites/playerProjectileSprite");
			
			if (i == 3 && !GetComponent<PlayerMovement>().focusMode) {
				projectile.GetComponent<ProjectileMovement> ().homing = true;
				projectile.GetComponent<ProjectileMovement> ().targetPos = FindNearestEnemy ();
				projectile.transform.rotation = Quaternion.FromToRotation (transform.position, projectile.GetComponent<ProjectileMovement> ().targetPos);

			} else {
				projectile.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
				projectile.GetComponent<ProjectileMovement> ().homing = false;
			}
			GameObject.Instantiate(projectile, newPosition, newRotation);
		}
		coolDownTimer = coolDown;

	}

	Vector3 FindNearestEnemy(){
		Vector3 targetPos = new Vector3();
		float magnitude = Mathf.Infinity;
		Vector3 currentPos = transform.position;
		ArrayList allEnemies = new ArrayList ();
		allEnemies.AddRange(GameObject.FindGameObjectsWithTag ("Enemy"));
		allEnemies.AddRange(GameObject.FindGameObjectsWithTag ("Boss"));
		allEnemies.AddRange(GameObject.FindGameObjectsWithTag ("MidBoss"));

		foreach (GameObject enemy in allEnemies) {
			Vector3 dir = enemy.transform.position - currentPos;
			float sqrToTarget = dir.sqrMagnitude;
			if (sqrToTarget < magnitude) {
				magnitude = sqrToTarget;
				targetPos = enemy.transform.position;
			}
		}

		if (magnitude > 400)
			targetPos = Vector3.up;
		//Debug.Log (targetPos + " and " + magnitude);
		return targetPos;
	}

	public void FocusWeapons(int dir){
		if (weaponsInUse.Count > 1) {
			GameObject weapon1 = (GameObject)weaponsInUse [1];
			GameObject weapon2 = (GameObject)weaponsInUse [2];
			if (dir > 0) {
				weapon1.transform.position = transform.position + new Vector3 (-0.4f, 0.8f, 0);
				weapon2.transform.position = transform.position + new Vector3 (0.4f, 0.8f, 0);
			} else {
				weapon1.transform.position = transform.position + new Vector3 (1f, 0f, 0);
				weapon2.transform.position = transform.position + new Vector3 (-1f, 0f, 0);
			}
		}
	}

	public void UpdateShootLevel(){
		damage = Game.control.player.stats.damage;
		coolDown = Game.control.player.stats.shootSpeed;
		bulletScale = Game.control.player.stats.bulletScale;
		//shootLevel = Game.control.player.shootLevel;

		foreach (GameObject weapon in weaponsInUse) {
			weapon.SetActive (false);
		}

		if (shootLevel >= 0) {
			if (weaponsInUse.Count > 1) {
				weaponsInUse.Clear ();
				weaponsInUse.Add ((GameObject)weapons [0]);
			}
			if (shootLevel >= 1) {
				weaponsInUse.Add ((GameObject)weapons [1]);
				weaponsInUse.Add ((GameObject)weapons [2]);
			}
			if (shootLevel >= 2) {
				weaponsInUse.Add ((GameObject)weapons [3]);
			}
		}

		foreach (GameObject weapon in weapons) {
			weapon.SetActive (false);
		}
		foreach (GameObject weapon in weaponsInUse) {
			weapon.SetActive (true);
		}
	}

}
