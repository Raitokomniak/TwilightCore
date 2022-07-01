using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsController : MonoBehaviour
{
    public int screenMode = 0;
    public int resolution = 0;
    List<Resolution> resolutions;
    public string[] screenModes = {"FullScreen", "Borderless Window", "Windowed"};

    void Start(){
        InitScreen();
        GetResolutions();
    }

    void InitScreen(){
        Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;
        SetScreenMode(0);
    }

    public void SetScreenMode(int mode){
        screenMode = mode;

        if(mode == 0) Screen.fullScreen = true;
        if(mode == 1) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        if(mode == 2) Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    FullScreenMode GetScreenMode(){
        if(screenMode == 0) return FullScreenMode.ExclusiveFullScreen;
        if(screenMode == 1) return FullScreenMode.FullScreenWindow;
        if(screenMode == 2) return FullScreenMode.Windowed;
        return FullScreenMode.Windowed;
    }

    public void SetResolution(int index){
        resolution = index;
        Resolution reso = Screen.resolutions[index];
        int w = reso.width;
        int h = reso.height;
        Screen.SetResolution(w, h, GetScreenMode(), 0);
    }

    public List<Resolution> GetResolutions(){
        //REMOVES DUPLICATE RESOS
        resolutions = new List<Resolution>();
        resolutions.AddRange(Screen.resolutions);
        List<Resolution> flagForRemoval = new List<Resolution>();
        for(int i = 0; i < resolutions.Count; i++){
            if(i>0) 
                if(resolutions[i].width == resolutions[i - 1].width && resolutions[i].height == resolutions[i - 1].height)
                    flagForRemoval.Add(resolutions[i]);
        }
        foreach(Resolution reso in flagForRemoval){
            resolutions.Remove(reso);
        }
        return resolutions;
    }

    public Resolution GetResolution(){
        if(resolutions == null) return new Resolution();
        return resolutions[resolution];
    }
}
