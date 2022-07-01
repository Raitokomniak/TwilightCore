using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_WaitToHome : BulletMovementPattern
{
    public BMP_WaitToHome(){}
    
    public BMP_WaitToHome(Pattern p, float _accelSpeed){
        pattern = p;
        accelMax = _accelSpeed;
        scale = new Vector3 (2,2,2);
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("WaitToHome", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 0;
        yield return Wait1;
        FindPlayer(bullet);
		yield return new WaitForSeconds (.3f);
        startHoming = true;
        accelMax = 25;
        accelSpeed = 10;
		SmoothAcceleration ();
    }
}
