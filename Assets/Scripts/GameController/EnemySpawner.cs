using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
	ArrayList waves;
	public int currentWave;
	public Wave curWave;
	public Wave midBossWave;
	public Wave bossWave;
	bool spawnerOn;

	IEnumerator spawnRoutine;
	IEnumerator spawnerRoutine;

	void Awake(){
		spawnerOn = false;
		//DestroyAllEnemies ();
		//DestroyAllProjectiles ();
	}

	public bool MidBossDead(){
		if(midBossWave != null)
			if(midBossWave.dead) return true;
			else return false;
		else return true;
	}

	IEnumerator SpawnerRoutine(){
		Game.control.stageHandler.ToggleTimer(true);
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
		
	public void StartSpawner(int currentStage)
	{
		waves = Game.control.stageHandler.InitWaves (currentStage);
		currentWave = 0;
		//Game.control.stageHandler.waves;
		
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
			spawnerRoutine = SpawnerRoutine();
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
			//enemy.GetComponent<EnemyLife>().Die();
            Destroy(enemy);
		}
	}
	
	public void DestroyAllProjectiles()
	{
		GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("EnemyProjectile");

		for(int i = 0; i < projectilesToDestroy.Length; i++){
			//Destroy(projectilesToDestroy[i].gameObject);
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
