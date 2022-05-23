using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	ArrayList waves;
	public int currentWave;
	public Wave curWave;
	public Wave midBossWave;
	bool spawnerOn;

	IEnumerator spawnRoutine;
	IEnumerator spawnerRoutine;

	void Awake(){
		spawnerOn = false;
		DestroyAllEnemies ();
		DestroyAllProjectiles ();
	}

	IEnumerator SpawnerRoutine(){
		Game.control.stageHandler.ToggleTimer(true);
		foreach (Wave wave in waves) {
			yield return new WaitUntil(() => Game.control.stageHandler.stageTimer >= wave.spawnTime);
			if (wave.isBoss || wave.isMidBoss){ DestroyAllProjectiles (); }

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
		waves = Game.control.enemyLib.stageWaves;
		
		if(waves.Count != 0) {
			foreach (Wave w in waves) {
				if (w.isMidBoss) {
					midBossWave = w;
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
		for(float i = wave.enemyCounter; i>0; i--){
			if(spawnerOn) wave.Spawn (currentWave);
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

	public void DestroyEnemyProjectiles(ArrayList bullets){
		ArrayList projectilesToDestroy = bullets;
		int i = 0;

		while(i < projectilesToDestroy.Count){
			GameObject bullet = (GameObject)projectilesToDestroy [i];
			bullets.Remove (bullet);
			Destroy(bullet.gameObject);
			i++;
		}
	}

	public void DestroyAllProjectiles()
	{
		GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("EnemyProjectile");

		for(int i = 0; i < projectilesToDestroy.Length; i++){
			Destroy(projectilesToDestroy[i].gameObject);
		}
	}
}
