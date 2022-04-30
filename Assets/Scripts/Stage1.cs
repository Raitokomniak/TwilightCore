using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1 : MonoBehaviour
{

    void Awake(){
        InitWaves(Game.control.stage.difficultyMultiplier); //1 very easy //2 easy //3 normal //4 hard //5 very hard
    }

	public void InitWaves(float difficultyMultiplier) {
        EnemyLib lib = Game.control.enemyLib;
		lib.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

/*
			//1st PHASE
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", .5f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (1f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.rightTop });
																				// .6f
			
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (3f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });
																				// .6f
			
			
			//2ND PHASE
			if(difficultyMultiplier > 3){
				
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", .5f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (4f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.rightTop });

			
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (15f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });
			
			}
			
			mp = new EnemyMovementPattern (lib.zigZag);
			p = new Pattern (lib.circle);
			p.Customize("BulletCount", Mathf.Ceil(2.8f * (difficultyMultiplier / 2)));
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (16f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (17f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });

			
			//PHASE3

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (28f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });
												 //28f
			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (31f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });
												 //31f

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (33f, mp, p, 3, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (36f, mp, p, 3, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });


			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 4 * difficultyMultiplier);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (41f, mp, p, 3, false, 40, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 4 * difficultyMultiplier);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Yellow");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (41.5f, mp, p, 3, false, 40, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });



			//MID-BOSS

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("StayTime", 27f);
			mp.Customize ("LeaveDir", "Up");
			Wave bossMid1 = new Wave(55f, mp, null, 1,  false, 150, false, 3f, 1); //for spawning immediately
			bossMid1.SetUpBoss (0.5f, "Asura", true);
	


			lib.NewWave (lib.stageWaves, bossMid1, new ArrayList { lib.middleTop });


			//CONTD
			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (82f, mp, p, 5, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });


			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (87f, mp, p, 3, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topRight });



			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", Mathf.Ceil(4 * (difficultyMultiplier / 2)));
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (90f, mp, p, 3, false, 0, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.topLeft });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 4 * difficultyMultiplier);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (90f, mp, p, 3, false, 40, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 4 * difficultyMultiplier);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Yellow");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (90.5f, mp, p, 3, false, 40, false, 3f / difficultyMultiplier, 0), new ArrayList { lib.leftTop });
*/

			// BIG BOSS
				
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("StayTime", 0);
			Wave boss1 = new Wave(1f, mp, null, 1,  false, 15, true, 3f, 2);
								//96f
			boss1.SetUpBoss (1, "Forest Guardian", false);
			lib.NewWave (lib.stageWaves, boss1, new ArrayList { lib.middleTop });
	}

/*
    public void InitWaves() {
        EnemyLib lib = Game.control.enemyLib;
		lib.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", .5f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (1f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.rightTop });
			
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (3f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.leftTop });
			
			
			//2ND PHASE
			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", .5f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (4f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.singleHoming);
			p.Customize (new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite ("Circle", "Big", "Red");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (15f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.leftTop });

			mp = new EnemyMovementPattern (lib.zigZag);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (16f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (17f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.topLeft });


			//PHASE3

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (28f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.topLeft });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (31f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.topRight });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (33f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { lib.topRight });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (36f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { lib.topLeft });


			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (41f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Yellow");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (41.5f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { lib.leftTop });



			//MID-BOSS

			lib.NewWave (lib.stageWaves, lib.bossMid1, new ArrayList { lib.middleTop });


			//CONTD
			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (82f, mp, p, 5, false, 0, false, .6f, 0), new ArrayList { lib.topRight });


			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Right");
			//mp.targetPos = new Vector3 (-14, 7, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (87f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { lib.topRight });

			mp = new EnemyMovementPattern (lib.stopOnce);
			mp.Customize ("LeaveDir", "Left");
			mp.targetPos = new Vector3 (1, 6, 0);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 10);
			p.Customize (new BulletMovementPattern (false, "Explode", 7f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (90f, mp, p, 3, false, 0, false, .6f, 0), new ArrayList { lib.topLeft });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (0, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Right");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Green");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (90f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { lib.rightTop });

			mp = new EnemyMovementPattern (lib.enterLeave);
			mp.targetPos = new Vector3 (-14, 7, 0);
			mp.speed = 2f;
			mp.Customize ("LeaveDir", "Left");
			mp.Customize ("StayTime", 2f);
			p = new Pattern (lib.circle);
			p.Customize ("BulletCount", 20);
			p.Customize (new BulletMovementPattern (false, "Explode", 11f, p, 0, 14));
			p.SetSprite ("Circle", "Glow", "Yellow");	//enmyCnt simul hlth isBoss, cd, hlthBars, spawnPositions
			lib.NewWave (lib.stageWaves, new Wave (90.5f, mp, p, 3, false, 40, false, .6f, 0), new ArrayList { lib.leftTop });


			// BIG BOSS

			lib.NewWave (lib.stageWaves, lib.boss1, new ArrayList { lib.middleTop });
	}
	*/
}
