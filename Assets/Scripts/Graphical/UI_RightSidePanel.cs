using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RightSidePanel : MonoBehaviour
{
    public Text levelTimer;
    public TextMeshProUGUI xp;
    public TextMeshProUGUI difficulty;
	public TextMeshProUGUI stage;
	public TextMeshProUGUI hiScore;
	public TextMeshProUGUI score;
    
    public List<GameObject> lifeSprites;
    public Transform livesContainer;
    public Object lifeSpritePrefab;

    //FOR DEVVING ONLY
    
   	public void UpdateTimer(float value){
		levelTimer.text = "Level Time: " + value.ToString ("F2");
	}

    public void UpdateLives(int lives){
        List<GameObject> tempList = lifeSprites;
        foreach(GameObject life in tempList) life.SetActive(false);

        for(int i = 0; i < lives; i++){
            if(tempList[i] == null) return;
            tempList[i].SetActive(true);
        }

        lifeSprites = tempList;
    }

    public void UpdateMaxLives(int lives){
        foreach(GameObject life in lifeSprites) Destroy(life);
        lifeSprites = new List<GameObject>();
        for(int i = 0; i < lives; i++){
            GameObject life = Instantiate(lifeSpritePrefab, new Vector3(livesContainer.position.x + (i * 50), livesContainer.position.y, livesContainer.position.z), Quaternion.identity) as GameObject;
            life.transform.SetParent(livesContainer);
            lifeSprites.Add(life);
        }
    }

    public void UpdateXP(int value){
        xp.text = "(XP: " + value.ToString () + " / " + Game.control.stageHandler.stats.xpCap + ")";
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
