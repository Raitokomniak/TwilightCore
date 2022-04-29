using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2 : MonoBehaviour
{
    void Awake(){
        InitWaves();
    }

    public void InitWaves() {
        EnemyLib lib = Game.control.enemyLib;
		lib.stageWaves.Clear ();
		Pattern p;
		EnemyMovementPattern mp;

		//															enmyCnt simul hlth 	isBoss, cd, hlthBars, spawnPositions
			p = new Pattern (lib.singleHoming);
			p.Customize(new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite("Circle", "Big", "Red");
			lib.NewWave (lib.stageWaves, new Wave (2f, lib.zigZag, p, 5, false, 0, false, 1f, 0), new ArrayList { lib.middleTop });
			/*w = (Wave)stage1Waves [stage1Waves.Count - 1];
			w.shootPattern.Customize(new BulletMovementPattern (true, null, 0.5f, w.shootPattern, 0, 14));
			w.shootPattern.SetSprite("Circle", "Big", "Red");



			p = new Pattern (circle);
			p.Customize(new BulletMovementPattern (true, "Explode", 2f, p, 0, 14));
			p.SetSprite("Circle", "Glow", "Green");
			NewWave (stageWaves, new Wave (3f, stopOnce, p, 6, false, 0, false, 1f, 0), new ArrayList { topRight });

			//NewWave (stage1Waves, new Wave (6f, zigZag, singleHoming, 6, false, 0, false, 1f,	0), new ArrayList { middleTop });
			//NewWave (stage1Waves, new Wave (10f, zigZag, circle, 6, false, 0, false, 1f,	0), new ArrayList { middleTop });
			p = new Pattern (circle);
			p.Customize(new BulletMovementPattern (true, "Explode", 6f, p, 0, 14));
			p.SetSprite("Circle", "Glow", "Yellow");
			NewWave (stageWaves, new Wave (10f, stopOnce, p, 6, false, 0, false, 1f, 0), new ArrayList { topRight });




			//p = new Pattern (laser);

			/*NewWave (stage1Waves, new Wave (25f, enterAndLeave, p, 2, true, 10, false,	1f, 0), new ArrayList {
				rightTop,
				leftTop
			});

			p = new Pattern (singleHoming);
			p.Customize(new BulletMovementPattern (true, null, 0.5f, p, 0, 14));
			p.SetSprite("Circle", "Big", "Red");
			NewWave (stageWaves, new Wave (15f, zigZag, p, 5, false, 0, false, 1f, 0), new ArrayList { middleTop });
			NewWave (stageWaves, new Wave (20f, stopOnce, p, 6, false, 0, false, 2f,	0), new ArrayList { topRight });

			NewWave (stageWaves, new Wave (boss2), new ArrayList { middleTop });

			break;
		*/
	}
}
