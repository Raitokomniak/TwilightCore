using UnityEngine;
using System.Collections;

public class MagneticRange : MonoBehaviour {

	public void Scale(int dir){
		if (dir > 0)
			GetComponent<SpriteRenderer> ().enabled = true;

		else GetComponent<SpriteRenderer>().enabled = false;

		if (dir > 0 && GetComponent<CircleCollider2D> ().radius < 6 || dir < 0 && GetComponent<CircleCollider2D> ().radius > 1)
			GetComponent<CircleCollider2D> ().radius += dir * 0.2f;
	}
}
