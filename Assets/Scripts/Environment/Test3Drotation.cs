using UnityEngine;
using System.Collections;

public class Test3Drotation : MonoBehaviour {

	float speed;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		speed = 30f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = transform.rotation * Quaternion.Euler (Time.deltaTime * speed, Time.deltaTime * speed, 0);
	}


}
