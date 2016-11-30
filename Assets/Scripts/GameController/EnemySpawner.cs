using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	ArrayList waves;
	public int currentWave;
	Wave curWave;
	public Wave bossWave;
	public Wave midBossWave;
	bool started;
	IEnumerator spawn;

	void Awake(){
		started = false;
		DestroyAllEnemies ();
		DestroyAllProjectiles ();
	}

	void Update () {
		if (started) {
			foreach (Wave wave in waves) {
				if (GameController.gameControl.stage.stageTimer >= wave.spawnTime && !wave.spawned) {
					wave.spawned = true;
					if (wave.isBoss)
						DestroyAllProjectiles ();
					else InitializeWave ();
					spawn = Spawn (wave);
					StartCoroutine (spawn);
				}
			}

			if (curWave == midBossWave) {
				if (midBossWave.dead) {
					InitializeWave ();
				}
			}
		}
	}
		
	public void StartSpawner()
	{
		started = false;
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
	}

	void InitializeWave(){	
		currentWave++;
		curWave = waves [currentWave] as Wave;
		GameController.gameControl.ui.UpdateStatPanel ("Wave", currentWave);
	}

	IEnumerator Spawn(Wave wave){
		while (started && wave.enemyCounter != 0) {
			wave.enemyCounter -= 1;
			wave.Spawn ();
			yield return new WaitForSeconds (1f);
		}
	}

	public Wave GetCurrentWave(){
		return curWave;
	}





	/////////////////////////////////
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
