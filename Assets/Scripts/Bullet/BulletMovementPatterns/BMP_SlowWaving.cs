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
        return CopyValues(_bmp, new BMP_SlowWaving());
    }


    public override IEnumerator ExecuteRoutine(){
        StartMoving (14);

        isMoving = true;
		
		rotation = bullet.transform.rotation;

		if(randomDirs) {
			float randomRot = bullet.transform.rotation.z;
			randomRot = Random.Range(-90,90);

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

		else {
//			Debug.Log("orig rot " + bullet.transform.rotation);

		//	spriteRotation = Quaternion.EulerAngles(0,0,63);
			spriteRotation.eulerAngles = new Vector3(0,0,0);
			/* while(bullet.activeSelf){
				/*
				for(float i = 0; i < 360; i+=1){
					//rotation = Quaternion.Euler(0, 0, bullet.transform.rotation.z + rotation.z + i);
					//rotation.eulerAngles = new Vector3(0,0, rotation.eulerAngles.z + i);
					//spriteRotation = new Vector3(0,0, rotation.eulerAngles.z + i);
					spriteRotation = Quaternion.Euler(0,0,rotation.eulerAngles.z + i);
					yield return new WaitForSeconds(.2f);
				}
					Vector3 tempRot = rotation.eulerAngles;
				for(float i = 360; i > 0; i-=1)
				{
				//rotation = Quaternion.Euler(0, 0, bullet.transform.rotation.z + rotation.z + tempRot.z + i);
					//rotation.eulerAngles = new Vector3(0,0, rotation.eulerAngles.z - tempRot.z + i);
					//spriteRotation = new Vector3(0,0, rotation.eulerAngles.z - tempRot.z + i);
					spriteRotation = Quaternion.Euler(0,0, rotation.eulerAngles.z - tempRot.z + i);
					yield return new WaitForSeconds(.2f);
				}
				Debug.Log("new rot " + rotation);

				yield return new WaitForSeconds(.2f);
			}*/
		}

    }
}
