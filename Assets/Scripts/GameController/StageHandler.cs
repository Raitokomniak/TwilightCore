using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageHandler : MonoBehaviour {

	Stage stageScript;

	public int difficultyMultiplier; //1 easy //3 normal //5 hard //8+ nightmarish
	public bool gameOver;
	public bool stageCompleted;
	public float stageTime;
	public float stageTimer;
	public int currentStage;
	public bool stageTimerOn;

	public bool stageOn;


	IEnumerator restartRoutine;
	IEnumerator deathRoutine;
	IEnumerator stageCompleteRoutine;
	IEnumerator timeUpRoutine;

	

	void Awake(){
		gameOver = false;
	}

	void Update () {
		if(stageTimerOn) {
			stageTimer += Time.deltaTime;
			Game.control.ui.UpdateTimer(stageTimer);
		}

		if (stageCompleted && Input.GetKeyDown (KeyCode.Z)) {
			Game.control.ui.StageCompleted (false);
			NextStage ();
		}
	}
	
	public void InitWaves(int stage){
		if(stage == 1){
			gameObject.AddComponent<Stage1>();
			stageScript = GetComponent<Stage1>();
			if(GetComponent<Stage2>()!=null) Destroy(GetComponent<Stage2>());
		}
		else if(stage == 2){
			if(GetComponent<Stage1>()!=null) Destroy(GetComponent<Stage1>());
			gameObject.AddComponent<Stage2>();
			stageScript = GetComponent<Stage2>();
		} 
	}


	public void ToggleTimer(bool value){
		stageTimerOn = value;
		if (stageTimerOn) {
			stageTime = 180f;
			stageTimer = 0;
		}
	}


	public void EndHandler (string endType)
	{
		Debug.Log("Endhandler");
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

		ToggleTimer(false);
		Game.control.sound.StopMusic ();
		if(stageScript != null) stageScript.StopStage();
		stageOn = false;
	}

	IEnumerator DeathHandling ()
	{
		gameOver = true;
		Game.control.ui.HideBossTimer();
		yield return new WaitForSeconds (2);

		Game.control.sound.StopMusic ();
		Game.control.ui.GameOverScreen (true);

		stageScript.StopStage();
		stageTimerOn = false;
		Game.control.enemySpawner.AbortSpawner();
		Game.control.dialog.EndDialog();

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
		//Game.control.MainMenu ();
		stageCompleted = false;
		Game.control.ui.StageCompleted (false);
		currentStage++;
		StartStage(2);
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
	
	public void RestartStage(int stage){
		stageScript.StopStage();
		Game.control.enemySpawner.AbortSpawner();
		StartStage(stage);
	}
	public void StartStage (int stage){
		currentStage = stage;
		gameOver = false;
		restartRoutine = StartStageRoutine();
		StartCoroutine(restartRoutine);
	}

	public void SetDifficulty(int diff){
		difficultyMultiplier = diff;
	}


	IEnumerator StartStageRoutine(){
		AsyncOperation loadScene = SceneManager.LoadSceneAsync("Level1");
		yield return new WaitUntil(() => loadScene.isDone == true);
	
		Game.control.ui = GameObject.Find("StageCanvas").GetComponent<UIController>();
		Game.control.ui.ToggleLoadingScreen(true);
		Game.control.player = GameObject.FindWithTag("Player").GetComponent<PlayerHandler> ();
		Game.control.pause.Unpause (false); //needs ui declaration
		yield return new WaitForSeconds(1);

	//Game.control.menu.ToggleMenu (false);
		Game.control.scene.SetUpEnvironment ();
		Game.control.player.Init ();
		Game.control.dialog.Init();
		Game.control.ui.InitStage ();
		Game.control.enemyLib.InitEnemyLib ();

		Game.control.player.gameObject.SetActive (true);
		Game.control.sound.PlayMusic ("Stage" + currentStage);
		Game.control.enemySpawner.StartSpawner (currentStage);

		stageScript.StartStageHandler();
		ToggleTimer (true);
		stageOn = true;

		
		Game.control.ui.ToggleLoadingScreen(false);
	}

}
