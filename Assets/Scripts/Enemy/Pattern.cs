﻿using UnityEngine;
using System.Collections;

public class Pattern
{
	SpriteLibrary spriteLib;

	public string name;

	GameObject bullet;
	public ArrayList bullets;
	EnemyShoot enemy;
	Sprite sprite;

	Vector3 newPosition;
	Quaternion bulletRotation;
	public BulletMovementPattern movement;

	public Phase phase;

	public float coolDown;
	public int bulletCount;
	float rotationMultiplier;
	float startingRotation;

	public GameObject animation;
	public bool animating;


	public int layers;
	int tempLayer;

	public bool stop;

	public float originMagnitude;
	public float tempMagnitude;

	int laserIndex;

	public bool bossSpecial;
	public bool spriteSetExternally;
	public bool allBulletsSpawned;

	public float loopCircles;

	public int lineDirection = 1;
	public bool customized;

	int rotationDirection;

	public Pattern (string _name, bool _bossSpecial, int _bulletCount, float _rotationMultiplier, float _coolDown, int _layers, float magnitude)
	{
		name = _name;
		bulletCount = _bulletCount;
		rotationMultiplier = _rotationMultiplier;
		coolDown = _coolDown;
		layers = _layers;
		tempLayer = 0;
		originMagnitude = magnitude;
		tempMagnitude = magnitude;


		laserIndex = 0;
		bossSpecial = _bossSpecial;

		loopCircles = 0f;
		spriteLib = GameController.gameControl.spriteLib;
		bullets = new ArrayList ();
		lineDirection = 1;

	}

	public Pattern (Pattern p)
	{
		name = p.name;
		bulletCount = p.bulletCount;
		rotationMultiplier = p.rotationMultiplier;
		coolDown = p.coolDown;
		layers = p.layers;
		tempLayer = 0;
		originMagnitude = p.originMagnitude;
		tempMagnitude = p.tempMagnitude;

		laserIndex = 0;
		bossSpecial = p.bossSpecial;
		spriteLib = GameController.gameControl.spriteLib;
		lineDirection = 1;
	}


	private void Instantiate (GameObject enemyBullet)
	{
		movement = new BulletMovementPattern (movement);
		bullet = (Object.Instantiate (enemyBullet, newPosition, bulletRotation) as GameObject);
		bullet.transform.SetParent (GameObject.FindWithTag ("BulletsRepo").transform);
		bullet.GetComponent<EnemyBulletMovement> ().SetUpBulletMovement (movement);
		bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
		enemy.bulletsShot.Add (bullet);
	}

	public IEnumerator Execute (GameObject enemyBullet, Vector3 pos, Quaternion rot, EnemyShoot _enemy)
	{
		enemy = _enemy;
		phase = enemy.phase;
		newPosition = pos;
		bulletRotation = rot;

		switch (name) {
		case "Circle":
			GameController.gameControl.sound.PlaySound ("Enemy", "Shoot", true);
			for (int i = 0; i < bulletCount; i++) {
				newPosition = SpawnInCircle (pos, 0f, GetAng (i, 360));
				Instantiate (enemyBullet);
			}
			break;
		case "Laser":
			if (bulletCount > 1) {
				for (int i = 0; i < bulletCount; i++) {
					float ang = (i * (360 / bulletCount) + (360 / bulletCount)) * 0.7f;
					if (i >= 3)
						ang += 60;

					newPosition = SpawnInCircle (pos + new Vector3 (0, 1f, 0), 1.5f, ang);

					movement = new BulletMovementPattern (false, "ExpandToLaser", 0f, this, 0, tempMagnitude);
					movement.laserIndex = laserIndex;
					laserIndex++;

					Instantiate (enemyBullet);

					bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
					if (laserIndex == 0)
						laserIndex++;
					else
						laserIndex = 0;
				}

			} else {
				sprite = Resources.Load<Sprite> ("enemyLaser");
				newPosition = pos - new Vector3 (0f, .2f, 0f);
				
				movement = new BulletMovementPattern (false, "PendulumLaser", 0f, this, 0, tempMagnitude);
				movement.laserIndex = laserIndex;
			
				Instantiate (enemyBullet);
			}

			break;
		case "Maelstrom":
			stop = false;

			while (!stop) {
				GameController.gameControl.sound.PlaySound ("Enemy", "Shoot", true);
				for (int i = 0; i < bulletCount; i++) {
					newPosition = SpawnInCircle (pos, 1.5f, GetAng (i, 360) + startingRotation);
					bulletRotation = SpawnInCircle (i, startingRotation);
					startingRotation += 0.5f * rotationDirection;
					Instantiate (enemyBullet);
				}
				yield return new WaitForSeconds (coolDown);
			}
			break;
		case "Cluster":
			int dir = 1;
			for (int i = 0; i < bulletCount; i++) {	
				newPosition = enemy.GetLocalPosition () + new Vector3 (Random.Range (-.8f, .8f), 1.5f * dir, 0);
				bulletRotation = bulletRotation * Quaternion.Euler (0, 0, 180f + (Random.Range (-10, 10)));
				movement = new BulletMovementPattern (false, "Aurora", 10f, this, tempLayer, tempMagnitude);
				Instantiate (enemyBullet);
				dir = -dir;

				yield return new WaitForSeconds (coolDown);
				CheckSoundPlay (i, 5);
				//if(i % 2 == 0) SetSprite ("Arrow", "Glow", "Red");
				//if(i % 3 == 0) SetSprite ("Arrow", "Glow", "Honey");
				//if(i % 4 == 0) SetSprite ("Arrow", "Glow", "Purple");
				//if(i % 5 == 0) SetSprite ("Arrow", "Glow", "Green");


			}
			allBulletsSpawned = true;
			break;
		case "Curtain":
			for (int i = 0; i < bulletCount; i++) {
				if (lineDirection == 1)
					newPosition = SpawnInLine (-15, 20, lineDirection, i);
				else
					newPosition = SpawnInLine (2, 20, lineDirection, i);
				movement = new BulletMovementPattern (movement);

				Instantiate (enemyBullet);
				bullet.GetComponent<SpriteRenderer> ().sprite = sprite;

				yield return new WaitForSeconds (coolDown);
				GameController.gameControl.sound.PlaySound ("Enemy", "Shoot", false);
			}
			lineDirection = -lineDirection;
			break;
		case "Spiral":
			if (loopCircles == 0)
				loopCircles = 360;
			Vector3 centerPos = pos;
			allBulletsSpawned = false;
			for (int i = 0; i < bulletCount; i++) {
				newPosition = SpawnInCircle (pos, 1f + (i * 0.1f), GetAng (i, loopCircles));
				Instantiate (enemyBullet);
				yield return new WaitForSeconds (coolDown);
				GameController.gameControl.sound.PlaySound ("Enemy", "Shoot", false);
			}

			allBulletsSpawned = true;
			break;

		case "SingleHoming":
			GameController.gameControl.sound.PlaySound ("Enemy", "Shoot", false);
			Instantiate (enemyBullet);
			break;
		case "PacMan":
			
			for (int i = 0; i < bulletCount; i++) {
				sprite = Resources.Load<Sprite> ("enemyProjectile2");
				newPosition = pos + new Vector3 (0f, 0f, 0f);
				bulletRotation = Quaternion.Euler (0f, 0f, startingRotation + (float)i * rotationMultiplier);
				movement = new BulletMovementPattern (false, null, 0.5f, this, 0, tempMagnitude);
				bullet.GetComponent<SpriteRenderer> ().sprite = sprite;
				startingRotation += 0.1f;
				Instantiate (enemyBullet);
			}
			break;
		case "SpiderWeb":
			bullets = new ArrayList ();
			for (int i = 0; i < bulletCount; i++) {
				
				newPosition = pos + new Vector3 (0f, 0f, 0f);
				bulletRotation = rot;
				animating = false;
				animation = (Resources.Load ("Images/Animations/Animation") as GameObject);
				movement = new BulletMovementPattern (movement);

				Instantiate (enemyBullet);
				bullet.GetComponent<SpriteRenderer> ().sprite = spriteLib.SetBulletSprite ("Circle", "Glow", "Red");
			}
			break;

		case "GiantWeb":
			bullets = new ArrayList ();
			yield return new WaitForSeconds (2f);
			pos = enemy.GetLocalPosition ();
			if (tempMagnitude > 0) {
				float b = bulletCount / 2 + tempMagnitude;
				for (int i = 0; i < Mathf.RoundToInt (b); i++) {
					newPosition = pos + new Vector3 (0f, 0f, 0f);
					bulletRotation = Quaternion.Euler (0f, 0f, i * (360 / b));
					animation = (Resources.Load ("Images/Animations/Animation") as GameObject);
					movement = new BulletMovementPattern (false, "StopAndRotate", 20f, this, tempLayer, tempMagnitude);
					Instantiate (enemyBullet);
					bullet.GetComponent<SpriteRenderer> ().sprite = spriteLib.SetBulletSprite ("Circle", "Big", "Red");
					bullets.Add (enemyBullet);
				}
				tempMagnitude -= 3;
				tempLayer++;
				tempLayer = UpdateLayer (tempLayer);

			} else {
				tempMagnitude = originMagnitude;
			}
			yield return new WaitForSeconds (coolDown);
				
			animating = false;
			break;
		}

	}

	void CheckSoundPlay (int i, int divider)
	{
		if (i % divider == 0)
			GameController.gameControl.sound.PlaySound ("Enemy", "Shoot", true);
	}


	float GetAng (int i, float fullDegrees)
	{
		float ang = i * (fullDegrees / bulletCount); 
		return ang;
	}

	public void Customize (string key, float value)
	{
		switch (key) {
		case "RotationDirection":
			rotationDirection = Mathf.RoundToInt (value);
			break;
		case "LoopCircles":
			loopCircles = value;
			break;
		case "BulletCount":
			bulletCount = Mathf.RoundToInt (value);
			break;
		case "CoolDown":
			coolDown = value;
			break;
		}
	}

	public void Customize (BulletMovementPattern _movement)
	{
		movement = new BulletMovementPattern (_movement);
	}

	Quaternion SpawnInCircle (int i, float startingRotation)
	{
		Quaternion rot = Quaternion.Euler (0f, 0f, (float)(i * (360 / bulletCount)) + startingRotation);
		return rot;
	}

	Vector3 SpawnInLine (float first, float magnitude, int dir, int index)
	{
		Vector3 pos = new Vector3 (first + (dir * (index * (magnitude / bulletCount))), enemy.GetLocalPosition ().y, 0);
		return pos;
	}


	Vector3 SpawnInCircle (Vector3 centerPos, float radius, float ang)
	{
		
		Vector3 position = new Vector3 (0, 0, 0); 
		position.x = centerPos.x + radius * Mathf.Sin (ang * Mathf.Deg2Rad); 
		position.y = centerPos.y - radius * Mathf.Cos (ang * Mathf.Deg2Rad); 
		position.z = centerPos.z;
		bulletRotation = Quaternion.Euler (0f, 0f, ang);
		return position;
	}

	public void InitPattern ()
	{
		tempMagnitude = originMagnitude;
	}

	public void Animate (float targetScale, Vector3 centerPoint)
	{
		if (animation != null && !animating) {
			animating = true;
			animation = (Object.Instantiate (animation, centerPoint, Quaternion.Euler (Vector3.zero)) as GameObject);
			animation.GetComponent<BulletAnimationController> ().SetScale (targetScale);
		}

	}

	public void SetSprite (string shape, string effect, string color)
	{
		sprite = spriteLib.SetBulletSprite (shape, effect, color);
	}

	private int UpdateLayer (int l)
	{
		if (l == layers)
			return 0;
		else
			return l;
	}
}
