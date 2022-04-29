using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
	public GameObject parentObject;
	public bool rotating;

	void Awake(){
		rotating = false;
	}
	
	void Update () {
		if (rotating) {
			transform.Rotate (0, 0, Time.deltaTime * 200);
		}
	}

	public void Scale(int dir, float max, bool rotateAfter, bool parent)
	{
		if(this.gameObject.activeSelf)
			StartCoroutine (_Scale (dir, max, rotateAfter, parent));
	}

	IEnumerator _Scale(int dir, float max, bool rotateAfter, bool parent){

		if (dir > 0) {
			for (float i = 0.1f; i < max; i += (max/10)) {
				if (parent)
					parentObject.transform.localScale = new Vector3 (i, i, 1);
				else
					transform.localScale = new Vector3 (i, i, 1);

				yield return new WaitForSeconds (0.01f);
			}

		} else if (dir < 0) {
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
