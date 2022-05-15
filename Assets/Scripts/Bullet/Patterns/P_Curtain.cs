using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Curtain : Pattern
{
	public P_Curtain(){
		bulletCount = 8;
		coolDown = .1f;
		originMagnitude = 10;
		tempMagnitude = originMagnitude;
	}

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;
		
        for (int i = 0; i < bulletCount; i++) {
				if (lineDirection == 1)
					newPosition = SpawnInLine (-15, 20, lineDirection, i);
				else
					newPosition = SpawnInLine (2, 20, lineDirection, i);
				bulletMovement = new BulletMovementPattern (bulletMovement);

				InstantiateBullet (enemyBullet);
				bullet.GetComponent<SpriteRenderer> ().sprite = sprite;

				yield return new WaitForSeconds (coolDown);
				Game.control.sound.PlaySound ("Enemy", "Shoot", false);
			}
			lineDirection = -lineDirection;
    }
}