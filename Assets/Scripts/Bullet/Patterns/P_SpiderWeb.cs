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
		Pattern p = this;

		BMP = new BMP_DownAndExplode(p, 7f, 2.6f);
	}

   public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        //yield return new WaitForSeconds(delayBeforeAttack);
		spawnedBullets = new ArrayList ();
		allBulletsSpawned = false;
		pos = enemy.transform.position;
		animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
		animation.GetComponent<SpriteAnimationController>().stayTime = 2f;
		bulletRotation = rot;
		animating = false;

		for (int i = 0; i < bulletCount; i++) {
			bullet = SpawnBullet ();
            bullet.GetComponent<BulletMovement>().spriteR.sprite = Game.control.spriteLib.SetBulletSprite ("Spider_Glow");
            bullet.GetComponent<BulletMovement>().glowRend.sprite = null;
		}
		yield return new WaitUntil(() => allBulletsSpawned == true);
		Animate(1, 2f, BMP.centerPoint);
		yield return new WaitForSeconds(0.5f);
		if(animation) animation.GetComponent<SpriteAnimationController>().rotationSpeed = 10f;
	}
}
