using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public List<Vector3> spawnPositions;
	public List<Vector3> enterDirections; //THE POSITION AN ENEMY WILL MOVE AFTER SPAWN
	public List<Vector3> leaveDirections; //THE POSITION AN ENEMY WILL MOVE WHEN LEAVING
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
		spawnPositions = new List<Vector3>();
		enterDirections = new List<Vector3>();
		leaveDirections = new List<Vector3>();
		enemyCounter = enemyCount;
		sprite = Game.control.spriteLib.SetEnemySprite(spriteName);
	}

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
		spawnPositions = new List<Vector3>();
		enterDirections = new List<Vector3>();
		leaveDirections = new List<Vector3>();
		phases = new ArrayList ();
		enemyCounter = enemyCount;
		sprite = Game.control.spriteLib.SetCharacterSprite(spriteName);
	}

	public void SetUpBoss(float index, string name, bool _isMidBoss){
		bossIndex = index;
		bossName = name;
		isMidBoss = _isMidBoss;
	}

	public void Spawn(int waveindex, int enemyIndex){
		EnemyMovementPattern pat = movementPattern.GetNewEnemyMovement(movementPattern);
		if(spawnPositions.Count > 1) pat.spawnPosition = spawnPositions[enemyIndex];
		GameObject enemy = GameObject.Instantiate (Resources.Load ("Prefabs/Enemy"), pat.spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;
		enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0, this);
		
		if(enterDirections.Count > 1) pat.enterDir = enterDirections[enemyIndex];
		if(leaveDirections.Count > 1) pat.leaveDir = leaveDirections[enemyIndex];

		enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (pat);
		

		if (isBoss || isMidBoss) {
			if	 (bossIndex == 0.5f) bossScript = enemy.AddComponent<Boss05> ();
			else if(bossIndex == 1f) bossScript = enemy.AddComponent<Boss1> ();
			else if(bossIndex == 2f) bossScript = enemy.AddComponent<Boss2> ();
			bossScript.Init();

			Game.control.enemySpawner.DestroyAllProjectiles();
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
}