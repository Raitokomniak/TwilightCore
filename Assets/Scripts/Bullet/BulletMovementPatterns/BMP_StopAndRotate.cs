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
       return CopyValues(_bmp, new BMP_StopAndRotate());
    }


    public override IEnumerator ExecuteRoutine(){
        
        bulletMovement = bullet.GetComponent<BulletMovement>();
        dontDestroy = true;
		rotation = bullet.transform.rotation;
		movementSpeed = 10;
		StartMoving (targetMagnitude);
		yield return new WaitUntil (() => bulletMovement.GetRemainingDistance (centerPoint) > targetMagnitude);
		Stop (bullet);
		yield return new WaitForSeconds(1f);
		pattern.allBulletsSpawned = true;
		if (layer == 0) RotateOnAxis (-1, 8f);
		if (layer == 1) RotateOnAxis (1, 8f);
		if (layer == 2) RotateOnAxis (-1, 8f);
		if (layer == 3) RotateOnAxis (1, 8f);

		yield return new WaitForSeconds(1f);
		
        isMoving = true;
		movementSpeed = 10f;
		rotateOnAxis = false;

		yield return new WaitForSeconds(1f);

		dontDestroy = false;
    }
}
