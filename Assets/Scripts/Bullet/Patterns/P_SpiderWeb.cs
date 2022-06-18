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

		BMP = new BMP_DownAndExplode(p, 7f, false, 2.6f);
	}

   public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
		spawnedBullets = new ArrayList ();
		allBulletsSpawned = false;
		pos = enemy.transform.position;

        yield return new WaitForSeconds(delayBeforeAttack);
        
		animation = (Resources.Load ("Images/Animations/SmallWeb") as GameObject);
		animation.GetComponent<SpriteAnimationController>().stayTime = 2f;
		bulletRotation = rot;
		animating = false;
		for (int i = 0; i < bulletCount; i++) {
			//SpawnBullet (enemyBullet, bulletMovement);
			SpawnBullet (BMP);
			bullet.GetComponent<BulletMovement>().spriteR.sprite = Game.control.spriteLib.SetBulletSprite ("Spider_Glow");
		}
		yield return new WaitUntil(() => allBulletsSpawned == true);
		Animate(1, 2f, BMP.centerPoint);
		yield return new WaitForSeconds(0.5f);
		if(animation) animation.GetComponent<SpriteAnimationController>().rotationSpeed = 10f;
	}
}
