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
        BMP_DownAndExplode bmp = new BMP_DownAndExplode();
        bmp.movementSpeed = _bmp.movementSpeed;
		bmp.scale = _bmp.scale;
        bmp.isHoming = _bmp.isHoming;
        bmp.pattern = _bmp.pattern;
        bmp.targetMagnitude = _bmp.targetMagnitude;
        return bmp;
    }


    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 5f;
		yield return new WaitForSeconds (1f);
        pattern.allBulletsSpawned = true;
		centerPoint = bullet.transform.position;
		CorrectRotation ();
        movementSpeed = 6f;
		Explode (2.5f);
		
        yield return new WaitUntil (() => bullet.GetComponent<EnemyBulletMovement> ().CheckDistance () > targetMagnitude);
        
		Stop (bullet);

		//yield return new WaitForSeconds (0.3f);
		RotateOnAxis (bullet, 1, 100f);

		yield return new WaitForSeconds (1f);
		CancelAxisRotation (10f);
    }

}
