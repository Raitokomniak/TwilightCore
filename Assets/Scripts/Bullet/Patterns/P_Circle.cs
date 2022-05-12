using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Circle : Pattern
{
    public P_Circle(){
        bulletCount = 7;
        tempMagnitude = originMagnitude;
    }

    public override IEnumerator ExecuteRoutine(){
       
        if(delayBeforeAttack > 0) yield return new WaitForSeconds(delayBeforeAttack);
        
        Game.control.sound.PlaySound ("Enemy", "Shoot", true);
			for (int i = 0; i < bulletCount; i++) {
				newPosition = SpawnInCircle (pos, 0f, GetAng (i, 360));
				InstantiateBullet (enemyBullet);
		}
    }
}
