using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {
	public bool paused;

	float initialTime;

	// Use this for initialization
	void Awake () {
		paused = false;
		initialTime = Time.timeScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(Game.control.stageHandler.stageOn && Input.GetKeyDown(KeyCode.Escape))
		{
			HandlePause();
		}
	}

	public void HandlePause()
	{
		if(!paused)
		{
			Game.control.sound.PauseMusic ();
			paused = true;
			Game.control.ui.PauseScreen(true);

			ResetTimeScale (false);
		}
		else
		{
			Unpause ();
		}
		Game.control.menu.ToggleMenu (paused);
	}

	public void Unpause(){
		Game.control.sound.ResumeMusic ();
		paused = false;
		Game.control.ui.PauseScreen(false);
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
