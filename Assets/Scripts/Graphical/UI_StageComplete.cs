using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_StageComplete : MonoBehaviour
{
    IEnumerator bonusRoutine;
    public TextMeshProUGUI scorebreakDown;
    public string scoreText;

    public void UpdateScoreBreakDown(){
        List<int> bonuses = Game.control.stageHandler.CalculateBonuses();
        int timeBonus = bonuses[0];
        int dayBonus = bonuses[1];
        int nightBonus = bonuses[2];
        int bossBonus = bonuses[3];
        long finalScore = Game.control.stageHandler.stats.score;

        scoreText = "BONUSES" + '\n' + '\n';
       // scorebreakDown.text = "BONUSES" + '\n' + '\n' + "Level Time: " + timeBonus + '\n' + "Day Points: " + dayBonus + '\n'  + "Night Points: " + nightBonus + '\n'  + '\n' + "Final score: " + finalScore;
        
        
        if(bonusRoutine != null) StopCoroutine(bonusRoutine); //BOSS DIED TWICE SO THIS FIRED TWICE, SO THIS IS JUST A FAILSAFE
        bonusRoutine = BonusRoutine(timeBonus, dayBonus, nightBonus, bossBonus, finalScore);
        StartCoroutine(bonusRoutine);
    
    }

    IEnumerator BonusRoutine(int timeBonus, int dayBonus, int nightBonus, int bossBonus, long finalScore){
        Game.control.stageHandler.countingStageEndBonuses = true;
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(.8f);

        scoreText += "Level Time: " + timeBonus.ToString() + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(.8f);

        scoreText += "Day Points: " + dayBonus.ToString() + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(.8f);

        scoreText += "Night Points: " + nightBonus.ToString() + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(.8f);

        if(bossBonus > 0) {
            scoreText += "Boss Bonus: " + bossBonus.ToString() + '\n';
            scorebreakDown.text = scoreText;
            Game.control.sound.PlaySound("Player", "Bonus", true);
            yield return new WaitForSeconds(.8f);
        }

        scoreText += "Final score: " + finalScore.ToString() + '\n' + '\n';
        scorebreakDown.text = scoreText;
        Game.control.sound.PlaySound("Player", "Bonus", true);
        yield return new WaitForSeconds(.8f);

        scoreText += "Press Z to continue";
        scorebreakDown.text = scoreText;
        yield return new WaitForSeconds(1.6f);
        Game.control.stageHandler.countingStageEndBonuses = false;
    }
}
