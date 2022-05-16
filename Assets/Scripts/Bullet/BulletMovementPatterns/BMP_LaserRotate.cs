using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_LaserRotate : BulletMovementPattern
{
     public BMP_LaserRotate(Pattern p){
        pattern = p;
    }


    public override IEnumerator ExecuteRoutine(){
        dontDestroy = true;
		centerPoint = bullet.transform.position;
		bullet.GetComponent<EnemyBulletMovement> ().isLaser = true;
		Explode (1);
		pattern.Animate(6, 1, centerPoint);
		Stop (bullet);
		yield return new WaitForSeconds (.5f);
		bullet.GetComponent<SpriteRenderer> ().sprite = Game.control.spriteLib.SetBulletSprite ("Laser", "Glow", "Purple");
		bullet.GetComponent<SpriteRenderer> ().sortingOrder = -1;
		Vector3 origColliderSize = bullet.GetComponent<BoxCollider2D>().size;
		scale = new Vector3 (0, 0, 0);
		for (float i = 0; i < 50; i++) {
			scale = new Vector3 (i * 0.02f, i, 1);
			bullet.GetComponent<BoxCollider2D> ().size = new Vector2 (i * 0.002f, i);
			//bullet.transform.position -= new Vector3(0, 0.25f, 0);
			//_RotateOnAxis (bullet, 1, 100f);
			yield return new WaitForSeconds (.02f);
		}
				
		int dir = laserIndex;

		if (laserIndex == 0) dir = 1;
		else dir = -1;
		RotateOnAxis (bullet, dir, 10f);
		yield return new WaitForSeconds (4f);
			/*_RotateOnAxis (bullet, -dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, dir, 10f);
			yield return new WaitForSeconds (4f);
			_RotateOnAxis (bullet, -dir, 10f);
			
			for (float i = 50; i > 0; i--) {
				scale = new Vector3 (i * 0.1f, scale.y, 1);
				yield return new WaitForSeconds (.01f);
			}
			GameObject.Destroy (bullet);*/
    }
}
