using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : Phaser
{
	void Awake(){
		bossIndex = 3;
		numberOfPhases = 6;
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
		difficulty = Game.control.stageHandler.difficultyMultiplier;
        ResetLists();
		GetComponent<EnemyMovement>().EnableSprite(true);
		Game.control.sound.StopLoopingEffects();
        Pattern p;

        switch (phase) {
			case 0:
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Clouded Mind: Stream of Tears");
                StartPhaseTimer(30);

                p = new P_PacMan(2);
               // p.infinite = true;
                p.SetSprite ("Tear", "Glow", "Blue", "Small");
                patterns.Add(p);

                p = new P_SingleHoming();
                p.infinite = true;
                p.SetSprite ("BigCircle", "Big", "Blue", "Huge");
                patterns.Add(p);

                p = new P_Laser(3, 1);
                p.bulletCount = 4;
                p.SetSprite ("Laser", "Glow", "Turquoise", "Huge");
                patterns.Add(p);


                yield return new WaitForSeconds(1);
                
                movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                shooter.BossShoot (patterns[1]);
                shooter.BossShoot (patterns[0]);

                while(!endOfPhase){
                    
                    shooter.BossShoot (patterns[2]);
                    yield return new WaitForSeconds(5);
                }

                patterns[0].Stop();
                patterns[1].Stop();

              break;
              case 1:
               	Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Purification: Stages of Grief");
                StartPhaseTimer(30);

				movement.pattern.speed = 4f;

				
                p = new P_Shape(Mathf.CeilToInt(5 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 2, true, true, false);
                p.BMP.axisRotateSpeed = 5;
                p.SetSprite ("Fireball", "Glow", "Black", "Small");	 
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(4 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 4, true, true, false);
                p.BMP.axisRotateSpeed = 4;
                p.SetSprite ("Fireball", "Glow", "Black", "Small");	 
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(3 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 5, true, true, false);
                p.BMP.axisRotateSpeed = 3;
                p.SetSprite ("Fireball", "Glow", "Black", "Small");	 
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(3 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 6, true, true, false);
                p.BMP.axisRotateSpeed = 2;
                p.SetSprite ("Fireball", "Glow", "Black", "Small");	 
                patterns.Add(p);
                
                p = new P_Shape(Mathf.CeilToInt(3 * (difficulty)), "Circle", 2);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 7, true, true, false);
                p.BMP.axisRotateSpeed = 1;
                p.SetSprite ("Fireball", "Glow", "Black", "Small");	 
                patterns.Add(p);

				while(!endOfPhase){
					movement.pattern.UpdateDirection("X4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds(1);

                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(1);
                    patterns[0].ModifyAllBullets("speed", -4);
                    
                    movement.pattern.UpdateDirection("X4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(1);
                    patterns[1].ModifyAllBullets("speed", -4);
                    patterns[0].ModifyAllBullets(Game.control.spriteLib.SetBulletSprite("Fireball", "Glow", "White"));

                    movement.pattern.UpdateDirection("X3");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[3]);
					yield return new WaitForSeconds(1);
                    patterns[2].ModifyAllBullets("speed", -4);
                    patterns[1].ModifyAllBullets(Game.control.spriteLib.SetBulletSprite("Fireball", "Glow", "White"));

                    movement.pattern.UpdateDirection("X4");
                    yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                    shooter.BossShoot (patterns[4]);
					yield return new WaitForSeconds(1);
                    patterns[3].ModifyAllBullets("speed", -4);
                    patterns[2].ModifyAllBullets(Game.control.spriteLib.SetBulletSprite("Fireball", "Glow", "White"));

                    yield return new WaitForSeconds(1);
                    patterns[4].ModifyAllBullets("speed", -4);
                    patterns[3].ModifyAllBullets(Game.control.spriteLib.SetBulletSprite("Fireball", "Glow", "White"));

				}
				break;
              case 2:
              Game.control.stageUI.BOSS.ShowActivatedPhase ("Clouded Mind: Whirl of Emotions");
                StartPhaseTimer(30);

                p = new P_Maelstrom();
				p.bulletCount = 2 * difficulty;
				p.BMP = new BMP_Explode(p, 6f);
				p.rotationDirection =  1;
				p.infinite = true;
				p.SetSprite ("Circle", "Glow", "Blue", "Big");
                patterns.Add(p);

                p = new P_Maelstrom();
				p.bulletCount = 2 * difficulty;
				p.BMP = new BMP_Explode(p, 6f);
				p.rotationDirection =  -1;
				p.infinite = true;
				p.SetSprite ("Tear", "Glow", "Turquoise", "Small");
                patterns.Add(p);

                shooter.BossShoot (patterns[0]);

                while(!endOfPhase){
                    for(int i = 0; i < 30; i++){
                        yield return new WaitForSeconds(1);
                        patterns[0].maelStromRotationMultiplier += .5f;
                        patterns[0].coolDown -= .007f;
                        if(i == 5) shooter.BossShoot (patterns[1]);
                        if(i > 5){
                            patterns[1].maelStromRotationMultiplier += .3f;
                            patterns[1].coolDown -= .007f;
                        }
                    }
                    patterns[1].Stop();
                }

              break;
              case 3:
              
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Mother's Fear: Death");
                movement.pattern.speed = 1f;
                StartPhaseTimer(30);

                p = new P_Curtain();
                p.bulletCount = Mathf.CeilToInt(1.8f * difficulty);  
                p.BMP = new BMP_Explode(p, 10f);
                p.SetSprite ("FireBall", "Glow", "Black", "Medium");
                p.SetGlow("Red");
                patterns.Add(p);

                p = new P_Spiral(10 * (int)difficulty, 1);
                p.loopCircles =  288 * 4;
                p.executeDelay = 1f;
                p.coolDown = (3f);
                p.infinite = false;
                p.BMP = new BMP_WaitAndExplode(p, 5, 1f);
                p.SetSprite ("BigCircle", "Big", "Black", "Big");
                patterns.Add(p);
                

                while(!endOfPhase){
                    
                    movement.pattern.UpdateDirection("F5");
                    shooter.BossShoot (patterns[0]);
                    shooter.BossShoot (patterns[1]);
                    yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
                    movement.pattern.UpdateDirection("E4");
                    shooter.BossShoot (patterns[0]);
                    shooter.BossShoot (patterns[1]);
                    yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
                    movement.pattern.UpdateDirection("G3");
                    shooter.BossShoot (patterns[0]);
                    shooter.BossShoot (patterns[1]);
                    yield return new WaitUntil (() => movement.pattern.HasReachedDestination (movement) == true);
                    if(movement.pattern.speed < 4) movement.pattern.speed += 1;
                    else movement.pattern.speed = 1;
                }

              break;
			case 4:
				Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Clouded Mind: It Rains, It Pours");
              //  GameObject pitcher = Instantiate(Resources.Load("Prefabs/WaterPitcher") as GameObject, transform.position, Quaternion.identity);
               // pitcher.transform.SetParent(gameObject.transform);

                StartPhaseTimer(30);
				movement.pattern.speed = 2f;



                
				p = new P_Shower();
                p.bulletCount = 5;
                p.SetSprite ("Circle", "Glow", "Blue", "Small");
                patterns.Add(p);

                p = new P_Rain(100);
                p.BMP = new BMP_RainDrop(p, 1);
                p.SetSprite ("Tear", "Glow", "Turquoise", "Small");
				patterns.Add(p);

                p = new P_Rain(100);
                p.BMP = new BMP_RainDrop(p, 1);
                p.SetSprite ("Tear", "Glow", "Blue", "Small");
				patterns.Add(p);

                p = new P_Circle();
                p.coolDown = 2;
                p.infinite = false;
                p.bulletCount = Mathf.CeilToInt(6 * (difficulty / 2));
                p.BMP = new BMP_WaitAndExplode(p, 7f, 1f);
                p.SetSprite ("BigCircle", "Big", "Blue", "Medium");
                patterns.Add(p);



                EnemyMovementPattern mp = new EnemyMovementPattern(transform.position);
                mp.SetWayPoints(new List<WayPoint>(){new WayPoint("C5", 3), new WayPoint("I4", 4), new WayPoint("C5", 3), new WayPoint("I4", 4)});
                GameObject enemy = GameObject.Instantiate (Resources.Load ("Prefabs/Enemy"), mp.spawnPosition, Quaternion.Euler (0, 0, 0)) as GameObject;
                
                //add and init components
                enemy.AddComponent<EnemyLife> ();
                enemy.GetComponentInChildren<SpriteRenderer> ().sprite = Resources.Load<Sprite>("Sprites/waterPitcher");
                enemy.GetComponentInChildren<SpriteRenderer> ().sortingOrder = 5;
                enemy.GetComponent<EnemyShoot> ().SetUpAndShoot (patterns[0], 1);
                enemy.GetComponent<BoxCollider2D>().enabled = false;
                enemy.GetComponent<EnemyMovement> ().staticSpriteDir = true;
                

				Game.control.sound.PlaySFXLoop("Rain");

                yield return new WaitForSeconds(1);

				while(!endOfPhase){
                    enemy.GetComponent<EnemyMovement> ().SetUpPatternAndMove (mp);
                    enemy.GetComponent<EnemyMovement> ().pattern.speed = 2f;
					movement.pattern.UpdateDirection("F5");
					//shooter.BossShoot (patterns[0]);
					shooter.BossShoot (patterns[1]);
                    shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds(2);
                    shooter.BossShoot (patterns[3]);
                    yield return new WaitForSeconds(2);
					movement.SmoothAcceleration(3);
					for(int i = 0; i < 2; i++){
						movement.pattern.UpdateDirection("C4");
						yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
						movement.SmoothAcceleration(3);
                        shooter.BossShoot (patterns[3]);
						movement.pattern.UpdateDirection("I2");
						yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
						movement.SmoothAcceleration(3);
					}
					patterns[0].Stop();
                    shooter.BossShoot (patterns[3]);
					yield return new WaitForSeconds(1);
					patterns[1].Stop();
                    patterns[2].Stop();
                    shooter.BossShoot (patterns[3]);
				}
                Destroy(enemy);
				break;
			case 5:
                Game.control.enemySpawner.DestroyAllEnemies();
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Mother's Fear: Loss");

                p = new P_Curtain();
                p.bulletCount = Mathf.CeilToInt(1.8f * difficulty);  
                p.BMP = new BMP_Explode(p, 10f);
                p.SetSprite ("FireBall", "Glow", "White", "Medium");
                p.SetGlow("Red");
                patterns.Add(p);

                p = new P_Curtain();
                p.lineDirection = -p.lineDirection;
                p.bulletCount = Mathf.CeilToInt(1.8f * difficulty);  
                p.BMP = new BMP_Explode(p, 10f);
                p.SetSprite ("FireBall", "Glow", "Black", "Medium");
                p.SetGlow("Red");
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(10 * difficulty), "ThreeCircles", 1);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 7, true, true, false);
                p.BMP.axisRotateSpeed = 4;
                p.SetSprite ("BigCircle", "Big", "White", "Big");
                p.SetGlow("Black");
                patterns.Add(p);

                p = new P_Shape(Mathf.CeilToInt(4 * difficulty), "ThreeCircles", 1);
                p.coolDown = 2;
                p.infinite = false;
                p.BMP = new BMP_Explode(p, 7, true, true, false);
                p.BMP.axisRotateSpeed = 4;
                p.SetSprite ("BigCircle", "Big", "BlackBlack", "Big");
                p.SetGlow("Red");
                patterns.Add(p);

                p = new P_Maelstrom(10, -1);
                p.coolDown = 1;
                p.infinite = true;
                p.BMP = new BMP_Explode(p, 3);
                p.SetSprite ("Circle", "Glow", "BlackRed", "Big");
                p.SetGlow("Red");
                patterns.Add(p);
                

                movement.pattern.UpdateDirection("X4");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
                yield return new WaitForSeconds(1);

                shooter.BossShoot (patterns[4]);
                while(!endOfPhase){
                    shooter.BossShoot (patterns[0]);
                    yield return new WaitForSeconds(1);
                    shooter.BossShoot (patterns[1]);

                    yield return new WaitForSeconds(1);
                    shooter.BossShoot (patterns[2]);
                    yield return new WaitForSeconds(1);

                    patterns[2].ModifyAllBullets("speed", -4);
                    patterns[2].ModifyAllBullets("rotatespeed", -4);

                    shooter.BossShoot (patterns[0]);
                    yield return new WaitForSeconds(1);
                    shooter.BossShoot (patterns[1]);

                    yield return new WaitForSeconds(1);
                    shooter.BossShoot (patterns[3]);
                    yield return new WaitForSeconds(1);

                    patterns[3].ModifyAllBullets("speed", -4);
                    patterns[3].ModifyAllBullets("rotatespeed", -4);
                }
                patterns[4].Stop();

				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
