using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_LaserPendulum : BulletMovementPattern
{

	public BMP_LaserPendulum(){
    }

    public BMP_LaserPendulum(Pattern p){
        pattern = p;
    }

	

    public override IEnumerator ExecuteRoutine(){
        dontDestroy = true;
		centerPoint = bullet.transform.position;

		Stop (bullet);
		scale = new Vector3 (0, 0, 0);
		for (float i = 0; i < 50; i++) {
			scale = new Vector3 (i * 0.1f, i, 1);
			bullet.transform.position -= new Vector3(0, 0.25f, 0);
			//_RotateOnAxis (bullet, 1, 100f);
			yield return new WaitForSeconds (.01f);
		}
		
		int dir = 1;
		if (laserIndex == 0)
			dir = 1;
		else
			dir = -1;
		yield return new WaitForSeconds (2f);
		RotateOnAxis (bullet, dir, 10f);
		yield return new WaitForSeconds (4f);
		RotateOnAxis (bullet, -dir, 10f);
		yield return new WaitForSeconds (4f);
		RotateOnAxis (bullet, dir, 10f);
		yield return new WaitForSeconds (4f);
		RotateOnAxis (bullet, -dir, 10f);

		for (float i = 50; i > 0; i--) {
			scale = new Vector3 (i * 0.1f, scale.y, 1);
			yield return new WaitForSeconds (.01f);
		}
		GameObject.Destroy (bullet);
    }
}
