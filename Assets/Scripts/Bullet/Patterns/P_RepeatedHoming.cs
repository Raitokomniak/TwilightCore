using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_RepeatedHoming : Pattern
{	
		public P_RepeatedHoming(){
			tempMagnitude = originMagnitude;
		}

        public override IEnumerator ExecuteRoutine(){
        if(delayBeforeAttack > 0) yield return new WaitForSeconds(delayBeforeAttack);
        while(!stop && enemyShoot != null){
				newPosition = enemyShoot.transform.position;
				Game.control.sound.PlaySound ("Enemy", "Shoot", false);
				InstantiateBullet (enemyBullet);
				yield return new WaitForSeconds (coolDown);
			}
            
        }
}
