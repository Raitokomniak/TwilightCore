using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Stop : BulletMovementPattern
{
    public BMP_Stop(){}
    public BMP_Stop(Pattern p){
        pattern = p;
        movementSpeed = 0;
    }

     public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("Stop", _bmp);
     }


    public override IEnumerator ExecuteRoutine(){
        Explode (14);
		rotation = bullet.transform.rotation;
		yield return Waitp1;
		movementSpeed = 1f;
		yield return Wait2;
		movementSpeed = 7f;
    }
}
