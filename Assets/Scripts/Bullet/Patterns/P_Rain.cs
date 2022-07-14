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
        yield return new WaitForSeconds(executeDelay);
        lib = Game.control.vectorLib;
		//pos = enemy.transform.position;

        string[] XCOORS = new string[9] {"B", "C", "D", "E", "F", "G", "H", "I", "J"};
       
        int posX = 0;
        //int previousPosx = 0;

        while(!stop){
                posX = Random.Range(0, 9);
                spawnPosition = lib.GetVector(XCOORS[posX] + "1");
                BMP.movementSpeed = 1f;
                BMP.accelMax = 1f;
                BMP.accelSpeed = 1;
                BMP.trail = true;
                BMP.moveWithForce = true;
                BMP.dontDestroy = true;
                
                //int randomSprite = Random.Range(0, 2);
                //if(randomSprite == 0) SetSprite ("Diamond", "Glow", "Blue", "Small");
                //if(randomSprite == 1) SetSprite ("Diamond", "Glow", "Turquoise", "Medium");
                //SpawnBullet (enemyBullet, bulletMovement);
				SpawnBullet ();
                yield return new WaitForSeconds(.2f);
        }
        allBulletsSpawned = true;
    }
}
