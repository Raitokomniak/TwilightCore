using UnityEngine;
using System.Collections;

public class BulletAnimationController : MonoBehaviour {

	SpriteRenderer _renderer;
	bool fadedIn;
	float scalingTime = 0.03f;
	float targetScale;
	public float stayTime = 0;

	void Awake(){
		_renderer = GetComponent<SpriteRenderer>();
	}
	void Update(){
		if(fadedIn) transform.Rotate (new Vector3(0,0, -(Time.deltaTime * 100f)));
	}

	public void SetScale(float scale, float scaleTimeMultiplier)
	{
		targetScale = scale;
		scalingTime = scaleTimeMultiplier;
		fadedIn = false;
		StartCoroutine (_FadeIn());
	}

	public void FadeAway(){
		StartCoroutine (_FadeAway ());
	}

	IEnumerator _FadeAway(){
		
		for (int i = 10; i > 0; i--) {
			_renderer.color = new Color (1, 1, 1, 0.1f * i);
			yield return new WaitForSeconds (0.1f);
		}

		Destroy (this.gameObject);
	}

	IEnumerator _FadeIn(){
		transform.localScale = new Vector3 (0, 0, 0);
		_renderer.color = new Color (1, 1, 1, 0);

		for (int i = 0; i < (10*targetScale); i++) {
			_renderer.color += new Color (1, 1, 1, 0.1f);
			transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			yield return new WaitForSeconds (0.03f * scalingTime);
		}
		yield return new WaitForSeconds (0.2f);
		fadedIn = true;
		yield return new WaitForSeconds (targetScale);
		yield return new WaitForSeconds(stayTime);
		FadeAway ();
	}
}
