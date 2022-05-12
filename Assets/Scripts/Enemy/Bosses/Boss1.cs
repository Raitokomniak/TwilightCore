using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Phaser
{
	void Awake(){
		bossIndex = 1;
	}

	public override void StopCoro(){
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		routineOver = true;
	}

    public override void ExecutePhase(int phase, Phaser _phaser){
		phaseExecuteRoutine = Execute (phase, _phaser);
		StartCoroutine (phaseExecuteRoutine);
    }

    IEnumerator Execute(int phase, Phaser phaser){
		difficultyMultiplier = Game.control.stageHandler.difficultyMultiplier;
		ResetLists();
		GetComponent<EnemyMovement>().EnableSprite(true);
		

		switch (phase) {
			case 0:
				patterns.Add(new Pattern (lib.spiral));
				patterns[0].Customize (new BulletMovementPattern (true, "WaitAndExplode", 5f, patterns[0], 0, 14));
				patterns[0].Customize ("LoopCircles", 288 * difficultyMultiplier);
				patterns[0].Customize ("BulletCount", 20 * difficultyMultiplier);
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].Customize ("RotationDirection", 1);
				patterns[1].Customize ("BulletCount", 2 * difficultyMultiplier);
				patterns[1].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[2].SetSprite ("Circle", "Glow", "Yellow");
				patterns[2].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[2], 0, 14));
				patterns[2].Customize ("BulletCount", 2 * difficultyMultiplier);
				patterns[2].Customize ("RotationDirection", -1);

				movementPatterns.Add(new EnemyMovementPattern (lib.centerHor));
				movementPatterns[0].Customize ("Speed", 7f);
				movementPatterns.Add(new EnemyMovementPattern (lib.rocking));
				movementPatterns[1].Customize ("Speed", 7f);


				while (!phaser.endOfPhase) {
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);

					WaitForSecondsFloat(2f);
					yield return new WaitUntil(() => timerDone == true);

					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);

					WaitForSecondsFloat(2.2f);
					yield return new WaitUntil(() => timerDone == true);

					patterns[1].StopPattern();
					patterns[2].StopPattern();

					enemyMove.SetUpPatternAndMove (new EnemyMovementPattern (lib.rocking));

					WaitForSecondsFloat(2f);
					yield return new WaitUntil(() => timerDone == true);
					enemy.BossShoot (patterns[0]);

					WaitForSecondsFloat(2.2f);
					yield return new WaitUntil(() => timerDone == true);

					enemy.BossShoot (patterns[0]);

					WaitForSecondsFloat(3f);
					yield return new WaitUntil(() => timerDone == true);
				}
			
				break;
            
			case 1:
				Game.control.sound.PlaySpellSound ("Enemy");
				Game.control.ui.ShowActivatedPhase ("Boss", "Hoodwink: Ninetailed Spear");

				patterns.Add(new Pattern (lib.curtain));
				patterns[0].Customize ("BulletCount", Mathf.Ceil(1.8f * difficultyMultiplier));  
				patterns[0].SetSprite ("Circle", "Bevel", "Lilac");
				patterns[0].Customize (new BulletMovementPattern (false, "TurnToSpears", 6f, patterns[0], 0, 14));

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].Customize ("RotationDirection", 1);
				patterns[1].Customize("BulletCount", 2 * difficultyMultiplier);
				patterns[1].SetSprite ("Circle", "Glow", "Green");
				

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[2].SetSprite ("Circle", "Glow", "Yellow");
				patterns[2].Customize (new BulletMovementPattern (true, "Explode", 4f, patterns[2], 0, 14));
				patterns[2].Customize ("RotationDirection", -1);
				patterns[2].Customize("BulletCount", 2 * difficultyMultiplier);

				movementPatterns.Add(new EnemyMovementPattern ("", new Vector3 (-15, 6f, 0f), false, 0));
				movementPatterns[0].Customize ("Speed", 7f);

				movementPatterns.Add(new EnemyMovementPattern ("", new Vector3 (1, 6, 0), false, 0));
				movementPatterns[1].Customize ("Speed", 7f);


				enemyMove.SetUpPatternAndMove (movementPatterns[0]);
				WaitForSecondsFloat(1f);
				yield return new WaitUntil(() => timerDone == true);
				
				while (!phaser.endOfPhase) {
					yield return new WaitUntil (() => movementPatterns[0].CheckIfReachedDestination (enemyMove) == true);
					enemyMove.SetUpPatternAndMove (movementPatterns[1]);
					enemy.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movementPatterns[0].CheckIfReachedDestination (enemyMove) == true);
                    WaitForSecondsFloat(1f);
					yield return new WaitUntil(() => timerDone == true);
                    enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);
					WaitForSecondsFloat(2.2f);
					yield return new WaitUntil(() => timerDone == true);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
					WaitForSecondsFloat(1f);
					yield return new WaitUntil(() => timerDone == true);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					enemy.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movementPatterns[0].CheckIfReachedDestination (enemyMove) == true);
					WaitForSecondsFloat(1f);
					yield return new WaitUntil(() => timerDone == true);
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);
					WaitForSecondsFloat(2.2f);
					yield return new WaitUntil(() => timerDone == true);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
					WaitForSecondsFloat(1f);
					yield return new WaitUntil(() => timerDone == true);
				}

				break;
			
			case 2:
				patterns.Add(new Pattern (lib.spiral));
				patterns[0].Customize (new BulletMovementPattern (true, "WaitAndExplode", 5f, patterns[0], 0, 14));
				patterns[0].Customize ("LoopCircles", 288 * difficultyMultiplier);
				patterns[0].Customize ("BulletCount", 20 * difficultyMultiplier);
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				movementPatterns.Add(new EnemyMovementPattern ("", new Vector3 (lib.centerX + 4f, enemy.transform.position.y, 0), false, 0));
				movementPatterns[0].Customize ("Teleport", 1);
				movementPatterns.Add(new EnemyMovementPattern ("", new Vector3 (lib.centerX - 4f, enemy.transform.position.y, 0), false, 0));
				movementPatterns[1].Customize ("Teleport", 1);

				while (!phaser.endOfPhase) {
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					enemy.BossShoot(patterns[0]);
					WaitForSecondsFloat(5f);
					yield return new WaitUntil(() => timerDone == true);
					enemyMove.SetUpPatternAndMove (movementPatterns[1]);
					enemy.BossShoot(patterns[0]);
					WaitForSecondsFloat(5f);
					yield return new WaitUntil(() => timerDone == true);
				}
				break;
			case 3:
				Game.control.ui.ShowActivatedPhase ("Boss", "Hoodwink: Fox Fires");

				patterns.Add(new Pattern ("Cluster", true, 30 * difficultyMultiplier, 0, 0.05f / difficultyMultiplier, 0, 1f));
																						//0.01f
				patterns[0].SetSprite ("Fireball", "Glow", "Orange");

				patterns.Add(new Pattern (lib.maelStrom));
				patterns[1].Customize (new BulletMovementPattern (true, "Explode", 6f, patterns[1], 0, 14));
				patterns[1].Customize ("RotationDirection", 1);
				patterns[1].SetSprite ("Circle", "Big", "Red");
				patterns[1].Customize ("BulletCount", Mathf.Ceil(1.2f * difficultyMultiplier));
				patterns[1].Customize ("CoolDown", 2.5f / difficultyMultiplier);

				movementPatterns.Add(new EnemyMovementPattern ("Swing", new Vector3 (-13, enemy.transform.position.y, 0), false, 0));
				movementPatterns[0].Customize ("Speed", 5f);
				movementPatterns[0].Customize ("Direction", 1);

				movementPatterns.Add(new EnemyMovementPattern ("", new Vector3 (lib.centerX, lib.centerY, 0), false, 0));

				while (!endOfPhase) {
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					WaitForSecondsFloat(2f);
					yield return new WaitUntil(() => timerDone == true);

					enemy.BossShoot (patterns[0]);

					WaitForSecondsFloat(4f);
					yield return new WaitUntil(() => timerDone == true);
					
					movementPatterns[0].Customize ("Speed", 7f);

					enemyMove.SetUpPatternAndMove (movementPatterns[1]);
					WaitForSecondsFloat(1f);
					yield return new WaitUntil(() => timerDone == true);

					enemy.BossShoot (patterns[1]);

					WaitForSecondsFloat(4f);
					yield return new WaitUntil(() => timerDone == true);

					patterns[1].StopPattern();
				}
				break;
		}
		Debug.Log("NATURAL END OF COROUTINE");
		phaser.routineOver = true;
	}
}
