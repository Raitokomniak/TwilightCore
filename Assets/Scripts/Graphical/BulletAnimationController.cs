using UnityEngine;
using System.Collections;

public class BulletAnimationController : MonoBehaviour {

	SpriteRenderer _renderer;
	bool fadedIn;
	float scalingTime = 0.03f;
	public float rotationSpeed;

	float targetScale;
	public float stayTime = 0;
	public bool scaleDown;

	public bool stop;

	
	void Awake(){
		_renderer = GetComponent<SpriteRenderer>();
	}
	void Update(){
		if(fadedIn) transform.Rotate (new Vector3(0,0, -(Time.deltaTime * (10 * rotationSpeed))));
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
			_renderer.color -= new Color (1, 1, 1, 0.1f);
			if(scaleDown) transform.localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
			//yield return new WaitForSeconds (0.03f * scalingTime);
			yield return new WaitForSeconds(scalingTime * Time.deltaTime);
		}

		if(!stop) {
			Destroy (this.gameObject);
		}
	}

	IEnumerator _FadeIn(){
		transform.localScale = new Vector3 (0, 0, 0);
		_renderer.color = new Color (1, 1, 1, 0);
		for (int i = 0; i < (10*targetScale); i++) {
			if(stop) break;
			_renderer.color += new Color (1, 1, 1, 0.1f);
			transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			yield return new WaitForSeconds(scalingTime * Time.deltaTime);
		}
		
		yield return new WaitForSeconds (0.2f);
		fadedIn = true;
		yield return new WaitForSeconds(stayTime);
		FadeAway ();
	}
}
