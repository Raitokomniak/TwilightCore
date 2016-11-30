using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {


	public static GameController gameControl;
	public static bool paused;

	[SerializeField] public UIController ui;
	[SerializeField] public DialogController dialog;
	[SerializeField] public GameEndHandler gameEnd;
	[SerializeField] public StageHandler stage;
	[SerializeField] public SceneHandler scene;
	[SerializeField] public EnemySpawner enemySpawner;
	[SerializeField] public EnemyLib enemyLib;
	[SerializeField] public PauseController pause;
	[SerializeField] public PlayerStats stats;
	[SerializeField] public SpriteLibrary spriteLib;
	[SerializeField] public MenuController menu;

	[SerializeField] public GameObject soundObject;
	[SerializeField] public SoundController sound;


	public void QuitGame(){
		Application.Quit ();
	}


	public void Awake(){
		if (gameControl	 == null) {
			DontDestroyOnLoad (gameObject);
			gameControl = this;
		} else if (gameControl != this) {
			Destroy (gameObject);
		}

		ui = GetComponent<UIController> ();
		dialog = GetComponent<DialogController> ();
		gameEnd = GetComponent<GameEndHandler> ();
		stage = GetComponent<StageHandler> ();
		scene = GetComponent<SceneHandler> ();
		enemySpawner = GetComponent<EnemySpawner> ();
		spriteLib = GetComponent<SpriteLibrary> ();
		enemyLib = GetComponent<EnemyLib> ();
		pause = GetComponent<PauseController> ();
		stats = GetComponent<PlayerStats> ();
		sound = soundObject.GetComponent<SoundController> ();
		menu = GetComponent<MenuController> ();

		menu.InitMenu ();
		sound.InitSound ();

		scene.CheckScene ();

	}

}
