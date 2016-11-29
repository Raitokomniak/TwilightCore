using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	bool rotating;
	Quaternion targetRotation;
	Vector3 targetPosition;
	bool moving;

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
