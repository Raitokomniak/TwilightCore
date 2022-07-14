using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_PacMan : Pattern
{
    public float pacManTightness = 1;

    public P_PacMan(float _tightness){
        bulletCount = 45;
		rotationMultiplier = 6f;
        pacManTightness = 1 + (_tightness * 0.1f);
		tempMagnitude = originMagnitude;
	}

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(executeDelay);
		pos = enemy.transform.position;

        while(!stop){
            float rotDif = 0.1f;
            
            for(int l = 0; l < 2; l++){

                for(int j = 0; j < 15; j++){
                    for (int i = 0; i < bulletCount; i++) {
                        spawnPosition = SpawnInCircle (pos, 1.5f, GetAng (i, 360, bulletCount) + startingRotation);
                        bulletRotation = Quaternion.Euler (0f, 0f, startingRotation + (float)(i * pacManTightness) * rotationMultiplier);
                        startingRotation += rotDif;

                        SpawnBullet ();
                        if(stop) break;
                    }
                    if(stop) break;
                    yield return new WaitForSeconds(.2f);
                }
                if(stop) break;
                rotDif = -rotDif;
            }
            yield return null;
            if(!infinite) break;
        }
    }
}
