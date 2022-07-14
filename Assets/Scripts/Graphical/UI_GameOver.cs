using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_GameOver : MonoBehaviour
{
    //
    // HANDLES BOTH GAMEOVER AND GAMECOMPLETE & SCORESAVE
    // 
	public GameObject gameOverOptionsContainer;
	public Transform gameOverImage;
	public Transform gameCompleteImage;
    public GameObject saveScoreScreen;

    public TextMeshProUGUI scoreInfo;
   	public GameObject saveScorePrompt;
	public GameObject saveScoreContainer;
    public TMP_InputField scoreSaveNameInput;
    
    public bool saveScoreOn;

    bool gameOver = false;
    bool gameComplete = false;


    public void SendScore(){
        if(gameOver) Game.control.menu.Menu("GameOverMenu");
		else if(gameComplete) Game.control.stageHandler.MainMenu();
        gameOver = false;
        gameComplete = false;
        saveScoreOn = false;
		Game.control.io.SaveScore(scoreSaveNameInput.text, Game.control.stageHandler.stats.score, Game.control.stageHandler.difficultyAsString);
    }

	public void GameCompleteScreen(bool value){
		this.gameObject.SetActive(value);
		gameOverImage.gameObject.SetActive(false);
		gameCompleteImage.gameObject.SetActive(true);
		saveScorePrompt.SetActive(true);
        gameComplete = true;
	}
	public void GameOverScreen(bool value){
		this.gameObject.SetActive(value);
		gameOverImage.gameObject.SetActive(true);
		gameCompleteImage.gameObject.SetActive(false);
		saveScorePrompt.SetActive(true);
        gameOver = true;
	}

	public void GameOverSelections(bool value){
		saveScorePrompt.SetActive(false);
		SaveScoreScreen(false);
		gameOverOptionsContainer.SetActive(true);
	}

    public void SaveScoreScreen(bool value){
		saveScoreScreen.SetActive(value);
		saveScorePrompt.SetActive(false);
		scoreSaveNameInput.ActivateInputField();
        saveScoreOn = true;
		scoreInfo.text = Game.control.stageHandler.stats.score.ToString() + " " + Game.control.stageHandler.difficultyAsString;
	}
    
}
