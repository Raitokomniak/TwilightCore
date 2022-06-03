using UnityEngine;
using System.Collections;
public class TopLayerParallaxController : MonoBehaviour {

	public float scrollSpeed;
	float tileSize = 34.2f;
	Vector2 startPosition;
	public bool moving;
	
	public void Init(){
		startPosition = transform.position;
		moving = true;
	}

	void Update () {
		if (moving) {
			float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSize);
			transform.position = startPosition + Vector2.down * newPosition;
		}
	}
}
