using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_PacMan : Pattern
{

	public P_PacMan(){
		bulletCount = 45;
		rotationMultiplier = 6f;
		tempMagnitude = originMagnitude;
	}

        public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        for (int i = 0; i < bulletCount; i++) {
				//sprite = Resources.Load<Sprite> ("Sprites/Circle_Glow_Red");
				spawnPosition = pos + new Vector3 (0f, 0f, 0f);
				bulletRotation = Quaternion.Euler (0f, 0f, startingRotation + (float)i * rotationMultiplier);
				//bulletMovement = new BulletMovementPattern (false, null, 0.5f, this, 0, tempMagnitude);
				startingRotation += 0.1f;
				//SpawnBullet (enemyBullet, bulletMovement);
				SpawnBullet (BMP);
				bullet.GetComponent<BulletMovement>().spriteR.sprite = sprite;
			}
            
        }
}
