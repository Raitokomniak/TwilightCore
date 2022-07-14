using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_DownAndExplode : BulletMovementPattern
{

    public BMP_DownAndExplode(){}

    public BMP_DownAndExplode(Pattern p, float _movementSpeed, float _targetMagnitude){
        pattern = p;
        movementSpeed = _movementSpeed;
        scale = new Vector3 (2,2,2);
        targetMagnitude = _targetMagnitude;
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        return CopyValues(_bmp, new BMP_DownAndExplode());
    }


    public override IEnumerator ExecuteRoutine(){
        bulletMovement = bullet.GetComponent<BulletMovement>();

        moveWithForce = false;
        movementSpeed = 5f;
		yield return new WaitForSeconds(1f);
        pattern.allBulletsSpawned = true;

        //i guess there was an extra bullet?
		if (pattern.spawnedBullets.Count != 0) 
            pattern.spawnedBullets.RemoveAt (pattern.spawnedBullets.Count - 1);

		centerPoint = bullet.transform.position;
        rotation.eulerAngles = new Vector3 (0f, 0f, (pattern.spawnedBullets.Count - 1) * (360 / (pattern.bulletCount)));

        movementSpeed = 6f;
		StartMoving (2.5f);
        yield return new WaitUntil (() => bulletMovement.GetRemainingDistance (centerPoint) > targetMagnitude);
		Stop (bullet);
        movementSpeed = 0f;
        
		yield return new WaitForSeconds (0.3f);
		RotateOnAxis (1, 10f);

		yield return new WaitForSeconds(1f);
		CancelAxisRotation (10f);
    }

}
