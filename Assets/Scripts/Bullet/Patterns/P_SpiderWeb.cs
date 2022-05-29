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
		bulletMovement = new BMP_DownAndExplode(this, 7f, false, 2.6f);
	}

   public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
	   allBulletsSpawned = false;
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        bullets = new ArrayList ();
		animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
		animation.GetComponent<BulletAnimationController>().stayTime = 2f;
		bulletRotation = rot;
		animating = false;
			
		for (int i = 0; i < bulletCount; i++) {
			bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / bulletCount));
			InstantiateBullet (enemyBullet, bulletMovement);
			//bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Circle", "Glow", "Red");
			bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Spider_Glow");
			bullets.Add (enemyBullet);
		}
		yield return new WaitUntil(() => allBulletsSpawned == true);
		Animate(1, 2f, bulletMovement.centerPoint);
		yield return new WaitForSeconds(0.5f);
		if(animation) animation.GetComponent<BulletAnimationController>().rotationSpeed = 10f;
	}
}
