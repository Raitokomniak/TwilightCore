using UnityEngine;
using System.Collections;

public class SpriteAnimationController : MonoBehaviour {

	SpriteRenderer _renderer;
	bool fadedIn;
	float scalingTime = 0.03f;
	public float rotationSpeed;

	float targetScale;
	public float stayTime = 0;
	public bool scaleDown;

    public bool rotating;

    public bool moving;
    public float movementSpeed = 5;
    public bool randomXDirOnAwake;
    float randomXDir;
    public bool fadeAwayOnAwake;

	public bool stop;

	public bool dontDestroy;
	
	void Awake(){
		_renderer = GetComponent<SpriteRenderer>();
        if(fadeAwayOnAwake) {
            targetScale = transform.localScale.x;
            scalingTime = 15;
            FadeAway();
        }
        if(randomXDirOnAwake) randomXDir = Random.Range(-5f, 5f);
	}
	void Update(){/*
		if(rotating) transform.Rotate (new Vector3(0,0, -(Time.deltaTime * (10 * rotationSpeed))));
        if(moving) {
            
            transform.position += new Vector3(Time.deltaTime * randomXDir, Time.deltaTime * movementSpeed, 0);
        }*/
	}

	public void SetScale(float scale, float scaleTimeMultiplier)
	{
		targetScale = scale;
		scalingTime = scaleTimeMultiplier;
		fadedIn = false;
		IEnumerator fadeIn = _FadeIn();
		StartCoroutine (fadeIn);
	}

	public void FadeAway(){
		IEnumerator fadeAway = _FadeAway();
		StartCoroutine (fadeAway);
	}

	IEnumerator _FadeAway(){
		for (int i = 0; i < (10*targetScale); i++) {
			if(stop) break;
			_renderer.color -= new Color (0, 0, 0, 0.1f);
			if(scaleDown) transform.localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
			//yield return new WaitForSeconds (0.03f * scalingTime);
			yield return new WaitForSeconds(scalingTime * Time.deltaTime);
		}

		if(!dontDestroy) {
			Destroy (this.gameObject);
		}
	}

	IEnumerator _FadeIn(){
		transform.localScale = new Vector3 (0, 0, 0);
		_renderer.color = new Color (1, 1, 1, 0);
		for (int i = 0; i < (10*targetScale); i++) {
			if(stop) break;
			//_renderer.color += new Color (1, 1, 1, 0.1f);
			//transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			//yield return new WaitForSeconds (0.03f * scalingTime);
			_renderer.color += new Color (1, 1, 1, 0.1f);
			transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			yield return new WaitForSeconds(scalingTime * Time.deltaTime);
		}
		
		yield return new WaitForSeconds (0.2f);
		fadedIn = true;
        rotating = true;
		yield return new WaitForSeconds(stayTime);
		FadeAway ();
	}
}
