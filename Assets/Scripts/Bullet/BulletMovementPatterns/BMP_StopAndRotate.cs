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
        BMP_StopAndRotate bmp = new BMP_StopAndRotate();
        bmp.movementSpeed = _bmp.movementSpeed;
		bmp.scale = _bmp.scale;
		bmp.layer = _bmp.layer;
		bmp.pattern = _bmp.pattern;
		bmp.targetMagnitude = _bmp.targetMagnitude;
        return bmp;
    }


    public override IEnumerator ExecuteRoutine(){
        dontDestroy = true;
		rotation = bullet.transform.rotation;
		movementSpeed = 10;
		Explode (targetMagnitude);
		yield return new WaitUntil (() => bullet.GetComponent<EnemyBulletMovement> ().GetRemainingDistance () > targetMagnitude);
		Stop (bullet);
		yield return new WaitForSeconds (1f);
		pattern.allBulletsSpawned = true;

		if (layer == 0) {
			RotateOnAxis (bullet, -1, 80f);
		} else if (layer == 1) {
			RotateOnAxis (bullet, 1, 80f);
		} else if (layer == 2) {
			RotateOnAxis (bullet, -1, 80f);
		} else if (layer == 3) {
			RotateOnAxis (bullet, 1, 80f);
		}

		yield return new WaitForSeconds (1f);
		
        isMoving = true;
		movementSpeed = 10f;
		rotateOnAxis = false;

		yield return new WaitForSeconds (1f);

		dontDestroy = false;
    }
}
