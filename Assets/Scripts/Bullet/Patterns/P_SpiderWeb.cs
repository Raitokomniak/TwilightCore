using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SpiderWeb : Pattern
{
	public P_SpiderWeb(){
		bossSpecial = true;
		bulletCount = 10;
		coolDown = 3;
		tempMagnitude = originMagnitude;
	}

   public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        bullets = new ArrayList ();
			animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
			bulletRotation = rot;
			animating = false;
			for (int i = 0; i < bulletCount; i++) {
				bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / bulletCount));
				bulletMovement = new BulletMovementPattern (bulletMovement);
				InstantiateBullet (enemyBullet);
				bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Circle", "Glow", "Red");
				bullets.Add (enemyBullet);
			}
        }
}
