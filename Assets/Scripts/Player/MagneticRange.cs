using UnityEngine;
using System.Collections;

public class MagneticRange : MonoBehaviour {
	public GameObject Player;
	float radius;


	// Use this for initialization
	void Awake () {
		radius = GetComponent<CircleCollider2D> ().radius;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Player.transform.position;
	}

	public void Scale(int dir){
		if (dir > 0) {
			GetComponent<SpriteRenderer> ().enabled = true;
		}
		else GetComponent<SpriteRenderer>().enabled = false;

		if (dir > 0 && GetComponent<CircleCollider2D> ().radius < 6 || dir < 0 && GetComponent<CircleCollider2D> ().radius > 1) {

			GetComponent<CircleCollider2D> ().radius += dir * 0.2f;
		}
	}
}
