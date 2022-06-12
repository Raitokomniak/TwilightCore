using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Aurora : BulletMovementPattern
{
	public BMP_Aurora(){}

    public BMP_Aurora(Pattern p, float _movementSpeed, float _magnitude){
		targetMagnitude = _magnitude;
		movementSpeed = _movementSpeed;
        pattern = p;
		scale = new Vector3 (2,2,2);
    }

	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        return CopyValues("Aurora", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        Explode (false, bullet, 14, 1);
		rotation = bullet.transform.rotation;
		yield return new WaitForSeconds (.1f);
		movementSpeed = .5f;
		yield return new WaitForSeconds (2f);
		movementSpeed = 0f;
		yield return new WaitUntil (() => pattern.allBulletsSpawned);
		movementSpeed = 8f;
		for (int i = 0; i < 2; i++) {
			Quaternion newRotation = Quaternion.Euler (0, 0, 150 * Random.Range (-1, 1));
			rotation = Quaternion.RotateTowards (bullet.transform.rotation, newRotation, Time.deltaTime);
			yield return new WaitForSeconds (1f);
			rotation = Quaternion.Euler (0, 0, 90 * Random.Range (-1, 1));
		yield return new WaitForSeconds (.5f);
        }
    }
}
