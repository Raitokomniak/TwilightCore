using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMaker : MonoBehaviour
{
    float trailSpawnCD = .07f;
	bool canSpawnTrail = true;
    ParticleSystem PS;
    ParticleSystemRenderer PSRenderer;
    
    void Awake(){
        PS = GetComponent<ParticleSystem>();
        PSRenderer = GetComponent<ParticleSystemRenderer>();
        PSRenderer.enabled = false;
    }

	public void MakeTrail(GameObject bullet, Sprite sprite){
        PSRenderer.enabled = true;
        PS.textureSheetAnimation.RemoveSprite(0);
        PS.textureSheetAnimation.AddSprite(sprite);
        PS.Play();
	}
}
