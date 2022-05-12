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


		//THIS DOESNT BELONG HERE
		if (wave.isBoss || wave.isMidBoss) {
			//phaser = gameObject.AddComponent<Phaser> ();
			//phaser.Init(wave.bossIndex, wave);
			if(wave.bossIndex == 0.5f) phaser = gameObject.AddComponent<Boss05> ();
			else if(wave.bossIndex == 1f) phaser = gameObject.AddComponent<Boss1> ();
			else if(wave.bossIndex == 2f) phaser = gameObject.AddComponent<Boss2> ();

			phaser.Init();
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
			StartCoroutine (pattern.Execute (enemyBullet, this));
			yield return new WaitForSeconds (coolDown);
		}
	}

	public void BossShoot(Pattern pat){
		StartCoroutine (pat.Execute (enemyBullet, this));
	}
}
