using UnityEngine;
using System.Collections;



public class EnemySpawner : MonoBehaviour {
	ArrayList waves;
	float enemyCounter;
	float spawnDelay;

	public int currentWave;
	Wave curWave;
	public Wave bossWave;
	public Wave midBossWave;
	bool started;


	void Update () {
		
	}
		
	public void StartSpawner()
	{
		GameController.gameControl.enemyLib.InitWaves (GameController.gameControl.stage.currentStage);
		currentWave = -1;
		waves = GameController.gameControl.enemyLib.stageWaves;
		bossWave = (Wave)waves [waves.Count - 1];
		foreach (Wave w in waves) {
			if (w.isBoss) {
				midBossWave = w;
				break;
			}
		}
		InitializeWave();
		started = true;
		StartCoroutine (Spawner ());
	}

	IEnumerator Spawner(){
		foreach (Wave wave in waves) {
			yield return new WaitUntil(() => GameController.gameControl.stage.stageTimer >= curWave.spawnTime && !curWave.spawned);
			StartCoroutine(Spawn(wave));
		}

	}

	IEnumerator Spawn(Wave wave){
		curWave.spawned = true;

		if (wave.isBoss)
			DestroyAllProjectiles ();
		else InitializeWave ();
		
		while (started && wave.enemyCounter != 0) {
//			Debug.Log (wave.enemyCounter);
			wave.enemyCounter -= 1;
			wave.Spawn ();
			yield return new WaitForSeconds (1f);
		}

		if (wave == midBossWave) {
			yield return new WaitUntil (() => curWave.dead == true);
			InitializeWave ();
		}
	}

	public void InitSpawner(){
		started = false;
	}

	public Wave GetCurrentWave(){
		return curWave;
	}


	void InitializeWave()
	{	
		currentWave++;
		enemyCounter = 0;
		curWave = waves [currentWave] as Wave;
		GameController.gameControl.ui.UpdateStatPanel ("Wave", currentWave);
	}


	/// 
	/// Destruction functions
	///

	public void DestroyAllEnemies()
	{
		int i = 0;
		GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

		while(i < enemiesToDestroy.Length){
			enemiesToDestroy[i].GetComponent<EnemyLife>().Die();
			i++;
		}
	}

	public void DestroyEnemyProjectiles(ArrayList bullets){
		ArrayList projectilesToDestroy = bullets;

		foreach(GameObject p in projectilesToDestroy) {
			if (p != null) {
				//Instantiate (Resources.Load ("corePoint"), p.transform.position + new Vector3 (Random.Range (-2, 2), Random.Range (-2, 2)), transform.rotation);
			}
		}
		int i = 0;

		while(i < projectilesToDestroy.Count)
		{
			GameObject bullet = (GameObject)projectilesToDestroy [i];
			bullets.Remove (bullet);
			Destroy(bullet.gameObject);
			i++;
		}
	}

	public void DestroyAllProjectiles()
	{
		GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("EnemyProjectile");
		foreach(GameObject p in projectilesToDestroy) {
			//Instantiate(Resources.Load("corePoint"), p.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2)), transform.rotation);
		}
		int i = 0;

		while(i < projectilesToDestroy.Length)
		{
			Destroy(projectilesToDestroy[i].gameObject);
			i++;
		}
	}
}
