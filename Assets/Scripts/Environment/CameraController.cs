using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	bool rotating;
	Quaternion targetRotation;
	Vector3 targetPosition;
	bool moving;
	Vector3 defaultPosition;
	Quaternion defaultRotation;

	void Awake(){
		defaultPosition = new Vector3 (100, 65, 72);
		defaultRotation = Quaternion.Euler (65, 0, 5);
		transform.position = defaultPosition;
		transform.rotation = defaultRotation;
	}

	void Update () {
		if (rotating)
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
			
		if(moving)
			transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime);
	}

	public void Rotate(Vector3 targetRot)
	{
		targetRotation = Quaternion.Euler (targetRot.x, targetRot.y, targetRot.z);
		rotating = true;
	}

	public void Move(Vector3 targetPos)
	{
		targetPosition = new Vector3(defaultPosition.x, targetPos.y, targetPos.z);
		moving = true;
	}

	public void MoveToDefault(){
		targetPosition = defaultPosition;
		moving = true;
	}

	public void RotateToDefault(){
		targetRotation = defaultRotation;
		rotating = true;
	}

	public void SetPosition(Vector3 targetPos){
		transform.position = targetPos;
	}

	public void SetRotation(Vector3 targetRot){
		transform.rotation = Quaternion.Euler (targetRot.x, targetRot.y, targetRot.z);
	}

	public void SetPositionToDefault(){
		transform.position = defaultPosition;
	}

	public void SetRotationToDefault(){
		transform.rotation = defaultRotation;
	}
}
