using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_GiantWeb : Pattern
{
	public P_GiantWeb(){
		bossSpecial = true;
		bulletCount = 30;
		layers = 2;
		originMagnitude = 12;
		tempMagnitude = originMagnitude;
	}

    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        allBulletsSpawned = false;
		yield return new WaitForSeconds(delayBeforeAttack);
		if(stop) yield return null;
		pos = enemy.transform.position;
        bullets = new ArrayList ();
		animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
		animation.GetComponent<BulletAnimationController>().stayTime = 3f;
		Animate(4.5f, 3, pos);

		tempMagnitude = originMagnitude;
		
		for(tempLayer = 0; tempLayer < 4; tempLayer++){
			if (tempMagnitude > 0 && !stop) {
				float b = bulletCount / 2 + tempMagnitude;
				for (int i = 0; i < Mathf.RoundToInt (b); i++) {
					newPosition = pos + new Vector3 (0f, 0f, 0f);
					bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / b));
					bulletMovement = new BMP_StopAndRotate(this, tempLayer, tempMagnitude);
					InstantiateBullet (enemyBullet, bulletMovement);
					bulletMovement = new BMP_StopAndRotate(this, tempLayer, tempMagnitude);
					bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Circle", "Big", "Red");
					bullets.Add (enemyBullet);
				}		
				tempMagnitude -= 3;
			} 
		}
		yield return new WaitUntil(() => allBulletsSpawned == true);
		animation.GetComponent<BulletAnimationController>().rotationSpeed = 6f;
		animating = false;


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
					bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Circle", "Big", "Red");
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
