using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_LaserExpand : BulletMovementPattern
{
	public BMP_LaserExpand(){}

    public BMP_LaserExpand(Pattern p){
        pattern = p;
		scale = new Vector3 (2,2,2);
    }

	 public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
        return CopyValues("LaserExpand", _bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        Collider2D coll = bullet.GetComponent<BoxCollider2D>();
		coll.enabled = false;
		dontDestroy = true;
		centerPoint = bullet.transform.position;
		Explode (1);
		pattern.Animate(6, 1, centerPoint);
		Stop (bullet);
		yield return new WaitForSeconds (.5f);
		bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Laser", "Glow", "Purple");
		bullet.GetComponent<SpriteRenderer> ().sortingOrder = -1;
		Vector2 origColliderSize = bullet.GetComponent<BoxCollider2D>().size;
		scale = new Vector3 (0, 0, 0);
		
        for (float i = 0; i < 50; i++) {
			bullet.transform.localScale = new Vector3 (i * 0.02f, i, 1);
			bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (i * 0.002f, i);
			yield return new WaitForSeconds (.02f);
		}
				
		int dir = laserIndex;

		if (laserIndex == 0)    dir = 1;
		else    dir = -1;

		yield return new WaitForSeconds (2f);

		coll.enabled = true;
		for (float i = 50; i > 0; i--) {
			bullet.transform.localScale = new Vector3 (i * 0.1f, bullet.transform.localScale.y, 1);
			yield return new WaitForSeconds (.01f);
		}
		GameObject.Destroy (bullet);
    }
}
