using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_WaitToHome : BulletMovementPattern
{
    public BMP_WaitToHome(){}
    
    public BMP_WaitToHome(Pattern p, float _accelSpeed, bool _trail){
        pattern = p;
        accelMax = _accelSpeed;
        scale = new Vector3 (2,2,2);
        trail = _trail;
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues(_bmp, new BMP_WaitToHome());
    }

    public override IEnumerator ExecuteRoutine(){
        bulletMovement = bullet.GetComponent<BulletMovement>();
        
        if(trail) {
            waitToTrail = true;
            trail = false;
        }
        movementSpeed = 0;
        yield return new WaitForSeconds(1f);
        FindPlayer(bullet);
		yield return new WaitForSeconds (.3f);
        if(waitToTrail) trail = true;
        
        startHoming = true;
        accelMax = 25;
        accelSpeed = 20;
		SmoothAcceleration ();
    }
}
