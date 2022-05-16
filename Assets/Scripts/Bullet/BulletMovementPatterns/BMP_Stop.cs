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
        BMP_Stop bmp = new BMP_Stop();
        bmp.movementSpeed = _bmp.movementSpeed;
		bmp.scale = _bmp.scale;
        bmp.isHoming = _bmp.isHoming;
        return bmp;
     }


    public override IEnumerator ExecuteRoutine(){
        Explode (14);
		rotation = bullet.transform.rotation;
		yield return new WaitForSeconds (.1f);
		movementSpeed = 1f;
		yield return new WaitForSeconds (2f);
		movementSpeed = 7f;
    }
}
