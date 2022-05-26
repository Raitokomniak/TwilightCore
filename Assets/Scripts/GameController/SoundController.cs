using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public bool disableSound;

	AudioSource playerSoundFXSource;
	AudioSource enemySoundFXSource;
	AudioSource bgMusicSource;

	//Music
	AudioClip BGM_mainmenu;

	AudioClip[] BGM_stageMusic;

	AudioClip[] BGM_bossMusic;

	//Sound
	AudioClip SFX_shoot;
	AudioClip SFX_enemyDie;
	AudioClip SFX_takeHit;
	AudioClip SFX_bonus;


	//Volumes
	public float SFXVolume;

	void Awake(){
		LoadMusic();
		LoadSFX();
	}

	void LoadMusic(){
		BGM_mainmenu = Resources.Load ("Sound/Music/MainMenu") as AudioClip;

		BGM_stageMusic = new AudioClip[2];
		BGM_stageMusic[0] = Resources.Load("Sound/Music/asura-who-remain-asura_piano") as AudioClip; //THIS IS JUST FOR FUN /// Resources.Load ("Sound/Music/Stage1") as AudioClip;
		BGM_stageMusic[1] = Resources.Load("Sound/Music/stage2") as AudioClip;

		BGM_bossMusic = new AudioClip[2];
		BGM_bossMusic[0] = Resources.Load ("Sound/Music/Boss1") as AudioClip;
		BGM_bossMusic[1] = Resources.Load ("Sound/Music/void-dance") as AudioClip;
	}

	void LoadSFX(){
		SFX_shoot = Resources.Load ("Sound/Shoot") as AudioClip;
		SFX_takeHit = Resources.Load ("Sound/TakeHit") as AudioClip;
		SFX_enemyDie = Resources.Load ("Sound/Die") as AudioClip;
		SFX_bonus = Resources.Load("Sound/Cancel2") as AudioClip;
	}

	public float GetBGMVolume(){
		return bgMusicSource.volume;
	}

	public void SetBGMVolume(float value){
		bgMusicSource.volume = value;
	}

	public void SetSFXVolume(float value){
		SFXVolume = value;
		playerSoundFXSource.volume = value;
		enemySoundFXSource.volume = value;
	}


	public void InitSound(){
		playerSoundFXSource = this.gameObject.AddComponent<AudioSource> ();
		enemySoundFXSource = this.gameObject.AddComponent<AudioSource> ();
		bgMusicSource = this.gameObject.AddComponent<AudioSource> ();

		SetSFXVolume(1f);
		SetBGMVolume(1f);

		if(disableSound){
			playerSoundFXSource.volume = 0;
			enemySoundFXSource.volume = 0;
			bgMusicSource.volume = 0;
		}
	}


	public void PlaySpellSound(string source){
		AudioSource s = new AudioSource();

		if(source == "Player")
			s = playerSoundFXSource;
		else if (source == "Enemy")
			s = enemySoundFXSource;

		s.PlayOneShot (Resources.Load ("Sound/Magic2") as AudioClip);
		s.PlayOneShot (Resources.Load ("Sound/Darkness8") as AudioClip);
	}

	public void PlayExampleSound(){
		playerSoundFXSource.PlayOneShot (SFX_shoot);
	}
	
	public void PlaySound(string source, string sound, bool oneShot)
	{
		AudioSource s = null;
		AudioClip c = null;

		if(sound == "Shoot")
			c = SFX_shoot;
		if(sound == "Die")
			c = SFX_enemyDie;
		if(sound == "TakeHit")
			c = SFX_takeHit;
		if(sound == "Bonus")
			c = SFX_bonus;


		if(source == "Player")
			s = playerSoundFXSource;
		else if (source == "Enemy")
			s = enemySoundFXSource;

		if (oneShot) {
			s.PlayOneShot (c);
		} else {
			s.clip = c;
			s.Play ();
		}
	}

	public void FadeOutMusic(){
		IEnumerator fadeOutRoutine = FadeOutRoutine();
		StartCoroutine(fadeOutRoutine);
	}

	IEnumerator FadeOutRoutine(){
		float tempVol = bgMusicSource.volume;
		for(float i = tempVol; i >= 0; i-=Time.deltaTime){
			bgMusicSource.volume = i;
			yield return new WaitForSeconds(Time.deltaTime);
		}
		bgMusicSource.Stop();
		bgMusicSource.volume = tempVol;
	}

	public void PauseMusic(){
		bgMusicSource.Pause ();
	}
	public void ResumeMusic(){
		bgMusicSource.Play ();
	}
	public void StopMusic(){
		bgMusicSource.Stop ();
	}

	public void PlayMusic(string type){
		PlayMusic(type, -1);
	}

	public void PlayMusic(string type, int i){
		bgMusicSource.Stop ();
		AudioClip c = null;
		i = i - 1;

		if(type == "MainMenu")
			c = BGM_mainmenu;
		else if(type == "Stage")
			c = BGM_stageMusic[i];
		else if(type == "Boss")
			c = BGM_bossMusic[i];

		bgMusicSource.clip = c;
		bgMusicSource.loop = true;
		ResumeMusic();
	}

}
