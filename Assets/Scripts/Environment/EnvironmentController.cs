using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {
	
	public float scrollSpeed;
	float initialSpeed;
	float offSet;
	public float targetSpeed;
	public float tileSize;

	Vector3 startPosition;

	public bool moving;
	public bool updatingSpeed;

	void Update () {
		moving = !Game.control.pause.paused;
		if (moving) {
			scrollSpeed += (targetSpeed - scrollSpeed) * 0.05f;
			transform.position -= new Vector3(0,0, scrollSpeed * Time.deltaTime);

			if (startPosition.z - transform.position.z >= tileSize) {
				transform.position = startPosition;
			}
		}
	}

	public void SetStartPosition(Vector3 startPos){
		startPosition = startPos;
		transform.position = startPosition;
	}

	public void SetScrollSpeed(float speed){
		initialSpeed = scrollSpeed;
		targetSpeed = speed;
		if (targetSpeed > scrollSpeed) {
			offSet = targetSpeed - scrollSpeed;
		} else {
			offSet = scrollSpeed - targetSpeed;
		}
	}

}
