using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SingleHoming : Pattern
{
    public P_SingleHoming(){
        tempMagnitude = originMagnitude;
        bulletCount = 1;
        BMP = new BMP_Explode(this, 8f);
        infinite = false;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        BMP.startHoming = true;
        yield return new WaitForSeconds(delayBeforeAttack);
        pos = enemy.transform.position;

		while(!stop){
            spawnPosition = enemy.transform.position;
            startingRotation = enemy.transform.rotation.z;
            Game.control.sound.PlaySound ("Enemy", "Shoot", false);
            SpawnBullet ();
            yield return new WaitForSeconds (coolDown);
            if(!infinite) break;
        }
    }
}
