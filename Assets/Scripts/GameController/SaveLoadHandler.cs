using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable] 
public class ScoreList {
    public List<ScoreSave> allScores;

    public ScoreList(){
        allScores = new List<ScoreSave>();
    }
}

[System.Serializable] 
public class ScoreSave {
	[SerializeField] public string name;
	[SerializeField] public long score;
    [SerializeField] public string clearTime;
    [SerializeField] public string difficulty;
    [SerializeField] public string date;


	public ScoreSave(string _name, long _score, string _difficulty){
		name = _name;
		score = _score;
        difficulty = _difficulty;
        date = System.DateTime.Now.ToShortDateString();
	}

	public ScoreSave(){

	}

    
    
}

public class SaveLoadHandler : MonoBehaviour {

    public ScoreList scoreList;
	public string appDataPath;

    void Awake(){
        //appDataPath = Application.dataPath;           //FOR DEVVING
		appDataPath = Application.persistentDataPath;   //FOR BUILD
        scoreList = new ScoreList();
    }
    

    public bool SaveScore(string name, long hiscore, string difficulty){
        ScoreSave score = new ScoreSave(name, hiscore, difficulty);
        scoreList.allScores.Add(score);
        string dataString = JsonUtility.ToJson(scoreList);

        File.WriteAllText(appDataPath + "/score.json", dataString);
        return true;
    }

    public static int CompareScores(ScoreSave x, ScoreSave y){
        return y.score.CompareTo(x.score);
    }

    public void LoadHiscoreByDifficulty(string difficulty){
        List<ScoreSave> compared = new List<ScoreSave>();

        foreach(ScoreSave score in scoreList.allScores){
            if(score.difficulty == difficulty) compared.Add(score);
        }

        if(compared.Count != 0){
            compared.Sort(CompareScores);
            Game.control.stageHandler.stats.hiScore = compared[0].score;
            Game.control.ui.RIGHT_SIDE_PANEL.UpdateHiScore(compared[0].score);
        }
        else Game.control.ui.RIGHT_SIDE_PANEL.UpdateHiScore(Game.control.stageHandler.stats.hiScore);
        //return compared[0].score;
    }

    public bool LoadScore(){
        if(File.Exists(appDataPath + "/score.json")){
			ScoreSave loadedHiScore = new ScoreSave();
            string rawJson = File.ReadAllText(appDataPath + "/score.json");
            scoreList = JsonUtility.FromJson<ScoreList>(rawJson);

			//Game.control.player.stats.hiScore = loadedHiScore.score;
			//Game.control.ui.UpdateHiScore(Game.control.player.stats.hiScore);
            return true;
        }
        else return false;
    }

    
    public bool SaveOptions(){
        OptionsValues options = new OptionsValues();
        options.autoScroll = Game.control.dialog.autoScroll;
        options.bgmVolume = Game.control.sound.GetBGMVolume();
        options.sfxVolume = Game.control.sound.SFXVolume;
        string dataString = JsonUtility.ToJson(options);
        //THIS DATAPATH HAS TO BE CHANGED TO BUILD DATAPATH
        File.WriteAllText(appDataPath + "/options.json", dataString);
        return true;
    }

    public void LoadOptions(){
        //THIS DATAPATH HAS TO BE CHANGED TO BUILD DATAPATH
        string path = appDataPath + "/options.json"; //FOR DEV
        
        if(File.Exists(appDataPath + "/options.json")){
            string rawJson = File.ReadAllText(appDataPath + "/options.json");
            OptionsValues options = JsonUtility.FromJson<OptionsValues>(rawJson);
            Game.control.options.LoadValuesFromFile(options);
            Debug.Log("Options loaded");
        }
        else {
            Debug.Log("No options file, default options");
            Game.control.options.DefaultOptions();
        }
    }


}