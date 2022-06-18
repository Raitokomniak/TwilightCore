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
            for(int i = 0; i < 15; i++){
                int dir = 1;
                spawnPosition = enemy.transform.position + new Vector3 (-2 + (Random.Range (-.8f, .8f)), 0, 0);
                bulletRotation = bulletRotation * Quaternion.Euler (0, 0, 180f + (Random.Range (-10, 10)));
                BMP = new BMP_RainDrop(this);
                int randomDir = Random.Range(-10, 10);
                BMP.rotation = Quaternion.Euler(0,0, BMP.rotation.z + (1f * randomDir));
                SetSprite ("Circle", "Glow", "Blue", "Small");
                BMP.accelMax = 30f;
                BMP.accelSpeed = 2;
                BMP.moveWithForce = true;
                //SpawnBullet (enemyBullet, bulletMovement);
				SpawnBullet (BMP);
                
                bullet.AddComponent<BulletBouncer>();
                bullet.GetComponent<BulletBouncer>().multiply = true;
                
                dir = -dir;
                yield return new WaitForSeconds (.06f);
            }
            yield return new WaitForSeconds (.2f);
            yield return null;
        }

        allBulletsSpawned = true;
    }


}
