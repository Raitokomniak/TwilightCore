﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour {
	public bool init;
	public float coolDownTimer;
	public int shootLevel;

	GameObject projectile;
	Sprite normalProjectile;
	Sprite focusProjectile;

	public GameObject weapon;
	public List<GameObject> weapons;
	public List<GameObject> weaponsInUse;

	List<Quaternion> sideWeaponRots;


	void Awake () {
		coolDownTimer = 0f;
		shootLevel = 0;
		projectile = Resources.Load("Prefabs/playerProjectile") as GameObject;
		normalProjectile = Resources.Load<Sprite>("Sprites/BulletSprites/starLight_bullet");
		focusProjectile = Resources.Load<Sprite>("Sprites/BulletSprites/playerProjectileSprite2");

		sideWeaponRots = new List<Quaternion>() {Quaternion.Euler(0,0,0), Quaternion.Euler(0,0,0)};
		
	}

	public void Init(){
		weaponsInUse = new List<GameObject> ();
		
		//hidesprites
		foreach (GameObject w in weapons) w.GetComponent<SpriteRenderer>().enabled = false;

		weapons[0].GetComponent<SpriteRenderer>().enabled = true;
		weaponsInUse.Add ((GameObject)weapons [0]);
		UpdateShootLevel (Game.control.stageHandler.stats.dayCoreLevel,Game.control.stageHandler.stats.nightCoreLevel);
		init = true;
	}

	void Update () {
		if(coolDownTimer > 0)  coolDownTimer -= Time.deltaTime;
	}
	
	public void DisableWeapons(){
		foreach(GameObject weapon in weapons) weapon.SetActive(false);
	}

    
    public bool CanShoot(){
		if(Game.control.loading) return false;
		if(!init) return false;
		if(Game.control.menu.menuOn) return false;
		if(coolDownTimer > 0) return false;
		if(Game.control.stageHandler.gameOver) return false;
        if(!Game.control.stageHandler.stageOn) return false;
		if(Game.control.pause.paused) return false;
		if(Game.control.dialog.handlingDialog) return false;
		return true;
	}

	public void Shoot()
	{
        bool focusMode = GetComponent<PlayerMovement> ().focusMode;

		Vector3 newPosition;
		Quaternion newRotation = Quaternion.Euler(0,0,0);
		Game.control.sound.PlaySound("Player", "Shoot", true);

		for (int i = 0; i < weaponsInUse.Count; i++) {
			//get pos rot
			weapon = (GameObject)weapons [i];
            if(focusMode) 
                 newPosition = weapon.transform.position + new Vector3(0, 3f, 0);
			else newPosition = weapon.transform.position + new Vector3(0, 0.8f, 0);
			
			if(i == 0 || i == 3) newRotation = weapon.transform.rotation;
			else if(!focusMode){
				int randomRot = Random.Range(10,40);
				if(i==1){
					newRotation = Quaternion.Euler(0,0, weapon.transform.rotation.z + randomRot);
				}
				else if(i==2){
					newRotation = Quaternion.Euler(0,0, weapon.transform.rotation.z - randomRot);
				}
			}
			//set srpite
			SetProjectileSprite();
			SetProjectileScale(i);
			GameObject playerProj = GameObject.Instantiate(projectile, newPosition, newRotation);
			if(!focusMode){
				playerProj.GetComponentInChildren<ProjectileRotator>().rotate = true;
			}
			
			
		}
        if(focusMode) coolDownTimer = Game.control.stageHandler.stats.nightShootSpeed;
		else coolDownTimer = Game.control.stageHandler.stats.dayShootSpeed;
	}

	void SetProjectileSprite(){
		if (GetComponent<PlayerMovement> ().focusMode) 
			 projectile.GetComponentInChildren<SpriteRenderer>().sprite = focusProjectile;
		else projectile.GetComponentInChildren<SpriteRenderer>().sprite = normalProjectile;

        if(shootLevel <= 3) projectile.GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,0.8f);
        else if(shootLevel > 3) projectile.GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,0.5f);
	}

	void SetProjectileScale(int i){
		if(shootLevel == 0) projectile.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		if(shootLevel == 1) projectile.transform.localScale = new Vector3 (2f, 2f, 2f);
		if(shootLevel > 1)  projectile.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
		if(shootLevel > 3 ) if(!GetComponent<PlayerMovement> ().focusMode) projectile.transform.localScale = new Vector3 (4f, 4f, 4f);
	}



	//THIS IS CLEARLY NOT WORKING
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

		if (magnitude > 400) targetPos = Vector3.up;

		return targetPos;
	}

	public void FocusWeapons(bool focus){
		if (weaponsInUse.Count > 1) {
			GameObject weapon1 = weaponsInUse [2];
			GameObject weapon2 = weaponsInUse [1];
			if (focus) {
				weapon1.transform.position = transform.position + new Vector3 (-0.4f, 0.8f, 0);
				weapon2.transform.position = transform.position + new Vector3 (0.4f, 0.8f, 0);
				weapon1.transform.rotation = Quaternion.Euler(0,0,0);
				weapon2.transform.rotation = Quaternion.Euler(0,0,0);
			} else {
				weapon1.transform.position = transform.position + new Vector3 (1f, 0f, 0);
				weapon2.transform.position = transform.position + new Vector3 (-1f, 0f, 0);
				weapon1.transform.rotation = sideWeaponRots[0];
				weapon2.transform.rotation = sideWeaponRots[1];
			}
		}
	}


	public void UpdateShootLevel(int dayCoreLevel, int nightCoreLevel){
		foreach (GameObject weapon in weaponsInUse) weapon.GetComponent<SpriteRenderer>().enabled = false;
		weaponsInUse.Clear ();

		//get average of core levels. if greater than before, apply
		float tempShootLevel = Mathf.CeilToInt(dayCoreLevel + nightCoreLevel / 2);
		if(tempShootLevel != shootLevel) {
			if(tempShootLevel < shootLevel) Game.control.stageUI.PlayToast ("Power Down");
			if(tempShootLevel > shootLevel) Game.control.stageUI.PlayToast ("Power Up");
			shootLevel = (int)tempShootLevel;
		} 
		
		//weapon count
		if(shootLevel >= 0) weaponsInUse.Add (weapons [0]);

		if(shootLevel >= 1){
			weaponsInUse.Add (weapons [1]);
			weaponsInUse.Add (weapons [2]);
		}
		if(shootLevel >= 3) weaponsInUse.Add (weapons [3]);

		//set scales
		if(shootLevel < 1) sideWeaponRots = new List<Quaternion>() {Quaternion.Euler(0,0,0), Quaternion.Euler(0,0,0)};

		if(shootLevel >= 1){
			sideWeaponRots = new List<Quaternion>() {Quaternion.Euler(0,0,-25), Quaternion.Euler(0,0,25)};
			weaponsInUse[1].transform.rotation = sideWeaponRots[0];
			weaponsInUse[2].transform.rotation = sideWeaponRots[1];
			foreach (GameObject weapon in weaponsInUse)
                weapon.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		}
		if(shootLevel > 3){
			weaponsInUse[3].transform.localScale = new Vector3 (2f, 2f, 2f);
		}

		//ENABLE sprites
		foreach (GameObject weapon in weaponsInUse) weapon.GetComponent<SpriteRenderer>().enabled = true;
	}

}
