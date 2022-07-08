using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Spiral : Pattern
{
	public P_Spiral(int _bulletCount, int _rotationDirection){
		bulletCount = _bulletCount;
		coolDown = 0.001f;
		coolDown = 1f;
		originMagnitude = 2;
		tempMagnitude = originMagnitude;
        rotationDirection = _rotationDirection;
        allBulletsSpawned = false;
	}


    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;
    
        if (loopCircles == 0) loopCircles = 360;
		
        Vector3 centerPos = pos;

		for (int i = 0; i < bulletCount; i++) {
			spawnPosition = SpawnInCircle (pos, 1f + (i * 0.1f), GetAng (i, loopCircles));
            //THIS YIELDS SOME WEIRD/COOL ROTATIONS
           // startingRotation = i * (360 / (bulletCount));
            //BMP.rotation.eulerAngles = new Vector3(0,0,i * (360 / (bulletCount)));
			SpawnBullet ();
            
			yield return new WaitForSeconds (coolDown * Time.deltaTime);
			if(stop) break;
			Game.control.sound.PlaySound ("Enemy", "Shoot", false);
		}

		allBulletsSpawned = true;
    }
}
