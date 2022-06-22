using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game control;

	[SerializeField] public VectorLib vectorLib;
	[SerializeField] public SpriteLibrary spriteLib;

    [SerializeField] public GraphicsController gfx;
	[SerializeField] public MainMenuUI mainMenuUI;
	[SerializeField] public UIController ui;
	[SerializeField] public DialogController dialog;
	[SerializeField] public StageHandler stageHandler;
	[SerializeField] public SaveLoadHandler io;
	[SerializeField] public EnvironmentHandler scene;
	[SerializeField] public EnemySpawner enemySpawner;
	[SerializeField] public BulletPooler bulletPool;
	
	[SerializeField] public PauseController pause;
	[SerializeField] public PlayerHandler player;
	
	[SerializeField] public MenuController menu;

	[SerializeField] public Options options;

	[SerializeField] public GameObject soundObject;
	[SerializeField] public SoundController sound;


    public bool loading;

	public void QuitGame(){
		Application.Quit ();
	}

	void Start(){
		MainMenu();
		io.LoadScore();
	}

	void Awake(){
		if (control	 == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}

        gfx = GetComponent<GraphicsController>();
		io = GetComponent<SaveLoadHandler>();
		dialog = GetComponent<DialogController> ();
		stageHandler = GetComponent<StageHandler> ();
		scene = GetComponent<EnvironmentHandler> ();
		enemySpawner = GetComponent<EnemySpawner> ();
		spriteLib = GetComponent<SpriteLibrary> ();
		vectorLib = GetComponent<VectorLib> ();
		pause = GetComponent<PauseController> ();
		options = GetComponent<Options>();
		bulletPool = GetComponent<BulletPooler>();

		sound = soundObject.GetComponent<SoundController> ();
		menu = GetComponent<MenuController> ();

		menu.InitMenu ();
		sound.InitSound ();

		//WOULDNT NEED THIS IF DLL
		Game.control.vectorLib.InitVectorLib();
	}

	public void MainMenu (){
		StartCoroutine(LoadMainMenu());
	}


	IEnumerator LoadMainMenu(){
        loading = true;
        if(GameObject.Find("MainMenuCanvas")) mainMenuUI = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuUI>();
        
        if(ui != null) ui.ToggleLoadingScreen(true);
        else if(mainMenuUI != null) mainMenuUI.ToggleLoadingScreen(true);

        if(stageHandler.stageOn){
            stageHandler.StopStage();
            yield return new WaitUntil(() => stageHandler.stageTimer == 0);
           // stageHandler.enabled = false;
        }
        
        
		if(GetCurrentScene() != "MainMenu") {
			AsyncOperation loadScene = SceneManager.LoadSceneAsync ("MainMenu");
			yield return new WaitUntil(() => loadScene.isDone == true);
		}
        
		mainMenuUI = GameObject.Find("MainMenuCanvas").GetComponent<MainMenuUI>();
        mainMenuUI.ToggleLoadingScreen(true);
		mainMenuUI.InitMainMenu ();
		sound.PlayMusic ("MainMenu");
		menu.Menu("MainMenu");

		io.LoadOptions();
        
		mainMenuUI.ToggleLoadingScreen(false);
        loading = false;
	}

	public void StartGame(){
		sound.StopMusicAndEffects();
		stageHandler.enabled = true;
		stageHandler.StartGame();
	}

	public string GetCurrentScene (){
		return SceneManager.GetActiveScene ().name;
	}

}
