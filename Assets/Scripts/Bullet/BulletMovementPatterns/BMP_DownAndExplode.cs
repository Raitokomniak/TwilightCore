using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_DownAndExplode : BulletMovementPattern
{

    public BMP_DownAndExplode(){}

    public BMP_DownAndExplode(Pattern p, float _movementSpeed, bool _isHoming, float _targetMagnitude){
        pattern = p;
        isHoming = _isHoming;
        movementSpeed = _movementSpeed;
        scale = new Vector3 (2,2,2);
        targetMagnitude = _targetMagnitude;
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        return CopyValues("DownAndExplode", _bmp);
    }


    public override IEnumerator ExecuteRoutine(){
        moveWithForce = false;
        movementSpeed = 5f;
		yield return Wait1;
        pattern.allBulletsSpawned = true;

        //i guess there was an extra bullet?
		if (pattern.spawnedBullets.Count != 0) 
            pattern.spawnedBullets.RemoveAt (pattern.spawnedBullets.Count - 1);

		centerPoint = bullet.transform.position;
        rotation.eulerAngles = new Vector3 (0f, 0f, (pattern.spawnedBullets.Count - 1) * (360 / (pattern.bulletCount)));

        movementSpeed = 6f;
		Explode (2.5f);
        yield return new WaitUntil (() => bulletMovement.GetRemainingDistance (centerPoint) > targetMagnitude);
		Stop (bullet);
        movementSpeed = 0f;
        
		yield return new WaitForSeconds (0.3f);
		RotateOnAxis (1, 100f);

		yield return Wait1;
		CancelAxisRotation (10f);
    }

}
