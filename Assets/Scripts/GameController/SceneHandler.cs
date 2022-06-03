using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
	public GameObject environment;
	public GameObject directionalLight;
	public GameObject parent;
	public GameObject[] planes;
	public CameraController e_camera;

	public void SetUpEnvironment ()
	{
		planes = new GameObject[4];
		parent = GameObject.Find ("EnvironmentParent");
		directionalLight = (GameObject)Instantiate (Resources.Load ("Prefabs/Environment/Stage" + Game.control.stageHandler.currentStage + "_DirectionalLight"));
		directionalLight.transform.SetParent(parent.transform);

		if (!GameObject.Find ("Environment")) {
			environment = (GameObject)Instantiate (Resources.Load ("Prefabs/Environment/Stage" + Game.control.stageHandler.currentStage + "_Environment"));
			environment.transform.SetParent (parent.transform);
			environment.GetComponent<EnvironmentController> ().SetStartPosition (new Vector3 (85.5f, -13, 88.1f));
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
		if (!GameObject.Find ("Environment4")) {
			GameObject environmentClone3 = (GameObject)Instantiate (environment, environment.transform.position + new Vector3 (0, 0, 143.7f), environment.transform.rotation);
			environmentClone3.transform.SetParent (environment.transform.parent);
			environmentClone3.name = "Environment4";
			environmentClone3.GetComponent<EnvironmentController> ().SetStartPosition (environment.transform.position + new Vector3 (0, 0, 143.7f));
			environmentClone3 = GameObject.Find ("Environment");
		}
		environment = GameObject.Find ("Environment");

		planes [0] = GameObject.Find ("Environment");
		planes [1] = GameObject.Find ("Environment2");
		planes [2] = GameObject.Find ("Environment3");
		planes [3] = GameObject.Find ("Environment4");

		e_camera = GameObject.Find ("EnvironmentCamera").GetComponent<CameraController>();
	}

	public void SetPlaneSpeed (float speed)
	{
		foreach (GameObject plane in planes) {
			plane.GetComponent<EnvironmentController> ().SetScrollSpeed (speed);
		}
	}
}
