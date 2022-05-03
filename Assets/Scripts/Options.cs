using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{

    public void UpdateOption(bool increase, int index){
        if(index == 0)
            AutoScrollOption();
        else if(index == 1)
            BGMVolume(increase);
        else if(index == 2)
            SFXVolume(increase);

        UpdateValueToUI(index);
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
            if(Game.control.dialog.autoScroll) Game.control.ui.UpdateOptionSelection(0,"ON");
            else Game.control.ui.UpdateOptionSelection(0,"OFF");
        }
        if(index == 1){
            float bgmVol = Game.control.sound.GetBGMVolume() * 10f;
            Game.control.ui.UpdateOptionSelection(1, bgmVol.ToString("F0"));
        }
        if(index == 2){
            float sfxVol = Game.control.sound.SFXVolume * 10f;
            Game.control.ui.UpdateOptionSelection(2, sfxVol.ToString("F0"));
        }
    }
    public void UpdateAllValues(){
        for(int i = 0; i < 3; i++){
            UpdateValueToUI(i);
        }
    }
}
