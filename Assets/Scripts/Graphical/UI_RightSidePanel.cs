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
    
    public GameObject lifeSpritesContainer;
    public List<GameObject> lifeSprites;
	public GameObject playerSpecialPanel;
	public TextMeshProUGUI playerSpecialText;


    
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


    public void InitLives(int lives){
        for(int i = 0; i < lifeSpritesContainer.transform.childCount; i++){
            GameObject life = lifeSpritesContainer.transform.GetChild(i).gameObject;
            lifeSprites.Add(life);
            life.SetActive(false);
        }
        for(int i = 0; i < lives; i++){
            lifeSprites[i].SetActive(true);
        }
    }

    public void UpdateXP(int value, int xpCap){
        xp.text = "(XP: " + value.ToString () + " / " + xpCap + ")";
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

    
	public void PlayerSpecialToast(bool day, string text){
        StopAllCoroutines();
		StartCoroutine (ToastRoutine (day, text));
	}

	IEnumerator ToastRoutine(bool day, string text){
		playerSpecialPanel.SetActive (true);
		playerSpecialText.text = text;

        if(day) playerSpecialText.color = new Color(1,0.6f,0,1);
        else playerSpecialText.color = new Color(0.08f, 0, 1,1);

		int dir = -1;

        playerSpecialPanel.transform.position = new Vector3(2163.5f, 82.3f, 0);

		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < 90; i+=1) {
				playerSpecialPanel.transform.position += new Vector3 (dir + (7 * dir), 0, 0);
				yield return new WaitForSeconds (0.005f);
			}
			dir = 1;
			yield return new WaitForSeconds (3f);
		}
		playerSpecialPanel.SetActive (false);
	}
}
