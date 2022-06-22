using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_WaitAndExplode : BulletMovementPattern
{

	public BMP_WaitAndExplode(){}

    public BMP_WaitAndExplode(Pattern p, float _movementSpeed){
        accelMax = _movementSpeed;
        pattern = p;
		scale = new Vector3 (2,2,2);
    }

	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("WaitAndExplode", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 0;
		rotation = bullet.transform.rotation;
		Explode (14);
		yield return new WaitUntil (() => pattern.allBulletsSpawned);
		yield return new WaitForSeconds(waitAndExplodeWaitTime);

		movementSpeed = accelMax;
		accelSpeed = 10f;
		SmoothAcceleration ();
		Explode (14);
		rotation = bullet.transform.rotation;

		yield return null;
    }
}
