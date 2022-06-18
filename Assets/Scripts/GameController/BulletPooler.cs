using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooler : MonoBehaviour
{
    List<GameObject> bulletsInPool;
	Transform bulletPool;

    public bool done;

    void Awake(){
        bulletsInPool = new List<GameObject>();
    }


    // AT START OF STAGE/GAME, INSTANTIATE BULLETS TO POOL 
	public void InstantiateBulletsToPool (int difficultyMultiplier)
	{
        IEnumerator routine = InstantiateRoutine(600);
		StartCoroutine(routine);
	}

	public void InstantiateMoreBullets(int amount){
		IEnumerator routine = InstantiateRoutine(amount);
		StartCoroutine(routine);
	}

	IEnumerator InstantiateRoutine(int amount){
		GameObject bulletPrefab = Resources.Load("Prefabs/enemyBullet") as GameObject;
		bulletPool = GameObject.Find("BulletPool").transform;
        GameObject instantiatedBullet = null;

		for(int i = 0; i < amount; i++){
			instantiatedBullet = (Object.Instantiate (bulletPrefab) as GameObject);
			instantiatedBullet.transform.SetParent (bulletPool);
			StoreBulletToPool(instantiatedBullet);
		}
		yield return null;
	}

	public void DestroyAll(){
		foreach(GameObject bullet in bulletsInPool){
			Destroy(bullet);
		}
	}

	public void StoreBulletToPool(GameObject bullet){
		bullet.GetComponent<BulletMovement>().Pool();
		bullet.transform.position = bulletPool.position;
        bulletsInPool.Add(bullet);
	}

	public GameObject FetchBulletFromPool(){
		/*if(bulletsInPool.Count <= 0) {
			Debug.Log("no more bullets, creating more");
			InstantiateMoreBullets(50);
			return null;
		}*/

		GameObject fetchedBullet = bulletsInPool[bulletsInPool.Count - 1];
		bulletsInPool.RemoveAt(bulletsInPool.Count - 1);
		return fetchedBullet;
	}
}
