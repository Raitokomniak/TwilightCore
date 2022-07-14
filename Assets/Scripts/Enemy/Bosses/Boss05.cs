using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss05 : Phaser
{
	void Awake(){
		bossIndex = 0.5f;
		numberOfPhases = 3;
		ignoreDialog = true;
        topLayerWaitTime = 2;
        Game.control.stageHandler.midBossOn = true;
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
		Pattern p = null;

		 switch(phase)
			{
			case 0:
				//DOES THIS BELONG HERE???
				Game.control.stageUI.WORLD.UpdateTopPlayer (5f); ////////
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Stolen Daffodil");
                topLayerWaitTime = 0;

			    GetComponent<EnemyLife>().SetInvulnerable (true);

				patterns.Add(new P_Maelstrom());
				patterns[0].BMP = new BMP_Explode(patterns[0], 6f);
				patterns[0].bulletCount =  Mathf.CeilToInt(4 * (difficulty / 2f));
				patterns[0].rotationDirection =  1;
				patterns[0].SetSprite ("Circle", "Glow", "Green", "Medium");

				patterns.Add(new P_Maelstrom());
				patterns[1].SetSprite ("Circle", "Glow", "Yellow", "Medium");
				patterns[1].bulletCount =  Mathf.CeilToInt(4 * (difficulty / 2f));
				patterns[1].BMP = new BMP_Explode(patterns[0], 6f);
				patterns[1].rotationDirection =  -1;

				movementPatterns.Add(shooter.wave.movementPattern);
				movement.SetUpPatternAndMove (movementPatterns[0]);
				yield return new WaitForSeconds (2f);

				shooter.BossShoot (patterns[0]);
				shooter.BossShoot (patterns[1]);

				while(Game.control.stageHandler.stageTimer < shooter.wave.spawnTime + 14f)
				{
					yield return null;
				}
				patterns[0].Stop();
				patterns[1].Stop();
				NextPhase();
				break;
			case 1:
			 	Game.control.sound.PlaySpellSound ("Enemy", "Default");
				Game.control.stageUI.BOSS.ShowActivatedPhase ("Twilight Core: Depulsio");
				GetComponent<EnemyLife>().SetInvulnerable (true);
				StartPhaseTimer(11);
				//Game.control.ui.BOSS.StartBossTimer (enemy.wave.movementPattern.stayTime);

				p = new P_Spiral(Mathf.CeilToInt(5 * difficulty), 1);
				p.SetSprite ("Circle", "Glow", "BlackPurple", "Medium");
				p.BMP = new BMP_WaitAndExplode(p, 3f, 0);
				p.BMP.accelMax = 20f;
				p.loopCircles = 288 * difficulty;
                patterns.Add(p);

				p = new P_Spiral(Mathf.CeilToInt(5 * difficulty), -1);
				p.SetSprite ("Circle", "Glow", "BlackLilac", "Medium");
				p.BMP = new BMP_WaitAndExplode(p, 3f, 0);
				p.BMP.accelMax = 20f;
				p.loopCircles = 288 * difficulty;
                patterns.Add(p);

				p = new P_Maelstrom();
				p.BMP = new BMP_Explode(p, 6f);
				p.rotationDirection = 1;
                p.maelStromRotationMultiplier = 1f;
				p.SetSprite ("BigCircle", "Big", "BlackPurple", "Huge");
				p.bulletCount =  Mathf.CeilToInt(2f * difficulty);
				p.coolDown = 2.5f / difficulty;
				patterns.Add(p);


				shooter.BossShoot (patterns[2]);

				while(!endOfPhase)
				{
					yield return new WaitForSeconds (1f);
					shooter.BossShoot (patterns[0]);
					yield return new WaitForSeconds (1f);
					shooter.BossShoot (patterns[1]);
				}
               
				break;
            case 2:
                movement.SetUpPatternAndMove(movement.pattern);
				movement.pattern.UpdateDirection("XU");
                yield return new WaitUntil(() => movement.pattern.HasReachedDestination(movement) == true);
				GetComponent<BossLife>().FakeDeath();
                Game.control.stageHandler.midBossOn = false;
                break;
            }

			yield return null;
	}
    
}
