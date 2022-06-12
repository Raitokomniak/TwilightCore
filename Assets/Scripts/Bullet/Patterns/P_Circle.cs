using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Circle : Pattern
{
    //SPAWNS BULLETS EVENLY IN A CIRCLE AROUND THE SHOOTER

    public P_Circle(int _bulletCount){
        bulletCount = _bulletCount;
        tempMagnitude = originMagnitude;
    }

    public P_Circle(){
        bulletCount = 7;
        tempMagnitude = originMagnitude;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;
        
        Game.control.sound.PlaySound ("Enemy", "Shoot", true);
			for (int i = 0; i < bulletCount; i++) {
				spawnPosition = SpawnInCircle (pos, 0f, GetAng (i, 360));
				InstantiateBullet (enemyBullet, bulletMovement);
		}
    }
}
