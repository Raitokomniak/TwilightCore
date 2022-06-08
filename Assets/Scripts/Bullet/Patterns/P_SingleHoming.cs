using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SingleHoming : Pattern
{
        public P_SingleHoming(){
                tempMagnitude = originMagnitude;
        }

        public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
                yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

                Game.control.sound.PlaySound ("Enemy", "Shoot", false);
                bulletMovement.startHoming = true;
		InstantiateBullet (enemyBullet, bulletMovement);
        }
}
