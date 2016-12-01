using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {
	public EnemyLife 		enemyLife;
	GameObject 		enemyBullet;

	float coolDown;

	int bossPhase;
	public bool superPhase;
	public ArrayList bulletsShot;

	public Phase phase;
	public Wave wave;

	// Use this for initialization
	void Awake () {
		enemyBullet = 	Resources.Load("enemyBullet") as GameObject;
		enemyLife = 	GetComponent<EnemyLife>();
		bossPhase 			= -1;
		wave = GameController.gameControl.enemySpawner.GetCurrentWave ();
		phase = (Phase)wave.phase;

		bulletsShot = new ArrayList ();

		if (wave.isBoss || wave.isMidBoss) {
			phase = gameObject.AddComponent<Phase> ();
			phase.endOfPhase = true;
		}
	}


	public Vector3 GetLocalPosition(){
		return transform.position;
	}

	public void SetUpAndShoot(Pattern p, float cd){
		coolDown = cd;
		StartCoroutine (Shoot (p));
	}


	IEnumerator Shoot(Pattern pattern){

		while (!enemyLife.GetInvulnerableState ()) {
			if (!enemyLife.GetInvulnerableState ()) {
				
				StartCoroutine (pattern.Execute (enemyBullet, transform.position, transform.rotation, this));
			}	
			yield return new WaitForSeconds (coolDown);

		}
	}

	////////////////
	//BOSS
	////////////////

	public void BossShoot(Pattern pat){
		StartCoroutine (pat.Execute (enemyBullet, transform.position, transform.rotation, this));
	}

	public void NextBossPhase() {
//		Debug.Log ("Next phase called");
		StartCoroutine(PhasingTime());
	}

	IEnumerator PhasingTime() {
		enemyLife.SetInvulnerable(true);
		if(!phase.endOfPhase) phase.EndPhase ();
		yield return new WaitUntil (() => GameController.gameControl.dialog.handlingDialog == false);


		yield return new WaitForSeconds(.5f);
	
		bossPhase++;
		enemyLife.SetInvulnerable(false);

		phase.StartPhase("Boss" + wave.bossIndex, bossPhase, this, GetComponent<EnemyMovement>());
	}

}
