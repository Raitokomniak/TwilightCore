using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Phaser
{
	void Awake(){
		bossIndex = 1;
		numberOfPhases = 4;
		Game.control.stageHandler.bossOn = true;
		Game.control.stageHandler.bossBonus = true;
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
		//odd number is super phase
		

		switch (phase) {
			case 0:
				patterns.Add(new P_Spiral(20 * difficultyMultiplier));
				patterns[0].bulletMovement = new BMP_WaitAndExplode (patterns[0], 5f);
				patterns[0].loopCircles =  288 * difficultyMultiplier;
				patterns[0].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[0].bulletMovement.accelSpeed = 30;

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode (patterns[1], 6f, false);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount =  2 * difficultyMultiplier;
				patterns[1].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[1].bulletMovement.accelSpeed = 30;

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[2].bulletMovement = new BMP_Explode (patterns[2], 6f, false);
				patterns[2].bulletCount =  2 * difficultyMultiplier;
				patterns[2].rotationDirection = -1;

				movementPatterns.Add(new EMP_Rock());
				movementPatterns[0].speed = 7f;

				while (!endOfPhase) {
					movement.movementPattern.UpdateDirection(vectorLib.centerX, vectorLib.topCenterY);
					yield return new WaitForSeconds(2f);
					
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);

					yield return new WaitForSeconds(2.2f);

					patterns[1].StopPattern();
					patterns[2].StopPattern();

					movement.SetUpPatternAndMove (movementPatterns[0]);

					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds(2.2f);

					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds(1f);
				}
				break;
            
			case 1:
				Game.control.sound.PlaySpellSound ("Enemy");
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Ninetailed Spear");
				StartPhaseTimer(30);

				patterns.Add(new P_Curtain());
				patterns[0].bulletCount = Mathf.CeilToInt(1.8f * difficultyMultiplier);  
				patterns[0].SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				patterns[0].bulletMovement = new BMP_TurnToSpears(patterns[0], 6f);
				patterns[0].bulletMovement.accelSpeed = 30;

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount = 2 * difficultyMultiplier;
				patterns[1].SetSprite ("Circle", "Glow", "Green", "Medium");

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[2].bulletMovement = new BMP_Explode(patterns[2], 4f, false);
				patterns[2].rotationDirection =  -1;
				patterns[2].bulletCount = 2 * difficultyMultiplier;

				movement.movementPattern.UpdateDirection(-15, 6);
				yield return new WaitForSeconds(1f);
				
				while (!endOfPhase) {
					yield return new WaitUntil (() => movement.movementPattern.CheckIfReachedDestination (movement) == true);
					//enemyMove.SetUpPatternAndMove (movementPatterns[1]);
					movement.movementPattern.UpdateDirection(1, 6);
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movement.movementPattern.CheckIfReachedDestination (movement) == true);
                    yield return new WaitForSeconds(1f);
                    shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2.2f);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
					yield return new WaitForSeconds(1f);
					//enemyMove.SetUpPatternAndMove (movementPatterns[0]);
					movement.movementPattern.UpdateDirection(-15, 6);
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movement.movementPattern.CheckIfReachedDestination (movement) == true);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
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
				patterns[0].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[0].bulletMovement.accelSpeed = 30;

				patterns.Add(new P_SingleHoming());
				patterns[1].bulletMovement = new BMP_TurnToSpears(patterns[1], 5f);
				patterns[1].bulletCount = 1;
				patterns[1].SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				patterns[1].bulletMovement.accelSpeed = 30;

				Vector2 teleP1 = new Vector2(vectorLib.centerX + 4f, shooter.transform.position.y);
				Vector2 teleP2 = new Vector2(vectorLib.centerX - 4f, shooter.transform.position.y);

				movementPatterns.Add(new EMP_Teleport());

				while (!endOfPhase) {
					movementPatterns[0].UpdateDirection(teleP1.x, teleP1.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					shooter.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP2.x, teleP2.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP1.x, teleP1.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);

					movementPatterns[0].UpdateDirection(teleP2.x, teleP2.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					shooter.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP1.x, teleP1.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection(teleP2.x, teleP2.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);

				}
				break;
			case 3:
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Fox Fires");
				StartPhaseTimer(30);
				
				patterns.Add(new P_Cluster(difficultyMultiplier));																		//0.01f
				patterns[0].SetSprite ("Fireball", "Glow", "Orange", "Medium");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false);
				patterns[1].rotationDirection = 1;
				patterns[1].SetSprite ("Circle", "Big", "Red", "Big");
				patterns[1].bulletCount =  Mathf.CeilToInt(1.2f * difficultyMultiplier);
				patterns[1].coolDown = 2.5f / difficultyMultiplier;

				movementPatterns.Add(new EMP_Swing(7, 1));
				movementPatterns[0].centerPoint = new Vector3 (vectorLib.centerX, vectorLib.topCenterY, 0);
				
				while (!endOfPhase) {
					movement.movementPattern.UpdateDirection(vectorLib.enterRight.x, vectorLib.enterRight.y);
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(2f);

					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds(4f);
					movement.moving = false;
					movementPatterns[0].speed = 7f;

					movementPatterns[0].UpdateDirection(vectorLib.centerX, vectorLib.centerY);
					movement.moving = true;
					yield return new WaitForSeconds(1f);

					shooter.BossShoot (patterns[1]);

					yield return new WaitForSeconds(4f);

					patterns[1].StopPattern();
				}
				break;
		}
		NextPhase();
		phaser.routineOver = true;
	}
}
