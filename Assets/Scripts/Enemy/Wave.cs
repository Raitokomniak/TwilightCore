using UnityEngine;
using System.Collections;

public class Wave
{
	public float spawnTime;
	public Vector3 spawnPosition;
	public bool spawned;
	public int enemyCount;
	public int enemyCounter;

	public int health;
	public int healthBars;
	public bool isBoss;
	public bool isMidBoss = false;
	public float shootSpeed;
	public bool simultaneous;
	public ArrayList spawnPositions;

	public EnemyMovementPattern movementPattern;
	public Pattern shootPattern;
	public ArrayList bossPatterns;
	public ArrayList bossMovePatterns;
	public ArrayList phases;

	public Sprite sprite;

	
	public float bossIndex; //there is no excuse for this to be a float, make an array or something
	public string bossName;

	public bool dead;



	public Wave(float _spawnTime, EnemyMovementPattern _movementPattern, Pattern _shootPattern, 
				int _enemyCount, bool _simultaneous, int _health, bool _isBoss, float _shootSpeed, int _healthBars)
	{
		spawnTime = _spawnTime;
		enemyCount = _enemyCount;
		simultaneous = _simultaneous;
		movementPattern = _movementPattern;
		
		if(_shootPattern != null) shootPattern = _shootPattern;

		health = _health;
		healthBars = _healthBars;
		isBoss = _isBoss;
		isMidBoss = false;
		shootSpeed = _shootSpeed;
		bossMovePatterns = new ArrayList();
		bossPatterns = new ArrayList();
		spawnPositions = new ArrayList ();
		phases = new ArrayList ();
		enemyCounter = enemyCount;
	}

	public Wave(Wave w)
	{
		spawnTime = w.spawnTime;
		enemyCount = w.enemyCount;
		simultaneous = w.simultaneous;

		movementPattern = w.movementPattern;
		shootPattern = w.shootPattern;

		health = w.health;
		healthBars = w.healthBars;
		isBoss = w.isBoss;
		shootSpeed = w.shootSpeed;
		bossMovePatterns = new ArrayList();
		bossPatterns = new ArrayList();
		spawnPositions = new ArrayList ();
		phases = new ArrayList ();

		bossIndex = w.bossIndex;
		bossName = w.bossName;
	}

	public void SetUpBoss(float index, string name, bool _isMidBoss){
		bossIndex = index;
		bossName = name;
		isMidBoss = _isMidBoss;
	}

	public void Spawn(int waveindex){
		Debug.Log("spawn " + waveindex);
		spawnPosition = (Vector3)spawnPositions [0];
		if(spawnPositions.Count > 1){
			spawnPositions.Reverse ();
			spawnPosition =  (Vector3)spawnPositions [spawnPositions.Count - 1];
		}

		GameObject enemy = GameObject.Instantiate (Resources.Load ("Prefabs/Enemy"), spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;

		enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0);
		enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (new EnemyMovementPattern(movementPattern));

		if (isBoss || isMidBoss) {
			enemy.GetComponent<SpriteRenderer> ().sprite = sprite;

			//this is such a stupid way to do this, make this better
			if (bossIndex % 1 == 0) { //IF NOT MID BOSS 
				enemy.tag = "Boss";
				enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0.3f);
				Game.control.stageHandler.ToggleTimer (false);

			} else {
				enemy.tag = "MidBoss";
				enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0f);
				Game.control.ui.StartBossTimer (movementPattern.stayTime);
			}
			
			enemy.GetComponent<EnemyShoot> ().phaser.NextBossPhase ();
			Game.control.ui.ToggleBossHealthSlider (true, enemy.GetComponent<EnemyLife> ().maxHealth, bossName);

		} else {
			enemy.GetComponent<EnemyShoot> ().SetUpAndShoot (shootPattern, shootSpeed);
		}
		spawned = true;
	}


	public void SetSpawnPositions(ArrayList positions)
	{
		foreach (Vector3 pos in positions) {
			spawnPositions.Add (pos);
		}
	}
}