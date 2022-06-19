using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_TurnToSpears : BulletMovementPattern
{
	public BMP_TurnToSpears(){}

    public BMP_TurnToSpears(Pattern p, float _movementSpeed){
        pattern = p;
        movementSpeed = _movementSpeed;
        hitBoxType = "Box";
		scale = new Vector3 (2,2,2);
    }

	public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues("TurnToSpears", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 0;
		rotation = Quaternion.Euler(0,0,0);
		yield return new WaitForSeconds (1f);
		bullet.GetComponent<BulletMovement>().spriteR.sprite = Game.control.spriteLib.SetBulletSprite ("Spear", "Bevel", "Lilac");
		bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (.15f, 2.9f);
        
		yield return new WaitForSeconds (1f);
		FindPlayer(bullet);
        bullet.transform.GetChild(2).gameObject.SetActive(true);
        bullet.GetComponentInChildren<HomingWarningLine>().ActivateLine();
        startHoming = true;
		yield return new WaitForSeconds (.5f);
		accelMax = 40;
        accelSpeed = 20;
		bullet.GetComponent<BulletMovement> ().SmoothAcceleration ();
        yield return new WaitForSeconds (.5f);
        if(bullet.GetComponentInChildren<HomingWarningLine>()) bullet.GetComponentInChildren<HomingWarningLine>().DisableLine();
    }
}
