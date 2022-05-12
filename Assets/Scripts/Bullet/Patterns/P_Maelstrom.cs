using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Maelstrom : Pattern
{
	public P_Maelstrom(){
		bulletCount = 10;
		coolDown = 0.2f;
	}

    public override IEnumerator ExecuteRoutine(){
        if(delayBeforeAttack > 0) yield return new WaitForSeconds(delayBeforeAttack);

		while (!stop) {
			Game.control.sound.PlaySound ("Enemy", "Shoot", true);
			
            for (int i = 0; i < bulletCount; i++) {
				newPosition = SpawnInCircle (pos, 1.5f, GetAng (i, 360) + startingRotation);
				bulletRotation = SpawnInCircle (i, startingRotation);
				startingRotation += 0.5f * rotationDirection;
				InstantiateBullet (enemyBullet);
				if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			}
			yield return new WaitForSeconds (coolDown);
		}
    }
}
