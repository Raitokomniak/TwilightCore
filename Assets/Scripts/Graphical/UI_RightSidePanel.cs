using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RightSidePanel : MonoBehaviour
{
    public TextMeshProUGUI levelTimer;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI lives;
    public TextMeshProUGUI xp;
    public TextMeshProUGUI difficulty;
	public TextMeshProUGUI stage;
	public TextMeshProUGUI hiScore;
	public TextMeshProUGUI score;

    int scoreAdditionalZeroes = 9;

   	public void UpdateTimer(float value){
		levelTimer.text = "Level Time: " + value.ToString ("F2");
	}

    public void UpdateLives(int value){
        lives.text = "Lives: " + value.ToString();
    }

    public void UpdateXP(int value){
        xp.text = "(XP: " + value.ToString () + " / " + Game.control.stageHandler.stats.xpCap + ")";
    }

    public void UpdateWave(int value){
        wave.text = "Wave: " + value.ToString();
    }

	public void UpdateDifficulty(string _difficulty){
		difficulty.text = _difficulty;
	}
    
	public void UpdateScore(long _score){
		score.text = "Score: " + GetZeroes(_score.ToString()) + _score.ToString();
	}

	public void UpdateHiScore(long _hiScore){
		hiScore.text = "HiScore: " + GetZeroes(_hiScore.ToString()) + _hiScore.ToString();
	}

    
    string GetZeroes(string score){
        string scoreString = "";
        string zero = "0";
        int maxZeroes = 9;

        for(int i = 0; i < maxZeroes - score.ToString().Length; i++){
            scoreString += zero;
        }

        return scoreString;
    }

    public void UpdateStage(string value){
        stage.text = value;
    }
}
