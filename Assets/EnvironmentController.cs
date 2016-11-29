﻿using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {
	
	public float scrollSpeed;
	float initialSpeed;
	public float targetSpeed;
	public float tileSize;

	Vector3 startPosition;

	public bool moving;
	public bool init;
	public bool updatingSpeed;


	void Awake () {
		init = false;
		startPosition = transform.position;
		//startPosition = transform.position;
		//moving = true;
	}

	void Update () {
		moving = !GameController.gameControl.pause.paused;
		if (moving) {
			scrollSpeed += (targetSpeed - scrollSpeed) * 0.01f;

			float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSize);
			transform.position = startPosition + Vector3.back * newPosition;
		}

	}

	public void SetScrollSpeed(float speed){
		initialSpeed = scrollSpeed;
		targetSpeed = speed;
	}

}
