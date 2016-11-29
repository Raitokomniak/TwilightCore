using UnityEngine;
using System.Collections;
public class TopLayerParallaxController : MonoBehaviour {

	public float scrollSpeed;
	public float tileSize;

	Vector2 startPosition;
	//Vector3 startPosition;

	public bool moving;

	void Awake () {
		startPosition = transform.position;
	}

	public void Init(){
		//transform.position = startPosition;
		moving = true;
	}

	void Update () {
		if (moving) {
			float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSize);
			transform.position = startPosition + Vector2.down * newPosition;
		}
	}
}
