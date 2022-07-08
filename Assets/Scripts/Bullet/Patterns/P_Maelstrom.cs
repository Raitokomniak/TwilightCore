using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Maelstrom : Pattern
{
	//SPAWNS BULLETS EVENLY IN A CIRCLE AROUND THE SHOOTER, BUT EVERY FOLLOWING ROW SLIGHTLY ROTATES TO A DIRECTION

	public P_Maelstrom(){
		bulletCount = 10;
		coolDown = 0.3f;
        rotationDirection = 1;
        patternName = "MaelStrom";
	}

    
	public P_Maelstrom(int _bulletCount, int _rotationDirection){
		bulletCount = _bulletCount;
        rotationDirection = _rotationDirection;
		coolDown = 0.3f;
        patternName = "MaelStrom";
	}

	public P_Maelstrom(int _bulletCount, float _coolDown, int _rotationDirection){
		bulletCount = _bulletCount;
        rotationDirection = _rotationDirection;
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
				SpawnBullet ();
                //BMP.bulletMovement.bulletRot = SpawnInCircle (i, startRot);
				if(circleDelay > 0) yield return new WaitForSeconds(circleDelay);
			}
			if(!infinite) yield break;
			yield return new WaitForSeconds (coolDown);
		}
    }
}
