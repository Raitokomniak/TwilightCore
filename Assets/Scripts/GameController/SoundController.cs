using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public static SoundController soundC;

	AudioSource playerSoundFXSource;
	AudioSource enemySoundFXSource;

	AudioSource bgMusicSource;

	AudioClip mainMenu;

	AudioClip stage1;
	AudioClip stage2;
	AudioClip boss1;
	AudioClip boss2;


	AudioClip playerTakeHit;
	AudioClip playerDead;
	AudioClip playerSwitchShoot;

	public bool bossMusicPlaying;

	// Use this for initialization
	void Start () {
	
	}

	void Awake(){
		playerSoundFXSource = this.gameObject.AddComponent<AudioSource> ();
		enemySoundFXSource = this.gameObject.AddComponent<AudioSource> ();
		bgMusicSource = this.gameObject.AddComponent<AudioSource> ();

		playerSoundFXSource.volume = 0.1f;
		enemySoundFXSource.volume = 0.1f;



	}

	public void InitSound(){
		bossMusicPlaying = false;
	}

	// Update is called once per frame
	void Update () {
	
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

	public void PlaySound(string source, string sound, bool oneShot)
	{
		AudioSource s = new AudioSource();
		AudioClip c = Resources.Load ("Sound/" + sound) as AudioClip;

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
		bgMusicSource.clip = Resources.Load ("Sound/" + music) as AudioClip;
		if (music == "MainMenu") {
			bgMusicSource.loop = true;
		}
		bgMusicSource.Play ();
	}

}
