using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SpiderWebLaser : Pattern
{
    public P_SpiderWebLaser(){
        originMagnitude = 15;
		tempMagnitude = originMagnitude;
        BMP = new BMP_LaserExpand(this);
        lib = Game.control.vectorLib;
        BMP.forceScale = true;
    }

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        allBulletsSpawned = false;
        string[] xcoors = new string[7]{"B", "C", "D", "E", "F", "G", "H"};
        yield return new WaitForSeconds(delayBeforeAttack);
        Debug.Log("bulletcount " + bulletCount);

        for (int i = -1; i < bulletCount * 2; i++) {
            float ang = 0;
            if(i < bulletCount){
                ang = 90 + (1 * Random.Range(-30, 30));
                spawnPosition = lib.GetVector("C" + (i+3).ToString());
            }
            if(i >= bulletCount){
                ang = 0 + (1 * Random.Range(-50, 50));
                int xcor = Random.Range(0, 7);
                spawnPosition = lib.GetVector(xcoors[xcor] + "10");
            }
            bulletRotation = Quaternion.Euler(0,0,ang);
            BMP = new BMP_LaserExpand(this);
           // SpawnBullet (enemyBullet, bulletMovement);
            SpawnBullet (BMP);
            bullet.GetComponent<BulletMovement> ().spriteR.sprite = sprite;
            yield return new WaitForSeconds(0.2f);
            if(stop) break;
        }

        allBulletsSpawned = true;
    }
}
