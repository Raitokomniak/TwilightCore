using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {
	public bool disableSound;

    //Boss music looping
    float loopStartPoint;
    bool bossMusicOn;

    bool canResumeBGM;
    bool canResumeSFX;
    
	List<AudioSource> SFXsources;
	public AudioSource playerSFXSource, enemySFXSource, generalSFXSource, menuSFXSource, loopingSFXSource, BGMSource;
    public AudioSource bossTimerSource;

	//Music
	AudioClip BGM_mainmenu;
	List<AudioClip> BGM_stageMusic;
	List<AudioClip> BGM_bossMusic;

	//SFX
	AudioClip SFX_shoot, SFX_enemyDie, SFX_takeHit, SFX_bonus, SFX_pickUp, SFX_extraLife, SFX_bossDie, SFX_bossTimerCountDown, SFX_bossHit, SFX_dialog;

	//SpellSFX
	AudioClip SFX_Spell_Default, SFX_Spell_NightCore1, SFX_Spell_DayCore1, SFX_Spell_Lotus, SFX_Spell_SmallLotus;

	//LoopSFX
	AudioClip SFX_Loop_River;

	//MenuSFX
	AudioClip SFX_cursor, SFX_selection, SFX_pause, SFX_cancel;

	//Volumes
	public float SFXVolume;

	void Awake(){
        InitSound();
		LoadMusic();
		LoadSFX();
	}

    void Update() {
       if(bossMusicOn) {
            if(Mathf.Abs(BGMSource.time - BGMSource.clip.length) < .05f) LoopMusicFromPoint();
       }
    }

	void LoadMusic(){
		BGM_mainmenu = Resources.Load ("Sound/Music/MainMenu") as AudioClip;

		BGM_stageMusic = new List<AudioClip>();
		BGM_stageMusic.Add(Resources.Load("Sound/Music/asura-who-remain-asura_lulmix") as AudioClip);
		BGM_stageMusic.Add(Resources.Load("Sound/Music/stage2_lulmix") as AudioClip);
		//BGM_stageMusic.Add(Resources.Load("Sound/Music/stage3") as AudioClip);
        BGM_stageMusic.Add(Resources.Load("Sound/Music/stage3_lulmix") as AudioClip);
        BGM_stageMusic.Add(Resources.Load("Sound/Music/stage3_lulmix") as AudioClip);

		BGM_bossMusic = new List<AudioClip>();
		//BGM_bossMusic.Add(Resources.Load ("Sound/Music/Boss1") as AudioClip);
        BGM_bossMusic.Add(Resources.Load ("Sound/Music/boss1_lulmix") as AudioClip);
		BGM_bossMusic.Add(Resources.Load ("Sound/Music/void-dance") as AudioClip);
		BGM_bossMusic.Add(Resources.Load ("Sound/Music/mothersfears_motherstears_lulmix") as AudioClip);
        BGM_bossMusic.Add(Resources.Load ("Sound/Music/boss4_wipmix") as AudioClip);
	}

	void LoadSFX(){
		SFX_shoot = Resources.Load ("Sound/SFX/Shoot") as AudioClip;
		SFX_takeHit = Resources.Load ("Sound/SFX/TakeHit") as AudioClip;
		SFX_enemyDie = Resources.Load ("Sound/SFX/Die") as AudioClip;
		SFX_bonus = Resources.Load("Sound/SFX/Cancel2") as AudioClip;
		SFX_pickUp = Resources.Load("Sound/SFX/Coin") as AudioClip;
		SFX_extraLife = Resources.Load("Sound/SFX/Heal7") as AudioClip; //APPLY WHEN IT IS TIME
        SFX_bossDie = Resources.Load("Sound/SFX/Up8") as AudioClip;
        SFX_bossTimerCountDown = Resources.Load("Sound/SFX/Shot2") as AudioClip;
        SFX_bossHit = Resources.Load("Sound/SFX/Explosion1") as AudioClip;
		
		SFX_Spell_Default = Resources.Load ("Sound/SFX/Magic2") as AudioClip;
		SFX_Spell_NightCore1 = Resources.Load ("Sound/SFX/Magic11") as AudioClip;
		SFX_Spell_DayCore1 = Resources.Load ("Sound/SFX/Magic8") as AudioClip;
        SFX_Spell_Lotus = Resources.Load ("Sound/SFX/Magic13") as AudioClip;
        SFX_Spell_SmallLotus = Resources.Load("Sound/SFX/Heal8") as AudioClip;

		SFX_cursor = Resources.Load("Sound/SFX/Cursor4") as AudioClip;
		SFX_selection = Resources.Load("Sound/SFX/Decision1") as AudioClip;
		SFX_pause = Resources.Load("Sound/SFX/Decision5") as AudioClip;
		SFX_cancel = Resources.Load("Sound/SFX/Cancel1") as AudioClip;
        SFX_dialog = Resources.Load("Sound/SFX/Cursor3") as AudioClip;

		SFX_Loop_River = Resources.Load("Sound/SFX/River") as AudioClip;
	}

	public float GetBGMVolume(){
		return BGMSource.volume;
	}

	public void SetBGMVolume(float value){
		BGMSource.volume = value;
	}

	public void SetSFXVolume(float value){
        SFXVolume = Mathf.Round(value * 10.0f) * 0.1f;
		foreach(AudioSource s in SFXsources) s.volume = value;
        loopingSFXSource.volume = value / 2;
	}


	public void InitSound(){
		SFXsources = new List<AudioSource>();
		loopingSFXSource.loop = true;

		SFXsources.Add(playerSFXSource);
		SFXsources.Add(enemySFXSource);
		SFXsources.Add(menuSFXSource);
		SFXsources.Add(loopingSFXSource);
        SFXsources.Add(bossTimerSource);
        SFXsources.Add(generalSFXSource);

		SetSFXVolume(1f);
		SetBGMVolume(1f);

		if(disableSound){
			foreach(AudioSource s in SFXsources) s.volume = 0;
			BGMSource.volume = 0;
		}
	}


	public void PlaySpellSound(string source, string spell){
		AudioSource s = new AudioSource();
		AudioClip c = null;

		if(source == "Player")      s = playerSFXSource;
		else if (source == "Enemy") s = enemySFXSource;

		if(spell == "Default")	  c = SFX_Spell_Default;
		if(spell == "NightCore1") c = SFX_Spell_NightCore1;
		if(spell == "DayCore1")   c = SFX_Spell_DayCore1;
        if(spell == "Lotus")      c = SFX_Spell_Lotus;
        if(spell == "SmallLotus") c = SFX_Spell_SmallLotus;

		s.PlayOneShot (c);
	}

	public void PlayExampleSound(){
		playerSFXSource.PlayOneShot (SFX_shoot);
	}

	public void PlayMenuSound(string sound){
		AudioClip c = null;

		if(sound == "Cursor") 	 c = SFX_cursor;
		if(sound == "Selection") c = SFX_selection;
		if(sound == "Pause") 	 c = SFX_pause;
		if(sound == "Cancel") 	 c = SFX_cancel;

		menuSFXSource.PlayOneShot (c);

	}
	
	public void PlaySound(string source, string sound, bool oneShot)
	{
		AudioSource s = null;
		AudioClip c = null;

		if(sound == "Shoot") 		c = SFX_shoot;
		if(sound == "Die") 			c = SFX_enemyDie;
        if(sound == "BossDie") 		c = SFX_bossDie;
        if(sound == "BossHit") 		c = SFX_bossHit;
		if(sound == "TakeHit")  	c = SFX_takeHit;
		if(sound == "Bonus")		c = SFX_bonus;
		if(sound == "PickUp")		c = SFX_pickUp;
		if(sound == "ExtraLife")	c = SFX_extraLife;
        if(sound == "CountDown")	c = SFX_bossTimerCountDown;
        if(sound == "Dialog")       c = SFX_dialog;

		if(source == "Player") 		s = playerSFXSource;
		else if (source == "Enemy") {
            if(sound == "CountDown") s = bossTimerSource;
            else                     s = enemySFXSource;
        }
        else if (source == "General") s = generalSFXSource; 

		if (oneShot) {
			s.PlayOneShot (c);
		}
		else {
			s.clip = c;
			s.Play ();
		}
	}

	public void PlaySFXLoop(string sound){
		AudioClip c = null;

		if(sound == "Rain") 	c = SFX_Loop_River;

		loopingSFXSource.clip = c;
		loopingSFXSource.Play ();
	}

	public void FadeOutMusic(){
        if(!BGMSource.isPlaying) return;
		IEnumerator fadeOutRoutine = FadeOutRoutine();
		StartCoroutine(fadeOutRoutine);
	}

	IEnumerator FadeOutRoutine(){
		float tempVol = BGMSource.volume;
		for(float i = tempVol; i >= 0; i-=Time.deltaTime){
			BGMSource.volume = i;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		StopMusic();
		BGMSource.volume = tempVol;
	}

    public void StopMusic(){
        BGMSource.Stop();
        canResumeBGM = false;
    }
	public void PauseMusic(){
		BGMSource.Pause ();
	}
	public void PauseEffects(){
		loopingSFXSource.Pause();
	}
	public void ResumeMusic(){
		if(!canResumeBGM) return;
        BGMSource.Play ();
	}

    void StartMusic(){
        BGMSource.Play ();
    }

	public void ResumeEffects(){
        if(!canResumeSFX) return;
		loopingSFXSource.Play();
	}
	public void StopMusicAndEffects(){
		BGMSource.Stop ();
		loopingSFXSource.Stop();
	}

	public void StopLoopingEffects(){
		loopingSFXSource.Stop();
        canResumeSFX = false;
	}

    void LoopMusicFromPoint(){
        BGMSource.time = loopStartPoint;
    }

	public void PlayMusic(string type){
		PlayMusic(type, -1);
	}

	public void PlayMusic(string type, int i){
        bossMusicOn = false;
		BGMSource.Stop ();
        BGMSource.time = 0;

		AudioClip c = null;
		i = i - 1;

		if(type == "MainMenu"){
			c = BGM_mainmenu;
            canResumeBGM = false;
        }
		else if(type == "Stage"){
			c = BGM_stageMusic[i];
            canResumeBGM = true;
        }
		else if(type == "Boss"){
            c = BGM_bossMusic[i];
            bossMusicOn = true;
            canResumeBGM = true;
            if(i == 0) loopStartPoint = 26.5f;
            if(i == 1) loopStartPoint = 12.79f;
            if(i == 2) loopStartPoint = 33.45f;
            if(i == 3) loopStartPoint = 42.39f;
        }
			
		//JESARIA BUILDIIN
		if(type == "MainMenu") Game.control.sound.StopLoopingEffects();
		if(type == "Stage") Game.control.sound.StopLoopingEffects();

		BGMSource.clip = c;
		BGMSource.loop = true;

/*
        ///DEBUG
        Debug.Log("bossmusicon " + bossMusicOn);
        Debug.Log("loopstartpoint " + loopStartPoint);
        BGMSource.time = BGMSource.clip.length - 3;*/

        StartMusic();
	}

}
