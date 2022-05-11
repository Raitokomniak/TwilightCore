using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
	public bool disableSound;

	AudioSource playerSoundFXSource;
	AudioSource enemySoundFXSource;
	AudioSource bgMusicSource;

	//Music
	AudioClip mainmenu;

	AudioClip[] stageMusic;

	AudioClip[] bossMusic;

	//Sound
	AudioClip shoot;
	AudioClip enemyDie;
	AudioClip takeHit;

	//Volumes
	public float SFXVolume;

	void Awake(){
		mainmenu = Resources.Load ("Sound/Music/MainMenu") as AudioClip;
		stageMusic = new AudioClip[2];
		stageMusic[0] = Resources.Load("Sound/Music/asura-who-remain-asura_piano") as AudioClip; //THIS IS JUST FOR FUN /// Resources.Load ("Sound/Music/Stage1") as AudioClip;
		stageMusic[1] = Resources.Load("Sound/Music/stage2") as AudioClip;

		bossMusic = new AudioClip[2];
		bossMusic[0] = Resources.Load ("Sound/Music/Boss1") as AudioClip;
		bossMusic[1] = Resources.Load ("Sound/Music/void-dance") as AudioClip;

		shoot = Resources.Load ("Sound/Shoot") as AudioClip;
		takeHit = Resources.Load ("Sound/TakeHit") as AudioClip;
		enemyDie = Resources.Load ("Sound/Die") as AudioClip;
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
		playerSoundFXSource.PlayOneShot (shoot);
	}

	public void PlaySound(string source, string sound, bool oneShot)
	{
		AudioSource s = null;
		AudioClip c = null;

		if(sound == "Shoot")
			c = shoot;
		if(sound == "Die")
			c = enemyDie;
		if(sound == "TakeHit")
			c = takeHit;


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
			c = mainmenu;
		else if(type == "Stage")
			c = stageMusic[i];
		else if(type == "Boss")
			c = bossMusic[i];

		bgMusicSource.clip = c;
		bgMusicSource.loop = true;
		ResumeMusic();
	}

}
