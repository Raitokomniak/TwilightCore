using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_WaitToHome : BulletMovementPattern
{
    public BMP_WaitToHome(){}
    
    public BMP_WaitToHome(Pattern p, float _accelSpeed){
        pattern = p;
        accelSpeed = _accelSpeed;
        scale = new Vector3 (2,2,2);
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        BMP_WaitToHome bmp = new BMP_WaitToHome();
		bmp.pattern = _bmp.pattern;
        bmp.movementSpeed = _bmp.movementSpeed;
        bmp.accelSpeed = _bmp.accelSpeed;
		bmp.scale = _bmp.scale;
        return bmp;
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 0;
		yield return new WaitForSeconds (1f);
		FindPlayer(bullet);
		yield return new WaitForSeconds (.3f);
	    //movementSpeed = 10f;
        accelSpeed = 25; //REMOVE THIS LATER
		bullet.GetComponent<EnemyBulletMovement> ().SmoothAcceleration ();
    }
}
