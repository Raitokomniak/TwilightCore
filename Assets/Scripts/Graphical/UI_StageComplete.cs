using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_StageComplete : MonoBehaviour
{   
    public GameObject container;
    IEnumerator bonusRoutine;
    public TextMeshProUGUI scorebreakDown;
    public string scoreText;
    int dayBonus;
    int nightBonus;
    int difficultyMultiplier;
    long finalScore;

    float defaultWaitTime = .4f;
    public float waitTime;


    
	public void Hide(){
		container.SetActive(false);
	}

    public void Show(){
		Game.control.stageUI.BOSS.HideUI();
		container.SetActive(true);
		UpdateScoreBreakDown();
	}


    public void UpdateScoreBreakDown(){
        
        List<int> bonuses = Game.control.stageHandler.CalculateBonuses();
        dayBonus = bonuses[0];
        nightBonus = bonuses[1];
        difficultyMultiplier = bonuses[2];
        finalScore = Game.control.stageHandler.stats.score;

        scoreText = "BONUSES" + '\n' + '\n';
        
        if(bonusRoutine != null) StopCoroutine(bonusRoutine); //BOSS DIED TWICE SO THIS FIRED TWICE, SO THIS IS JUST A FAILSAFE
        bonusRoutine = BonusRoutine();
        StartCoroutine(bonusRoutine);
    
    }

    IEnumerator BonusRoutine(){
        Game.control.inputHandler.skipContext = "Bonus";
        waitTime = defaultWaitTime;
        
        Game.control.stageHandler.countingStageEndBonuses = true;
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(waitTime);

        scoreText += "Day Points: " + dayBonus.ToString() + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(waitTime);

        scoreText += "Night Points: " + nightBonus.ToString() + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(waitTime);

        /////////////////////////
        scoreText += "Difficulty multiplier: " + (0.3f * difficultyMultiplier).ToString("F1") + "x" + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(waitTime);

        scoreText += "Final score: " + finalScore.ToString() + '\n' + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(waitTime);

        scoreText += "Press Z to continue";
        scorebreakDown.text = scoreText;
        Game.control.stageHandler.countingStageEndBonuses = false;
    }
}
