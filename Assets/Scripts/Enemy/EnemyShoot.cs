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
	IEnumerator patternRoutine;
    WaitForSeconds shootWait;

    bool patternOver = false;
    bool canShoot;

	void Awake () {
        patternOver = false;
		wave = Game.control.enemySpawner.curWave;
		if(wave.isBoss || wave.isMidBoss) enemyLife = GetComponent<BossLife>();
		else if(tag == "Enemy") enemyLife = GetComponent<EnemyLife>();

		bulletsShot = new ArrayList ();
	}

    void Update(){
        canShoot = CanShoot();
    }

	public void SetUpAndShoot(Pattern p, float _shootSpeed){
        patternOver = false;
		pattern = p;
		shootSpeed = _shootSpeed;
        shootWait = new WaitForSeconds(shootSpeed);
		StartCoroutine (ShootRoutine ());
	}

	public void StopPattern(){
        if(patternRoutine == null) return;
		StopCoroutine(patternRoutine);
		pattern.StopPattern();
		patternOver = true;
	}

    bool CanShoot(){
        if(OutOfBounds()) return false;
        if(patternOver) return false;
        return true;
    }

	IEnumerator ShootRoutine(){
		if(tag == "Enemy") enemyLife = GetComponent<EnemyLife>();

       
		while (isActiveAndEnabled) {
            if(canShoot){
                patternRoutine = pattern.Execute (this);
                StartCoroutine (patternRoutine);

                if(wave.oneShot) break;
                yield return shootWait;
            }
            yield return null;
		}
	}

    bool OutOfBounds(){
		float y = transform.position.y;
		float x = transform.position.x;
		float[] walls = Game.control.stageUI.WORLD.GetBoundaries();
        if(walls == null) return true;

		if (y < walls[0] || x < walls[1] + 1 || y > walls[2] - 1 || x > walls[3] - 1)
            return true;
		else return false;
	}

	public void BossShoot(Pattern pat){
		StartCoroutine (pat.Execute (this));
	}
}
