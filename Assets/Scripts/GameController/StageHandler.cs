using UnityEngine;
using System.Collections;

public class StageHandler : MonoBehaviour {
	public float stageTime;
	public float stageTimer;
	public int currentStage;
	public bool timer;

	public GameObject Player;
	IEnumerator stageHandler;

	void Awake(){
		stageHandler = StageHandlerRoutine ();
	}

	void Update () {
		if(timer) stageTimer += Time.deltaTime;
		GameController.gameControl.ui.UpdateTimer (stageTimer);
	}

	public void ToggleTimer(bool value){
		timer = value;
		if (timer) {
			stageTime = 180f;
			stageTimer = 0;
		}
	}

	public void ToggleTimer(){
		timer = !timer;
	}


	public void StopStageHandling(){
		StopCoroutine (stageHandler);
	}


	IEnumerator StageHandlerRoutine(){
		GameController g = GameController.gameControl;
		switch (currentStage) {
		case 1:
			while (g.scene == null && g.scene.planes.Count < 2) yield return null;
			while (stageTimer < 2f) yield return null;
			g.scene.SetPlaneSpeed (3f);
			while (stageTimer < 8f) yield return null;

			g.scene.RotateCamera (35, 0, 0);

			while (stageTimer < 14f) yield return null;
			g.scene.RotateCamera (35, 0, -5);

			while (stageTimer < 24f) yield return null;
			g.ui.UpdateStageText (currentStage);

			g.scene.MoveCamera (50, 0, 72);
			g.scene.RotateCamera (25, 0, 5);

			g.scene.SetPlaneSpeed (10f);
			while (stageTimer < 55f) yield return null;

			while (!g.enemySpawner.midBossWave.dead) yield return null;
			yield return new WaitForSeconds (1f);
			g.dialog.StartDialog ("Boss", 0.5f, true);
			while (g.dialog.handlingDialog) yield return null;
			g.scene.SetPlaneSpeed (15f);

			while (stageTimer < 96f) yield return null;
			while (g.dialog.handlingDialog) yield return null;
			g.sound.PlayMusic ("Boss1");

			break;
		}
		
	}




	public void InitStage(bool fullReset){

		if (fullReset) {
			Debug.Log ("FULL RESET");
			StopCoroutine (stageHandler);
	
			currentStage = 0;
			//GameController.gameControl.stats.Init ();
			GameController.gameControl.ui.InitStage ();
			GameController.gameControl.stats.Init ();
			GameController.gameControl.stage.Player.SetActive (true);
			if (GameController.gameControl.pause.paused)
				GameController.gameControl.pause.Unpause ();
			GameController.gameControl.enemyLib.InitEnemyLib ();
			ToggleTimer (true);

			currentStage += 1;

			GameController.gameControl.sound.PlayMusic ("Stage" + currentStage);

			GameController.gameControl.enemySpawner.StartSpawner ();
			StartCoroutine (stageHandler);

		}

	}

}
