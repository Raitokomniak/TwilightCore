using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Explode : BulletMovementPattern
{
    public BMP_Explode(){}

    //FOR SETTING UP IN WAVE INITS
    public BMP_Explode(Pattern p, float _movementSpeed, bool _isHoming){
        pattern = p;
        isHoming = _isHoming;
        movementSpeed = _movementSpeed;
		scale = new Vector3 (2,2,2);
    }

    //FOR INSTANTIATING FOR EACH BULLET
    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        BMP_Explode bmp = new BMP_Explode();
        bmp.movementSpeed = _bmp.movementSpeed;
		bmp.scale = _bmp.scale;
        bmp.isHoming = _bmp.isHoming;
        return bmp;
    }

    public override IEnumerator ExecuteRoutine(){    
        Explode (false, bullet, 14, 1);
        rotation = bullet.transform.rotation;
        isMoving = true;
        yield return null;
    }
}
