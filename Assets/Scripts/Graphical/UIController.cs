using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIController : MonoBehaviour {
	public GameObject stageWorldUI;

	public UI_RightSidePanel RIGHT_SIDE_PANEL;
	public UI_LeftSidePanel LEFT_SIDE_PANEL;
	public UI_Dialog DIALOG;
	public UI_Boss BOSS;


	public float[] wallBoundaries;
	public GameObject playAreaLeftWall;
	public GameObject playAreaRightWall;
	public GameObject playAreaTopWall;
	public GameObject playAreaBottomWall;

	public GameObject[] topLayers;
	public GameObject[] bgs;



	//Boss


	//Level
	public GameObject stageEndPanel;
	public TextMeshProUGUI stageText;
	public TextMeshProUGUI rightPanelStageText;
	public GameObject stagePanel;
	public TextMeshProUGUI toast;


	//Special
	public GameObject playerSpecialPanel;
	public TextMeshProUGUI playerSpecialText;


	//Screens
	public GameObject gameOver;
	public GameObject pauseScreen;
	public GameObject pauseMenuPanel;
	public GameObject loadingScreen;
	public GameObject optionsScreen;
	public GameObject saveScoreScreen;


	//Dialog

	//OPTIONS
    public GameObject optionsContainer;
	public GameObject optionsValueContainer;

	//GAMEOVER
	public GameObject saveScorePrompt;
	public GameObject saveScoreContainer;
	public GameObject gameOverOptionsContainer;
	
	public Transform gameOverImage;
	public Transform gameCompleteImage;

	public TMP_InputField scoreSaveNameInput;

	public TextMeshProUGUI scoreInfo;

	void Awake(){
		ToggleLoadingScreen(false);
		ToggleOptionsScreen(false);
	}

	void Update(){
		if(scoreSaveNameInput.gameObject.activeSelf){
			if(Input.GetKeyDown(KeyCode.Return)){
				//DONT DO LIKE THIS, THIS IS JUST A TEMP SOLUTION
				if(gameOverImage.gameObject.activeSelf) Game.control.menu.Menu("GameOverMenu");
				else if(gameCompleteImage.gameObject.activeSelf) Game.control.MainMenu();
				Game.control.io.SaveScore(scoreSaveNameInput.text, Game.control.stageHandler.stats.score, Game.control.stageHandler.difficultyAsString);
			}
		}

	}

	public void ToggleSaveScorePrompt(){
	}

	public void UpdateMenuSelection(string context, int index){
		TextMeshProUGUI[] allSelections = null;
		TextMeshProUGUI[] optionsValues = null;

		if (context == "PauseMenu") {
			allSelections = pauseMenuPanel.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		else if(context == "OptionsMenu"){
			allSelections = optionsContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
			optionsValues = optionsValueContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		else if(context == "GameOverMenu"){
			allSelections = gameOverOptionsContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		else if(context == "SaveScorePrompt"){
			allSelections = saveScoreContainer.transform.GetComponentsInChildren<TextMeshProUGUI> ();
		}
		
		foreach (TextMeshProUGUI text in allSelections) {
			text.fontStyle = TMPro.FontStyles.Normal;
		}
		if(context == "OptionsMenu"){
			foreach (TextMeshProUGUI text in optionsValues) {
				text.fontStyle = TMPro.FontStyles.Normal;
			}
		}
		allSelections[index].fontStyle = TMPro.FontStyles.Bold;
		if(context == "OptionsMenu") optionsValues[index].fontStyle = TMPro.FontStyles.Bold;
	}

	public void UpdateOptionSelection(int index, string text){
		TextMeshProUGUI option = optionsValueContainer.transform.GetChild(index).GetComponent<TextMeshProUGUI>();
		option.text = text;
	}
	

	public void InitStage(){
		stageWorldUI.SetActive (true);

		playAreaLeftWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (0).gameObject;
		playAreaRightWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (1).gameObject;
		playAreaTopWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (2).gameObject;
		playAreaBottomWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (3).gameObject;
		LEFT_SIDE_PANEL.UpdateCoreCharge ("Day", 0);
		LEFT_SIDE_PANEL.UpdateCoreCharge ("Night", 0);

		BOSS.bossHealthSlider.gameObject.SetActive(false);
		BOSS.bossTimer.gameObject.SetActive(false);
		BOSS.bossNamePanel.SetActive (false);
		stageEndPanel.SetActive(false);
		saveScoreScreen.SetActive(false);
		gameOver.SetActive(false);
		PauseScreen(false);
		DIALOG.dialogPanel.SetActive(false);

		RIGHT_SIDE_PANEL.UpdateDifficulty(Game.control.stageHandler.difficultyAsString);
		//xp.text = "XP: " + 0 + " / " + Game.control.player.stats.xpCap;

		foreach (GameObject parallax in topLayers) {
			parallax.GetComponent<TopLayerParallaxController> ().Init ();
		}
		foreach (GameObject parallax in bgs) {
			parallax.GetComponent<ParallaxController> ().Init ();
		}
		ResetTopLayer ();
		
		
	}

	public float[] GetBoundaries(){
		wallBoundaries = new float[4]{playAreaBottomWall.transform.position.y, playAreaLeftWall.transform.position.x, playAreaTopWall.transform.position.y, playAreaRightWall.transform.position.x};
		return wallBoundaries;
	}


	////////////////
	//BOSS
	

	public void ShowActivatedPlayerPhase(string text)
	{
		StartCoroutine (_ShowActivatedPlayerPhase (text));
	}

	IEnumerator _ShowActivatedPlayerPhase(string text){
		playerSpecialPanel.SetActive (true);
		playerSpecialText.text = text;

		int dir = -1;

		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < 60; i+=1) {
				playerSpecialPanel.transform.position += new Vector3 (dir + (7 * dir), 0, 0);
				yield return new WaitForSeconds (0.005f);
			}
			dir = 1;
			yield return new WaitForSeconds (5f);
		}
		playerSpecialPanel.SetActive (false);
	}

	//////////////////////////////
	/// 
	/// 
	/// 

	void ResetTopLayer(){
		foreach (GameObject layer in topLayers) {
			Sprite sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/Stage"+ Game.control.stageHandler.currentStage);
			layer.GetComponent<Image> ().sprite = sprite;
			layer.GetComponent<TopLayerParallaxController> ().scrollSpeed = 26;
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
			layer.GetComponent<TopLayerParallaxController> ().scrollSpeed = 5f;
		}

		layer1.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		layer2.GetComponent<Image> ().color = new Color (1, 1, 1, 0);
		for (float i = 0; i < 2; i += 0.1f) {
			layer1.GetComponent<Image> ().color = new Color (1, 1, 1, i);
			layer2.GetComponent<Image> ().color = new Color (1, 1, 1, i);
			yield return new WaitForSeconds (0.1f);
		}
	}

	

	public void UpdateStageText(int stageID, string stageName, string BGMtext)
	{
		stageText.text = "Stage " + stageID.ToString() + " - " + stageName + '\n' + "BGM: " + BGMtext;
		rightPanelStageText.text = stageName;
	}

	public void ShowStageText(){
		IEnumerator stageText = StageText();
		StartCoroutine(stageText);
	}

	IEnumerator StageText()
	{
		stagePanel.SetActive (true);
		yield return new WaitForSeconds(3);
		stagePanel.gameObject.SetActive(false);
	}


	public void StageCompleted(bool value)
	{
		BOSS.HideBossTimer();
		stageEndPanel.SetActive(value);
	}

	public void SaveScoreScreen(bool value){
		saveScoreScreen.SetActive(value);
		saveScorePrompt.SetActive(false);
		scoreInfo.text = Game.control.stageHandler.stats.score.ToString() + " " + Game.control.stageHandler.difficultyAsString;
	}

	public void GameCompleteScreen(bool value){
		gameOver.SetActive(value);
		gameOverImage.gameObject.SetActive(false);
		gameCompleteImage.gameObject.SetActive(true);
		saveScorePrompt.SetActive(true);
	}

	public void GameOverScreen(bool value){
		gameOver.SetActive(value);
		gameOverImage.gameObject.SetActive(true);
		gameCompleteImage.gameObject.SetActive(false);
		saveScorePrompt.SetActive(true);
	}

	public void GameOverSelections(bool value){
		saveScorePrompt.SetActive(false);
		SaveScoreScreen(false);
		gameOverOptionsContainer.SetActive(true);
	}


	public void PauseScreen(bool value){
		pauseScreen.SetActive(value);
	}


	public void PlayToast(string text){
		toast.text = text;
	}

	IEnumerator PlayToast(){
		toast.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		toast.gameObject.SetActive (false);
	}
	
	public void ToggleLoadingScreen(bool toggle){
		loadingScreen.SetActive(toggle);
	}

	//OPTIONS
	public void ToggleOptionsScreen(bool toggle){
		pauseMenuPanel.SetActive(!toggle);
		optionsScreen.SetActive(toggle);
	}


}
