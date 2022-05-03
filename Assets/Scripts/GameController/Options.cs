using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class OptionsValues {
        [SerializeField] public bool autoScroll;
        [SerializeField] public float bgmVolume;
        [SerializeField] public float sfxVolume;

        public OptionsValues(){

        }
}

public class Options : MonoBehaviour
{
    OptionsValues options;

   // public GameObject optionsContainer;
	//public GameObject optionsValueContainer;

    public void UpdateOption(bool increase, int index){
        if(index == 0)
            AutoScrollOption();
        else if(index == 1)
            BGMVolume(increase);
        else if(index == 2)
            SFXVolume(increase);

        UpdateValueToUI(index);
        SaveOptions();
    }

    void AutoScrollOption(){
       Game.control.dialog.ToggleAutoScroll(!Game.control.dialog.autoScroll);
    }

    void BGMVolume(bool increase){
        float tempValue = Game.control.sound.GetBGMVolume();

        if(increase && tempValue < 1) {
            tempValue += 0.1f;
        }
        else if(!increase && tempValue > 0){
            tempValue -= 0.1f;
        }

        Game.control.sound.SetBGMVolume(tempValue);
    }

    void SFXVolume(bool increase){
        float tempValue = Game.control.sound.SFXVolume;

        if(increase && tempValue < 1) {
            tempValue += 0.1f;
        }
        else if(!increase && tempValue > 0){
            tempValue -= 0.1f;
        }

        Game.control.sound.SetSFXVolume(tempValue);
        Game.control.sound.PlayExampleSound();
    }


    public void UpdateValueToUI(int index){
        if(index == 0){
            if(Game.control.mainMenuUI != null){
                if(Game.control.dialog.autoScroll) Game.control.mainMenuUI.UpdateOptionSelection(0,"ON");
                else Game.control.mainMenuUI.UpdateOptionSelection(0,"OFF");
            }
            else {
                if(Game.control.dialog.autoScroll) Game.control.ui.UpdateOptionSelection(0,"ON");
                else Game.control.ui.UpdateOptionSelection(0,"OFF");
            }
            
        }
        if(index == 1){
            float bgmVol = Game.control.sound.GetBGMVolume() * 10f;

            if(Game.control.mainMenuUI != null)
                Game.control.mainMenuUI.UpdateOptionSelection(1, bgmVol.ToString("F0"));
            else {
                Game.control.ui.UpdateOptionSelection(1, bgmVol.ToString("F0"));
            }
            
        }
        if(index == 2){
            float sfxVol = Game.control.sound.SFXVolume * 10f;

            if(Game.control.mainMenuUI != null)
                Game.control.mainMenuUI.UpdateOptionSelection(2, sfxVol.ToString("F0"));
            else {
                Game.control.ui.UpdateOptionSelection(2, sfxVol.ToString("F0"));
            }
        }
    }
    public void UpdateAllValues(){
        for(int i = 0; i < 3; i++){
            UpdateValueToUI(i);
        }
    }

    public bool SaveOptions(){
        options = new OptionsValues();
        options.autoScroll = Game.control.dialog.autoScroll;
        options.bgmVolume = Game.control.sound.GetBGMVolume();
        options.sfxVolume = Game.control.sound.SFXVolume;
        string dataString = JsonUtility.ToJson(options);
        //THIS DATAPATH HAS TO BE CHANGED TO BUILD DATAPATH
        File.WriteAllText(Game.control.appDataPath + "/Resources/json/options.json", dataString);
        return true;
    }

    public bool LoadOptions(){
         //THIS DATAPATH HAS TO BE CHANGED TO BUILD DATAPATH
        if(File.Exists(Game.control.appDataPath + "/Resources/json/options.json")){
            string rawJson = File.ReadAllText(Game.control.appDataPath + "/Resources/json/options.json");
            options = JsonUtility.FromJson<OptionsValues>(rawJson);
            LoadValuesFromFile();
            return true;
        }
        else return false;
    }

    void LoadValuesFromFile(){
        Game.control.dialog.autoScroll = options.autoScroll;
        Game.control.sound.SetBGMVolume(options.bgmVolume);
        Game.control.sound.SetSFXVolume(options.sfxVolume);
        if(Game.control.ui != null) UpdateAllValues();
    }
}
