using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Phaser
{
	void Awake(){
		bossIndex = 2;
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
		difficulty = Game.control.stageHandler.difficultyMultiplier;
        ResetLists();
		GetComponent<EnemyMovement>().EnableSprite(true);
        Pattern p;

        switch (phase) {
			case 0:
				Game.control.enemySpawner.DestroyAllEnvironmentalHazards();
                Game.control.stageUI.BOSS.ShowActivatedPhase ("Itsy Bitsy");
                StartPhaseTimer(30);
				
				patterns.Add(new P_Spiral(2 * difficulty, 1));
				patterns[0].SetSprite ("Diamond", "Glow", "Red", "Small");
                patterns[0].infinite = true;
				patterns[0].BMP = new BMP_SlowWaving(patterns[0], 4f, true);

				patterns.Add(new P_SpiderWeb());
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].SetSprite ("Spider_Glow", "Medium");

				patterns.Add(new P_SingleHoming());
				patterns[2].coolDown = 4f / difficulty;
				patterns[2].infinite = true;
				patterns[2].bulletCount = Mathf.CeilToInt(4 * (difficulty / 2));
				patterns[2].BMP = new BMP_WaitToHome(patterns[2], 20f, true);
				patterns[2].SetSprite ("Arrow", "Glow", "Red", "Small");	

				shooter.BossShoot (patterns[2]);
				while (!endOfPhase) {
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(3);
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					shooter.BossShoot(patterns[0]);
					yield return new WaitForSeconds(3);
					movement.pattern.UpdateDirection("H4");
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(3);
					movement.pattern.UpdateDirection("C4");
					shooter.BossShoot (patterns[1]);
					yield return new WaitForSeconds(3);
				}
				patterns[2].Stop();
				break;
			case 1:
                Game.control.sound.PlaySpellSound ("Enemy", "Default");
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Deva's Offering: Indra's Net");
				StartPhaseTimer(30);

				patterns.Add(new P_GiantWeb());
			//patterns[0].bulletCount = 6 * difficultyMultiplier;
				if(difficulty == 5) patterns[0].bulletCount = 30; 
				else patterns[0].bulletCount = 2 * difficulty;
				patterns[0].executeDelay = 2f;
				patterns[0].BMP = new BMP_Explode(patterns[0], 6f);
				patterns[0].SetSprite("BigCircle", "Big", "Red", "Big");

				patterns.Add(new P_Maelstrom());
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].BMP = new BMP_Explode(patterns[1], 6f);
				patterns[1].rotationDirection =  1;
				patterns[1].infinite = true;
				patterns[1].SetSprite ("Diamond", "Glow", "Red", "Small");

				movement.pattern.UpdateDirection("X3");
				yield return new WaitForSeconds(2);
				while (!endOfPhase) {
					shooter.BossShoot (patterns[1]);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds (9);
					patterns[1].Stop();
					yield return new WaitForSeconds (1);
				}
				patterns[0].Stop();
				patterns[1].Stop();
				break;
			case 2:
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Itsier, Bitsier");
				StartPhaseTimer(30);
				p = new P_SpiderWebLaser();
                p.SetSprite ("Laser", "Glow", "Red", "Small");
				p.bulletCount = 3;
				p.rotationDirection =  0;
                patterns.Add(p);

				patterns.Add(new P_Maelstrom());
				//patterns[1].SetSprite ("Spider_Glow");
				patterns[1].coolDown = .5f;
				patterns[1].rotationDirection =  2;
				patterns[1].SetSprite ("Diamond", "Glow", "Red", "Small");
				patterns[1].bulletCount = 2 * difficulty;
				patterns[1].BMP = new BMP_Explode(patterns[1], 6f);

				patterns.Add(new P_SingleHoming());
				patterns[2].coolDown = 4f / difficulty;
				patterns[2].infinite = true;
				patterns[2].bulletCount = Mathf.CeilToInt(4 * (difficulty / 2));
				patterns[2].BMP = new BMP_WaitToHome(patterns[2], 20f, true);
				patterns[2].SetSprite ("Arrow", "Glow", "Red", "Small");	

				while (!endOfPhase) {
					
					movement.pattern.UpdateDirection("X3");
					yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
					
					shooter.BossShoot (patterns[0]);
					

					yield return new WaitForSeconds (2f);
					movement.pattern.UpdateDirection("C4");
					shooter.BossShoot (patterns[2]);
					yield return new WaitForSeconds (2f);
					shooter.BossShoot(patterns[1]);
					shooter.BossShoot (patterns[0]);

					yield return new WaitForSeconds (2f);
					movement.pattern.UpdateDirection("H4");

					yield return new WaitForSeconds(2f);
					patterns[1].Stop();
					patterns[2].Stop();
				}
				break;
			case 3:
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Deva's Offering: Void Dance");
				StartPhaseTimer(30);

				P_VoidPortal portal = new P_VoidPortal(20f, 15, 40);
				portal.dontDestroyAnimation = true;
				patterns.Add(portal);
				patterns[0].infinite = false;
				patterns[0].BMP = new BMP_Explode(patterns[0], 0);
				patterns[0].executeDelay = 1f;

				patterns.Add(new P_Spiral(6 * difficulty, 1));
				patterns[1].loopCircles =  288 * 3;
				patterns[1].SetSprite ("Diamond", "Glow", "Red", "Small");
				patterns[1].BMP =new BMP_WaitAndExplode(patterns[1], 6f, 0f);

				patterns.Add(new P_Spiral(6 * difficulty, -1));
				patterns[2].loopCircles =  288 * 3;
				patterns[2].SetSprite ("Arrow", "Glow", "White", "Small");
				patterns[2].BMP = new BMP_WaitAndExplode(patterns[2], 6f, 0f);

				patterns.Add(new P_Maelstrom());
				patterns[3].bulletCount = Mathf.CeilToInt(.6f * difficulty);
				patterns[3].BMP = new BMP_SlowWaving(patterns[3], 3f, true);
				patterns[3].rotationDirection =  1;
				patterns[3].infinite = true;
				patterns[3].SetSprite ("Diamond", "Glow", "White", "Small");


				movement.pattern.UpdateDirection("X3");
				yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
				
				yield return new WaitForSeconds (2f);
				
				shooter.BossShoot(patterns[0]);
				shooter.BossShoot(patterns[3]);
				yield return new WaitForSeconds (2f);
				while (!endOfPhase) {
					shooter.BossShoot(patterns[1]);
					yield return new WaitForSeconds (1f);
					shooter.BossShoot(patterns[2]);
					yield return new WaitForSeconds (1f);
				}
				patterns[0].animation.GetComponent<SpriteAnimationController>().stop = true;
				patterns[3].Stop();
				break;
			}
        yield return new WaitForEndOfFrame();
    }
}
