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
		yield return Wait1;
		bulletMovement.spriteR.sprite = Game.control.spriteLib.SetBulletSprite ("Spear", "Bevel", "Purple");
        bulletMovement.glowRend.sprite = Game.control.spriteLib.SetBulletGlow("Spear");
        bulletMovement.glowRend.color = Game.control.spriteLib.GetColor("Purple");

		bulletBoxCollider.size = new Vector2 (.15f, 2.9f);
        
		yield return Wait1;
		FindPlayer(bullet);
        bullet.transform.GetChild(2).gameObject.SetActive(true);
        if(bulletHomingWarningLine) bulletHomingWarningLine.ActivateLine();
        startHoming = true;
		yield return new WaitForSeconds (.5f);
		accelMax = 40;
        accelSpeed = 40;
		SmoothAcceleration ();
        yield return new WaitForSeconds (.5f);
        if(bulletHomingWarningLine) bulletHomingWarningLine.DisableLine();
    }
}
