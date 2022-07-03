using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_StopAndRotate : BulletMovementPattern
{
	public BMP_StopAndRotate(){
	}

    public BMP_StopAndRotate(Pattern p, int _layer, float _targetMagnitude){
        pattern = p;
        layer = _layer;
		targetMagnitude = _targetMagnitude;
		scale = new Vector3 (2,2,2);
    }

	 public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("StopAndRotate", _bmp);
    }


    public override IEnumerator ExecuteRoutine(){
        dontDestroy = true;
		rotation = bullet.transform.rotation;
		movementSpeed = 10;
		Explode (targetMagnitude);
		yield return new WaitUntil (() => bulletMovement.GetRemainingDistance (centerPoint) > targetMagnitude);
		Stop (bullet);
		yield return Wait1;
		pattern.allBulletsSpawned = true;
		if (layer == 0) RotateOnAxis (-1, 80f);
		if (layer == 1) RotateOnAxis (1, 80f);
		if (layer == 2) RotateOnAxis (-1, 80f);
		if (layer == 3) RotateOnAxis (1, 80f);

		yield return Wait1;
		
        isMoving = true;
		movementSpeed = 10f;
		rotateOnAxis = false;

		yield return Wait1;

		dontDestroy = false;
    }
}
