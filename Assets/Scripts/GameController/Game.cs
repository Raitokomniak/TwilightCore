using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game control;
	public static bool paused;

	[SerializeField] public MainMenuUI mainMenuUI;
	[SerializeField] public UIController ui;
	[SerializeField] public DialogController dialog;
	[SerializeField] public StageHandler stageHandler;
	[SerializeField] public SceneHandler scene;
	[SerializeField] public EnemySpawner enemySpawner;
	[SerializeField] public EnemyLib enemyLib;
	[SerializeField] public PauseController pause;
	[SerializeField] public PlayerHandler player;
	[SerializeField] public SpriteLibrary spriteLib;
	[SerializeField] public MenuController menu;

	[SerializeField] public GameObject soundObject;
	[SerializeField] public SoundController sound;

	IEnumerator startGameRoutine;

	public void QuitGame(){
		Application.Quit ();
	}

	void Start(){
		MainMenu();
	}

	void Awake(){
		if (control	 == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}
		
		dialog = GetComponent<DialogController> ();
		stageHandler = GetComponent<StageHandler> ();
		scene = GetComponent<SceneHandler> ();
		enemySpawner = GetComponent<EnemySpawner> ();
		spriteLib = GetComponent<SpriteLibrary> ();
		enemyLib = GetComponent<EnemyLib> ();
		pause = GetComponent<PauseController> ();

		sound = soundObject.GetComponent<SoundController> ();
		menu = GetComponent<MenuController> ();

		menu.InitMenu ();
		sound.InitSound ();
	}

	public void MainMenu (){
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
		menu.ToggleMenu (true);
		yield return new WaitForEndOfFrame();
	}

	public void StartGame(){
		sound.StopMusic();
		stageHandler.StartStage(1);
	}

	public string GetCurrentScene (){
		return SceneManager.GetActiveScene ().name;
	}

}
