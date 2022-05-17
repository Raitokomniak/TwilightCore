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
}
