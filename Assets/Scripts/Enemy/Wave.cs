using UnityEngine;
using System.Collections;

public class Wave
{
	public float spawnTime;
	public Vector3 spawnPosition;
	public bool spawned;
	public float enemyCount;
	public float enemyCounter;

	public int health;
	public int healthBars;
	public bool isBoss;
	public float shootSpeed;
	public bool simultaneous;
	public ArrayList spawnPositions;

	public EnemyLife life;
	public EnemyMovementPattern movementPattern;
	public Pattern shootPattern;
	public ArrayList bossPatterns;
	public ArrayList bossMovePatterns;
	public ArrayList phases;
	public ArrayList phasePatterns;

	public Sprite sprite;
	public Phase phase;

	public float bossIndex;
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
		//shootPattern = ;

		health = _health;
		healthBars = _healthBars;
		isBoss = _isBoss;
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

	public void SetUpBoss(float index, string name){
		bossIndex = index;
		bossName = name;

	}

	public void Spawn(){
		//Debug.Log ("spawn");
		spawnPosition = (Vector3)spawnPositions [0];
		if(spawnPositions.Count > 1){
			spawnPositions.Reverse ();
			spawnPosition =  (Vector3)spawnPositions [spawnPositions.Count - 1];
		}

		GameObject enemy = GameObject.Instantiate (Resources.Load ("Enemy"), spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;


		enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0);
		enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (new EnemyMovementPattern(movementPattern));

		if (isBoss) {
			

			enemy.GetComponent<SpriteRenderer> ().sprite = sprite;
			if (bossIndex % 1 == 0) { //IF NOT MID BOSS
				enemy.tag = "Boss";
				enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0.3f);
				GameController.gameControl.stage.ToggleTimer (false);
				GameController.gameControl.dialog.StartDialog ("Boss", bossIndex, false);

			} else {
				enemy.tag = "MidBoss";
				enemy.GetComponent<EnemyLife> ().SetHealth (health, healthBars, 0f);
			}
			enemy.GetComponent<EnemyShoot> ().NextBossPhase ();
			GameController.gameControl.ui.ToggleBossHealthSlider (true, enemy.GetComponent<EnemyLife> ().maxHealth, bossName);

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

	public void AdjustSpawnPosition(Vector3 pos)
	{
		spawnPosition = pos;
	}

	public void SetSpeed(float s)
	{
		shootSpeed = s;
	}

	public void SetPhase(Phase _phase){
		phase = _phase;
	}


	public void SetBossPatterns(Pattern p)
	{
		bossPatterns.Add(p);
	}

	public void SetBossMovePatterns(EnemyMovementPattern m)
	{
		bossMovePatterns.Add(m);
	}
}