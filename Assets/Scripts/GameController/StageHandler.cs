using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageHandler : MonoBehaviour {
	
	public int difficultyMultiplier;
	public bool gameOver;
	public bool stageCompleted;
	public float stageTime;
	public float stageTimer;
	public int currentStage;
	public bool timer;
	IEnumerator stageHandlerRoutine;

	IEnumerator restartRoutine;
	IEnumerator deathRoutine;
	IEnumerator stageCompleteRoutine;
	IEnumerator timeUpRoutine;

	

	void Awake(){
		stageHandlerRoutine = StageHandlerRoutine ();
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

	public void ToggleTimer(bool value){
		timer = value;
		if (timer) {
			stageTime = 180f;
			stageTimer = 0;
		}
	}

	IEnumerator StageHandlerRoutine(){
		SceneHandler scene = Game.control.scene;
		Game.control.ui.UpdateStageText (currentStage);

		switch (currentStage) {
		case 1:
			while (scene == null) yield return null;
			while (stageTimer < 4f) yield return null;
			while (stageTimer < 8f) yield return null;
			scene.SetPlaneSpeed (10f);
			scene.RotateCamera (35, 0, 0);

			while (stageTimer < 14f) yield return null;
			scene.RotateCamera (35, 0, -5);
			scene.SetPlaneSpeed (1f);
			while (stageTimer < 24f) yield return null;
			Game.control.ui.ShowStageText();

			scene.MoveCamera (50, 0, 72);
			scene.RotateCamera (25, 0, 5);

			scene.SetPlaneSpeed (10f);
			while (stageTimer < 55f) yield return null;

			while (!Game.control.enemySpawner.midBossWave.dead) yield return null;
			yield return new WaitForSeconds (1f);
			Game.control.dialog.StartDialog ("Boss", 0.5f, true);
			while (Game.control.dialog.handlingDialog) yield return null;
			scene.SetPlaneSpeed (15f);

			while (stageTimer < 96f) yield return null;
			scene.SetPlaneSpeed (3f);

			break;
		case 2:
			yield return new WaitForSeconds (2f);
			Debug.Log ("stage2");
			break;
		}

		
	}

	public void InitStage(bool fullReset){
		if (fullReset) {
			StopCoroutine (stageHandlerRoutine);
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
			StartCoroutine (stageHandlerRoutine);
		} else {
			StopCoroutine (stageHandlerRoutine);
			currentStage += 1;
			ToggleTimer (true);
			Game.control.sound.PlayMusic ("Stage" + currentStage);
			Game.control.enemySpawner.StartSpawner (currentStage);
		}
		
		stageHandlerRoutine = StageHandlerRoutine ();
		StartCoroutine (stageHandlerRoutine);
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
		Game.control.ui.ToggleBossTimer (false);

		yield return new WaitForSeconds (2);

		Game.control.sound.StopMusic ();
		Game.control.ui.GameOverScreen (true);

	}

	IEnumerator StageCompleteHandling ()
	{
		stageCompleted = true;
		Game.control.ui.ToggleBossTimer (false);
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
			StopCoroutine(StageHandlerRoutine());
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

		Game.control.stage.InitStage (true);
	}

}
