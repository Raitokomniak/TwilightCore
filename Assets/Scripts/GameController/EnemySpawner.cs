using UnityEngine;
using System.Collections;

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
		DestroyAllEnemies ();
		DestroyAllProjectiles ();
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
				Game.control.ui.RIGHT_SIDE_PANEL.UpdateWave(currentWave);
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

		DestroyAllEnemies();
		DestroyAllProjectiles();
		
		return true;
	}
		
	public void StartSpawner(int currentStage)
	{
		Game.control.stageHandler.InitWaves (currentStage);
		currentWave = 0;
		waves = Game.control.stageHandler.stageWaves;
		
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
			Game.control.stageHandler.EndHandler("StageComplete");
		}
	}


	public IEnumerator Spawn(Wave wave){
		for(int i = 0; i < wave.enemyCounter; i++){
			if(spawnerOn) {
				if(wave.isBoss || wave.isMidBoss) wave.SpawnBoss ();
				else wave.Spawn (i);
			}
			else break;
			yield return new WaitForSeconds (1f);
		}
	}


	/////////////////////////////////
	/// Destruction functions
	///

	public void DestroyAllEnemies()
	{
		GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject enemy in enemiesToDestroy){
			enemy.GetComponent<EnemyLife>().Die();
		}
	}
	
	public void DestroyAllProjectiles()
	{
		GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("EnemyProjectile");

		for(int i = 0; i < projectilesToDestroy.Length; i++){
			Destroy(projectilesToDestroy[i].gameObject);
		}
	}

	public void DestroyAllEnvironmentalHazards(){
		GameObject[] hazardsToDestroy = GameObject.FindGameObjectsWithTag("EnvironmentalHazard");

		for(int i = 0; i < hazardsToDestroy.Length; i++){
			Destroy(hazardsToDestroy[i].gameObject);
		}
	}
}
