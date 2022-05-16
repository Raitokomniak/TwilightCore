using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_VoidPortal : Pattern
{

    public P_VoidPortal(){
        bulletCount = 10;
		coolDown = 3;
		tempMagnitude = originMagnitude;
    }


    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        bullets = new ArrayList ();

        animation = (Resources.Load ("Images/Animations/A_VoidPortal") as GameObject);
        animation.GetComponent<BulletAnimationController> ().stayTime = 5f;
        animating = false;

        bulletRotation = rot;
			
		for (int i = 0; i < bulletCount; i++) {
			bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / bulletCount));
			//bulletMovement = new BulletMovementPattern (bulletMovement);
            bulletMovement = bulletMovement.GetNewBulletMovement(bulletMovement);
			InstantiateBullet (enemyBullet, bulletMovement);
			bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Circle", "Glow", "Red");
		}
    }
}
