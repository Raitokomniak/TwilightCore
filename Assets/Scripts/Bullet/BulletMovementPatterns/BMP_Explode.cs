using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Explode : BulletMovementPattern
{
    public BMP_Explode(){}

    public BMP_Explode(Pattern p, float _movementSpeed){
        pattern = p;
        movementSpeed = _movementSpeed;
		//scale = new Vector3 (2,2,2);
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("Explode", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){    
        Explode (14);
        rotation = bullet.transform.rotation;
        if(startHoming) FindPlayer(bullet);
        isMoving = true;
        yield return null;
    }
}
