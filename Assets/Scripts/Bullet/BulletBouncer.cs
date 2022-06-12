using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBouncer : MonoBehaviour
{
    public bool bounced;

    public bool multiply;

    public bool newInstance;

    void Awake()
    {
        GetComponent<Rigidbody2D>().sharedMaterial = Resources.Load("PhysicsMaterials/RainDropBounce") as PhysicsMaterial2D;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public void StopBounce(){
        GetComponent<Rigidbody2D>().sharedMaterial = null;
        GetComponent<BoxCollider2D>().isTrigger = true;

        if(multiply && !newInstance){
            Sprite sprite = GetComponentInParent<EnemyBulletMovement>().BMP.pattern.sprite;
            GameObject newInstance = Instantiate(gameObject, transform.position, Quaternion.identity);
            Vector3 scale = gameObject.transform.localScale;
            gameObject.transform.localScale = new Vector3(scale.x / 1.5f, scale.y / 1.5f, 0);
            newInstance.GetComponent<BulletBouncer>().newInstance = true;

            newInstance.transform.localScale = new Vector3(scale.x / 1.5f, scale.y / 1.5f, 0);
            newInstance.GetComponent<EnemyBulletMovement>().BMP = gameObject.GetComponent<EnemyBulletMovement>().BMP;
            Vector2 velo = gameObject.GetComponent<Rigidbody2D>().velocity;
            newInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(velo.x - 1f, velo.y - 1f); // gameObject.GetComponent<Rigidbody2D>().velocity;
            newInstance.GetComponent<Rigidbody2D>().sharedMaterial = null;
            newInstance.GetComponent<BulletBouncer>().multiply = false;
            newInstance.GetComponent<BoxCollider2D>().isTrigger = true;
            multiply = false;
        }
    }

}
