using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Cluster : Pattern
{
	public P_Cluster(int difficultyMultiplier){
		bossSpecial = true;
		bulletCount = 30 * difficultyMultiplier;
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
				newPosition = enemyShoot.GetLocalPosition () + new Vector3 (Random.Range (-.8f, .8f), 1.5f * dir, 0);
				bulletRotation = bulletRotation * Quaternion.Euler (0, 0, 180f + (Random.Range (-10, 10)));
				bulletMovement = new BulletMovementPattern (false, "Aurora", 10f, this, tempLayer, tempMagnitude);
				InstantiateBullet (enemyBullet);
				dir = -dir;

				yield return new WaitForSeconds (coolDown);
				CheckSoundPlay (i, 5);
				//if(i % 2 == 0) SetSprite ("Arrow", "Glow", "Red");
				//if(i % 3 == 0) SetSprite ("Arrow", "Glow", "Honey");
				//if(i % 4 == 0) SetSprite ("Arrow", "Glow", "Purple");
				//if(i % 5 == 0) SetSprite ("Arrow", "Glow", "Green");


			}
			allBulletsSpawned = true;
    }
}
