using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Aurora : BulletMovementPattern
{
	public BMP_Aurora(){}

    public BMP_Aurora(Pattern p, float _movementSpeed, float _magnitude){
        name = "aurora";
		targetMagnitude = _magnitude;
		movementSpeed = _movementSpeed;
        pattern = p;
		scale = new Vector3 (2,2,2);
    }

	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        return CopyValues("Aurora", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        Explode (14);
		rotation = bullet.transform.rotation;
		yield return Waitp1;
		movementSpeed = .5f;
		yield return Wait1;
		movementSpeed = 0f;
		yield return new WaitUntil (() => pattern.allBulletsSpawned);
        
		movementSpeed = 8f;
        accelSpeed = 1;

        SmoothAcceleration();
        
		Quaternion newRotation = Quaternion.Euler (0, 0, 150 * Random.Range (-1, 1));
		rotation = Quaternion.RotateTowards (bullet.transform.rotation, newRotation, Time.deltaTime);

		yield return Wait2;
        
        if(bulletSprite.name != "Fireball_Glow_Orange") {
            yield break;
        }

        SmoothAcceleration();
        rotation = Quaternion.Euler (0, 0, 90 * Random.Range (-1, 1));
    }
}
