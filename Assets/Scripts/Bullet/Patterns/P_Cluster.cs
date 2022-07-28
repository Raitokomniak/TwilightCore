using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Cluster : Pattern
{
	//SPAWNS BULLETS IN A RANDOM CLUSTER AROUND A POINT
	public P_Cluster(int _bulletCount){
		bossSpecial = true;
		bulletCount = _bulletCount;
		originMagnitude = 1;
		tempMagnitude = originMagnitude;
        BMP = new BMP_Aurora(this, 10f, tempMagnitude);
        patternName = "Cluster";
	}

 	public override IEnumerator ExecuteRoutine (EnemyShoot enemy)
	{
		yield return new WaitForSeconds(executeDelay);
		pos = enemy.transform.position;
        int dir = 1;
		for (int i = 0; i < bulletCount; i++) {	
			spawnPosition = enemyShoot.transform.position + new Vector3 (Random.Range (-.8f, .8f), 1.5f * dir, 0);
			bulletRotation = bulletRotation * Quaternion.Euler (0, 0, 180f + (Random.Range (-10, 10)));
			SpawnBullet ();
			dir = -dir;
			yield return new WaitForSeconds (coolDown);
			CheckSoundPlay (i, 5);
			if(stop) break;
		}

		allBulletsSpawned = true;
    }
}


