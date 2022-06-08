﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour {
	public bool disableSound;

	List<AudioSource> SFXsources;
	AudioSource playerSFXSource;
	AudioSource enemySFXSource;
	AudioSource menuSFXSource;
	AudioSource BGMSource;

	//Music
	AudioClip BGM_mainmenu;

	AudioClip[] BGM_stageMusic;

	AudioClip[] BGM_bossMusic;

	//SFX
	AudioClip SFX_shoot;
	AudioClip SFX_enemyDie;
	AudioClip SFX_takeHit;
	AudioClip SFX_bonus;
	AudioClip SFX_pickUp;


	//SpellSFX
	AudioClip SFX_Spell_Default;
	AudioClip SFX_Spell_NightCore1;
	AudioClip SFX_Spell_DayCore1;

	//MenuSFX
	AudioClip SFX_cursor;
	AudioClip SFX_selection;
	AudioClip SFX_pause;
	AudioClip SFX_cancel;

	//Volumes
	public float SFXVolume;

	void Awake(){
		LoadMusic();
		LoadSFX();
	}

	void LoadMusic(){
		BGM_mainmenu = Resources.Load ("Sound/Music/MainMenu") as AudioClip;

		BGM_stageMusic = new AudioClip[3];
		BGM_stageMusic[0] = Resources.Load("Sound/Music/asura-who-remain-asura_piano") as AudioClip; //THIS IS JUST FOR FUN /// 
		//BGM_stageMusic[0] = Resources.Load ("Sound/Music/Stage1") as AudioClip;
		//BGM_stageMusic[1] = Resources.Load("Sound/Music/stage2") as AudioClip;
		BGM_stageMusic[1] = Resources.Load("Sound/Music/stage2_lulmix") as AudioClip;
		BGM_stageMusic[2] = Resources.Load("Sound/Music/asura-who-remain-asura_piano") as AudioClip;

		BGM_bossMusic = new AudioClip[2];
		BGM_bossMusic[0] = Resources.Load ("Sound/Music/Boss1") as AudioClip;
		BGM_bossMusic[1] = Resources.Load ("Sound/Music/void-dance") as AudioClip;
	}

	void LoadSFX(){
		SFX_shoot = Resources.Load ("Sound/SFX/Shoot") as AudioClip;
		SFX_takeHit = Resources.Load ("Sound/SFX/TakeHit") as AudioClip;
		SFX_enemyDie = Resources.Load ("Sound/SFX/Die") as AudioClip;
		SFX_bonus = Resources.Load("Sound/SFX/Cancel2") as AudioClip;
		SFX_pickUp = Resources.Load("Sound/SFX/Coin") as AudioClip;
		//SFX_extralife = = Resources.Load("Sound/SFX/Heal8") as AudioClip; //APPLY WHEN IT IS TIME
		
		SFX_Spell_Default = Resources.Load ("Sound/SFX/Magic2") as AudioClip;
		SFX_Spell_NightCore1 = Resources.Load ("Sound/SFX/Magic11") as AudioClip;
		SFX_Spell_DayCore1 = Resources.Load ("Sound/SFX/Magic8") as AudioClip;

		SFX_cursor = Resources.Load("Sound/SFX/Cursor4") as AudioClip;
		SFX_selection = Resources.Load("Sound/SFX/Decision1") as AudioClip;
		SFX_pause = Resources.Load("Sound/SFX/Decision5") as AudioClip;
		SFX_cancel = Resources.Load("Sound/SFX/Cancel1") as AudioClip;
	}

	public float GetBGMVolume(){
		return BGMSource.volume;
	}

	public void SetBGMVolume(float value){
		BGMSource.volume = value;
	}

	public void SetSFXVolume(float value){
		SFXVolume = value;
		foreach(AudioSource s in SFXsources) s.volume = value;

		menuSFXSource.volume = value / 2;
	}


	public void InitSound(){
		SFXsources = new List<AudioSource>();
		playerSFXSource = this.gameObject.AddComponent<AudioSource> ();
		enemySFXSource =  this.gameObject.AddComponent<AudioSource> ();
		menuSFXSource =   this.gameObject.AddComponent<AudioSource> ();
		BGMSource = 	  this.gameObject.AddComponent<AudioSource> ();

		SFXsources.Add(playerSFXSource);
		SFXsources.Add(enemySFXSource);
		SFXsources.Add(menuSFXSource);

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

		if(sound == "Shoot") 	c = SFX_shoot;
		if(sound == "Die") 		c = SFX_enemyDie;
		if(sound == "TakeHit")  c = SFX_takeHit;
		if(sound == "Bonus")	c = SFX_bonus;
		if(sound == "PickUp")	c = SFX_pickUp;

		if(source == "Player") 		s = playerSFXSource;
		else if (source == "Enemy") s = enemySFXSource;

		if (oneShot) s.PlayOneShot (c);
		else {
			s.clip = c;
			s.Play ();
		}
	}

	public void FadeOutMusic(){
		IEnumerator fadeOutRoutine = FadeOutRoutine();
		StartCoroutine(fadeOutRoutine);
	}

	IEnumerator FadeOutRoutine(){
		float tempVol = BGMSource.volume;
		for(float i = tempVol; i >= 0; i-=Time.deltaTime){
			BGMSource.volume = i;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		BGMSource.Stop();
		BGMSource.volume = tempVol;
	}

	public void PauseMusic(){
		BGMSource.Pause ();
	}
	public void ResumeMusic(){
		BGMSource.Play ();
	}
	public void StopMusic(){
		BGMSource.Stop ();
	}

	public void PlayMusic(string type){
		PlayMusic(type, -1);
	}

	public void PlayMusic(string type, int i){
		BGMSource.Stop ();
		AudioClip c = null;
		i = i - 1;

		if(type == "MainMenu")
			c = BGM_mainmenu;
		else if(type == "Stage")
			c = BGM_stageMusic[i];
		else if(type == "Boss")
			c = BGM_bossMusic[i];

		BGMSource.clip = c;
		BGMSource.loop = true;
		ResumeMusic();
	}

}
