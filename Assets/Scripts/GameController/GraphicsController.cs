using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsController : MonoBehaviour
{
    public int screenMode = 0;
    public int resolution = 0;
    public List<Resolution> resolutions;
    public string[] screenModes = {"FullScreen", "Borderless Window", "Windowed"};

    void Start(){
        InitScreen();
    }

    
    void Update(){
        if(Game.control.stageHandler.stageOn && !Application.isFocused && !Game.control.pause.paused) {
            Game.control.menu.Menu("PauseMenu");
            Game.control.pause.HandlePause();
        }
    }

    void ListResolutions(){
        resolutions = new List<Resolution>();
        foreach(Resolution r in Screen.resolutions){
            if(r.width == 720 && r.height == 400)   resolutions.Add(r);
            if(r.width == 1280 && r.height == 720)  resolutions.Add(r);
            if(r.width == 1600 && r.height == 900)  resolutions.Add(r);
            if(r.width == 1920 && r.height == 1080) resolutions.Add(r);
            if(r.width == 2560 && r.height == 1440) resolutions.Add(r);
        }
        RemoveDuplicateResolutions();
    }
    public void SetLoadedValues(int _screenMode, int _resolution){
        screenMode = _screenMode;
        resolution = _resolution;
        SetResolution(resolution);
        SetScreenMode(screenMode);
    }

    void InitScreen(){
        Application.targetFrameRate = -1;
		QualitySettings.vSyncCount = 0;
        ListResolutions();
        SetResolution(0);
        SetScreenMode(2);
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
        Resolution reso = resolutions[resolution];
        int w = reso.width;
        int h = reso.height;
        Screen.SetResolution(w, h, GetScreenMode(), 0);
    }

    public List<Resolution> RemoveDuplicateResolutions(){
        //REMOVES DUPLICATE RESOS
        
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
        return resolutions[resolution];
    }
}
