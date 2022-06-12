using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Rain : Pattern
{
    public P_Rain(int _bulletCount){
        bulletCount = _bulletCount;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        allBulletsSpawned = false;
        yield return new WaitForSeconds(delayBeforeAttack);
        lib = Game.control.vectorLib;
		//pos = enemy.transform.position;

        string[] XCOORS = new string[9] {"B", "C", "D", "E", "F", "G", "H", "I", "J"};
       
        int posX = 0;
        //int previousPosx = 0;

        while(!stop){
            //for(int i = 0; i < bulletCount; i++){
               /*while(posX == previousPosx) {
                    posX = Random.Range(0, 9);
                    yield return null;
                }*/

                posX = Random.Range(0, 9);
                
               // previousPosx = posX;
                spawnPosition = lib.GetVector(XCOORS[posX] + "1");
                bulletMovement = new BMP_RainDrop(this);
                bulletMovement.accelMax = 40f;
                bulletMovement.accelSpeed = 4;
                bulletMovement.trail = true;
                bulletMovement.moveWithForce = true;
                bulletMovement.dontDestroy = true;
                
                int randomSprite = Random.Range(0, 2);
                if(randomSprite == 0) SetSprite ("Diamond", "Glow", "Blue", "Tiny");
                if(randomSprite == 1) SetSprite ("Diamond", "Glow", "Turquoise", "Tiny");
                InstantiateBullet (enemyBullet, bulletMovement);
                yield return new WaitForSeconds(.3f);
            //}
        }
        allBulletsSpawned = true;
    }
}
