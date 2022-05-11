using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable] 
public class ScoreSave {
	[SerializeField] public string name;
	[SerializeField] public long score;

	public ScoreSave(string _name, long _score){
		name = _name;
		score = _score;
	}

	public ScoreSave(){

	}
}


public class StageHandler : MonoBehaviour {

	Stage stageScript;

	public int difficultyMultiplier; //1 easy //3 normal //5 hard //8+ nightmarish
	public bool gameOver;
	public bool stageCompleted;
	public float stageTimer;
	public int currentStage;
	public bool stageTimerOn;

	public bool stageOn;


	IEnumerator restartRoutine;
	IEnumerator deathRoutine;
	IEnumerator stageCompleteRoutine;
	IEnumerator timeUpRoutine;

	ScoreSave currentHiScore;


	void Update () {
		if(stageOn){
			if(stageTimerOn) {
				stageTimer += Time.deltaTime;
				Game.control.ui.UpdateTimer(stageTimer);
			}
		}
		else {
			if (CanAdvanceStage() && Input.GetKeyDown (KeyCode.Z)) {
				Game.control.ui.StageCompleted (false);
				NextStage ();
			}
		}
	}

	bool CanAdvanceStage(){
		if(gameOver) return false;
		if(!stageCompleted) return false;
		else return true;
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
		stageTimer = 0;
	}


	public bool CheckIfAllPickUpsGone(){
		if(GameObject.FindGameObjectWithTag("ExpPoint")) return false;
		if(GameObject.FindGameObjectWithTag("DayCorePoint")) return false;
		if(GameObject.FindGameObjectWithTag("NightCorePoint")) return false;
		return true;
	}

	public void EndHandler (string endType)
	{
		Game.control.io.SaveScore();

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
		
	}

	IEnumerator DeathHandling ()
	{
		gameOver = true;
		Game.control.ui.HideBossTimer();
		yield return new WaitForSeconds (2);

		Game.control.sound.StopMusic ();
		Game.control.menu.Menu("GameOverMenu");

		stageScript.StopStage();
		stageOn = false;
		stageTimerOn = false;
		Game.control.enemySpawner.AbortSpawner();
		Game.control.dialog.EndDialog();

	}

	IEnumerator StageCompleteHandling ()
	{
		
		Game.control.ui.HideBossTimer();
		Game.control.ui.ToggleBossHealthSlider (false, 0, "");
		yield return new WaitUntil(() => CheckIfAllPickUpsGone() == true);
		stageCompleted = true;
		stageOn = false;
		yield return new WaitForSeconds (1);
		Game.control.ui.StageCompleted (true);
		yield return new WaitForSeconds (2);

		//NextStage ();
	}

	void NextStage ()
	{
		//Game.control.MainMenu ();
		Game.control.ui.StageCompleted (false);
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
		StartStage(stage);
	}
	public void StartStage (int stage){
		gameOver = false;
		stageCompleted = false;
		stageTimerOn = false;
		currentStage = stage;
		restartRoutine = StartStageRoutine();
		StartCoroutine(restartRoutine);
	}

	public void SetDifficulty(int diff){
		difficultyMultiplier = diff;
	}


	IEnumerator StartStageRoutine(){
		yield return new WaitUntil(() => Game.control.enemySpawner.AbortSpawner() == true);
		
		AsyncOperation loadScene = SceneManager.LoadSceneAsync("Level1");
		yield return new WaitUntil(() => loadScene.isDone == true);
		
		
		Game.control.ui = GameObject.Find("StageCanvas").GetComponent<UIController>();
		Game.control.ui.ToggleLoadingScreen(true);
		Game.control.player = GameObject.FindWithTag("Player").GetComponent<PlayerHandler> ();
		Game.control.pause.Unpause (false); //needs ui declaration
		Game.control.enemySpawner.DestroyAllEnemies();
		Game.control.enemySpawner.DestroyAllProjectiles();
		yield return new WaitForSeconds(1);

	//Game.control.menu.ToggleMenu (false);
		Game.control.scene.SetUpEnvironment ();
		Game.control.player.Init ();
		Game.control.io.LoadScore();
		Game.control.dialog.Init();
		Game.control.ui.InitStage ();
		Game.control.menu.InitMenu();
		Game.control.enemyLib.InitEnemyLib ();

		Game.control.player.gameObject.SetActive (true);
		Game.control.sound.PlayMusic ("Stage", currentStage);
		Game.control.enemySpawner.StartSpawner (currentStage);

		stageTimer = 0;
		stageScript.StartStageHandler();
		stageOn = true;

		
		Game.control.ui.ToggleLoadingScreen(false);
	}

}
