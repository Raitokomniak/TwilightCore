using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsValues {
    [SerializeField] public int screenMode;
    [SerializeField] public int resolution;
    [SerializeField] public bool autoScroll;
    [SerializeField] public float bgmVolume;
    [SerializeField] public float sfxVolume;

    public OptionsValues(){}
}

public class Options : MonoBehaviour
{
    public string[] optionStrings;

    void Awake(){
        optionStrings = new string[5];
    }

    public void UpdateOption(bool increase, int index){
        if(index == 0) ScreenMode(increase);
        if(index == 1) Resolution(increase);
        if(index == 2) AutoScrollOption();
        if(index == 3) BGMVolume(increase);
        if(index == 4) SFXVolume(increase);

        GetOptionStrings();
        UpdateValueToUI(index, optionStrings[index]);
        Game.control.io.SaveOptions();
    }

    void GetOptionStrings(){
        optionStrings[0] = Game.control.gfx.screenModes[Game.control.gfx.screenMode];
        optionStrings[1] = Game.control.gfx.GetResolution().width.ToString() + " x " + Game.control.gfx.GetResolution().height.ToString();

        if(Game.control.dialog.autoScroll)
             optionStrings[2] = "ON";
        else optionStrings[2] = "OFF";

        optionStrings[3] = (Game.control.sound.GetBGMVolume() * 10f).ToString("F0");
        optionStrings[4] = (Game.control.sound.SFXVolume * 10f).ToString("F0");
    }

    void ScreenMode(bool increase){
        int tempValue = Game.control.gfx.screenMode;

        if(increase && tempValue < Game.control.gfx.screenModes.Length - 1) tempValue++;
        else if (!increase && tempValue > 0) tempValue--;

        Game.control.gfx.SetScreenMode(tempValue);
    }

    void Resolution(bool increase){
        int tempValue = Game.control.gfx.resolution;
        if(increase && tempValue < Game.control.gfx.resolutions.Count - 1) 
            tempValue++;
        else if(!increase && tempValue > 0)
            tempValue--;

        Game.control.gfx.SetResolution(tempValue);
    }

    void AutoScrollOption(){
       Game.control.dialog.ToggleAutoScroll(!Game.control.dialog.autoScroll);
    }

    void BGMVolume(bool increase){
        float tempValue = Game.control.sound.GetBGMVolume();

        if(increase && tempValue < 1)       tempValue += 0.1f;
        else if(!increase && tempValue > 0) tempValue -= 0.1f;

        Game.control.sound.SetBGMVolume(tempValue);
    }

    void SFXVolume(bool increase){
        float tempValue = Game.control.sound.SFXVolume;

        if(increase && tempValue < 1)       tempValue += 0.1f;
        else if(!increase && tempValue > 0) tempValue -= 0.1f;

        Game.control.sound.SetSFXVolume(tempValue);
        Game.control.sound.PlayExampleSound();
    }


    public void UpdateValueToUI(int index, string value){
        Game.control.ui.UpdateOptionSelection(index,value);
    }

    public void UpdateAllValues(){
        GetOptionStrings();
        for(int i = 0; i < optionStrings.Length; i++)
            UpdateValueToUI(i, optionStrings[i]);
    }

    public void LoadValuesFromFile(OptionsValues file){
        Game.control.gfx.SetLoadedValues(file.screenMode, file.resolution);
        Game.control.dialog.autoScroll = file.autoScroll;
        Game.control.sound.SetBGMVolume(file.bgmVolume);
        Game.control.sound.SetSFXVolume(file.sfxVolume);
        UpdateAllValues();
    }

    
    public void DefaultOptions(){
        Game.control.gfx.SetLoadedValues(2, 0);
        Game.control.dialog.autoScroll = true;
        Game.control.sound.SetBGMVolume(1);
        Game.control.sound.SetSFXVolume(1);
        GetOptionStrings();
    }
}
