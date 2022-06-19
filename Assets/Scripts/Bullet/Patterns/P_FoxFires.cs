using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_FoxFires : Pattern
{
	//SPAWNS BULLETS IN A RANDOM CLUSTER AROUND A POINT

	public P_FoxFires(int difficultyMultiplier, int _bulletCount){
		bossSpecial = true;
		bulletCount = _bulletCount;
		coolDown = .05f / difficultyMultiplier;
		originMagnitude = 1;
		tempMagnitude = originMagnitude;
	}

 	public override IEnumerator ExecuteRoutine (EnemyShoot enemy)
	{
		yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        int dir = 1;
			for (int i = 0; i < bulletCount; i++) {	
				spawnPosition = enemyShoot.transform.position + new Vector3 (Random.Range (-.8f, .8f), 1.5f * dir, 0);
				bulletRotation = bulletRotation * Quaternion.Euler (0, 0, 180f + (Random.Range (-10, 10)));
				BMP = new BMP_Aurora(this, 10f, tempMagnitude);
				SpawnBullet (BMP);
				dir = -dir;

				yield return new WaitForSeconds (coolDown);
				CheckSoundPlay (i, 5);
				if(stop) break;
				//if(i % 2 == 0) SetSprite ("Arrow", "Glow", "Red");
				//if(i % 3 == 0) SetSprite ("Arrow", "Glow", "Honey");
				//if(i % 4 == 0) SetSprite ("Arrow", "Glow", "Purple");
				//if(i % 5 == 0) bullet.SetSprite ("Arrow", "Glow", "Green");

			}
			allBulletsSpawned = true;
    }
}


