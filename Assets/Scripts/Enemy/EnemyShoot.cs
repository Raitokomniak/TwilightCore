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
	IEnumerator shootRoutine;

	void Awake () {
		wave = Game.control.enemySpawner.curWave;
		enemyBullet = Resources.Load("Prefabs/enemyBullet") as GameObject;
		
		if(wave.isBoss || wave.isMidBoss) enemyLife = GetComponent<BossLife>();
		else if(tag == "Enemy") enemyLife = GetComponent<EnemyLife>();

		
		bulletsShot = new ArrayList ();
	}

	public void SetUpAndShoot(Pattern p, float _shootSpeed){
		pattern = p;
		shootSpeed = _shootSpeed;
		StartCoroutine (ShootRoutine ());
	}

	public void StopPattern(){
		StopCoroutine(shootRoutine);
		pattern.StopPattern();
		canShoot = false;
	}

	IEnumerator ShootRoutine(){
		if(tag == "Enemy") enemyLife = GetComponent<EnemyLife>();
		while (!enemyLife.GetInvulnerableState () && canShoot) {
			shootRoutine = pattern.Execute (enemyBullet, this);
			StartCoroutine (shootRoutine);
			yield return new WaitForSeconds (shootSpeed);
		}
	}

	public void BossShoot(Pattern pat){
		if(canShoot){
			StartCoroutine (pat.Execute (enemyBullet, this));
		}
	}
}
