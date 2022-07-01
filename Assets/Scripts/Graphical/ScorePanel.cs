using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    public Object scoreSlotPrefab;
    public List<GameObject> scoreSlots;
    public Transform contentPanel;

    public static int CompareScores(ScoreSave x, ScoreSave y){
        return y.score.CompareTo(x.score);
    }
    
    void Awake()
    {
        Game.control.io.LoadScore();
        List<ScoreSave> scoreObjects = Game.control.io.scoreList.allScores;
        scoreObjects.Sort(CompareScores);
        if(scoreObjects.Count > 10) scoreObjects.RemoveRange(10, scoreObjects.Count - 10);

        scoreSlots = new List<GameObject>();
        for(int i = 1; i < 11; i++){
            GameObject slot = Instantiate(scoreSlotPrefab) as GameObject;
            scoreSlots.Add(slot);
            slot.transform.SetParent(contentPanel);
            slot.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            slot.GetComponent<RectTransform>().offsetMax = new Vector2(0, 50);
            slot.GetComponent<RectTransform>().position = new Vector3(109, 950 - (100 + (75 * i)), 0);
            TextMeshProUGUI[] textObjects = slot.transform.GetComponentsInChildren<TextMeshProUGUI>();
            int scoreIndex = i - 1;
            textObjects[0].text = i.ToString();

            if(scoreIndex < scoreObjects.Count){
                textObjects[1].text = scoreObjects[scoreIndex].name;
                textObjects[2].text = scoreObjects[scoreIndex].score.ToString();
                textObjects[3].text = scoreObjects[scoreIndex].difficulty;
                textObjects[4].text = scoreObjects[scoreIndex].date;
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
