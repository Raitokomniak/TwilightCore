using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Spiral : Pattern
{
	public P_Spiral(){
		bulletCount = 30;
		coolDown = 0.001f;
		originMagnitude = 2;
		tempMagnitude = originMagnitude;
	}


    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;
		
        if (loopCircles == 0)
				loopCircles = 360;
			Vector3 centerPos = pos;
			allBulletsSpawned = false;
			for (int i = 0; i < bulletCount; i++) {
				newPosition = SpawnInCircle (pos, 1f + (i * 0.1f), GetAng (i, loopCircles));
				InstantiateBullet (enemyBullet, bulletMovement);
				yield return new WaitForSeconds (coolDown);
				Game.control.sound.PlaySound ("Enemy", "Shoot", false);
			}

			allBulletsSpawned = true;
    }
}
