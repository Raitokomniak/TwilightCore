using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {
	public bool paused;
	float initialTime;

	void Awake () {
		paused = false;
		initialTime = Time.timeScale;
	}

	public void HandlePause()
	{
		if(Game.control.stageHandler.stageOn){
			if(!paused) Pause();
			else Unpause (true);
		}
	}
	
	void Pause(){
		paused = true;
		Game.control.sound.PauseMusicAndEffects ();
		Game.control.ui.TogglePauseScreen(true);
		ResetTimeScale (false);
	}

	public void Unpause(bool resumeMusic){
		if(resumeMusic) Game.control.sound.ResumeMusicAndEffects ();
		paused = false;
		Game.control.ui.TogglePauseScreen(false);
		ResetTimeScale (true);
	}

	public void ResetTimeScale(bool toggled)
	{
		if (toggled) {
			Time.timeScale = initialTime;
		} else {
			initialTime = Time.timeScale;
			Time.timeScale = 0;
		}
	}
}
