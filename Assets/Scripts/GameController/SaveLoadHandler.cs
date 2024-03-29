using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable] 
public class ScoreList {
    public List<ScoreSave> allScores;

    public ScoreList(){ allScores = new List<ScoreSave>(); }
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

	public ScoreSave(){}
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
            Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateHiScore(compared[0].score);
        }
        else Game.control.stageUI.RIGHT_SIDE_PANEL.UpdateHiScore(Game.control.stageHandler.stats.hiScore);
    }

    public bool LoadScore(){
        if(File.Exists(appDataPath + "/score.json")){
			ScoreSave loadedHiScore = new ScoreSave();
            string rawJson = File.ReadAllText(appDataPath + "/score.json");
            scoreList = JsonUtility.FromJson<ScoreList>(rawJson);
            return true;
        }
        else return true;
    }

    
    public bool SaveOptions(){
        OptionsValues options = new OptionsValues();
        options.autoScroll = Game.control.dialog.autoScroll;
        options.bgmVolume =  Game.control.sound.GetBGMVolume();
        options.sfxVolume =  Game.control.sound.SFXVolume;
        options.screenMode = Game.control.gfx.screenMode;
        options.resolution = Game.control.gfx.resolution;
        string dataString = JsonUtility.ToJson(options);
        File.WriteAllText(appDataPath + "/options.json", dataString);
        return true;
    }

    public void LoadOptions(){
        string path = appDataPath + "/options.json";
        
        if(File.Exists(appDataPath + "/options.json")){
            string rawJson = File.ReadAllText(appDataPath + "/options.json");
            OptionsValues options = JsonUtility.FromJson<OptionsValues>(rawJson);
            Game.control.options.LoadValuesFromFile(options);
        }
        else {
            Debug.Log("No options file, default options");
            Game.control.options.DefaultOptions();
        }
    }

    public List<Vector3> LoadShape(string shape){
        ShapeVectorSaveData data = new ShapeVectorSaveData();
        List<Vector3> vectors = new List<Vector3>();
        TextAsset asset = Resources.Load<TextAsset>("PatternShapeData/shape_" + shape);
        
        if(asset != null){
            data = JsonUtility.FromJson<ShapeVectorSaveData>(asset.text);
            vectors = data.shapeVectors;
            return vectors;
        }
        else return null;
    }
}