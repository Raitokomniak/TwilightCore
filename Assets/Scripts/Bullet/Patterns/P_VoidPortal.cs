using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_VoidPortal : Pattern
{
    float stayTime;
    float targetScale;
    
    public P_VoidPortal(float _stayTime, float _targetScale){
       // bulletCount = 1;
		coolDown = 3;
		tempMagnitude = originMagnitude;
        stayTime = _stayTime;
        targetScale = _targetScale;
    }


    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        bullets = new ArrayList ();

        animation = (Resources.Load ("Images/Animations/A_VoidPortal") as GameObject);
        animation.GetComponent<BulletAnimationController> ().stayTime = stayTime;
        animation.GetComponent<BulletAnimationController> ().scaleDown = true;
        animating = false;
		
        //InstantiateBullet (enemyBullet, bulletMovement);
        Animate(targetScale, 1, enemy.transform.position); 
    }
}
