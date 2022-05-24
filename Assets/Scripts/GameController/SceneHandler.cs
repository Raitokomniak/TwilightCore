using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
	public GameObject environment;
	public GameObject directionalLight;
	public GameObject parent;
	//public Camera environmentCamera;
	public GameObject[] planes;
	public CameraController camera;

	public void SetUpEnvironment ()
	{
		planes = new GameObject[3];
		parent = GameObject.Find ("EnvironmentParent");
		directionalLight = (GameObject)Instantiate (Resources.Load ("Prefabs/Environment/Stage" + Game.control.stageHandler.currentStage + "_DirectionalLight"));
		directionalLight.transform.SetParent(parent.transform);

		if (!GameObject.Find ("Environment")) {
			environment = (GameObject)Instantiate (Resources.Load ("Prefabs/Environment/Stage" + Game.control.stageHandler.currentStage + "_Environment"));
			environment.transform.SetParent (parent.transform);
			environment.GetComponent<EnvironmentController> ().SetStartPosition (new Vector3 (40.5f, -13, 88.1f));
			environment.name = "Environment";
		}

		if (!GameObject.Find ("Environment2")) {
			GameObject environmentClone = (GameObject)Instantiate (environment, environment.transform.position + new Vector3 (0, 0, 47.9f), environment.transform.rotation);
			environmentClone.transform.SetParent (environment.transform.parent);
			environmentClone.name = "Environment2";
			environmentClone.GetComponent<EnvironmentController> ().SetStartPosition (environment.transform.position + new Vector3 (0, 0, 47.9f));
			environmentClone = GameObject.Find ("Environment2");
		}
		if (!GameObject.Find ("Environment3")) {
			GameObject environmentClone2 = (GameObject)Instantiate (environment, environment.transform.position + new Vector3 (0, 0, 95.8f), environment.transform.rotation);
			environmentClone2.transform.SetParent (environment.transform.parent);
			environmentClone2.name = "Environment3";
			environmentClone2.GetComponent<EnvironmentController> ().SetStartPosition (environment.transform.position + new Vector3 (0, 0, 95.8f));
			environmentClone2 = GameObject.Find ("Environment");
		}
		environment = GameObject.Find ("Environment");

		planes [0] = GameObject.Find ("Environment");
		planes [1] = GameObject.Find ("Environment2");
		planes [2] = GameObject.Find ("Environment3");

		camera = GameObject.Find ("EnvironmentCamera").GetComponent<CameraController>();
	}

	public void SetPlaneSpeed (float speed)
	{
		foreach (GameObject plane in planes) {
			plane.GetComponent<EnvironmentController> ().SetScrollSpeed (speed);
		}
	}

/*
	public void RotateCamera (float x, float y, float z)
	{
		Quaternion newRotation = Quaternion.Euler (x, y, z);
		//environmentCamera = GameObject.Find ("EnvironmentCamera").GetComponent<Camera> ();
		environmentCamera.Rotate (newRotation);
	}

	public void MoveCamera (float x, float y, float z)
	{
		Vector3 newPosition = new Vector3 (x, y, z);
		//environmentCamera = GameObject.Find ("EnvironmentCamera").GetComponent<Camera> ();
		environmentCamera.Move (newPosition);
	}*/
}
