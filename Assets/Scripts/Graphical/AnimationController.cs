using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
	public GameObject parentObject;
	bool rotating;
	public float rotationSpeed;

	void Awake(){
		rotating = false;
	}
	
	void Update () {
		if (rotating) {
			transform.Rotate (0, 0, Time.deltaTime * 50 * rotationSpeed);
		}
	}

	public void StartRotating(float speed){
		rotationSpeed = speed;
		rotating = true;
	}
	public void StopRotating(){
		rotating = false;
	}

	public void Scale(bool up, float max, bool rotateAfter, bool parent)
	{
		if(this.gameObject.activeSelf)
			StartCoroutine (_Scale (up, max, rotateAfter, parent));
	}

	IEnumerator _Scale(bool up, float max, bool rotateAfter, bool parent){

		if (up) {
			for (float i = 0.1f; i < max; i += (max/10)) {
				if (parent)
					parentObject.transform.localScale = new Vector3 (i, i, 1);
				else
					transform.localScale = new Vector3 (i, i, 1);

				yield return new WaitForSeconds (0.01f);
			}

		} else {
			for (float i = max; i > 0; i -= (max/10)) {
				if (parent)
					parentObject.transform.localScale = new Vector3 (i, i, 1);
				else
					transform.localScale = new Vector3 (i, i, 1);

				yield return new WaitForSeconds (0.01f);
			}
		}
		if (rotateAfter) {
			rotating = true;
		}
	}

}
