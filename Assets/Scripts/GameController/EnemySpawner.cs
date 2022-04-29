using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	ArrayList waves;
	public int currentWave;
	public Wave curWave;
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
				if (Game.control.stage.stageTimer >= wave.spawnTime && !wave.spawned) {
					wave.spawned = true;
					if (wave.isBoss || wave.isMidBoss){
						DestroyAllProjectiles ();
					}

					InitializeWave ();
					spawn = Spawn (wave);
					StartCoroutine (spawn);
				}
			}

			if (curWave != null && curWave == midBossWave) {
				if (midBossWave.dead) {
					Debug.Log("midboss dead");
					InitializeWave ();
				}
			}
		}
	}

	public void AbortSpawner(){
		started = false;
		StopCoroutine(spawn);
		DestroyAllEnemies();
		DestroyAllProjectiles();
		//curWave = null;
	}
		
	public void StartSpawner(int currentStage)
	{
		started = false;
		Game.control.enemyLib.InitWaves (currentStage);
		currentWave = 0;
		waves = Game.control.enemyLib.stageWaves;
		bossWave = (Wave)waves [waves.Count - 1];

		foreach (Wave w in waves) {
			if (w.isMidBoss) {
				Debug.Log("is midboss");
				midBossWave = w;
			}
			if (w.isBoss) {
				bossWave = w;
			}
		}
		started = true;
	}

	void InitializeWave(){	
		if (currentWave < waves.Count) {	
			curWave = waves [currentWave] as Wave;
			
			currentWave++;
			Game.control.ui.UpdateStatPanel ("Wave", currentWave);
		}
	}

	public IEnumerator Spawn(Wave wave){
		for(float i = wave.enemyCounter; i>0; i--){
				wave.Spawn ();
				yield return new WaitForSeconds (1f);
		}
	}


	/////////////////////////////////
	/// Destruction functions
	///

	public void DestroyAllEnemies()
	{
		int i = 0;
		GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag("Enemy");

		foreach(GameObject enemy in enemiesToDestroy){
			enemy.GetComponent<EnemyLife>().Die();
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

		for(int i = 0; i < projectilesToDestroy.Length; i++){
			Destroy(projectilesToDestroy[i].gameObject);
		}
	}
}
