using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_GiantLotus : Pattern
{
	//CUSTOM PATTERN FOR BOSS2 WHERE BULLETS SPAWN IN 4 ROWS OF CIRCLES

	public P_GiantLotus(){
		bossSpecial = true;
		
		originMagnitude = 6;
		tempMagnitude = originMagnitude;
	}

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        allBulletsSpawned = false;
		yield return new WaitForSeconds(executeDelay);
		if(stop) yield return null;
		pos = enemy.transform.position;
        spawnedBullets = new List<GameObject>();

		tempMagnitude = originMagnitude;
		
		for(tempLayer = 0; tempLayer < 4; tempLayer++){
			if (tempMagnitude > 0 && !stop) {
				float b = bulletCount / 2 + tempMagnitude;
				for (int i = 0; i < Mathf.RoundToInt (b); i++) {
					spawnPosition = pos + new Vector3 (0f, 0f, 0f);
					bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / b));
					BMP = new BMP_StopAndRotate(this, tempLayer, tempMagnitude);
                    BMP.rotateTime = 5f;
                    BMP.axisRotateSpeed = 2f;
				    SpawnBullet ();
				}		
				tempMagnitude -= 1;
			} 
		}
		yield return new WaitUntil(() => allBulletsSpawned == true);


		/*

		for(tempLayer = 0; tempLayer < 4; tempLayer++){
			if (tempMagnitude > 0 && !stop) {
				float b = bulletCount / 2 + tempMagnitude;
				for (int i = 0; i < Mathf.RoundToInt (b); i++) {
					newPosition = pos + new Vector3 (0f, 0f, 0f);
					bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / b));
					animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
					
					//bulletMovement = new BulletMovementPattern (false, "StopAndRotate", 20f, this, tempLayer, tempMagnitude);
					bulletMovement = new BMP_StopAndRotate(this, tempLayer, tempMagnitude);
					InstantiateBullet (enemyBullet, bulletMovement);
					bullet.GetComponent<BulletMovement>().spriteR.sprite = Game.control.spriteLib.SetBulletSprite ("BigCircle", "Big", "Red");
					bullets.Add (enemyBullet);
				}
					
				tempMagnitude -= 3;
				yield return null;
				} 
				
			else {
				tempMagnitude = originMagnitude;
				if(stop) break;
			}
			
			if(stop) break;
			yield return new WaitForSeconds (coolDown);
			}
			
			animating = false;*/
    }
}
