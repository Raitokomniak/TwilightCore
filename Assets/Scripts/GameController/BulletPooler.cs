using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
   // Transform[] bulletsInPool;
    GameObject bulletPrefab;
    List<GameObject> bulletsInPool;
	public Transform bulletPool;
    GameObject fetchedBullet;
    public Transform bulletContainer;

    public bool done;

    void Awake(){
        bulletsInPool = new List<GameObject>();
    }


    // AT START OF STAGE/GAME, INSTANTIATE BULLETS TO POOL 
	public void InstantiateBulletsToPool (int amount)
	{
        bulletPrefab = Resources.Load("Prefabs/enemyBullet") as GameObject;
        bulletPool = GameObject.Find("BulletPool").transform;
        bulletContainer = GameObject.Find("Bullets").transform;
        done = false;
        IEnumerator routine = InstantiateRoutine(amount);
        StartCoroutine(routine);
	}

    IEnumerator InstantiateRoutine(int amount){
        GameObject instantiatedBullet = null;

		for(int i = 0; i < amount; i++){
			instantiatedBullet = Object.Instantiate (bulletPrefab) as GameObject;
			StoreBulletToPool(instantiatedBullet);
		}
        done = true;
        yield return null;
    }

	public void StoreBulletToPool(GameObject bullet){
        StoreBulletToPool(bullet.GetComponent<BulletMovement>());
	}

    public void StoreBulletToPool(BulletMovement bullet){
		bullet.Pool();
		bullet.transform.position = bulletPool.position;
        bullet.transform.SetParent(bulletPool);
        bulletsInPool.Add(bullet.gameObject);
        bullet.gameObject.SetActive(false);
	}

	public GameObject FetchBulletFromPool(){
        if(bulletsInPool.Count < 10) InstantiateBulletsToPool(50);
		fetchedBullet = bulletsInPool[bulletsInPool.Count - 1];
		bulletsInPool.RemoveAt(bulletsInPool.Count - 1);
        if(fetchedBullet == null) return null;
        if(bulletContainer == null) return null;
        fetchedBullet.transform.SetParent(bulletContainer);
        fetchedBullet.SetActive(true);
		return fetchedBullet;
	}
}
