using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIController : MonoBehaviour {
	EnemyLife bossLife;


	public Canvas UICanvas;

	public GameObject stageUI;

	public GameObject stageWorldUI;
	public GameObject playAreaLeftWall;
	public GameObject playAreaRightWall;
	public GameObject playAreaTopWall;
	public GameObject playAreaBottomWall;

	public GameObject[] topLayers;
	public GameObject[] bgs;

	public GameObject bossInvulnerableImage;

	//Boss
	public Slider bossHealthSlider;
	public GameObject bossMiniHealthBar;
	GameObject miniHealthBar;
	List<GameObject> bars;
	public GameObject bossNamePanel;
	public TextMeshProUGUI bossName;

	public GameObject bossX;
	public GameObject bossPattern;
	public TextMeshProUGUI bossPatternText;
	public TextMeshProUGUI bossTimer;

	//THESE MIGHT NOT BELONG HERE, MOVE THEM SOMEWHERE ELSE
	float bossStayTimer;
	float bossStayTime;
	bool bossStayTimerOn;

	//Level
	public TextMeshProUGUI levelTimer;
	public TextMeshProUGUI lives;
	public Text xp;
	public TextMeshProUGUI wave;
	public GameObject stageEndPanel;
	public TextMeshProUGUI stageText;
	public TextMeshProUGUI rightPanelStageText;
	public GameObject stagePanel;
	public TextMeshProUGUI toast;

	//Stats
	public Text pointsToAssign;

	public Text DMGstat;
	public Text SPDstat;
	public Text SCAstat;
	public Slider DMGstatSlider;
	public Slider SPDstatSlider;
	public Slider SCAstatSlider;

	public TextMeshProUGUI hiScore;
	public TextMeshProUGUI score;

	//Special
	public Slider dayCoreSlider;
	public Slider nightCoreSlider;
	public GameObject playerSpecialPanel;
	public TextMeshProUGUI playerSpecialText;
	IEnumerator depleteCoreChargeRoutine;

	//Screens
	public GameObject gameOver;
	public GameObject pauseScreen;
	public GameObject pauseMenuPanel;
	public GameObject loadingScreen;

	//Dialog
	public GameObject dialog;
	public Image dialogBG;
	public TextMeshProUGUI dialogName;
	public TextMeshProUGUI dialogContent;
	public Image dialogRightChar;
	public Image dialogLeftChar;
	public TextMeshProUGUI bossDialogName;
	public TextMeshProUGUI bossDialogDescription;


	void Awake(){
		ToggleLoadingScreen(false);
		ToggleBossHealthSlider(false, 0, "");
		HideBossTimer();
	}

	void Update(){
		if(bossStayTimerOn) {
			if(bossStayTimer > 0){
				bossStayTimer-=Time.deltaTime;
				UpdateBossTimer(bossStayTimer);
			}
			else {
				HideBossTimer();
			}
		}
	}

	public void UpdateMenuSelection(string context, int index){
		Text[] allOptions = null;
		GameObject panel = null;

		if (context == "PauseMenu") {
			allOptions = pauseMenuPanel.transform.GetComponentsInChildren<Text> ();
			panel = pauseMenuPanel;
		}
		
		foreach (Text text in allOptions) {
			text.fontStyle = FontStyle.Normal;
		}
		panel.transform.GetChild (index).GetComponent<Text> ().fontStyle = FontStyle.Bold;
	}

	public void InitStage(){
		stageWorldUI.SetActive (true);
		stageUI.SetActive (true);

		playAreaLeftWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (0).gameObject;
		playAreaRightWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (1).gameObject;
		playAreaTopWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (2).gameObject;
		playAreaBottomWall = Game.control.ui.stageWorldUI.transform.GetChild (2).GetChild (3).gameObject;
		UpdateCoreCharge ("Day", 0);
		UpdateCoreCharge ("Night", 0);

		stageUI.SetActive (true);
		bossHealthSlider.gameObject.SetActive(false);
		bossTimer.gameObject.SetActive(false);
		bossNamePanel.SetActive (false);
		stageEndPanel.SetActive(false);

		gameOver.SetActive(false);
		pauseScreen.SetActive(false);
		dialog.SetActive(false);

		xp.text = "XP: " + 0 + " / " + Game.control.player.stats.xpCap;

		foreach (GameObject parallax in topLayers) {
			parallax.GetComponent<TopLayerParallaxController> ().Init ();
		}
		foreach (GameObject parallax in bgs) {
			parallax.GetComponent<ParallaxController> ().Init ();
		}
		ResetTopLayer ();
		
		//this doesnt belong in ui 
		Game.control.player.movement.SetUpBoundaries(playAreaLeftWall.transform.position.x, playAreaRightWall.transform.position.x, playAreaBottomWall.transform.position.y, playAreaTopWall.transform.position.y);
	}

	public void UpdateTimer(float value){
		levelTimer.text = "Level Time: " + value.ToString ("F2");
	}

	////////////////
	//BOSS
	public void UpdateBossXPos(float posX, bool enabled){
		if (enabled)
			bossX.SetActive (true);
		else
			bossX.SetActive (false);
		
		bossX.transform.position = new Vector3 (posX, bossX.transform.position.y, 0);
	}

	public void HideBossTimer(){
		bossStayTimerOn = false;
		bossTimer.gameObject.SetActive(false);
	}
	public void StartBossTimer(float time){
		bossTimer.gameObject.SetActive(true);
		bossStayTime = time;
		bossStayTimer = time;
		bossTimer.text = time.ToString("F1");
		bossStayTimerOn = true;
	}

	public void UpdateBossTimer(float value){
		bossTimer.text = value.ToString("F1");
	}

	public void ToggleBossHealthSlider(bool value, float maxHealth, string name){
		bossX.SetActive (value	);
		bossName	.gameObject.SetActive(value);
		bossHealthSlider.gameObject.SetActive(value);
		bossHealthSlider.maxValue = maxHealth;
		bossHealthSlider.value = maxHealth;
		bossNamePanel.SetActive (value);
		bossName.text = name;
	}

	public void UpdateBossHealth(float h)
	{
		bossHealthSlider.value = h;
	}

	public void ToggleInvulnerable(bool toggle){
		bossInvulnerableImage.SetActive(toggle);
	}

	public void UpdateBossHealthBars(int h){
		if (h > 1) {
			if (miniHealthBar == null)
				for (int i = 0; i < h - 1; i++) {
					miniHealthBar = Instantiate (bossMiniHealthBar, Vector3.zero, transform.rotation) as GameObject;
					miniHealthBar.transform.SetParent (bossHealthSlider.transform);
					miniHealthBar.transform.position = new Vector3 (130 + i * 20, 630, 0);
					bars = new List<GameObject> ();
					bars.Add (miniHealthBar);
				}
			else {
				
				/*for (int i = 0; i <= h; i++) {
					Destroy (bars [i]);

				}*/
			}
		} else {
			Destroy (miniHealthBar);
		}
	}

	public void ShowActivatedPhase(string target, string text)
	{
		StartCoroutine (_ShowActivatedPhase (target, text));
	}

	IEnumerator _ShowActivatedPhase(string target, string text){
		GameObject targetPanel;
		TextMeshProUGUI targetText;

		if (target == "Boss") {
			targetPanel = bossPattern;
			targetText = bossPatternText;
		} else {
			targetPanel = playerSpecialPanel;
			targetText = playerSpecialText;
		}
		targetPanel.SetActive (true);
		targetText.text = text;

		int dir = -1;

		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < 60; i+=1) {
				targetPanel.transform.position += new Vector3 (dir + (7 * dir), 0, 0);
				yield return new WaitForSeconds (0.005f);
			}
			dir = 1;
			yield return new WaitForSeconds (2f);
		}
		targetPanel.SetActive (false);
	}

	//////////////////////////////
	/// 
	/// 
	/// 

	void ResetTopLayer(){
		foreach (GameObject layer in topLayers) {
				Sprite sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/Stage1");
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
	public void UpdateBG(string type)
	{
		Sprite sprite;

		foreach (GameObject bg in bgs) {
			sprite = Resources.Load<Sprite> ("Images/Backgrounds/" + type);
			bg.GetComponent<Image> ().sprite = sprite;
			//bg.GetComponent<ParallaxController> ().scrollSpeed = 15;
		}
	}
	public void UpdateBG(float speed)
	{
		foreach (GameObject bg in bgs) {
			StartCoroutine (SmoothTransitionBG (bg.GetComponent<ParallaxController>(), speed));
		}
	}

	IEnumerator SmoothTransitionBG(ParallaxController par, float speed)
	{
		float difference = par.scrollSpeed - speed;

		//Acceleration
		if (difference < 0) {
			for (float i = par.scrollSpeed; i < speed; i += 0.01f) {
				par.scrollSpeed = i;
				yield return new WaitForSeconds (0.01f);
			}
		}//Deceleration 
		else {
			for (float i = speed; i > par.scrollSpeed; i -= 0.01f) {
				par.scrollSpeed = i;
				yield return new WaitForSeconds (0.01f);
			}
		}

		yield return new WaitForSeconds (0.01f);
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
			/*switch (type) {
			/*case "Boss1_0":
				sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/bossWeb");
				layer.GetComponent<Image> ().sprite = sprite;
				layer.GetComponent<ParallaxController> ().scrollSpeed = 15;
				break;
			case "Boss1_1":
				sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/bossWeb2");
				layer.GetComponent<Image> ().sprite = sprite;
				layer.GetComponent<ParallaxController> ().scrollSpeed = 15;
				break;
			case "Boss1_2":
				sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/bossWeb");
			layer.GetComponent<Image> ().sprite = sprite;
			layer.GetComponent<ParallaxController> ().scrollSpeed = 15;
			break;
			case "Boss1_3":
				sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/bossWeb3");
				layer.GetComponent<Image> ().sprite = sprite;
				layer.GetComponent<ParallaxController> ().scrollSpeed = 15;
				break;
			case "Stage1":
				sprite = Resources.Load<Sprite> ("Images/Backgrounds/TopLayers/sparkle");
				layer.GetComponent<Image> ().sprite = sprite;
				layer.GetComponent<ParallaxController> ().scrollSpeed = 26;
				break;
		}*/

	

	}

	

	public void UpdateStageText(int stageID)
	{
		stageText.text = "Stage " + stageID.ToString();
		if(stageID==1) rightPanelStageText.text = "Asura's Path";
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
		HideBossTimer();
		stageEndPanel.SetActive(value);
	}

	public void GameOverScreen(bool value){
		gameOver.SetActive(value);
	}

	public void PauseScreen(bool value){
		pauseScreen.SetActive(value);
	}


	//////////////////////////
	// RIGHTSIDE PANEL

	public void PlayToast(string type){
		switch (type) {
		case "PowerUp":
			toast.text = "Power Up!";
			StartCoroutine (PlayToast ());
			break;
		case "PowerDown":
			toast.text = "Power down";
			StartCoroutine (PlayToast ());
			break;
		}
	}

	IEnumerator PlayToast(){
		toast.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		toast.gameObject.SetActive (false);
	}


	public void UpdateStatPanel(string key, float value)
	{
		switch (key) {
		case "XP":
			xp.text = "XP: " + value.ToString () + " / " + Game.control.player.stats.xpCap;
			break;
		case "Lives":
			lives.text = "Lives: " + value.ToString();
			break;
		case "UpgradePoints":
			pointsToAssign.text = "Points to assign: " + value.ToString();
			break;
		case "Wave":
			wave.text = "Wave: " + value.ToString();
			break;
		}

	}

	public void UpdateStatPoints(string stat, int value, int cap){
		Text text = null;
		Slider slider = null;

		switch (stat) {
		case "DMG":
			text = DMGstat;
			slider = DMGstatSlider;
			break;
		case "SPD":
			text = SPDstat;
			slider = SPDstatSlider;
			break;
		case "SCA":
			text = SCAstat;
			slider = SCAstatSlider;
			break;
		}

		text.text = value.ToString () + "/" + cap;
		slider.maxValue = cap;
		slider.value = value;
	}
	
	public void ToggleLoadingScreen(bool toggle){
		loadingScreen.SetActive(toggle);
	}

	//////////////////////////
	// PLAYER SPECIAL


	public void CoreInUse(string core)
	{
		Image dayOverLay = dayCoreSlider.transform.GetChild (1).GetComponent<Image> ();
		Image dayFill = dayCoreSlider.transform.GetChild (0).GetChild (0).GetComponent<Image> ();
		Image nightOverLay = nightCoreSlider.transform.GetChild (1).GetComponent<Image> ();
		Image nightFill = nightCoreSlider.transform.GetChild (0).GetChild (0).GetComponent<Image> ();

		Color dayFillColor = Color.yellow;
		Color nightFillColor = Color.magenta;

		if (core == "Day") {
			dayOverLay.color = new Color (1, 1, 1, 1f);
			dayFill.color = dayFillColor;

			nightOverLay.color = new Color (1, 1, 1, 0.5f);
			nightFill.color = nightFillColor - new Color (0, 0, 0, .5f);
		} else if (core == "Night") {
			nightOverLay.color = new Color (1, 1, 1, 1f);
			nightFill.color = nightFillColor;
				
			dayOverLay.color = new Color (1, 1, 1, 0.5f);
			dayFill.color = dayFillColor - new Color (0, 0, 0, .5f);
		}
	}

	public void UpdateCoreCharge(string core, int updatedCharge){
		if (core == "Day") {
			dayCoreSlider.value = updatedCharge;
		} else if (core == "Night") {
			nightCoreSlider.value = updatedCharge;
		}

	}

	public void DepleteCoreCharge(string core, float specialAttackTime, int current, int threshold){
		depleteCoreChargeRoutine = DepleteCoreChargeRoutine(core, specialAttackTime, current, threshold);
		StartCoroutine(depleteCoreChargeRoutine);
	}

	public IEnumerator DepleteCoreChargeRoutine(string core, float specialAttackTime, int current, int threshold){
		Slider slider = null;
		if (core == "Day")
			slider = dayCoreSlider;
		else if (core == "Night")
			slider = nightCoreSlider;
		
		for (float i = current; i > threshold; i -= 1f) {
			slider.value = i;
			yield return new WaitForSeconds (0.008f * specialAttackTime);
		}
	}

	public void UpdateScore(long _score){
		score.text = "Score: " + _score.ToString();
	}
	public void UpdateHiScore(long _hiScore){
		hiScore.text = "HiScore: " + _hiScore.ToString();
	}

	//////////////////////////
	// DIALOG

	public void ToggleDialog(bool value){
		dialog.SetActive(value);
	}

	public void InitSpeakers(string main, string boss, float index)
	{
		dialogRightChar.gameObject.SetActive (false);
		dialogLeftChar.gameObject.SetActive (false);
		switch (main) {
		case "Soma":
			dialogRightChar.gameObject.SetActive (true);	
			dialogRightChar.sprite = Resources.Load<Sprite> ("Images/DialogCharacters/mainchar");
			break;
		default:
			
			break;
		}

		if (boss.Contains ("Boss")) {
			if (index != 0.5) {
				bossDialogName.transform.parent.gameObject.SetActive (true);
				dialogLeftChar.gameObject.SetActive (true);
				dialogLeftChar.sprite = Resources.Load<Sprite> ("Images/DialogCharacters/Boss" + index);
			}
		} else {
			bossDialogName.transform.parent.gameObject.SetActive (false);
		}
	}

	public void UpdateDialog(string name, string content){


		dialogName.text = name;
		dialogContent.text = content;


		if (name.Contains ("Boss")) {
			
			dialogBG.transform.localScale = new Vector3 (-1, 1, 1);

			if (dialogRightChar.GetComponent<Image>().color.a != .3f) {
				//dialogRightChar.transform.position += new Vector3 (10, 0, 0);
				//dialogLeftChar.transform.position += new Vector3 (10, 0, 0);
			}
			dialogLeftChar.GetComponent<Image> ().color = new Color (1, 1, 1, 1);
			dialogRightChar.GetComponent<Image> ().color = new Color (1, 1, 1, .3f);
		}
		else 
		{
			
			dialogBG.transform.localScale = new Vector3 (1, 1, 1);
			
			if (dialogLeftChar.GetComponent<Image>().color.a != .3f) {
			//dialogLeftChar.transform.position -= new Vector3 (10, 0, 0);
			//dialogRightChar.transform.position -= new Vector3 (10, 0, 0);
			}
			dialogLeftChar.GetComponent<Image> ().color = new Color (1, 1, 1, .3f);
			dialogRightChar.GetComponent<Image> ().color = new Color (1, 1, 1, 1f);
		}


	}

	public void UpdateBossInfo(string name, string description){
		bossDialogName.transform.parent.gameObject.SetActive (true);
		bossDialogName.text = name;
		bossDialogDescription.text = description;
	}



}
