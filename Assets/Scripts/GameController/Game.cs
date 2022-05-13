using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game control;
	public static bool paused;

	public bool InStages;

	[SerializeField] public MainMenuUI mainMenuUI;
	[SerializeField] public UIController ui;
	[SerializeField] public DialogController dialog;
	[SerializeField] public StageHandler stageHandler;
	[SerializeField] public SaveLoadHandler io;
	[SerializeField] public SceneHandler scene;
	[SerializeField] public EnemySpawner enemySpawner;
	[SerializeField] public EnemyLib enemyLib;
	[SerializeField] public PauseController pause;
	[SerializeField] public PlayerHandler player;
	[SerializeField] public SpriteLibrary spriteLib;
	[SerializeField] public MenuController menu;

	[SerializeField] public Options options;

	[SerializeField] public GameObject soundObject;
	[SerializeField] public SoundController sound;

	IEnumerator startGameRoutine;

	public void QuitGame(){
		Application.Quit ();
	}

	void Start(){
		Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;
		MainMenu();
		io.LoadScore();///////////////////////////////////////////
	}

	void Awake(){
		if (control	 == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}

		io = GetComponent<SaveLoadHandler>();
		dialog = GetComponent<DialogController> ();
		stageHandler = GetComponent<StageHandler> ();
		scene = GetComponent<SceneHandler> ();
		enemySpawner = GetComponent<EnemySpawner> ();
		spriteLib = GetComponent<SpriteLibrary> ();
		enemyLib = GetComponent<EnemyLib> ();
		pause = GetComponent<PauseController> ();
		options = GetComponent<Options>();

		sound = soundObject.GetComponent<SoundController> ();
		menu = GetComponent<MenuController> ();

		menu.InitMenu ();
		sound.InitSound ();
	}

	public void MainMenu (){
		//InStages = false;
		stageHandler.enabled = false;
		StartCoroutine(LoadMainMenu());
	}

	IEnumerator LoadMainMenu(){
		if(GetCurrentScene() != "MainMenu") {
			AsyncOperation loadScene = SceneManager.LoadSceneAsync ("MainMenu");
			yield return new WaitUntil(() => loadScene.isDone == true);
		}
		mainMenuUI = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuUI>();
		mainMenuUI.InitMainMenu ();
		sound.PlayMusic ("MainMenu");
		menu.Menu("MainMenu");

		Game.control.io.LoadOptions();
		
		yield return new WaitForEndOfFrame();
	}

	public void StartGame(){
		sound.StopMusic();
		stageHandler.enabled = true;
		stageHandler.StartGame();
		//InStages = true;
	}

	public string GetCurrentScene (){
		return SceneManager.GetActiveScene ().name;
	}

}
