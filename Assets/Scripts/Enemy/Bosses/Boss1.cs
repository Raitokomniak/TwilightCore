using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Phaser
{
	void Awake(){
		bossIndex = 1;
		Game.control.stageHandler.bossOn = true;
		Game.control.stageHandler.bossBonus = true;
	}

	public override void StopCoro(){
		if(phaseExecuteRoutine != null) StopCoroutine (phaseExecuteRoutine);
		
		routineOver = true;
	}

//	public 

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
				patterns.Add(new P_Spiral(20 * difficultyMultiplier));
				patterns[0].bulletMovement = new BMP_WaitAndExplode (patterns[0], 5f);
				patterns[0].loopCircles =  288 * difficultyMultiplier;
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode (patterns[1], 6f, false);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount =  2 * difficultyMultiplier;
				patterns[1].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow");
				patterns[2].bulletMovement = new BMP_Explode (patterns[2], 6f, false);
				patterns[2].bulletCount =  2 * difficultyMultiplier;
				patterns[2].rotationDirection = -1;

				movementPatterns.Add(new EMP_Rock());
				movementPatterns[0].speed = 7f;

				while (!phaser.endOfPhase) {
					enemyMove.movementPattern.UpdateDirection(lib.centerX, lib.topCenterY);
					yield return new WaitForSeconds(2f);
					
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);

					yield return new WaitForSeconds(2.2f);

					patterns[1].StopPattern();
					patterns[2].StopPattern();

					enemyMove.SetUpPatternAndMove (movementPatterns[0]);

					yield return new WaitForSeconds(1f);
					enemy.BossShoot (patterns[0]);

					yield return new WaitForSeconds(2.2f);

					enemy.BossShoot (patterns[0]);

					yield return new WaitForSeconds(1f);
				}
				break;
            
			case 1:
				Game.control.sound.PlaySpellSound ("Enemy");
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Ninetailed Spear");

				patterns.Add(new P_Curtain());
				patterns[0].bulletCount = Mathf.CeilToInt(1.8f * difficultyMultiplier);  
				patterns[0].SetSprite ("Circle", "Bevel", "Lilac");
				patterns[0].bulletMovement = new BMP_TurnToSpears(patterns[0], 6f);

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount = 2 * difficultyMultiplier;
				patterns[1].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow");
				patterns[2].bulletMovement = new BMP_Explode(patterns[2], 4f, false);
				patterns[2].rotationDirection =  -1;
				patterns[2].bulletCount = 2 * difficultyMultiplier;

				enemyMove.movementPattern.UpdateDirection(-15, 6);
				yield return new WaitForSeconds(1f);
				
				while (!phaser.endOfPhase) {
					yield return new WaitUntil (() => enemyMove.movementPattern.CheckIfReachedDestination (enemyMove) == true);
					//enemyMove.SetUpPatternAndMove (movementPatterns[1]);
					enemyMove.movementPattern.UpdateDirection(1, 6);
					enemy.BossShoot (patterns[0]);
					yield return new WaitUntil (() => enemyMove.movementPattern.CheckIfReachedDestination (enemyMove) == true);
                    yield return new WaitForSeconds(1f);
                    enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2.2f);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
					yield return new WaitForSeconds(1f);
					//enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					enemyMove.movementPattern.UpdateDirection(-15, 6);
					enemy.BossShoot (patterns[0]);
					yield return new WaitUntil (() => enemyMove.movementPattern.CheckIfReachedDestination (enemyMove) == true);
					yield return new WaitForSeconds(1f);
					enemy.BossShoot (patterns[1]);
					enemy.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2.2f);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
					yield return new WaitForSeconds(1f);
				}

				break;
			
			case 2:
				patterns.Add(new P_Spiral(20 * difficultyMultiplier));
				//patterns[0].bulletMovement = new BulletMovementPattern (true, "WaitAndExplode", 5f, patterns[0], 0, 14);
				patterns[0].bulletMovement = new BMP_WaitAndExplode(patterns[0], 5f);
				patterns[0].loopCircles = 288 * difficultyMultiplier;
				patterns[0].SetSprite ("Circle", "Glow", "Green");

				patterns.Add(new P_SingleHoming());
				patterns[1].bulletMovement = new BMP_TurnToSpears(patterns[1], 5f);
				patterns[1].bulletCount = 1;
				patterns[1].SetSprite ("Circle", "Bevel", "Lilac");

				Vector2 teleP1 = new Vector2(lib.centerX + 4f, enemy.transform.position.y);
				Vector2 teleP2 = new Vector2(lib.centerX - 4f, enemy.transform.position.y);

				movementPatterns.Add(new EMP_Teleport());

				while (!phaser.endOfPhase) {
					movementPatterns[0].UpdateDirection(teleP1.x, teleP1.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					enemy.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2f);
					enemy.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP2.x, teleP2.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					enemy.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP1.x, teleP1.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					enemy.BossShoot(patterns[1]);

					movementPatterns[0].UpdateDirection(teleP2.x, teleP2.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					enemy.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2f);
					enemy.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP1.x, teleP1.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					enemy.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP2.x, teleP2.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					enemy.BossShoot(patterns[1]);

				}
				break;
			case 3:
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Fox Fires");

				patterns.Add(new P_Cluster(difficultyMultiplier));
																						//0.01f
				patterns[0].SetSprite ("Fireball", "Glow", "Orange");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false);
				patterns[1].rotationDirection = 1;
				patterns[1].SetSprite ("Circle", "Big", "Red");
				patterns[1].bulletCount =  Mathf.CeilToInt(1.2f * difficultyMultiplier);
				patterns[1].coolDown = 2.5f / difficultyMultiplier;

				movementPatterns.Add(new EMP_Swing(7, 1));
				movementPatterns[0].centerPoint = new Vector3 (lib.centerX, lib.topCenterY, 0);
				
				while (!endOfPhase) {
					enemyMove.movementPattern.UpdateDirection(lib.enterRight.x, lib.enterRight.y);
					enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(2f);

					enemy.BossShoot (patterns[0]);

					yield return new WaitForSeconds(4f);
					enemyMove.moving = false;
					movementPatterns[0].speed = 7f;

					movementPatterns[0].UpdateDirection(lib.centerX, lib.centerY);
					enemyMove.moving = true;
					yield return new WaitForSeconds(1f);

					enemy.BossShoot (patterns[1]);

					yield return new WaitForSeconds(4f);

					patterns[1].StopPattern();
				}
				break;
		}
		Debug.Log("NATURAL END OF COROUTINE");
		phaser.routineOver = true;
	}
}
