using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_SlowWaving : BulletMovementPattern
{

	public BMP_SlowWaving(){}

    public BMP_SlowWaving(Pattern p, float _movementSpeed, bool _randomDirs){
        pattern = p;
		randomDirs = _randomDirs;
		movementSpeed = _movementSpeed;
		scale = new Vector3 (2,2,2);
    }

 	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        BMP_SlowWaving bmp = new BMP_SlowWaving();
		bmp.pattern = _bmp.pattern;
        bmp.movementSpeed = _bmp.movementSpeed;
		bmp.scale = _bmp.scale;
		bmp.randomDirs = _bmp.randomDirs;
        return bmp;
    }


    public override IEnumerator ExecuteRoutine(){
        Explode (14);
		//Explode (false, bullet, 14, 1);
        rotation = bullet.transform.rotation;
        isMoving = true;
		
		float randomRot = bullet.transform.rotation.z;
		if(randomDirs) randomRot = Random.Range(-90,90);
        while(bullet.activeSelf){		
			for(float i = 0; i < 70; i+=5){
				rotation = Quaternion.Euler(0, 0, randomRot + rotation.z + i);
				yield return new WaitForSeconds(.02f);
			}
				Quaternion tempRot = rotation;
			for(float i = 70; i > 0; i-=5)
			{
				rotation = Quaternion.Euler(0, 0, randomRot + tempRot.z + i);
				yield return new WaitForSeconds(.02f);
			}
		}
    }
}
