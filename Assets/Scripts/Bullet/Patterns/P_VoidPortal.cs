using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_VoidPortal : Pattern
{
    float stayTime;
    
    public P_VoidPortal(float _stayTime){
        bulletCount = 10;
		coolDown = 3;
		tempMagnitude = originMagnitude;
        stayTime = _stayTime;
    }


    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        bullets = new ArrayList ();

        animation = (Resources.Load ("Images/Animations/A_VoidPortal") as GameObject);
        animation.GetComponent<BulletAnimationController> ().stayTime = stayTime;
        animation.GetComponent<BulletAnimationController> ().scaleDown = true;
        animating = false;

        bulletRotation = rot;
			
		for (int i = 0; i < bulletCount; i++) {
			bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / bulletCount));
            bulletMovement = bulletMovement.GetNewBulletMovement(bulletMovement);
			InstantiateBullet (enemyBullet, bulletMovement);
            Animate(5, 1, enemy.transform.position);
		}
        yield return new WaitForSeconds(2f);
        animation.GetComponent<BulletAnimationController> ().FadeAway();
        
    }
}
