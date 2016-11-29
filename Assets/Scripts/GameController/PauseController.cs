using UnityEngine;
using System.Collections;

public class PauseController : MonoBehaviour {
	public bool paused;

	float initialTime;

	// Use this for initialization
	void Awake () {
		paused = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			HandlePause();
		}
	}

	public void HandlePause()
	{

		if(!paused)
		{
			GameController.gameControl.sound.PauseMusic ();
			paused = true;
			GameController.gameControl.ui.PauseScreen(true);

			ResetTimeScale (false);
		}
		else
		{
			Unpause ();
		}
		GameController.gameControl.menu.ToggleMenu (paused);
	}

	public void Unpause(){
		GameController.gameControl.sound.ResumeMusic ();
		paused = false;
		GameController.gameControl.ui.PauseScreen(false);
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
