﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	public int currentWave;
	public Wave curWave;
	public Wave midBossWave;
	public Wave bossWave;
	bool spawnerOn;

	IEnumerator spawnRoutine;
	IEnumerator spawnerRoutine;

    public bool holdTimer = false;

	void Awake(){
		spawnerOn = false;
	}

	public bool MidBossDead(){
		if(midBossWave != null)
			if(midBossWave.dead) return true;
			else return false;
		else return true;
	}

    public bool EnemiesAlive(){
        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0) return false;
        else return true;
    }

	IEnumerator SpawnerRoutine(List<Wave> waves){
		if(!holdTimer) Game.control.stageHandler.ToggleTimer(true);
		foreach (Wave wave in waves) {
			yield return new WaitUntil(() => Game.control.stageHandler.stageTimer >= wave.spawnTime);

			if (currentWave < waves.Count) {	
				curWave = waves [currentWave] as Wave;
				currentWave++;
			}

			spawnRoutine = Spawn (wave);
			if(spawnerOn) StartCoroutine (spawnRoutine);
			else break;
		}
	}

	public bool AbortSpawner(){
		spawnerOn = false;
		if(spawnRoutine != null) 	StopCoroutine(spawnRoutine);
		if(spawnerRoutine != null) 	StopCoroutine(spawnerRoutine);
		return true;
	}
		
	public void StartSpawner(List<Wave> waves)
	{
		currentWave = 0;

		if(waves.Count != 0) {
			foreach (Wave w in waves) {
				if (w.isMidBoss) {
					midBossWave = w;
				}
				else if(w.isBoss){
					bossWave = w;
				}
			}
			spawnerOn = true;
			spawnerRoutine = SpawnerRoutine(waves);
			StartCoroutine(spawnerRoutine);
		}
		else {
			//REMOVE COMMENTS FOR FASTER DEBUG
			//Game.control.stageHandler.EndHandler("StageComplete");
		}
	}


	public IEnumerator Spawn(Wave wave){
		for(int i = 0; i < wave.enemyCounter; i++){
			if(spawnerOn) {
				if(wave.isBoss || wave.isMidBoss) wave.SpawnBoss ();
				else wave.Spawn (i);
			}
			else break;
			yield return new WaitForSeconds (wave.spawnCoolDown);
		}
	}




	/////////////////////////////////
	/// Destruction functions
	///

	public void DestroyAllEnemies()
	{
		GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject enemy in enemiesToDestroy){
            IEnumerator routine = DestroyAllEnemiesRoutine(enemy);
            StartCoroutine(routine);
		}
	}

    IEnumerator DestroyAllEnemiesRoutine(GameObject enemy){
        enemy.GetComponent<EnemyLife>().PlayFX("Hit");
        enemy.GetComponent<EnemyLife>().DisableEnemy();
        yield return new WaitForSeconds(2);
        Destroy(enemy);
    }
	
	public void DestroyAllProjectiles()
	{
		GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("EnemyProjectile");

		for(int i = 0; i < projectilesToDestroy.Length; i++){
            Game.control.bulletPool.StoreBulletToPool(projectilesToDestroy[i]);
		}
	}

    public void DestroyAllPickUpPoints()
	{   
        List<GameObject> pointsToDestroy = new List<GameObject>();
		pointsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("DayCorePoint"));
        pointsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("NightCorePoint"));
        pointsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("ExpPoint"));

		for(int i = 0; i < pointsToDestroy.Count; i++){
			Destroy(pointsToDestroy[i].gameObject);
		}
	}

	public void DestroyAllEnvironmentalHazards(){
		GameObject[] hazardsToDestroy = GameObject.FindGameObjectsWithTag("EnvironmentalHazard");

		for(int i = 0; i < hazardsToDestroy.Length; i++){
			Destroy(hazardsToDestroy[i].gameObject);
		}
	}
}
