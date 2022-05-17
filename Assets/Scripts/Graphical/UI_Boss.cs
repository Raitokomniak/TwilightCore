using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Boss : MonoBehaviour
{
    public Slider bossHealthSlider;
//	public GameObject bossMiniHealthBar;
//	GameObject miniHealthBar;
//	List<GameObject> bars;
	public GameObject bossNamePanel;
	public TextMeshProUGUI bossName;

	public GameObject bossInvulnerableImage;
	public GameObject bossX;
	public GameObject bossSpellToast;
	public TextMeshProUGUI bossSpellText;
	public TextMeshProUGUI bossTimer;

	//THESE MIGHT NOT BELONG HERE, MOVE THEM SOMEWHERE ELSE
	float bossStayTimer;
	float bossStayTime;
	bool bossStayTimerOn;

    void Awake(){
        ToggleBossHealthSlider(false, 0, "");
		HideBossTimer();
    }
    
	public void HideUI(){
		bossHealthSlider.gameObject.SetActive(false);
		bossTimer.gameObject.SetActive(false);
		bossNamePanel.SetActive (false);
	}

    void Update(){
		if(bossStayTimerOn) {
			if(bossStayTimer > 0){
				bossStayTimer-=Time.deltaTime;
				UpdateBossTimer(bossStayTimer);
			}
			else {
				HideBossTimer();
			}
		}
    }

    public void UpdateBossXPos(float posX, bool enabled){
		if (enabled)
			bossX.SetActive (true);
		else
			bossX.SetActive (false);
		
		bossX.transform.position = new Vector3 (posX, bossX.transform.position.y, 0);
	}

	public void HideBossTimer(){
		bossStayTimerOn = false;
		bossTimer.gameObject.SetActive(false);
	}
	public void StartBossTimer(float time){
		bossTimer.gameObject.SetActive(true);
		bossStayTime = time;
		bossStayTimer = time;
		bossTimer.text = time.ToString("F1");
		bossStayTimerOn = true;
	}

	public void UpdateBossTimer(float value){
		bossTimer.text = value.ToString("F1");
	}

	public void ToggleBossHealthSlider(bool value, float maxHealth, string name){
		bossX.SetActive (value	);
		bossName	.gameObject.SetActive(value);
		bossHealthSlider.gameObject.SetActive(value);
		bossHealthSlider.maxValue = maxHealth;
		bossHealthSlider.value = maxHealth;
		bossNamePanel.SetActive (value);
		bossName.text = name;
	}


	public void UpdateBossHealth(float h)
	{
		bossHealthSlider.value = h;
	}

	public void ToggleInvulnerable(bool toggle){
		bossInvulnerableImage.SetActive(toggle);
	}

/*
	public void UpdateBossHealthBars(int h){
		if (h > 1) {
			if (miniHealthBar == null)
				for (int i = 0; i < h - 1; i++) {
					miniHealthBar = Instantiate (bossMiniHealthBar, Vector3.zero, transform.rotation) as GameObject;
					miniHealthBar.transform.SetParent (bossHealthSlider.transform);
					miniHealthBar.transform.position = new Vector3 (130 + i * 20, 630, 0);
					bars = new List<GameObject> ();
					bars.Add (miniHealthBar);
				}
			else {
				
				/*for (int i = 0; i <= h; i++) {
					Destroy (bars [i]);

				}
			}
		} else {
			Destroy (miniHealthBar);
		}
	}*/

    public void ShowActivatedPhase(string text)
	{
		StartCoroutine (_ShowActivatedBossPhase (text));
	}

	IEnumerator _ShowActivatedBossPhase(string text){
		bossSpellToast.SetActive (true);
		bossSpellText.text = text;

		int dir = -1;

		for (int j = 0; j < 2; j++) {
			for (int i = 0; i < 60; i+=1) {
				bossSpellToast.transform.position += new Vector3 (dir + (7 * dir), 0, 0);
				yield return new WaitForSeconds (0.005f);
			}
			dir = 1;
			yield return new WaitForSeconds (5f);
		}
		bossSpellToast.SetActive (false);
	}
}
