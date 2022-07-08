using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletSprite {
    public string shape;
    public string effect;
    public string colorS;
    public string size;
    public Sprite sprite;
    public Color color;

    public BulletSprite(string _shape, string _effect, string _colorS, string _size){
        shape = _shape;
        effect = _effect;
        colorS = _colorS;
        size = _size;
    }
}
public class Pattern
{
    BulletSprite bSprite;
    public string patternName;
	public VectorLib lib;
	public GameObject bullet;
	public ArrayList spawnedBullets;
	public EnemyShoot enemyShoot;
	public Sprite sprite;
	public Sprite glowSprite;

    public SpriteRenderer glowRenderer;

	public Vector3 spawnPosition;
	public Quaternion bulletRotation;
	public BulletMovementPattern BMP;
	public float bulletSize = 1;

	public IEnumerator routine;
	//public GameObject enemyBullet;

	public bool startsHoming;
	public float delayBeforeAttack = 0; //DEFAULT
	public float coolDown = 1; //DEFAULT
	public int bulletCount = 1; //DEFAULT
	public float rotationMultiplier = 0;  //DEFAULT
	public float startingRotation = 0;

	public float maelStromRotationMultiplier = 0.5f;

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
        /*if(spawnedBullets == null) return;
        foreach(GameObject b in spawnedBullets){
            if(b != null && BMP != null) b.GetComponent<BulletMovement>().StopCoroutine(BMP.ExecuteRoutine());
        }*/
	}
	
	/// 
	/*
	public void SpawnBullet (GameObject enemyBullet, BulletMovementPattern _bulletMovement)
	{
		if(bulletMovement == null) bulletMovement = new BMP_Explode(this, 5f); //DEFAULT
		bulletMovement = bulletMovement.GetNewBulletMovement(bulletMovement);
		
		//bullet = (Object.Instantiate (enemyBullet, spawnPosition, bulletRotation) as GameObject);   // GET BULLET FROM POOL
		
		bullet.transform.SetParent (GameObject.FindWithTag ("BulletsRepo").transform);
		
		spawnedBullets.Add(bullet);
		bullet.GetComponent<BulletMovement> ().SetUpBulletMovement (bulletMovement, enemyShoot);
		bullet.transform.GetChild(0).GetComponent<SpriteRenderer> ().sprite = sprite;
		bullet.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = glowSprite;
		
		bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
		if(enemyShoot.bulletsShot != null) enemyShoot.bulletsShot.Add (bullet);
	}*/

	public GameObject SpawnBullet ()
	{
		if(BMP == null) BMP = new BMP_Explode(this, 5f); //DEFAULT
		BMP = BMP.GetNewBulletMovement(BMP);
		bullet = Game.control.bulletPool.FetchBulletFromPool();
        if(bullet == null) {
            bullet = GameObject.Instantiate(Resources.Load("Prefabs/enemyBullet"), spawnPosition, Quaternion.identity) as GameObject;
            bullet.transform.SetParent(Game.control.bulletPool.bulletContainer);
        }
        //bullet = GameObject.Instantiate(Resources.Load("Prefabs/enemyBullet"), spawnPosition, Quaternion.identity) as GameObject;
       // BMP.bullet = bullet;
        BMP.ReceiveBulletData(bullet);

		if(bullet != null){
			bullet.transform.position = spawnPosition;
			bullet.transform.rotation = bulletRotation;
			bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);

			spawnedBullets.Add(bullet);
			bullet.transform.GetChild(0).GetComponent<SpriteRenderer> ().sprite = sprite;
			bullet.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = glowSprite;
            if(bSprite != null) bullet.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Game.control.spriteLib.GetColor(bSprite.colorS);
            bullet.GetComponent<BulletMovement>().enabled = true;
			bullet.GetComponent<BulletMovement>().Init(BMP, enemyShoot);
		
            return bullet;
		}

        return null;
	}

	public IEnumerator Execute(EnemyShoot _enemy){
		spawnedBullets = new ArrayList();
		enemyShoot = _enemy;
		pos = _enemy.transform.position;
		rot = _enemy.transform.rotation;

		spawnPosition = _enemy.transform.position;
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
			animation.SetActive(true);
			animation.GetComponent<SpriteAnimationController> ().SetScale (targetScale, scaleTime);
		}
	}

	public void SetSprite (string shape, string effect, string color, string size)
	{
        bSprite = new BulletSprite(shape, effect, color, size);
		sprite = Game.control.spriteLib.SetBulletSprite (shape, effect, color);
        glowSprite = Game.control.spriteLib.SetBulletGlow (shape);

		SetSize(size);
	}

	void SetSize(string size){
		if(size == "Tiny")    bulletSize = 1;
		if(size == "Small")   bulletSize = 2;
		if(size == "Medium")  bulletSize = 2.5f;
		if(size == "Big")     bulletSize = 3.5f;
		if(size == "Huge")    bulletSize = 5;
	}

	public void SetSprite (string shape, string size)
	{
        bSprite = new BulletSprite(shape, "", "", size);
		sprite = Game.control.spriteLib.SetBulletSprite (shape, "", "");
        glowSprite = null;
		SetSize(size);
	}

}