﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameEndHandler : MonoBehaviour
{

	public bool gameOver;
	public bool stageCompleted;

	// Use this for initialization
	void Awake ()
	{
		gameOver = false;
	}

	void Update(){
		if (stageCompleted && Input.GetKeyDown (KeyCode.Z)) {
			GameController.gameControl.ui.StageCompleted (false);
			NextStage ();
		}
	}

	//Checks the type of ending event
	public void EndHandler (string endType)
	{
		switch (endType) {
		case "GameOver":
			StartCoroutine (DeathHandling ());
			break;

		case "StageComplete":
			StartCoroutine (StageCompleteHandling ());
			break;

		case "TimeUp":
			StartCoroutine (TimeUp ());
			break;
		}
	}



	//If player dies, game over
	IEnumerator DeathHandling ()
	{
		gameOver = true;
		GameController.gameControl.ui.ToggleBossTimer (false);

		yield return new WaitForSeconds (2);

		GameController.gameControl.sound.StopMusic ();
		GameController.gameControl.ui.GameOverScreen (true);

	}

	//If boss killed, complete stage
	IEnumerator StageCompleteHandling ()
	{
		stageCompleted = true;
		GameController.gameControl.ui.ToggleBossTimer (false);
		GameController.gameControl.ui.ToggleBossHealthSlider (false, 0, "");

		yield return new WaitForSeconds (2);
		GameController.gameControl.ui.StageCompleted (true);
		yield return new WaitForSeconds (2);


		//NextStage ();
	}

	void NextStage ()
	{
		GameController.gameControl.scene.MainMenu ();
		stageCompleted = false;
		/*GameController.gameControl.ui.StageCompleted (false);
		stageCompleted = false;
		GameController.gameControl.stage.InitStage (false);
		*/
	}

	//If time is up, boss leaves the screen and stage is completed

	IEnumerator TimeUp ()
	{
		GameObject boss = GameObject.FindWithTag ("Boss");
		boss.GetComponent<EnemyLife> ().SetInvulnerable (true);
		boss.GetComponent<EnemyMovement> ().SetUpPatternAndMove (GameController.gameControl.enemyLib.leaving);
		yield return new WaitForSeconds (2);
		StartCoroutine (StageCompleteHandling ());
	}
		


	//Restart by reloading the scene

	public void Restart ()
	{
		//gameOver = false;
		//GameController.gameControl.pause.Unpause ();
		//GameController.gameControl.ui.GameOverScreen(false);
		GameController.gameControl.enemySpawner.StopAllCoroutines ();
		GameController.gameControl.stage.StopAllCoroutines ();
		GameController.gameControl.scene.DeConstructScene ();
		GameController.gameControl.stage.StopStageHandling ();
		//GameController.gameControl.stage.InitStage (true);
		SceneManager.LoadScene ("Level1");
	}
}
