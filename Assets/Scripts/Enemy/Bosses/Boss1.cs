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
		VectorLib lib = Game.control.vectorLib;
		difficulty = Game.control.stageHandler.difficultyMultiplier;
		ResetLists();
		GetComponent<EnemyMovement>().EnableSprite(true);
		//odd number is super phase
		

		switch (phase) {
			case 0:
				patterns.Add(new P_Spiral(20 * difficulty));
				patterns[0].bulletMovement = new BMP_WaitAndExplode (patterns[0], 5f);
				patterns[0].loopCircles =  288 * difficulty;
				patterns[0].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[0].bulletMovement.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode (patterns[1], 6f, false, false);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount =  2 * difficulty;
				patterns[1].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[1].bulletMovement.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[2].bulletMovement = new BMP_Explode (patterns[2], 6f, false, false);
				patterns[2].bulletCount =  2 * difficulty;
				patterns[2].rotationDirection = -1;

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].speed = 7f;
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 4)});
			
				while (!endOfPhase) {
					
					yield return new WaitForSeconds(2.2f);
					movement.pattern.UpdateDirection("X3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);

					yield return new WaitForSeconds(2.2f);

					patterns[1].StopPattern();
					patterns[2].StopPattern();

					movement.pattern.UpdateDirection("C3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds(2.2f);

					movement.pattern.UpdateDirection("I3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds(2.2f);
				}
				
				break;
            
			case 1:
				Game.control.sound.PlaySpellSound ("Enemy", "Default");
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Ninetailed Spear");
				StartPhaseTimer(30);

				patterns.Add(new P_Curtain());
				patterns[0].bulletCount = Mathf.CeilToInt(1.8f * difficulty);  
				patterns[0].SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				patterns[0].bulletMovement = new BMP_TurnToSpears(patterns[0], 6f);
				patterns[0].bulletMovement.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false, false);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].SetSprite ("Circle", "Glow", "Green", "Medium");

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[2].bulletMovement = new BMP_Explode(patterns[2], 4f, false, false);
				patterns[2].rotationDirection =  -1;
				patterns[2].bulletCount = 2 * difficulty;

				movement.pattern.force = false;
				movement.pattern.UpdateDirection("B3");
				yield return new WaitForSeconds(1f);
				
				while (!endOfPhase) {
					yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
					movement.pattern.UpdateDirection("I3");
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
                    yield return new WaitForSeconds(1f);
                    shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2.2f);
					patterns[1].StopPattern();
					patterns[2].StopPattern();
					yield return new WaitForSeconds(1f);
					movement.pattern.UpdateDirection("B3");
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
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
				movement.pattern.force = true;
				patterns.Add(new P_Spiral(20 * difficulty));
				//patterns[0].bulletMovement = new BulletMovementPattern (true, "WaitAndExplode", 5f, patterns[0], 0, 14);
				patterns[0].bulletMovement = new BMP_WaitAndExplode(patterns[0], 5f);
				patterns[0].loopCircles = 288 * difficulty;
				patterns[0].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[0].bulletMovement.accelMax = 30;

				patterns.Add(new P_SingleHoming());
				patterns[1].bulletMovement = new BMP_TurnToSpears(patterns[1], 5f);
				patterns[1].bulletCount = 1;
				patterns[1].SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				patterns[1].bulletMovement.accelMax = 30;

				//Vector2 teleP1 = new Vector2(vectorLib.GetVector("X1").x + 4f, shooter.transform.position.y);
				//Vector2 teleP2 = new Vector2(vectorLib.GetVector("X1").x - 4f, shooter.transform.position.y);

				//movementPatterns.Add(new EMP_Teleport());
				movementPatterns.Add(new EnemyMovementPattern());
				//movementPatterns[0].infinite = true;
				movementPatterns[0].teleports = true;

				while (!endOfPhase) {
					movementPatterns[0].UpdateDirection("D4");
					movement.SetUpPatternAndMove (movementPatterns[0]);
					shooter.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection("H4");
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection("D4");
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);

					movementPatterns[0].UpdateDirection("H4");
					movement.SetUpPatternAndMove (movementPatterns[0]);
					shooter.BossShoot(patterns[0]);
					yield return new WaitForSeconds(2f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection("D4");
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);
					movementPatterns[0].UpdateDirection("H4");
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitForSeconds(4f);
					shooter.BossShoot(patterns[1]);

				}
				break;
			case 3:
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Fox Fires");
				StartPhaseTimer(30);
				
				patterns.Add(new P_FoxFires(difficulty, 40 * difficulty));
				patterns[0].SetSprite ("Fireball", "Glow", "Orange", "Small");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletMovement = new BMP_Explode(patterns[1], 6f, false, false);
				patterns[1].rotationDirection = 1;
				patterns[1].SetSprite ("Circle", "Big", "Red", "Big");
				patterns[1].bulletCount =  Mathf.CeilToInt(1.2f * difficulty);
				patterns[1].coolDown = 2.5f / difficulty;

				//movementPatterns.Add(new EMP_Swing(15, 1));
				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("B3", 1), new WayPoint("J3", 5, 1), new WayPoint("XY", 5)});
				movementPatterns[0].centerPoint = vectorLib.GetVector("X3");
				

				while (!endOfPhase) {
					movement.SetUpPatternAndMove (movementPatterns[0]);
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					movementPatterns[0].force = false;
					movementPatterns[0].speed = 17f;
					movement.SmoothAcceleration(50);
					yield return new WaitForSeconds(2f);
					shooter.BossShoot (patterns[0]);
					yield return new WaitUntil(() => movement.pattern.rotateOnAxis == false);
					movement.moving = false;
					movementPatterns[0].force = true;
					movementPatterns[0].speed = 7f;
					movement.moving = true;
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
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
