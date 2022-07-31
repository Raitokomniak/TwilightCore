using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Boss : MonoBehaviour
{
    public Slider bossHealthSlider;
    // REINSTATE MINI HEALTH BAR
//	public GameObject bossMiniHealthBar;
//	GameObject miniHealthBar;
//	List<GameObject> bars;
	public GameObject bossNamePanel;
	public TextMeshProUGUI bossName;

	public GameObject bossInvulnerableImage;
	public GameObject bossX;
	public GameObject bossSpellToast;
	public TextMeshProUGUI bossSpellText;
	public Text bossTimer;

    void Awake(){
        ToggleBossHealthSlider(false, 0, "");
    }
    
	public void HideUI(){
		bossHealthSlider.gameObject.SetActive(false);
		bossTimer.gameObject.SetActive(false);
		bossNamePanel.SetActive (false);
		bossSpellToast.SetActive (false);
        ToggleBossXPos(false);
	}


    public void RevealUI(){
        bossHealthSlider.gameObject.SetActive(true);
		bossTimer.gameObject.SetActive(true);
		bossNamePanel.SetActive (true);
		bossSpellToast.SetActive (true);
        ToggleBossXPos(true);
    }

    public void UpdateBossXPos(float posX){
		bossX.transform.position = new Vector3 (posX, bossX.transform.position.y, 0);
	}

	public void UpdateBossXPos(float posX, bool teleporting){
		UpdateBossXPos(posX);
		ToggleBossXPos(!teleporting);
	}

	public void ToggleBossXPos(bool toggle){
		bossX.SetActive(toggle);
	}

	public void HideBossTimer(){
		bossTimer.gameObject.SetActive(false);
	}
	public void StartBossTimer(float time){
		bossTimer.gameObject.SetActive(true);
		bossTimer.text = time.ToString("F1");
	}

	public void UpdateBossTimer(float value){
		bossTimer.text = value.ToString("F1");
	}

	public void ToggleBossHealthSlider(bool value, float maxHealth, string name){
		ToggleBossXPos(value);
		bossName	.gameObject.SetActive(value);
		bossHealthSlider.gameObject.SetActive(value);
		bossHealthSlider.maxValue = maxHealth;
		bossHealthSlider.value = maxHealth;
		bossNamePanel.SetActive (value);
		bossName.text = name;
	}


	public void UpdateBossHealth(float h, float maxHealth)
	{
		bossHealthSlider.value = h;
        bossHealthSlider.maxValue = maxHealth;
	}

	public void ToggleInvulnerable(bool toggle){
		bossInvulnerableImage.SetActive(toggle);
	}

/*
	public void UpdateBossHealthBars(int h){
		// activate wanted amount
	}*/

    public void ShowActivatedPhase(string text){
		bossSpellToast.SetActive (true);
		bossSpellText.text = text;
	}

	public void HideSpell(){
		bossSpellToast.SetActive (false);
	}
}
