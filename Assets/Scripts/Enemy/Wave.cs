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
	public ArrayList phases;

	public Sprite sprite;

	public Phaser bossScript;
	
	public float bossIndex; //there is no excuse for this to be a float, make an array or something
	public string bossName;

	public bool dead;


	//FOR REGURAL ENEMIES
	
	public Wave(float _spawnTime, EnemyMovementPattern _movementPattern, Pattern _shootPattern, 
				int _enemyCount, bool _simultaneous, int _health, float _shootSpeed, string spriteName)
	{
		spawnTime = _spawnTime;
		enemyCount = _enemyCount;
		simultaneous = _simultaneous;
		movementPattern = _movementPattern;
		
		if(_shootPattern != null) shootPattern = _shootPattern;

		health = _health;
		shootSpeed = _shootSpeed;
		spawnPositions = new ArrayList ();
		enemyCounter = enemyCount;
		sprite = Game.control.spriteLib.SetEnemySprite(spriteName);
	}

	/*
	public Wave(float _spawnTime, EnemyMovementPattern _movementPattern, Pattern _shootPattern, 
				int _enemyCount, bool _simultaneous, int _health, bool _isBoss, float _shootSpeed, int _healthBars, string spriteName)
	{
		spawnTime = _spawnTime;
		enemyCount = _enemyCount;
		simultaneous = _simultaneous;
		movementPattern = _movementPattern;
		
		if(_shootPattern != null) shootPattern = _shootPattern;

		health = _health;
		shootSpeed = _shootSpeed;
		spawnPositions = new ArrayList ();
		enemyCounter = enemyCount;
		sprite = Game.control.spriteLib.SetEnemySprite(spriteName);
	}*/

	//FOR BOSSES
	public Wave(EnemyMovementPattern _movementPattern, float _spawnTime, int _health, bool _isBoss, int _healthBars, string spriteName)
	{
		movementPattern = _movementPattern;
		enemyCount = 1;
		spawnTime = _spawnTime;
		health = _health;
		healthBars = _healthBars;
		isBoss = _isBoss;
		isMidBoss = !isBoss;
		spawnPositions = new ArrayList ();
		phases = new ArrayList ();
		enemyCounter = enemyCount;
		sprite = Game.control.spriteLib.SetCharacterSprite(spriteName);
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
		spawnPosition = (Vector3)spawnPositions [0];
		if(spawnPositions.Count > 1){
			spawnPositions.Reverse ();
			spawnPosition =  (Vector3)spawnPositions [spawnPositions.Count - 1];
		}

		GameObject enemy = GameObject.Instantiate (Resources.Load ("Prefabs/Enemy"), spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;

		enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0, this);
		enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (movementPattern);

		if (isBoss || isMidBoss) {
			if(bossIndex == 0.5f) bossScript = enemy.AddComponent<Boss05> ();
			else if(bossIndex == 1f) bossScript = enemy.AddComponent<Boss1> ();
			else if(bossIndex == 2f) bossScript = enemy.AddComponent<Boss2> ();
			bossScript.Init();

			enemy.GetComponent<SpriteRenderer> ().sprite = sprite;

			//this is such a stupid way to do this, make this better
			if (bossIndex % 1 == 0) { //IF NOT MID BOSS 
				enemy.tag = "Boss";
				enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0.3f, this);
			} 
			else {
				enemy.tag = "MidBoss";
				enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0f, this);
				Game.control.ui.BOSS.StartBossTimer (movementPattern.stayTime);
			}

			bossScript.NextPhase ();
			Game.control.ui.BOSS.ToggleBossHealthSlider (true, enemy.GetComponent<EnemyLife> ().maxHealth, bossName);

		} 
		else {
			enemy.GetComponent<SpriteRenderer> ().sprite = sprite;
			enemy.GetComponent<EnemyShoot> ().SetUpAndShoot (shootPattern, shootSpeed);
		}
	}


	public void SetSpawnPositions(ArrayList positions){
		foreach (Vector3 pos in positions) {
			spawnPositions.Add (pos);
		}
	}
}