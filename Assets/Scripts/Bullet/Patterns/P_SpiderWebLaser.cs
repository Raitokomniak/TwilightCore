using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SpiderWebLaser : Pattern
{
    int laserIndex = 0;

    public P_SpiderWebLaser(){
        originMagnitude = 15;
		tempMagnitude = originMagnitude;
        bulletMovement = new BMP_LaserExpand(this);
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
            
            for (int i = -1; i < bulletCount / 2; i++) {
                float ang = 90 + (10 * Random.Range(-3, 3));
                newPosition = new Vector3(-18.5f, 4 * i, 0);
                bulletRotation = Quaternion.Euler(0,0,ang);
                bulletMovement.laserIndex = laserIndex;
                laserIndex++;
                InstantiateBullet (enemyBullet, bulletMovement);
                bullet.GetComponent<SpriteRenderer> ().sprite = sprite;

                yield return new WaitForSeconds(0.2f);
                if(stop) break;
            }
                    
            for (int i = -1; i < bulletCount / 2; i++) {
                float ang = 270 + (10 * Random.Range(-3, 3));
                newPosition = new Vector3(6.5f, 4 * i, 0);
                bulletRotation = Quaternion.Euler(0,0,ang);
                bulletMovement.laserIndex = laserIndex;
                laserIndex++;

                InstantiateBullet (enemyBullet, bulletMovement);

                bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
                yield return new WaitForSeconds(0.2f);
                if(stop) break;
            }

            for (int i = -1; i < bulletCount; i++) {
                float ang = 0 + (10 * Random.Range(-5, 5));
                newPosition = new Vector3(-16 + (i * 5), 13, 0);
                bulletRotation = Quaternion.Euler(0,0,ang);
                bulletMovement.laserIndex = laserIndex;
                laserIndex++;

                InstantiateBullet (enemyBullet, bulletMovement);

                bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
                yield return new WaitForSeconds(0.2f);
                if(stop) break;
            }
        }
}
