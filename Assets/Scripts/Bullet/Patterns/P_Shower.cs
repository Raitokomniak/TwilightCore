using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Shower : Pattern
{
    public P_Shower(){

    }

     public P_Shower(int _bulletCount){
        bulletCount = _bulletCount;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        allBulletsSpawned = false;
        yield return new WaitForSeconds(delayBeforeAttack);
        lib = Game.control.vectorLib;
		pos = enemy.transform.position;

        while(!stop){
        	int dir = 1;
			//for (int i = 0; i < bulletCount; i++) {	
				spawnPosition = enemy.transform.position + new Vector3 (-2 + (Random.Range (-.8f, .8f)), 0, 0);
				bulletRotation = bulletRotation * Quaternion.Euler (0, 0, 180f + (Random.Range (-10, 10)));
				bulletMovement = new BMP_RainDrop(this);
                int randomDir = Random.Range(-10, 10);
                bulletMovement.rotation = Quaternion.Euler(0,0, bulletMovement.rotation.z + (1f * randomDir));
                SetSprite ("Circle", "Glow", "Blue", "Small");
                bulletMovement.accelMax = 30f;
                bulletMovement.accelSpeed = 2;
                bulletMovement.moveWithForce = true;
               // bulletMovement.dontDestroy = false;
				InstantiateBullet (enemyBullet, bulletMovement);
                
                bullet.AddComponent<BulletBouncer>();
                bullet.GetComponent<BulletBouncer>().multiply = true;
                
				dir = -dir;

				yield return new WaitForSeconds (.06f);
                //yield return new WaitForSeconds (1f);
		   // }
            yield return null;
        }

        allBulletsSpawned = true;
    }


}
