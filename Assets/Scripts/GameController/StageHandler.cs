using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageHandler : MonoBehaviour {

	Stage stageScript;

	public int difficultyMultiplier;
	public bool gameOver;
	public bool stageCompleted;
	public float stageTime;
	public float stageTimer;
	public int currentStage;
	public bool timer;


	IEnumerator restartRoutine;
	IEnumerator deathRoutine;
	IEnumerator stageCompleteRoutine;
	IEnumerator timeUpRoutine;

	

	void Awake(){
		gameOver = false;
		difficultyMultiplier = 8;
	}

	void Update () {
		if(timer) {stageTimer += Time.deltaTime;
			Game.control.ui.UpdateTimer(stageTimer);
		}
//		if(Game.control.ui != null) Game.control.ui.UpdateTimer (stageTimer);

		if (stageCompleted && Input.GetKeyDown (KeyCode.Z)) {
			Game.control.ui.StageCompleted (false);
			NextStage ();
		}
	}
	
	public void InitWaves(int stage){
		if(stage == 1){
			Debug.Log("add stage 1");
			gameObject.AddComponent<Stage1>();
			stageScript = GetComponent<Stage1>();
		}
		else if(stage == 2){
			gameObject.AddComponent<Stage2>();
			stageScript = GetComponent<Stage2>();
		} 
	}


	public void ToggleTimer(bool value){
		timer = value;
		if (timer) {
			stageTime = 180f;
			stageTimer = 0;
		}
	}

	public void InitStage(bool fullReset){
		if (fullReset) {
			Debug.Log("fullreset");
			if(stageScript != null) stageScript.StopStage();
			currentStage = 0;
			//Game.control.player = GameObject.FindWithTag("Player").GetComponent<PlayerHandler>();
			Game.control.player.Init ();
			Game.control.ui.InitStage ();
			
			Game.control.player.gameObject.SetActive (true);
			if (Game.control.pause.paused)
				Game.control.pause.Unpause ();
			Game.control.enemyLib.InitEnemyLib ();
			ToggleTimer (true);

			currentStage += 1;

			Game.control.sound.PlayMusic ("Stage" + currentStage);

			Game.control.enemySpawner.StartSpawner (currentStage);
			stageScript.StartStageHandler();
		} else {
			stageScript.StopStage();
			currentStage += 1;
			ToggleTimer (true);
			Game.control.sound.PlayMusic ("Stage" + currentStage);
			Game.control.enemySpawner.StartSpawner (currentStage);
		}
		
		stageScript.StartStageHandler();
	}


	public void EndHandler (string endType)
	{
		switch (endType) {
		case "GameOver":
			deathRoutine = DeathHandling();
			StartCoroutine (deathRoutine);
			break;

		case "StageComplete":
			stageCompleteRoutine =StageCompleteHandling();
			StartCoroutine (stageCompleteRoutine);
			break;

		case "TimeUp":
			timeUpRoutine = TimeUp();
			StartCoroutine(timeUpRoutine);
			break;
		}
	}

	IEnumerator DeathHandling ()
	{
		gameOver = true;
		Game.control.ui.HideBossTimer();
		yield return new WaitForSeconds (2);

		Game.control.sound.StopMusic ();
		Game.control.ui.GameOverScreen (true);

	}

	IEnumerator StageCompleteHandling ()
	{
		stageCompleted = true;
		Game.control.ui.HideBossTimer();
		Game.control.ui.ToggleBossHealthSlider (false, 0, "");

		yield return new WaitForSeconds (2);
		Game.control.ui.StageCompleted (true);
		yield return new WaitForSeconds (2);

		//NextStage ();
	}

	void NextStage ()
	{
		Game.control.MainMenu ();
		stageCompleted = false;
		/*GameController.gameControl.ui.StageCompleted (false);
		stageCompleted = false;
		GameController.gameControl.stage.InitStage (false);
		*/
	}

	//If time is up, boss leaves the screen and stage is completed

	IEnumerator TimeUp ()
	{
		GameObject boss = GameObject.FindWithTag ("Boss");
		boss.GetComponent<EnemyLife> ().SetInvulnerable (true);
		boss.GetComponent<EnemyMovement> ().SetUpPatternAndMove (Game.control.enemyLib.leaving);
		yield return new WaitForSeconds (2);
		StartCoroutine (StageCompleteHandling ());
	}
		
	public void StartStage (bool restart, string levelName){
		gameOver = false;
		restartRoutine = StartStageRoutine(restart, levelName);
		StartCoroutine(restartRoutine);
	}

	IEnumerator StartStageRoutine(bool restart, string levelName){

		ToggleTimer(false);
		
		string scene = "";
		if(restart) {
			scene = SceneManager.GetActiveScene().name;
			stageScript.StopStage();
			Game.control.enemySpawner.AbortSpawner();
			Game.control.pause.Unpause();
		}

		else scene = levelName;

		AsyncOperation loadScene = SceneManager.LoadSceneAsync(scene);
		yield return new WaitUntil(() => loadScene.isDone == true);
		
		

		Game.control.sound.StopMusic ();
		Game.control.ui = GameObject.Find("StageCanvas").GetComponent<UIController>();
		Game.control.player = GameObject.FindWithTag("Player").GetComponent<PlayerHandler> ();
		
		
		Game.control.menu.ToggleMenu (false);
		Game.control.scene.SetUpEnvironment ();

		Game.control.ui.ToggleLoadingScreen(true);

		yield return new WaitForSeconds(1);

		Game.control.ui.ToggleLoadingScreen(false);

		Game.control.stageHandler.InitStage (true);
	}

}
