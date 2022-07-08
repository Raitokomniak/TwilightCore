using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_WaitAndExplode : BulletMovementPattern
{


	public BMP_WaitAndExplode(){}

    public BMP_WaitAndExplode(Pattern p, float _movementSpeed, float _waitTime){
        accelMax = _movementSpeed;
        waitAndExplodeWaitTime = _waitTime;
        pattern = p;
		scale = new Vector3 (2,2,2);
    }

	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("WaitAndExplode", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 0;
		rotation = bullet.transform.rotation;
		StartMoving (14);
		yield return new WaitUntil (() => pattern.allBulletsSpawned == true);
		yield return new WaitForSeconds(waitAndExplodeWaitTime);
		movementSpeed = accelMax;
		accelSpeed = 20f;
		SmoothAcceleration ();
		StartMoving (14);
		rotation = bullet.transform.rotation;

		yield return null;
    }
}
