using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMaker : MonoBehaviour
{

    void Awake(){
        IEnumerator fadeTrail = FadeTrailSprite();
		StartCoroutine(fadeTrail);
    }

    IEnumerator FadeTrailSprite(){
		for(float i = .8f; i >= 0; i-=Time.deltaTime * 100){
			GetComponent<SpriteRenderer>().color = new Color(1,1,1,i);
			yield return new WaitForSeconds(.05f);
		}
		Destroy(this.gameObject);
		yield return null;
	}
	
}
