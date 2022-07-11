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
       return CopyValues(_bmp, new BMP_Stop());
     }


    public override IEnumerator ExecuteRoutine(){
        StartMoving (14);
		rotation = bullet.transform.rotation;
		yield return new WaitForSeconds(.1f);
		movementSpeed = 1f;
		yield return new WaitForSeconds(2f);
		movementSpeed = 7f;
    }
}
