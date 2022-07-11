using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;


public class StageHandler : MonoBehaviour {

    int STARTING_STAGE = 3;

    AsyncOperation loadScene;
	public Stage stageScript;
    public Phaser bossScript;

    bool start;

    public Intro intro;

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

    public bool onBonusScreen;

	IEnumerator startStageRoutine;



	void Update () {
		if(stageOn){
            if(!stageTimerOn || Game.control.pause.paused) return;
			
            if(Game.control.pause.playerHitTimerOn) stageTimer += Time.unscaledDeltaTime;
            else stageTimer += Time.deltaTime;

			Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateTimer(stageTimer);
		}
		else {
			if(AllowInput()){
				if (CanAdvanceStage() && Input.GetKeyDown (KeyCode.Z)) {
					Game.control.stageUI.STAGEEND.Hide();
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
        if(!bossOn && !midBossOn) return;

        if(bossScript.bossBonus)Game.control.stageUI.PlayToast("Boss bonus failed...");
        bossScript.bossBonus = false;
        
    }

	public List<int> CalculateBonuses(){
		List<int> bonuses = new List<int>();
		int dayBonus = stats.dayCorePoints * 10;
		int nightBonus = stats.nightCorePoints * 10;
		stageTimer = 0;

		int bonusTimesDifficulty = Mathf.CeilToInt((dayBonus + nightBonus) * (0.3f * difficultyMultiplier));

		Game.control.player.GainScore(bonusTimesDifficulty);

		bonuses.Add(dayBonus);
		bonuses.Add(nightBonus);
		bonuses.Add(difficultyMultiplier);

		return bonuses;
	}

	//THERE MUST BE A BETTER WAY
	public ArrayList InitWaves(int stage){
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
        return waves;
	}

	public void NewWave(Wave w){
		if (w.isBoss || w.isMidBoss) {
			w.sprite = spriteLib.SetCharacterSprite ("Boss" + w.bossIndex);
		}
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
		Game.control.stageUI.BOSS.HideUI();
		yield return new WaitForSeconds (2);

		if(stageScript != null) stageScript.StopStage();

		Game.control.menu.Menu("SaveScorePrompt");
		if(death) Game.control.stageUI.GAMEOVER.GameOverScreen (true);
		else      Game.control.stageUI.GAMEOVER.GameCompleteScreen (true);
		
		stageOn = false;
		stageTimerOn = false;
		Game.control.enemySpawner.AbortSpawner();
		Game.control.dialog.EndDialog();
	}

	IEnumerator StageCompleteHandling ()
	{	
        onBonusScreen = true;
		stats.lives = Game.control.player.health.lives;
        Game.control.player.special.SaveToStats();
		Game.control.stageUI.WORLD.UpdateTopPlayer ("Stage" + Game.control.stageHandler.currentStage);
		Game.control.stageUI.BOSS.HideUI();
		Game.control.stageUI.BOSS.ToggleBossHealthSlider (false, 0, "");
		yield return new WaitUntil(() => CheckIfAllPickUpsGone() == true);
		yield return new WaitForSeconds (1);
		stageCompleted = true;
		stageOn = false;
		Game.control.stageUI.STAGEEND.Show ();
		yield return new WaitUntil(() => countingStageEndBonuses == false);
	}

	void NextStage ()
	{
		Game.control.stageUI.STAGEEND.Hide ();
		currentStage++;
		StartStage(currentStage);
	}
	
    public void StopStage(){
        if(stageScript != null) stageScript.StopStage();
		stageTimer = 0;
		ToggleTimer(false);
    }

	public void RestartStage(){
		if(stageScript != null) stageScript.StopStage();
        stats.score = 0;
        stats.hiScore = 0;
		StartStage(currentStage);
	}

	public void StartGame(){
        start = true;
		stats = new PlayerStats();
		StartStage(STARTING_STAGE);
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

    public void MainMenu(){
        Game.control.MainMenu();
        // WAS THINKING OF ADDING A QUICK FADE HERE
    }

    void LoadStage(){
        loadScene = SceneManager.LoadSceneAsync("Level1");
    }

	IEnumerator StartStageRoutine(){
        //STOP PREVIOUS SCRENE
        if(Game.control.stageUI != null) Game.control.stageUI.ToggleLoadingScreen(true);
		Game.control.loading = true;
		stageTimer = 0;
		ToggleTimer(false);
		Game.control.enemySpawner.DestroyAllProjectiles();
        Game.control.enemySpawner.DestroyAllEnemies();
        
        //RELOAD STAGE
        LoadStage();
		yield return new WaitUntil(() => loadScene.isDone == true);
		
        
    
        Game.control.SetUI("Stage");
		Game.control.stageUI.ToggleLoadingScreen(true);
		
		Game.control.player = GameObject.FindWithTag("Player").GetComponent<PlayerHandler> ();

        //ENEMYSPAWNER MIGHT NOT RESET
		yield return new WaitUntil(() => Game.control.enemySpawner.AbortSpawner() == true);
        

        Game.control.bulletPool.InstantiateBulletsToPool(100 * difficultyMultiplier);
        yield return new WaitUntil(() => Game.control.bulletPool.done == true);
		
		Game.control.pause.Unpause (false);
		
        //SAFETY MEASURE FOR ENEMYSPAWNER TO TRULY ABORT. THERE MUST BE A BETTER WAY
		yield return new WaitForSeconds(1f);

		spriteLib = Game.control.spriteLib;
		waves = new ArrayList();
		
		Game.control.scene.SetUpEnvironment ();
		Game.control.io.LoadHiscoreByDifficulty(difficultyAsString);
		Game.control.dialog.Init();
		Game.control.stageUI.InitStage ();
		Game.control.menu.InitMenu();
		Game.control.player.Init();
		Game.control.player.gameObject.SetActive (true);

        if(start) {
            Game.control.player.special.GameInit();
            Game.control.stageUI.ToggleLoadingScreen(false);
            intro.Run();
            while(!intro.introDone) yield return null;
        }
        start = false;


		Game.control.sound.PlayMusic ("Stage", currentStage);
		Game.control.enemySpawner.StartSpawner (currentStage);

		stageScript.StartStageHandler();
		stageOn = true;
        onBonusScreen = false;
        
		Game.control.stageUI.ToggleLoadingScreen(false);
		Game.control.loading = false;
	}

}
