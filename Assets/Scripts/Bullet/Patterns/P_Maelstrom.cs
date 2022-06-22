using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Maelstrom : Pattern
{
	//SPAWNS BULLETS EVENLY IN A CIRCLE AROUND THE SHOOTER, BUT EVERY FOLLOWING ROW SLIGHTLY ROTATES TO A DIRECTION

	public P_Maelstrom(){
		bulletCount = 10;
		coolDown = 0.2f;
        patternName = "MaelStrom";
	}

	public P_Maelstrom(int _bulletCount, float _coolDown){
		bulletCount = _bulletCount;
		coolDown = _coolDown;
        patternName = "MaelStrom";
	}

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		float startRot = startingRotation;

		while (!stop) {
			pos = enemy.transform.position;
			Game.control.sound.PlaySound ("Enemy", "Shoot", true);
			
			for (int i = 0; i < bulletCount; i++) {
				spawnPosition = SpawnInCircle (pos, 1.5f, GetAng (i, 360) + startRot);
				bulletRotation = SpawnInCircle (i, startRot);
				startRot += maelStromRotationMultiplier * rotationDirection;
				SpawnBullet (BMP);
				if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			}
			if(!infinite) yield break;
			yield return new WaitForSeconds (coolDown);
		}
    }
}
