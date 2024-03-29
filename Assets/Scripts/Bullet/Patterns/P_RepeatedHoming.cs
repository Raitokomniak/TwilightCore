using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_RepeatedHoming : Pattern
{	
		public P_RepeatedHoming(){
			tempMagnitude = originMagnitude;
		}

        public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(executeDelay);
		pos = enemy.transform.position;
		
		while(!stop && enemyShoot != null){
			spawnPosition = enemyShoot.transform.position;
			Game.control.sound.PlaySound ("Enemy", "Shoot", false);
			//SpawnBullet (enemyBullet, bulletMovement);
			bullet = SpawnBullet ();
			yield return new WaitForSeconds (coolDown);
		}
    }
}
