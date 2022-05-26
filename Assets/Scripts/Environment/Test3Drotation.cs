using UnityEngine;
using System.Collections;

public class Test3Drotation : MonoBehaviour {
	float speed = 30f;

	void Update () {
		transform.rotation = transform.rotation * Quaternion.Euler (Time.deltaTime * speed, Time.deltaTime * speed, 0);
	}
}
