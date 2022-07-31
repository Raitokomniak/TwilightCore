using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    public List<Transform> scoreSlots;

    public static int CompareScores(ScoreSave x, ScoreSave y){
        return y.score.CompareTo(x.score);
    }
    
    public void Activate(){
        Game.control.io.LoadScore();
        List<ScoreSave> scoreObjects = Game.control.io.scoreList.allScores;
        scoreObjects.Sort(CompareScores);
        if(scoreObjects.Count > 10) scoreObjects.RemoveRange(10, scoreObjects.Count - 10);

        for(int i = 0; i < 10; i++){
            GameObject slot = scoreSlots[i].gameObject;
            TextMeshProUGUI[] textObjects = slot.GetComponentsInChildren<TextMeshProUGUI>();
            //int scoreIndex = i;
            textObjects[0].text = (i + 1).ToString();
            
            if(i < scoreObjects.Count){
                textObjects[1].text = scoreObjects[i].name;
                textObjects[2].text = scoreObjects[i].score.ToString();
                textObjects[3].text = scoreObjects[i].difficulty;
                textObjects[4].text = scoreObjects[i].date;
            }
            else {
                textObjects[1].text = "-";
                textObjects[2].text = "-";
                textObjects[3].text = "-";
                textObjects[4].text = "-";
            }
        }
    }
}
