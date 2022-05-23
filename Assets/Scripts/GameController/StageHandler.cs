using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;


public class StageHandler : MonoBehaviour {

	Stage stageScript;

	public PlayerStats stats;
	PlayerStats statsAtStageStart;

	public int difficultyMultiplier; //1 easy //3 normal //5 hard //8+ nightmarish
	public string difficultyAsString;
	public bool gameOver;
	public bool stageCompleted;
	public float stageTimer;
	public bool countingStageEndBonuses;

	public bool bossOn;
	public bool bossBonus = false;

	int stageCount = 2;
	public int currentStage;
	public bool stageTimerOn;

	public bool stageOn;

	IEnumerator startStageRoutine;


	void Update () {
		if(stageOn){
			if(stageTimerOn) {
				stageTimer += Time.deltaTime;
				Game.control.ui.RIGHT_SIDE_PANEL.UpdateTimer(stageTimer);
			}
		}
		else {
			if (CanAdvanceStage() && Input.GetKeyDown (KeyCode.Z)) {
				Game.control.ui.HideStageCompletedScreen();
				NextStage ();
			}
		}
	}

	bool CanAdvanceStage(){
		if(gameOver) return false;
		if(!stageCompleted) return false;
		if(countingStageEndBonuses) return false;
		else return true;
	}


	public List<int> CalculateBonuses(){
		List<int> bonuses = new List<int>();
		int timeBonus = (500 - Mathf.RoundToInt(stageTimer)) * 10;
		int dayBonus = Game.control.player.special.dayCorePoints * 100;
		int nightBonus = Game.control.player.special.nightCorePoints * 100;
		stageTimer = 0;
		
		Game.control.player.GainScore(timeBonus);
		Game.control.player.GainScore(dayBonus);
		Game.control.player.GainScore(nightBonus);

		int bossBonusScore = 0;
		if(bossBonus) {
			bossBonusScore = 10000;
			Game.control.player.GainScore(bossBonusScore);
		}
		
		bonuses.Add(timeBonus);
		bonuses.Add(dayBonus);
		bonuses.Add(nightBonus);
		bonuses.Add(bossBonusScore);

		return bonuses;
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
		if(!stageTimerOn) stageTimer = 0;
	}


	public bool CheckIfAllPickUpsGone(){
		if(GameObject.FindGameObjectWithTag("ExpPoint")) return false;
		if(GameObject.FindGameObjectWithTag("DayCorePoint")) return false;
		if(GameObject.FindGameObjectWithTag("NightCorePoint")) return false;
		return true;
	}

	public void EndHandler (string endType)
	{
		bossOn = false;

		if(stageScript != null) stageScript.StopStage();

		if(endType == "GameOver"){
			Game.control.sound.FadeOutMusic();
			IEnumerator deathRoutine = DeathHandling();
			StartCoroutine (deathRoutine);
		}
		else if(endType == "StageComplete"){
			Game.control.sound.FadeOutMusic();
			IEnumerator stageCompleteRoutine = StageCompleteHandling();
			StartCoroutine (stageCompleteRoutine);
		}
		ToggleTimer(false);
	}

	IEnumerator GameCompleteHandling(){
		gameOver = true;
		Game.control.ui.BOSS.HideBossTimer();
		yield return new WaitForSeconds (2);

		Game.control.sound.StopMusic ();
		Game.control.menu.Menu("SaveScorePrompt");
		Game.control.ui.GAMEOVER.GameCompleteScreen(true);

		stageOn = false;
		stageTimerOn = false;
		Game.control.enemySpawner.AbortSpawner();
		Game.control.dialog.EndDialog();
	}


	IEnumerator DeathHandling ()
	{
		gameOver = true;
		Game.control.ui.BOSS.HideBossTimer();
		yield return new WaitForSeconds (2);

		Game.control.sound.StopMusic ();
		Game.control.menu.Menu("SaveScorePrompt");
		Game.control.ui.GAMEOVER.GameOverScreen (true);

		stageOn = false;
		stageTimerOn = false;
		Game.control.enemySpawner.AbortSpawner();
		Game.control.dialog.EndDialog();

	}

	IEnumerator StageCompleteHandling ()
	{	
		Debug.Log("stagecompletehandling");
		stats.lives = Game.control.player.health.lives;
		Game.control.ui.WORLD.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage);
		Game.control.ui.BOSS.HideBossTimer();
		Game.control.ui.BOSS.ToggleBossHealthSlider (false, 0, "");
		yield return new WaitUntil(() => CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds (1);
		stageCompleted = true;
		stageOn = false;
		Game.control.ui.ShowStageCompletedScreen ();
		yield return new WaitUntil(() => countingStageEndBonuses == false);

		if(currentStage == stageCount) {
			IEnumerator gameCompleteRoutine = GameCompleteHandling();
			StartCoroutine(gameCompleteRoutine);
		}
	}

	void NextStage ()
	{
		//Game.control.MainMenu ();
		Game.control.ui.HideStageCompletedScreen ();
		currentStage++;
		StartStage(currentStage);
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
	
	public void RestartStage(){
		if(stageScript != null) stageScript.StopStage();
		StartStage(currentStage);
	}

	public void StartGame(){
		stats = new PlayerStats();
		StartStage(1);
	}

	public void StartStage (int stage){
		ToggleTimer(false);
		gameOver = false;
		stageCompleted = false;
		currentStage = stage;
		startStageRoutine = StartStageRoutine();
		StartCoroutine(startStageRoutine);
	}

	public void SetDifficulty(int diff){
		difficultyMultiplier = diff;
		if(diff == 1) difficultyAsString = "Very Easy";
		if(diff == 3) difficultyAsString = "Easy";
		if(diff == 5) difficultyAsString = "Normal";
		if(diff == 10) difficultyAsString = "Nightmare";
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

		Game.control.enemyLib.InitEnemyLib ();
		Game.control.scene.SetUpEnvironment ();
		Game.control.io.LoadHiscoreByDifficulty(difficultyAsString);

		Game.control.dialog.Init();
		Game.control.ui.InitStage ();
		
		Game.control.menu.InitMenu();
		Game.control.player.Init();
		Game.control.player.gameObject.SetActive (true);
		

		Game.control.sound.PlayMusic ("Stage", currentStage);
		Game.control.enemySpawner.StartSpawner (currentStage);

		stageTimer = 0;
		stageScript.StartStageHandler();
		stageOn = true;

		Game.control.ui.ToggleLoadingScreen(false);
	}

}
