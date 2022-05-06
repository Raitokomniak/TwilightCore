using UnityEngine;
using System.Collections;
using System.IO;

public class SaveLoadHandler : MonoBehaviour {

	public string appDataPath;

    void Awake(){
        //appDataPath = Application.dataPath;           //FOR DEVVING
		appDataPath = Application.persistentDataPath;   //FOR BUILD
    }

    public bool SaveScore(){
        ScoreSave score = new ScoreSave("Player", Game.control.player.stats.hiScore);
        string dataString = JsonUtility.ToJson(score);
        File.WriteAllText(appDataPath + "/score.json", dataString);
        return true;
    }

    public bool LoadScore(){
        if(File.Exists(appDataPath + "/score.json")){
			ScoreSave loadedHiScore = new ScoreSave();
            string rawJson = File.ReadAllText(appDataPath + "/score.json");
            loadedHiScore = JsonUtility.FromJson<ScoreSave>(rawJson);
			Game.control.player.stats.hiScore = loadedHiScore.score;
			Game.control.ui.UpdateHiScore(Game.control.player.stats.hiScore);
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