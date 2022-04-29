using UnityEngine;
using System.Collections;

public class BulletAnimationController : MonoBehaviour {

	bool fadedIn;
	float targetScale;

	void Update(){
		if(fadedIn) transform.Rotate (new Vector3(0,0, -(Time.deltaTime * 100f)));
	}

	public void SetScale(float scale)
	{
		targetScale = scale;
		fadedIn = false;
		StartCoroutine (_FadeIn());
	}

	public void FadeAway(){
		StartCoroutine (_FadeAway ());
	}

	IEnumerator _FadeAway(){
		
		for (int i = 10; i > 0; i--) {
			GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0.1f * i);
			yield return new WaitForSeconds (0.1f);
		}

		Destroy (this.gameObject);
	}

	IEnumerator _FadeIn(){
		transform.localScale = new Vector3 (0, 0, 0);
		GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);

		for (int i = 0; i < (10*targetScale); i++) {
		//while (transform.localScale.sqrMagnitude < new Vector3(targetScale, targetScale, targetScale).sqrMagnitude) {
			GetComponent<SpriteRenderer> ().color += new Color (1, 1, 1, 0.1f);
			transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			yield return new WaitForSeconds (0.03f);
		//}
		}
		yield return new WaitForSeconds (0.2f);
		fadedIn = true;
		yield return new WaitForSeconds (targetScale);
		FadeAway ();
	}
}
