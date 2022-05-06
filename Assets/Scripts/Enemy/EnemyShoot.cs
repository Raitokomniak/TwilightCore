using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {
	EnemyLife enemyLife;
	public Phaser phaser;
	public Wave wave;
	GameObject enemyBullet;
	public ArrayList bulletsShot;
	float coolDown;


	void Awake () {
		enemyBullet = Resources.Load("Prefabs/enemyBullet") as GameObject;
		enemyLife = GetComponent<EnemyLife>();

		wave = Game.control.enemySpawner.curWave;
		bulletsShot = new ArrayList ();

		if (wave.isBoss || wave.isMidBoss) {
			phaser = gameObject.AddComponent<Phaser> ();
			phaser.Init(wave.bossIndex, wave);
		}
	}

	public Vector3 GetLocalPosition(){
		return transform.position;
	}
	
	public void SetUpAndShoot(Pattern p, float cd){
		coolDown = cd;
		StartCoroutine (ShootRoutine (p));
	}

	IEnumerator ShootRoutine(Pattern pattern){
		while (!enemyLife.GetInvulnerableState ()) {
			StartCoroutine (pattern.Execute (enemyBullet, transform.position, transform.rotation, this));
			yield return new WaitForSeconds (coolDown);
		}
	}

	public void BossShoot(Pattern pat){
		StartCoroutine (pat.Execute (enemyBullet, transform.position, transform.rotation, this));
	}
}
