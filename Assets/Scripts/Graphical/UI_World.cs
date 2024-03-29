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

	public GameObject pickUpThreshold;

	public GameObject FXLayer;

	void Awake(){
		this.gameObject.SetActive(true);
		HideFXLayer();
	}

    public float[] GetBoundaries(){
        if(playAreaBottomWall == null || playAreaLeftWall == null || playAreaTopWall == null || playAreaRightWall == null) return null;

		wallBoundaries = new float[4]{playAreaBottomWall.transform.position.y, playAreaLeftWall.transform.position.x, playAreaTopWall.transform.position.y, playAreaRightWall.transform.position.x};
		return wallBoundaries;
	}

	public void TogglePickUpThreshold(bool toggle){
		if(toggle && !pickUpThreshold.activeSelf) 		pickUpThreshold.SetActive(true);
		else if(!toggle && pickUpThreshold.activeSelf)	pickUpThreshold.SetActive(false);
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

	public void ShowFXLayer(string color){
		FXLayer.SetActive(true);
		if(color == "Night") FXLayer.GetComponent<SpriteRenderer>().color = new Color(0.05f, 0.05f, 0.16f, 0.3f);
		if(color == "Light") FXLayer.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.6f, 0.3f);
	}

	public void HideFXLayer(){
		FXLayer.SetActive(false);
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
