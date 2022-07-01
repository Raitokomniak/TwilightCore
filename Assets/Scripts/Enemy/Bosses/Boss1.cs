using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Phaser
{
	void Awake(){
		bossIndex = 1;
		numberOfPhases = 4;
		Game.control.stageHandler.bossOn = true;
        Game.control.stageHandler.bossScript = this;
		bossBonus = true;
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

		Pattern p;
		//odd number is super phase
		

		switch (phase) {
			case 0:
				Game.control.ui.BOSS.ShowActivatedPhase ("Daffodil Hypnosis");
                StartPhaseTimer(30);

				//daffodil center
				p = new P_Circle(10 * difficulty);
				p.BMP = new BMP_Explode(p, 2);
				p.BMP.waitAndExplodeWaitTime = 1f;
				p.loopCircles =  100 * difficulty;
				p.SetSprite ("Circle", "Glow", "Orange", "Small");
				p.BMP.accelMax = 5;
				patterns.Add(p);

				//daffodil petals
				p = new P_Maelstrom();
				p.BMP = new BMP_Explode (p, 6f);
				p.rotationDirection =  1;
				p.bulletCount =  2 * difficulty;
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP.accelMax = 30;
				patterns.Add(p);

				p = new P_Maelstrom();
				p.SetSprite ("Circle", "Glow", "Yellow", "Medium");
				p.BMP = new BMP_Explode (p, 6f);
				p.bulletCount =  2 * difficulty;
				p.rotationDirection = -1;
				patterns.Add(p);

				//daffodil leaves
				p = new P_Maelstrom(3 * difficulty, 0.2f);
				p.BMP = new BMP_Explode(p, 10);
				p.rotationDirection = 1;
				p.startingRotation = 40;
				p.maelStromRotationMultiplier = 0.03f;
				p.SetSprite ("Diamond", "Glow", "Green", "Small");
				p.coolDown = 0.3f;
				patterns.Add(p);

				p = new P_Maelstrom(3 * difficulty, 0.2f);
				p.BMP = new BMP_Explode(p, 10);
				p.rotationDirection = -1;
				p.maelStromRotationMultiplier = 0.03f;
				p.startingRotation = 40;
				p.SetSprite ("Diamond", "Glow", "Green", "Small");
				p.coolDown = 0.3f;
				patterns.Add(p);

				//spear

				p = new P_SingleHoming();
				p.BMP = new BMP_TurnToSpears(p, 5f);
				p.bulletCount = 1;
				p.SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				p.BMP.accelMax = 30;
				patterns.Add(p);

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].speed = 7f;
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 4)});

				while (!endOfPhase) {
					
					movement.pattern.UpdateDirection("X3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[0]);
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[3]);
					shooter.BossShoot (patterns[4]);
					
					yield return new WaitForSeconds(1f);
					
					
				
					yield return new WaitForSeconds(3f);
					patterns[3].StopPattern();
					patterns[4].StopPattern();
					patterns[1].StopPattern();
					patterns[2].StopPattern();

					movement.pattern.UpdateDirection("C3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[5]);

					movement.pattern.UpdateDirection("I3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

					shooter.BossShoot (patterns[5]);

					
				}
				
				break;
            
			case 1:
				Game.control.sound.PlaySpellSound ("Enemy", "Default");
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Ninetailed Spear");
				Game.control.ui.WORLD.SetTopLayerSpeed(2f);

				StartPhaseTimer(30);

				patterns.Add(new P_Curtain());
				patterns[0].bulletCount = Mathf.CeilToInt(1.8f * difficulty);  
				patterns[0].SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				patterns[0].BMP = new BMP_TurnToSpears(patterns[0], 6f);
				patterns[0].BMP.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[1].BMP = new BMP_Explode(patterns[1], 6f);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].SetSprite ("Circle", "Glow", "Green", "Medium");

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[2].BMP = new BMP_Explode(patterns[2], 4f);
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

				StartPhaseTimer(30);

				Game.control.ui.BOSS.ShowActivatedPhase ("Double Daffodil Hypnosis");
				patterns.Add(new P_Spiral(20 * difficulty));
				patterns[0].BMP = new BMP_WaitAndExplode (patterns[0], 25f);
				patterns[0].loopCircles =  288 * difficulty;
				patterns[0].SetSprite ("Circle", "Glow", "Green", "Medium");
				patterns[0].BMP.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[1].BMP = new BMP_Explode (patterns[1], 6f);
				patterns[1].rotationDirection =  1;
				patterns[1].bulletCount =  2 * difficulty;
				patterns[1].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[1].BMP.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[2].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[2].BMP = new BMP_Explode (patterns[2], 6f);
				patterns[2].bulletCount =  2 * difficulty;
				patterns[2].rotationDirection = -1;

				patterns.Add(new P_Spiral(10 * difficulty));
				patterns[3].BMP = new BMP_Explode(patterns[3], 2f);
				patterns[3].loopCircles =  288 * difficulty;
				patterns[3].SetSprite ("Circle", "Glow", "Orange", "Small");
				patterns[3].BMP.accelMax = 30;
				
				patterns.Add(new P_Maelstrom(3 * difficulty, 0.2f));
				patterns[4].BMP = new BMP_Explode(patterns[4], 10);
				patterns[4].rotationDirection = 1;
				patterns[4].startingRotation = 40;
				patterns[4].maelStromRotationMultiplier = 0.01f;
				patterns[4].SetSprite ("Diamond", "Glow", "Green", "Small");
				patterns[4].coolDown = 0.2f;

				patterns.Add(new P_Maelstrom(3 * difficulty, 0.2f));
				patterns[5].BMP = new BMP_Explode(patterns[5], 10);
				patterns[5].rotationDirection = -1;
				patterns[5].maelStromRotationMultiplier = 0.01f;
				patterns[5].startingRotation = 40;
				patterns[5].SetSprite ("Diamond", "Glow", "Green", "Small");
				patterns[5].coolDown = 0.2f;

				patterns.Add(new P_SingleHoming());
				patterns[6].BMP = new BMP_TurnToSpears(patterns[6], 5f);
				patterns[6].bulletCount = 1;
				patterns[6].SetSprite ("Circle", "Bevel", "Lilac", "Medium");
				patterns[6].BMP.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[7].BMP = new BMP_Explode (patterns[7], 6f);
				patterns[7].rotationDirection =  1;
				patterns[7].bulletCount =  2 * difficulty;
				patterns[7].SetSprite ("Circle", "Glow", "Orange", "Medium");
				patterns[7].BMP.accelMax = 30;

				patterns.Add(new P_Maelstrom());
				patterns[8].SetSprite ("Circle", "Glow", "Orange", "Medium");
				patterns[8].BMP = new BMP_Explode (patterns[8], 6f);
				patterns[8].bulletCount =  2 * difficulty;
				patterns[8].rotationDirection = -1;

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].speed = 7f;
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("X3", 4)});

				while (!endOfPhase) {
					
					movement.pattern.UpdateDirection("X3");
					
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(1f);
					shooter.BossShoot (patterns[4]);
					shooter.BossShoot (patterns[5]);
					shooter.BossShoot (patterns[7]);
					shooter.BossShoot (patterns[8]);
					yield return new WaitForSeconds(1f);
					
					shooter.BossShoot (patterns[3]);
					shooter.BossShoot (patterns[6]);
					patterns[4].StopPattern();
					patterns[5].StopPattern();
					patterns[7].StopPattern();
					patterns[8].StopPattern();
				
					yield return new WaitForSeconds(3f);
					shooter.BossShoot (patterns[6]);
					patterns[1].StopPattern();
					patterns[2].StopPattern();

					movement.pattern.UpdateDirection("C3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[6]);

					yield return new WaitForSeconds(.5f);

					shooter.BossShoot (patterns[6]);

					movement.pattern.UpdateDirection("I3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);

					shooter.BossShoot (patterns[6]);

					yield return new WaitForSeconds(.5f);

					shooter.BossShoot (patterns[6]);

				}
				
				
				break;
			case 3:
				Game.control.ui.BOSS.ShowActivatedPhase ("Hoodwink: Fox Fires");
				//StartPhaseTimer(30);
				
				p = new P_FoxFires(difficulty, 40 * difficulty);
				p.SetSprite ("Fireball", "Glow", "Orange", "Small");
                patterns.Add(p);

				p = new P_Maelstrom();
				p.BMP = new BMP_Explode(p, 6f);
				p.rotationDirection = 1;
                p.infinite = true;
				p.SetSprite ("BigCircle", "Big", "Red", "Huge");
				p.bulletCount =  Mathf.CeilToInt(1.2f * difficulty);
				p.coolDown = 2.5f / difficulty;
                patterns.Add(p);

				movementPatterns.Add(new EnemyMovementPattern());
				movementPatterns[0].SetWayPoints(new List<WayPoint>(){new WayPoint("C3", 1), new WayPoint("I3", 5, 1), new WayPoint("XY", 5)});
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
                    patterns[0].StopPattern();
					movementPatterns[0].force = true;
					movementPatterns[0].speed = 7f;
					movement.moving = true;
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    yield return new WaitForSeconds(.5f);
                    patterns[1].BMP = new BMP_Explode(p, 6f);
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
