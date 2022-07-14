using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_LaserExpand : BulletMovementPattern
{

	public BMP_LaserExpand(){}

    public BMP_LaserExpand(Pattern p){
        pattern = p;
        laserMaxScale = 100;
		//scale = new Vector3 (2,2,2);
    }

    public BMP_LaserExpand(Pattern p, float _maxScale){
        pattern = p;
        laserMaxScale = _maxScale;
		//scale = new Vector3 (2,2,2);
    }

	 public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        return CopyValues(_bmp, new BMP_LaserExpand());
    }

    public override IEnumerator ExecuteRoutine(){
        bulletMovement = bullet.GetComponent<BulletMovement>();
        bulletBoxCollider = bullet.GetComponent<BoxCollider2D>();
        bulletMovement.spriteR.enabled = false;
		moveWithForce = false;
		forceScale = true;
		scale = new Vector3 (0, 0, 0);
		bulletMovement.rb.isKinematic = true;
        
		yield return null;
		
		bulletBoxCollider.enabled = false;
		dontDestroy = true;
		centerPoint = bullet.transform.position;
		StartMoving (1);
		pattern.Animate(6, 1, centerPoint);
		Stop (bullet);
		yield return new WaitForSeconds (.5f);
		//bulletMovement.spriteR.sprite = Game.control.spriteLib.SetBulletSprite ("Laser", "Glow", "Purple");
        bulletMovement.spriteR.enabled = true;
		bulletMovement.spriteR.sortingOrder = -1;
		Vector2 origColliderSize = bulletBoxCollider.size;

        
        
        for (float i = 0; i < 50; i++) {
            scale = new Vector3 (i * 0.02f * laserMaxScale, i / 1.5f, 1);
			bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (i * 0.002f * laserMaxScale, 2);
			yield return new WaitForSeconds (.02f);
		}

		yield return new WaitForSeconds(laserDelay);

		bulletBoxCollider.enabled = true;
        bulletBoxCollider.size = new Vector2 (origColliderSize.x, bulletBoxCollider.size.y);
        float max = 50 * laserMaxScale;
        
		for (float i = 50; i > 0; i--) {
			/*scale = new Vector3 (i * 0.1f, bullet.transform.localScale.y, 1);
			//yield return new WaitForSeconds (.005f);
            if(i == max / 2) bulletBoxCollider.enabled = false;
            yield return new WaitForSeconds(Time.deltaTime);*/
            scale = new Vector3 (i * 0.1f * laserMaxScale, bullet.transform.localScale.y, 1);
			yield return new WaitForSeconds (.01f);
		}
        bulletBoxCollider.enabled = false;
        scale = new Vector3 (0, 0, 0);


        


/*
        for (float i = 0; i < 50; i++) {
			scale = new Vector3 (i * 0.02f, i, 1);
			bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (i * 0.002f, i);
			yield return new WaitForSeconds (.02f);
		}

		yield return new WaitForSeconds(2f);

		bulletBoxCollider.enabled = true;
		for (float i = 50; i > 0; i--) {
			scale = new Vector3 (i * 0.1f, bullet.transform.localScale.y, 1);
			yield return new WaitForSeconds (.01f);
		}*/






        //GameObject.Destroy(bullet);
       bulletMovement.Pool();
    }
}
