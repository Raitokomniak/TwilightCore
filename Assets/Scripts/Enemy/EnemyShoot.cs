﻿using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {
	EnemyLife enemyLife;
	public Wave wave;
	GameObject enemyBullet;
	public ArrayList bulletsShot;
	float shootSpeed;

	void Awake () {
		enemyBullet = Resources.Load("Prefabs/enemyBullet") as GameObject;
		enemyLife = GetComponent<EnemyLife>();

		wave = Game.control.enemySpawner.curWave;
		bulletsShot = new ArrayList ();
	}

	public Vector3 GetLocalPosition(){
		return transform.position;
	}
	
	public void SetUpAndShoot(Pattern p, float _shootSpeed){
		shootSpeed = _shootSpeed;
		StartCoroutine (ShootRoutine (p));
	}

	IEnumerator ShootRoutine(Pattern pattern){
		while (!enemyLife.GetInvulnerableState ()) {
			
			StartCoroutine (pattern.Execute (enemyBullet, this));
			yield return new WaitForSeconds (shootSpeed);
		}
	}

	public void BossShoot(Pattern pat){
		StartCoroutine (pat.Execute (enemyBullet, this));
	}
}
