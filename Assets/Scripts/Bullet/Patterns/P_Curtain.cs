using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Curtain : Pattern
{
	//SPAWNS BULLETS IN A STRAIGHT LINE STARTING FROM SHOOTER POSITION

	public P_Curtain(){
		bulletCount = 8;
		coolDown = .1f;
		originMagnitude = 10;
		tempMagnitude = originMagnitude;
	}

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(executeDelay);
		pos = enemy.transform.position;
        for (int i = 0; i < bulletCount; i++) {
			if(stop) break;
            spawnPosition = SpawnInLine (pos.x, 20, lineDirection, i);
			SpawnBullet ();
			bullet.GetComponent<BulletMovement>().spriteR.sprite = sprite;
			yield return new WaitForSeconds (coolDown);
			Game.control.sound.PlaySound ("Enemy", "Shoot", false);
		}
		lineDirection = -lineDirection;
    }
}
