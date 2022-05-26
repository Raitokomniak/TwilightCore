using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyShoot : MonoBehaviour {
	EnemyLife enemyLife;
	public Wave wave;
	GameObject enemyBullet;
	public ArrayList bulletsShot;
	float shootSpeed;
	Pattern pattern;
	public bool canShoot = true;

	void Awake () {
		enemyBullet = Resources.Load("Prefabs/enemyBullet") as GameObject;
		enemyLife = GetComponent<EnemyLife>();

		wave = Game.control.enemySpawner.curWave;
		bulletsShot = new ArrayList ();
	}

	public void SetUpAndShoot(Pattern p, float _shootSpeed){
		pattern = p;
		shootSpeed = _shootSpeed;
		StartCoroutine (ShootRoutine ());
	}

	public void StopPattern(){
		pattern.StopPattern();
		canShoot = false;
	}

	IEnumerator ShootRoutine(){
		while (!enemyLife.GetInvulnerableState () && canShoot) {
			StartCoroutine (pattern.Execute (enemyBullet, this));
			yield return new WaitForSeconds (shootSpeed);
		}
	}

	public void BossShoot(Pattern pat){
		if(canShoot){
			StartCoroutine (pat.Execute (enemyBullet, this));
		}
	}
}
