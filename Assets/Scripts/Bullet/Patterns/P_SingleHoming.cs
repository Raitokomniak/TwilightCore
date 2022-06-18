using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SingleHoming : Pattern
{
    public P_SingleHoming(){
        tempMagnitude = originMagnitude;
        BMP = new BMP_Explode(this, 8f);
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        BMP.startHoming = true;
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;
        Game.control.sound.PlaySound ("Enemy", "Shoot", false);
		SpawnBullet (BMP);
    }
}
