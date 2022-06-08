using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_World : MonoBehaviour
{
    public float[] wallBoundaries;
	public GameObject playAreaLeftWall;
	public GameObject playAreaRightWall;
	public GameObject playAreaTopWall;
	public GameObject playAreaBottomWall;
    public GameObject[] topLayers;

	public Transform pickUpThreshold;


	void Awake(){
		this.gameObject.SetActive(true);
	}
    public void GetWalls(){
        //playAreaLeftWall = transform.GetChild (2).GetChild (0).gameObject;
		//playAreaRightWall = transform.GetChild (2).GetChild (1).gameObject;
		//playAreaTopWall = transform.GetChild (2).GetChild (2).gameObject;
		//playAreaBottomWall = transform.GetChild (2).GetChild (3).gameObject;
    }

    public float[] GetBoundaries(){
		wallBoundaries = new float[4]{playAreaBottomWall.transform.position.y, playAreaLeftWall.transform.position.x, playAreaTopWall.transform.position.y, playAreaRightWall.transform.position.x};
		return wallBoundaries;
	}

    public void InitParallaxes(){
        foreach (GameObject parallax in topLayers) {
			parallax.GetComponent<TopLayerParallaxController> ().Init ();
		}
    }

    public void ResetTopLayer(){
		foreach (GameObject layer in topLayers) {
			Sprite sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/Stage"+ Game.control.stageHandler.currentStage);
			layer.GetComponent<Image> ().sprite = sprite;
			layer.GetComponent<TopLayerParallaxController> ().scrollSpeed = 26;
		}
	}

	public void SetTopLayerSpeed(float speed){
		foreach (GameObject layer in topLayers) {
			layer.GetComponent<TopLayerParallaxController> ().scrollSpeed = speed;
		}
	}

	public void UpdateTopPlayer(string phase){
		StartCoroutine (_UpdateTopLayer (phase));
	}

	public void UpdateTopPlayer(float speed)
	{
		foreach (GameObject layer in topLayers) {
			layer.GetComponent<TopLayerParallaxController> ().scrollSpeed = speed;
		}
	}

	public IEnumerator _UpdateTopLayer(string type)
	{
//		Debug.Log ("update " + type);
		Sprite sprite;
		GameObject layer1 = topLayers [0];
		GameObject layer2 = topLayers [1];

		for (float i = 2; i >= 0; i -= 0.1f) {
			layer1.GetComponent<Image> ().color = new Color (1, 1, 1, i);
			layer2.GetComponent<Image> ().color = new Color (1, 1, 1, i);
			yield return new WaitForSeconds (0.05f);
		}
		sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/" + type);
		foreach (GameObject layer in topLayers) {
			layer.GetComponent<Image> ().sprite = sprite;
			//layer.GetComponent<TopLayerParallaxController> ().scrollSpeed = 5f;
		}

		layer1.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		layer2.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		for (float i = 0; i < 2; i += 0.1f) {
			layer1.GetComponent<Image> ().color = new Color (1, 1, 1, i);
			layer2.GetComponent<Image> ().color = new Color (1, 1, 1, i);
			yield return new WaitForSeconds (0.1f);
		}
	}
}
