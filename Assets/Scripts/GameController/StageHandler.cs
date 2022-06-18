using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;


public class StageHandler : MonoBehaviour {

	Stage stageScript;
    public Phaser bossScript;


	SpriteLibrary spriteLib;
	public ArrayList waves;

	public PlayerStats stats;

	public int difficultyMultiplier; //1 easy //3 normal //5 hard //8+ nightmarish
	public string difficultyAsString;
	public bool gameOver;
	public bool stageCompleted;
	public float stageTimer;
	public bool countingStageEndBonuses;

	public bool bossOn;
    public bool midBossOn;

	int stageCount = 3;
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
			if(AllowInput()){
				if (CanAdvanceStage() && Input.GetKeyDown (KeyCode.Z)) {
					Game.control.ui.HideStageCompletedScreen();
					if(currentStage == stageCount) {
						IEnumerator gameCompleteRoutine =  GameOverRoutine(false);
						StartCoroutine(gameCompleteRoutine);
					}
					else NextStage ();
				}
			}
		}
	}

	bool AllowInput(){
		if(Game.control.loading) return false;
		return true;
	}

	bool CanAdvanceStage(){
		if(gameOver) return false;
		if(!stageCompleted) return false;
		if(countingStageEndBonuses) return false;
		else return true;
	}

    public void DenyBossBonus(){
        if(bossOn || midBossOn) bossScript.bossBonus = false;
    }


	public List<int> CalculateBonuses(){
		List<int> bonuses = new List<int>();
		int timeBonus = (500 - Mathf.RoundToInt(stageTimer)) * 10;
		if(timeBonus < 0) timeBonus = 0;
		int dayBonus = Game.control.player.special.dayCorePoints * 10;
		int nightBonus = Game.control.player.special.nightCorePoints * 10;
		stageTimer = 0;

		int bonusTimesDifficulty = Mathf.CeilToInt((timeBonus + dayBonus + nightBonus) * (0.3f * difficultyMultiplier));

		Game.control.player.GainScore(bonusTimesDifficulty);

		bonuses.Add(timeBonus);
		bonuses.Add(dayBonus);
		bonuses.Add(nightBonus);
		bonuses.Add(difficultyMultiplier);

		return bonuses;
	}

	//THERE MUST BE A BETTER WAY
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
		else if(stage == 3){
			if(GetComponent<Stage2>()!=null) Destroy(GetComponent<Stage2>());
			gameObject.AddComponent<Stage3>();
			stageScript = GetComponent<Stage3>();
		} 
	}

	public void NewWave(Wave w){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		waves.Add(w);
	}

	public void NewWave(Wave w, List<Vector3> spawnPositions){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		w.spawnPositions = spawnPositions;
		w.FillPositionsArraysByEnemyCount();
		waves.Add(w);
	}

	public void NewWave(Wave w, List<Vector3> spawnPositions, List<Vector3> enterDirections, List<Vector3> leaveDirections){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
		w.spawnPositions = spawnPositions;
		//w.enterDirections = enterDirections;
		//w.leaveDirections = leaveDirections;
		w.FillPositionsArraysByEnemyCount();
		waves.Add(w);
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
		stageOn = false;
		

		Game.control.sound.FadeOutMusic();

		if(endType == "GameOver"){
			IEnumerator deathRoutine = GameOverRoutine(true);
			StartCoroutine (deathRoutine);
		}
		else if(endType == "StageComplete"){
			
			IEnumerator stageCompleteRoutine = StageCompleteHandling();
			StartCoroutine (stageCompleteRoutine);
		}
		ToggleTimer(false);
	}

	IEnumerator GameOverRoutine(bool death){
		
		gameOver = true;
		Game.control.ui.BOSS.HideUI();
		yield return new WaitForSeconds (2);

		if(stageScript != null) stageScript.StopStage();

		Game.control.menu.Menu("SaveScorePrompt");
		if(death) Game.control.ui.GAMEOVER.GameOverScreen (true);
		else Game.control.ui.GAMEOVER.GameCompleteScreen (true);
		
		stageOn = false;
		stageTimerOn = false;
		Game.control.enemySpawner.AbortSpawner();
		Game.control.dialog.EndDialog();
	}

	IEnumerator StageCompleteHandling ()
	{	
		stats.lives = Game.control.player.health.lives;
		Game.control.ui.WORLD.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage);
		Game.control.ui.BOSS.HideUI();
		Game.control.ui.BOSS.ToggleBossHealthSlider (false, 0, "");
		yield return new WaitUntil(() => CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds (1);
		stageCompleted = true;
		stageOn = false;
		Game.control.ui.ShowStageCompletedScreen ();
		yield return new WaitUntil(() => countingStageEndBonuses == false);

		
	}

	void NextStage ()
	{
		//Game.control.MainMenu ();
		Game.control.ui.HideStageCompletedScreen ();
		currentStage++;
		StartStage(currentStage);
	}

/*
	//If time is up, boss leaves the screen and stage is completed
	IEnumerator TimeUp ()
	{
		GameObject boss = GameObject.FindWithTag ("Boss");
		boss.GetComponent<EnemyLife> ().SetInvulnerable (true);
		boss.GetComponent<EnemyMovement> ().SetUpPatternAndMove (Game.control.enemyLib.leaving);
		yield return new WaitForSeconds (2);
		StartCoroutine (StageCompleteHandling ());
	}*/
	
    public void StopStage(){
        if(stageScript != null) stageScript.StopStage();
		stageTimer = 0;
		ToggleTimer(false);
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
		if(diff == 2) difficultyAsString = "Very Easy";
		if(diff == 3) difficultyAsString = "Easy";
		if(diff == 5) difficultyAsString = "Normal";
		if(diff == 10) difficultyAsString = "Nightmare";
	}


	IEnumerator StartStageRoutine(){
        if(Game.control.ui != null) Game.control.ui.ToggleLoadingScreen(true);
		Game.control.loading = true;
		stageTimer = 0;
		ToggleTimer(false);
		Game.control.bulletPool.DestroyAll();
        Game.control.enemySpawner.DestroyAllEnemies();
        

		AsyncOperation loadScene = SceneManager.LoadSceneAsync("Level1");
		yield return new WaitUntil(() => loadScene.isDone == true);
		
		Game.control.ui = GameObject.Find("StageCanvas").GetComponent<UIController>();
		Game.control.ui.ToggleLoadingScreen(true);
		
		Game.control.player = GameObject.FindWithTag("Player").GetComponent<PlayerHandler> ();
		Game.control.bulletPool.InstantiateBulletsToPool(difficultyMultiplier);

		yield return new WaitUntil(() => Game.control.enemySpawner.AbortSpawner() == true);
		
		Game.control.pause.Unpause (false);
		
		yield return new WaitForSeconds(1f);

		spriteLib = Game.control.spriteLib;
		waves = new ArrayList();
		
		Game.control.scene.SetUpEnvironment ();
		Game.control.io.LoadHiscoreByDifficulty(difficultyAsString);
		Game.control.dialog.Init();
		Game.control.ui.InitStage ();
		Game.control.menu.InitMenu();
		Game.control.player.Init();
		Game.control.player.gameObject.SetActive (true);
		Game.control.sound.PlayMusic ("Stage", currentStage);
		Game.control.enemySpawner.StartSpawner (currentStage);

		stageScript.StartStageHandler();
		stageOn = true;

		Game.control.ui.ToggleLoadingScreen(false);
		Game.control.loading = false;
	}

}
