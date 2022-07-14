using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Laser : Pattern
{   
    int size = 1;
    float laserDelay = 2f;

	public P_Laser(int _size, float delay){
		coolDown = 10;
        size = _size;
        laserDelay = delay;
		originMagnitude = 15;
		tempMagnitude = originMagnitude;
	}
	
    public override IEnumerator ExecuteRoutine(EnemyShoot enemy){
        yield return new WaitForSeconds(executeDelay);
		pos = enemy.transform.position;
		
			if (bulletCount > 1) {
				for (int i = 0; i < bulletCount; i++) {
					float ang = (i * (360 / bulletCount) + (360 / bulletCount)) * 0.7f;
					if (i >= 3) ang += 30;

					spawnPosition = SpawnInCircle (pos + new Vector3 (0, 1f, 0), 1.5f, ang);
                    
					BMP = new BMP_LaserExpand(this, size);
                    BMP.rotation = Quaternion.Euler(0,0,ang);
                    BMP.laserDelay = laserDelay;

				    SpawnBullet ();
                }
			} 
			else {
				spawnPosition = pos - new Vector3 (0f, .2f, 0f);
				BMP = new BMP_LaserExpand(this, size);
				SpawnBullet ();
			}
    }
}
