using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Pattern
{
	public GameObject bullet;
	public ArrayList bullets;
	public EnemyShoot enemyShoot;
	public Sprite sprite;

	public Vector3 newPosition;
	public Quaternion bulletRotation;
	public BulletMovementPattern bulletMovement;

	public IEnumerator routine;
	public GameObject enemyBullet;

	public float delayBeforeAttack = 0; //DEFAULT
	public float coolDown = 1; //DEFAULT
	public int bulletCount = 1; //DEFAULT
	public float rotationMultiplier = 0;  //DEFAULT
	public float startingRotation = 0;

	public float circleDelay;

	public GameObject animation;
	public bool animating;


	public int layers = 1;  //DEFAULT
	public int tempLayer;

	public bool infinite = true; //DEFAULT BECAUSE BOSSES, SEE IF YOU BOTHER TO CHANGE THIS
	public bool stop;

	public float originMagnitude = 100;  //DEFAULT
	public float tempMagnitude;



	public bool bossSpecial = false;
	public bool allBulletsSpawned;

	public float loopCircles;

	public int lineDirection = 1;


	public int rotationDirection;


	public Vector3 pos;
	public Quaternion rot;

	public Pattern(){}


	public void StopPattern(){
		//Debug.Log("stoppat");
		stop = true;
	}

	public void InstantiateBullet (GameObject enemyBullet, BulletMovementPattern _bulletMovement)
	{
		if(bulletMovement == null) bulletMovement = new BMP_Explode(this, 5f, false); //DEFAULT
		bulletMovement = bulletMovement.GetNewBulletMovement(bulletMovement);
		bullet = (Object.Instantiate (enemyBullet, newPosition, bulletRotation) as GameObject);
		bullet.transform.SetParent (GameObject.FindWithTag ("BulletsRepo").transform);
		bullet.GetComponent<EnemyBulletMovement> ().SetUpBulletMovement (bulletMovement);
		bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
		if(enemyShoot.bulletsShot != null) enemyShoot.bulletsShot.Add (bullet);
	}

	public IEnumerator Execute(GameObject _enemyBullet, EnemyShoot _enemy){
		enemyShoot = _enemy;
		pos = _enemy.transform.position;
		rot = _enemy.transform.rotation;
		enemyBullet = _enemyBullet;

		newPosition = _enemy.transform.position;
		bulletRotation = _enemy.transform.rotation;
		stop = false;
		 
		routine = ExecuteRoutine (_enemy);
		return routine;
	}

    public virtual IEnumerator ExecuteRoutine (EnemyShoot enemy){
        yield return null;
    }

	public void CheckSoundPlay (int i, int divider)
	{
		if (i % divider == 0)
			Game.control.sound.PlaySound ("Enemy", "Shoot", true);
	}


	public float GetAng (int i, float fullDegrees)
	{
		float ang = i * (fullDegrees / bulletCount); 
		return ang;
	}

	public Quaternion SpawnInCircle (int i, float startingRotation)
	{
		Quaternion rot = Quaternion.Euler (0f, 0f, (float)(i * (360 / bulletCount)) + startingRotation);
		return rot;
	}

	public Vector3 SpawnInLine (float first, float magnitude, int dir, int index)
{
		Vector3 pos = new Vector3 (first + (dir * (index * (magnitude / bulletCount))), enemyShoot.transform.position.y, 0);
		return pos;
	}


	public Vector3 SpawnInCircle (Vector3 centerPos, float radius, float ang)
	{
		Vector3 position = Vector3.zero;
		position.x = centerPos.x + radius * Mathf.Sin (ang * Mathf.Deg2Rad); 
		position.y = centerPos.y - radius * Mathf.Cos (ang * Mathf.Deg2Rad); 
		position.z = centerPos.z;
		bulletRotation = Quaternion.Euler (0f, 0f, ang);
		return position;
	}

	public void Animate (float targetScale, float scaleTime, Vector3 centerPoint)
	{
		if (animation != null && !animating) {
			animating = true;
			animation = (Object.Instantiate (animation, centerPoint, Quaternion.Euler (Vector3.zero)) as GameObject);
			animation.GetComponent<BulletAnimationController> ().SetScale (targetScale, scaleTime);
		}
	}

	public void SetSprite (string shape, string effect, string color)
	{
		sprite = Game.control.spriteLib.SetBulletSprite (shape, effect, color);
	}

	public void SetSprite (string shape)
	{
		sprite = Game.control.spriteLib.SetBulletSprite (shape);
	}

}
