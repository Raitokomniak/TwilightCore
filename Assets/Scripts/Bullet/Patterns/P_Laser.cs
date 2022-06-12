using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Laser : Pattern
{  
    int laserIndex = 0;

	public P_Laser(){
		coolDown = 10;
		originMagnitude = 15;
		tempMagnitude = originMagnitude;
	}
	
    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;
		
			if (bulletCount > 1) {
				for (int i = 0; i < bulletCount; i++) {
					float ang = (i * (360 / bulletCount) + (360 / bulletCount)) * 0.7f;
					if (i >= 3)
						ang += 30;

					spawnPosition = SpawnInCircle (pos + new Vector3 (0, 1f, 0), 1.5f, ang);

				//	bulletMovement = new BulletMovementPattern (false, "ExpandToLaser", 0f, this, 0, tempMagnitude);
					bulletMovement = new BMP_LaserExpand(this);
					bulletMovement.laserIndex = laserIndex;
					laserIndex++;

					InstantiateBullet (enemyBullet, bulletMovement);

					bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
					if (laserIndex == 0)
						laserIndex++;
					else
						laserIndex = 0;
				}

			} 
			else {
				sprite = Resources.Load<Sprite> ("Sprites/enemyLaser");
				spawnPosition = pos - new Vector3 (0f, .2f, 0f);
				
				//bulletMovement = new BulletMovementPattern (false, "PendulumLaser", 0f, this, 0, tempMagnitude);
				bulletMovement = new BMP_LaserPendulum(this);
				bulletMovement.laserIndex = laserIndex;
			
				InstantiateBullet (enemyBullet, bulletMovement);
			}
    }
}
