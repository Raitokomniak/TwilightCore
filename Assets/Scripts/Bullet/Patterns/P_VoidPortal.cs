using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_VoidPortal : Pattern
{
    float stayTime;
    float targetScale;

    float scalingTime;

    public bool dontDestroyAnimation = false;
    
    public P_VoidPortal(float _stayTime, float _targetScale, float _scalingTime){
       // bulletCount = 1;
		coolDown = 3;
		tempMagnitude = originMagnitude;
        stayTime = _stayTime;
        targetScale = _targetScale;
        scalingTime = _scalingTime;
    }


    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(delayBeforeAttack);
		pos = enemy.transform.position;

        bullets = new ArrayList ();

        animation = (Resources.Load ("Images/Animations/A_VoidPortal") as GameObject);
        animation.GetComponent<BulletAnimationController> ().stayTime = stayTime;
        animation.GetComponent<BulletAnimationController> ().rotationSpeed = 10f;
        animation.GetComponent<BulletAnimationController>().dontDestroy = dontDestroyAnimation;
        animation.GetComponent<BulletAnimationController> ().scaleDown = true;
        animating = false;
		
        //InstantiateBullet (enemyBullet, bulletMovement);
        Animate(targetScale, scalingTime, enemy.transform.position); 
    }
}
