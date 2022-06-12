using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Explode : BulletMovementPattern
{
    public BMP_Explode(){}

    //FOR SETTING UP IN WAVE INITS
    public BMP_Explode(Pattern p, float _movementSpeed, bool _isHoming, bool _startHoming){
        pattern = p;
        isHoming = _isHoming;
        startHoming = _startHoming;
        movementSpeed = _movementSpeed;
		scale = new Vector3 (2,2,2);
    }

    //FOR INSTANTIATING FOR EACH BULLET
    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("Explode", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){    
        Explode (false, bullet, 14, 1);
        rotation = bullet.transform.rotation;
        isMoving = true;
        yield return null;
    }
}
