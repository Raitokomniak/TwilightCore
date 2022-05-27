using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_WaitAndExplode : BulletMovementPattern
{

	public BMP_WaitAndExplode(){}

    public BMP_WaitAndExplode(Pattern p, float _movementSpeed){
        accelSpeed = _movementSpeed;
        pattern = p;
		scale = new Vector3 (2,2,2);
    }

	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        BMP_WaitAndExplode bmp = new BMP_WaitAndExplode();
		bmp.pattern = _bmp.pattern;
        bmp.accelSpeed = _bmp.accelSpeed;
		bmp.scale = _bmp.scale;
        return bmp;
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 0;
		rotation = bullet.transform.rotation;
		Explode (false, bullet, 14, 1);
		yield return new WaitUntil (() => pattern.allBulletsSpawned);
		yield return new WaitForSeconds (.2f);
		movementSpeed = accelSpeed;
		bullet.GetComponent<EnemyBulletMovement> ().SmoothAcceleration ();
		Explode (false, bullet, 14, 1);
		rotation = bullet.transform.rotation;

		yield return null;
    }
}
