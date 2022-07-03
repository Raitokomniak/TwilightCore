using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game control;
    

	[SerializeField] public VectorLib vectorLib;
	[SerializeField] public SpriteLibrary spriteLib;

    [SerializeField] public GraphicsController gfx;
    [SerializeField] public SaveLoadHandler io;
	[SerializeField] public MenuController menu;
	[SerializeField] public Options options;
    [SerializeField] public SoundController sound;

	[SerializeField] public DialogController dialog;
	[SerializeField] public StageHandler stageHandler;

	[SerializeField] public EnvironmentHandler scene;
	[SerializeField] public EnemySpawner enemySpawner;
	[SerializeField] public BulletPooler bulletPool;
	
	[SerializeField] public PauseController pause;
	[SerializeField] public PlayerHandler player;
	

    
	[SerializeField] public UI_MAINMENU mainMenuUI;
	[SerializeField] public UI_STAGE stageUI;
    [SerializeField] public UI ui; //WHICH UI IN USE

    public bool loading;

	public void QuitGame(){
		Application.Quit ();
	}

	void Start(){
        if(GetCurrentScene() == "MainMenu") SetUI("MainMenu");
        else SetUI("Stage");
		MainMenu();
	}

	void Awake(){
		if (control	 == null) {
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {
			Destroy (gameObject);
		}
	}


	public void MainMenu (){
		StartCoroutine(LoadMainMenu());
	}

    public void SetUI(string type){
        if  (type == "MainMenu") {
            mainMenuUI = GameObject.Find("MainMenuCanvas").GetComponent<UI_MAINMENU>();
            ui = mainMenuUI;
        }
        else if (type == "Stage")    {
            stageUI = GameObject.Find("StageCanvas").GetComponent<UI_STAGE>();
            ui = stageUI;
        }
    }


	IEnumerator LoadMainMenu(){
        loading = true;
        
        ui.ToggleLoadingScreen(true);

		if(GetCurrentScene() != "MainMenu") {
            if(stageHandler.stageOn){
                stageHandler.StopStage();
                yield return new WaitUntil(() => stageHandler.stageTimer == 0);
            }
			AsyncOperation loadScene = SceneManager.LoadSceneAsync ("MainMenu");
			yield return new WaitUntil(() => loadScene.isDone == true);
		}

        yield return new WaitUntil(() => GameObject.Find("MainMenuCanvas") != null);
        
        SetUI("MainMenu");
        ui.ToggleLoadingScreen(true);
		ui.InitMainMenu ();
		sound.PlayMusic ("MainMenu");
		menu.Menu("MainMenu");
		io.LoadOptions();
	    ui.ToggleLoadingScreen(false);
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
