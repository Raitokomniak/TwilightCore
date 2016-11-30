using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	bool rotating;
	Quaternion targetRotation;
	Vector3 targetPosition;
	bool moving;

	void Awake(){
		transform.position = new Vector3 (50, 5, 72);
		transform.rotation = Quaternion.Euler (40, 0, 5);
	}

	// Update is called once per frame
	void Update () {
		if (rotating) {
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
		}
		if(moving)
			transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
	}

	public void Rotate(Quaternion targetRot)
	{
		targetRotation = targetRot;
		rotating = true;
	}

	public void Move(Vector3 targetPos)
	{
		targetPosition = targetPos;
		moving = true;
	}
}
