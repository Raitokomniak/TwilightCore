using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SingleHoming : Pattern
{
        public P_SingleHoming(){
                tempMagnitude = originMagnitude;
        }

        public override IEnumerator ExecuteRoutine(){
                if(delayBeforeAttack > 0) yield return new WaitForSeconds(delayBeforeAttack);

                Game.control.sound.PlaySound ("Enemy", "Shoot", false);
		InstantiateBullet (enemyBullet);
        }
}
