using UnityEngine;
using System.Collections;

public class StageHandler : MonoBehaviour {

	//Full level time and counting down timer
	public float stageTime;
	public float stageTimer;
	public int currentStage;
	public bool started;
	public bool timer;

	public GameObject Player;



	void Update () {
		if (started && !GameController.gameControl.gameEnd.stageCompleted) {
			UpdateTimer ();
		}
	}


	public void ResumeStage()
	{
		if (!started) {
			started = true;

			GameController.gameControl.enemySpawner.StartSpawner ();

			GameController.gameControl.sound.PlayMusic ("Stage" + currentStage);
			StartCoroutine (StageHandlerRoutine ());
		}
	}

	IEnumerator StageHandlerRoutine(){
		GameController g = GameController.gameControl;
		switch (currentStage) {
		case 1:
			yield return new WaitUntil (() => g.scene != null);
			yield return new WaitUntil (() => g.scene.planes.Count >= 2);
			/*g.scene.SetPlaneSpeed (3f);
			yield return new WaitForSeconds (4f);
			g.scene.SetPlaneSpeed (6f);
			yield return new WaitForSeconds (4f);*/
			g.scene.SetPlaneSpeed (3f);

			g.scene.RotateCamera (35, 0, 0);

			yield return new WaitUntil (() => stageTimer >= 14f);
			g.scene.RotateCamera (35, 0, -5);
			//g.scene.SetPlaneSpeed (6f);

			yield return new WaitUntil (() => stageTimer >= 24f);
			g.ui.UpdateStageText (currentStage);

			g.scene.MoveCamera (50, 0, 72);
			g.scene.RotateCamera (25, 0, 5);

			//g.scene.SetPlaneSpeed (10f);

			yield return new WaitUntil (() => stageTimer >= 55f);
			//g.ui.UpdateTopPlayer (0f);

			yield return new WaitUntil (() => g.enemySpawner.midBossWave.dead == true);
			yield return new WaitForSeconds (1f);

			g.dialog.StartDialog ("Boss", 0.5f, true);
			yield return new WaitUntil (() => g.dialog.handlingDialog == false);
			//g.scene.SetPlaneSpeed (15f);

			yield return new WaitUntil (() => stageTimer >= 96);
			//g.scene.SetPlaneSpeed (5f);
			yield return new WaitUntil (() => g.dialog.handlingDialog == false);
			g.sound.PlayMusic ("Boss1");

			break;
		}
		
	}


	void UpdateTimer()
	{
		if(timer && stageTimer < stageTime && !GameController.gameControl.gameEnd.stageCompleted && !GameController.gameControl.gameEnd.gameOver)
		{
			stageTimer += Time.deltaTime;
		}
		else if (stageTimer >= stageTime) GameController.gameControl.gameEnd.EndHandler("TimeUp");

		GameController.gameControl.ui.levelTimerValue = stageTimer;
	}


	public void ToggleTimer(bool value){
		timer = value;
		if (timer) {
			stageTime = 180f;
			stageTimer = 0;
			GameController.gameControl.ui.levelTimerValue = stageTimer;
		}
	}

	public void InitStage(){
		if (currentStage < 1) {
			Debug.Log ("FULL RESET");
			currentStage = 0;
			GameController.gameControl.stats.Init ();
			GameController.gameControl.sound.InitSound ();
			GameController.gameControl.ui.InitStage ();
			GameController.gameControl.stage.Player.SetActive (true);
			Player.GetComponent<PlayerLife> ().InitLife ();
			if(GameController.gameControl.pause.paused) 			GameController.gameControl.pause.HandlePause ();

			GameController.gameControl.enemyLib.InitEnemyLib ();

		}
		Debug.Log ("SOFT RESET");

		currentStage += 1;
		started = false;
		ToggleTimer (true);

		Debug.Log ("Stage " + currentStage);
		GameController.gameControl.ui.UpdateBG ("Stage" + currentStage);
		GameController.gameControl.enemySpawner.InitSpawner ();
		GameController.gameControl.stage.ResumeStage();
	}

}
