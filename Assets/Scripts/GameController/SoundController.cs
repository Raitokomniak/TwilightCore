using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public static SoundController soundC;

	//public bool disableMusic;
	public bool disableSound;

	AudioSource playerSoundFXSource;
	AudioSource enemySoundFXSource;
	AudioSource bgMusicSource;
	public bool bossMusicPlaying;

	//Music
	AudioClip mainmenu;
	AudioClip stage1;
	AudioClip boss1;

	//Sound
	AudioClip shoot;
	AudioClip enemyDie;
	AudioClip takeHit;

	//Volumes
	public float SFXVolume;

	void Awake(){
		mainmenu = Resources.Load ("Sound/Music/MainMenu") as AudioClip;
		//stage1 = Resources.Load ("Sound/Music/Stage1") as AudioClip;
		stage1 = Resources.Load("Sound/Music/asura-who-remain-asura_piano") as AudioClip; //THIS IS JUST FOR FUN
		boss1 = Resources.Load ("Sound/Music/Boss1") as AudioClip;

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

		bossMusicPlaying = false;

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

		switch (sound) {
		case "Shoot":
			c = shoot;
			break;
		case "Die":
			c = enemyDie;
			break;
		case "TakeHit":
			c = takeHit;
			break;
		}

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

	public void StopMusic()
	{
		bgMusicSource.Stop ();
	}

	public void PlayMusic(string music)
	{
		if (music.Contains ("Boss")) {
			bossMusicPlaying = true;
		}

		AudioClip c = null;

		switch (music) {
		case "MainMenu":
			c = mainmenu;
			break;
		case "Stage1":
			c = stage1;
			break;
		case "Boss1":
			c = boss1;
			break;
		}
		bgMusicSource.clip = c;
		if (music == "MainMenu") {
			bgMusicSource.loop = true;
		}

		ResumeMusic();
		
	}

}
