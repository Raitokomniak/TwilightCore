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

    public override IEnumerator ExecuteRoutine(){
        if(delayBeforeAttack > 0) yield return new WaitForSeconds(delayBeforeAttack);
        bullets = new ArrayList ();
				yield return new WaitForSeconds (2f);
				pos = enemyShoot.GetLocalPosition ();
				
				for(tempLayer = 0; tempLayer < 4; tempLayer++){
					if (tempMagnitude > 0) {
						float b = bulletCount / 2 + tempMagnitude;
						for (int i = 0; i < Mathf.RoundToInt (b); i++) {
							newPosition = pos + new Vector3 (0f, 0f, 0f);
							bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / b));
							animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
							bulletMovement = new BulletMovementPattern (false, "StopAndRotate", 20f, this, tempLayer, tempMagnitude);
							InstantiateBullet (enemyBullet);
							bullet.GetComponent<SpriteRenderer> ().sprite = spriteLib.SetBulletSprite ("Circle", "Big", "Red");
							bullets.Add (enemyBullet);
						}
						tempMagnitude -= 3;
						yield return null;
					} else {
						tempMagnitude = originMagnitude;
					}
					if(!stop) yield return new WaitForSeconds (coolDown);
					else break;
				}

				animating = false;
        }
}
