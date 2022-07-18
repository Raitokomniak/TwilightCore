using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave
{
	public float spawnTime;
	public float spawnCoolDown = 1;
	public int enemyCount;
	public int enemyCounter;

    public bool hasHealth;
	public int health;
	public int healthBars;
	public bool isBoss;
	public bool isMidBoss = false;
	public float shootSpeed;
	public bool simultaneous; // NOT USED YET BUT DONT DELETE
	public List<Vector3> spawnPositions;
	//public List<Vector3> enterDirections; //BECOMING OBSOLETE !!!!!!!!!!!!!!!!!!!!!!!!
	//public List<Vector3> leaveDirections; //BECOMING OBSOLETE !!!!!!!!!!!!!!!!!!!!!!!!
	public EnemyMovementPattern movementPattern;
	public Pattern shootPattern;
	public bool oneShot;
	public Sprite sprite;
	
	public bool invulnerable;

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
		//enterDirections = new List<Vector3>();
		//leaveDirections = new List<Vector3>();
		enemyCounter = enemyCount;	
		sprite = Game.control.spriteLib.SetEnemySprite(spriteName);
		
	}

	//FOR BOSSES
	public Wave(EnemyMovementPattern _movementPattern, float _spawnTime, int _health, bool _isBoss, int _healthBars)
	{
		movementPattern = _movementPattern;
		enemyCount = 1;
		spawnTime = _spawnTime;
		health = _health;
		healthBars = _healthBars;
		isBoss = _isBoss;
		isMidBoss = !isBoss;
		spawnPositions = new List<Vector3>();
		//enterDirections = new List<Vector3>();
		//leaveDirections = new List<Vector3>();
		enemyCounter = enemyCount;
	}

	public void SetUpBoss(float index, string name, bool _isMidBoss){
		bossIndex = index;
		bossName = name;
		isMidBoss = _isMidBoss;
	}

	public void FillPositionArray(List<Vector3> array){
		int timesToMultiply = 0;
		int tempPosCount = array.Count;

		if(array.Count > 1){
			if(enemyCount > tempPosCount){
				timesToMultiply = Mathf.FloorToInt(enemyCount / array.Count - 1);

				if(timesToMultiply > 0){
					for(int i = 0; i < timesToMultiply; i++){
						for(int j = 0; j < tempPosCount; j++)	{
							array.Add(array[j]);
						}
					}
				}
			}
		}
	}
	//if more enemies than spawn/enter/leave positions, multiplies the amount of positions so enemyindex is not OOB
	public void FillPositionsArraysByEnemyCount(){
		FillPositionArray(spawnPositions);
		//FillPositionArray(enterDirections);
		//FillPositionArray(leaveDirections);
	}

	public void Spawn(int enemyIndex){
		//create enemy instance at movement pat spawn pos
		EnemyMovementPattern pat = new EnemyMovementPattern(movementPattern);

		if(spawnPositions.Count > 1) pat.spawnPosition = spawnPositions[enemyIndex];
		GameObject enemy = GameObject.Instantiate (Resources.Load ("Prefabs/Enemy"), pat.spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;
		
		//add and init components
		enemy.AddComponent<EnemyLife> ();
		enemy.GetComponent<EnemyLife> ().SetHealth (health);
		enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (pat);
		enemy.GetComponentInChildren<SpriteRenderer> ().sprite = sprite;
		enemy.GetComponent<EnemyShoot> ().SetUpAndShoot (shootPattern, shootSpeed);

		//apply modifiers
		if(pat.hideSpriteOnSpawn) enemy.GetComponent<EnemyMovement>().EnableSprite(false);
		if(pat.disableHitBox) 	  enemy.GetComponent<BoxCollider2D>().enabled = false;
	}

	public void SpawnBoss(){
		//create boss instance at movement pat spawn pos
		EnemyMovementPattern pat = new EnemyMovementPattern(movementPattern);
		GameObject enemy = GameObject.Instantiate (Resources.Load ("Prefabs/Enemy"), pat.spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;

		//add and init components
		enemy.AddComponent<BossLife>();
		enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (pat);
		if(isBoss) 	  enemy.tag = "Boss";
		if(isMidBoss) enemy.tag = "MidBoss";
		enemy.GetComponent<EnemyLife>().enabled = false;
		GetBossScript(enemy);

		enemy.GetComponent<BossLife> ().Init (health, healthBars, bossScript);
		enemy.GetComponentInChildren<SpriteRenderer> ().sprite = sprite;

		GameObject MCspriteAnim = enemy.transform.GetChild(2).gameObject;
		MCspriteAnim.SetActive(true);
		MCspriteAnim.GetComponent<AnimationController>().StartRotating(1f);
		//MCspriteAnim.GetComponent<SpriteAnimationController>().rotationSpeed

		//apply modifiers
		if(pat.hideSpriteOnSpawn) enemy.GetComponent<EnemyMovement>().EnableSprite(false);
		if(pat.disableHitBox)     enemy.GetComponent<BoxCollider2D>().enabled = false;

		//start execution
		Game.control.enemySpawner.DestroyAllProjectiles();
		bossScript.NextPhase ();
		Game.control.stageUI.BOSS.ToggleBossHealthSlider (true, enemy.GetComponent<BossLife> ().maxHealth, bossName);
	}

    void GetBossScript(GameObject enemy){
        if	 (bossIndex == 0.5f) bossScript = enemy.AddComponent<Boss05> ();
		else if(bossIndex == 1f) bossScript = enemy.AddComponent<Boss1> ();
		else if(bossIndex == 2f) bossScript = enemy.AddComponent<Boss2> ();
		else if(bossIndex == 3f) bossScript = enemy.AddComponent<Boss3> ();
        else if(bossIndex == 4f) bossScript = enemy.AddComponent<Boss4> ();
        else if(bossIndex == 4.1f) bossScript = enemy.AddComponent<Boss4_1> ();
        else if(bossIndex == 4.2f) bossScript = enemy.AddComponent<Boss4_2> ();
        else if	(bossIndex == 2.5f) bossScript = enemy.AddComponent<Boss25> ();

        bossScript.Init();
    }
}