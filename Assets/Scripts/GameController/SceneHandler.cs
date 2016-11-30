using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

	public GameObject environment;
	public GameObject parent;
	public Camera environmentCamera;
	public ArrayList planes;

	public bool init;

	/////////////////////
	/// SCENE TRANSITIONING
	/// 
	public void MainMenu ()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	public void CharSelect ()
	{
		SceneManager.LoadScene ("CharSelect");
	}

	public void StartGame ()
	{
		SceneManager.LoadSceneAsync ("Level1");
		GameController.gameControl.sound.StopMusic ();
	}

	public string GetCurrentScene ()
	{
		return SceneManager.GetActiveScene ().name;
	}

	public void CheckScene ()
	{
		if (SceneManager.GetActiveScene ().name == "MainMenu") {
			GameController.gameControl.ui.InitMainMenu ();
			GameController.gameControl.stage.Player.SetActive (false);
			GameController.gameControl.sound.PlayMusic ("MainMenu");
			GameController.gameControl.menu.ToggleMenu (true);
		} else if (SceneManager.GetActiveScene ().name == "CharSelect") {
			GameController.gameControl.ui.InitCharSelect ();
			GameController.gameControl.stage.Player.SetActive (false);
			GameController.gameControl.menu.ToggleMenu (true);
		} else {
			if (!init) {
				GameController.gameControl.stage.InitStage (true);
				SetUpEnvironment ();
				init = true;
				GameController.gameControl.menu.ToggleMenu (false);
			}


		}
			
	}

	void OnLevelWasLoaded ()
	{
		CheckScene ();
	}

	public void DeConstructScene ()
	{
		
	}


	/////////////////////
	/// ENVIRONMENT SETUP AND DELEGATION
	/// 
	void SetUpEnvironment ()
	{
		if (planes != null) {
			planes.Clear ();
		}
		planes = new ArrayList ();
		environment = GameObject.Find ("Environment");
		environment.GetComponent<EnvironmentController> ().SetStartPosition (new Vector3 (40.5f, -13, 88.1f));

		GameObject environmentClone = null;
		GameObject environmentClone2 = null;
		if (!GameObject.Find ("Environment2")) {
			environmentClone = (GameObject)Instantiate (environment, environment.transform.position + new Vector3 (0, 0, 47.9f), environment.transform.rotation);
			environmentClone.transform.SetParent (environment.transform.parent);
			environmentClone.name = "Environment2";
			environmentClone.GetComponent<EnvironmentController> ().SetStartPosition (environment.transform.position + new Vector3 (0, 0, 47.9f));
		}
		if (!GameObject.Find ("Environment3")) {
			environmentClone2 = (GameObject)Instantiate (environment, environment.transform.position + new Vector3 (0, 0, 95.8f), environment.transform.rotation);
			environmentClone2.transform.SetParent (environment.transform.parent);
			environmentClone2.name = "Environment3";
			environmentClone2.GetComponent<EnvironmentController> ().SetStartPosition (environment.transform.position + new Vector3 (0, 0, 95.8f));
		}
		planes.Add (environment);
		planes.Add (environmentClone);
		planes.Add (environmentClone2);

		environment = (GameObject)planes [0];
		environmentClone = (GameObject)planes [1];
		environmentClone2 = (GameObject)planes [2];
	}

	public void SetPlaneSpeed (float speed)
	{
		planes [0] = GameObject.Find ("Environment");
		planes [1] = GameObject.Find ("Environment2");
		planes [2] = GameObject.Find ("Environment3");
		foreach (GameObject plane in planes) {
			//if (!plane.GetComponent<EnvironmentController> ().init)
			//	plane.GetComponent<EnvironmentController> ().Init ();
			plane.GetComponent<EnvironmentController> ().SetScrollSpeed (speed);
		}
	}

	public void RotateCamera (float x, float y, float z)
	{
		Quaternion newRotation = Quaternion.Euler (x, y, z);
		environmentCamera = GameObject.Find ("EnvironmentCamera").GetComponent<Camera> ();
		environmentCamera.GetComponent<CameraController> ().Rotate (newRotation);
	}

	public void MoveCamera (float x, float y, float z)
	{
		Vector3 newPosition = new Vector3 (x, y, z);
		environmentCamera = GameObject.Find ("EnvironmentCamera").GetComponent<Camera> ();
		environmentCamera.GetComponent<CameraController> ().Move (newPosition);
	}





}
