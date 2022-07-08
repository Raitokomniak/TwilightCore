using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LeftSidePanel : MonoBehaviour
{
	public Slider dayCoreSlider;
	public Slider nightCoreSlider;

	public Transform dayMultiplierTexts;
	public Transform nightMultiplierTexts;

    IEnumerator depleteCoreChargeRoutine;


    public void HighLightCoreInUse(string core)
	{
		Image dayOverLay = dayCoreSlider.transform.GetChild (1).GetComponent<Image> ();
		Image dayFill = dayCoreSlider.transform.GetChild (0).GetChild (0).GetComponent<Image> ();
		Image nightOverLay = nightCoreSlider.transform.GetChild (1).GetComponent<Image> ();
		Image nightFill = nightCoreSlider.transform.GetChild (0).GetChild (0).GetComponent<Image> ();

		Color dayFillColor = Color.yellow;
		Color nightFillColor = Color.magenta;

		if (core == "Day") {
			dayOverLay.color = new Color (1, 1, 1, 1f);
			dayFill.color = dayFillColor;

			nightOverLay.color = new Color (1, 1, 1, 0.5f);
			nightFill.color = nightFillColor - new Color (0, 0, 0, .5f);
		} else if (core == "Night") {
			nightOverLay.color = new Color (1, 1, 1, 1f);
			nightFill.color = nightFillColor;
				
			dayOverLay.color = new Color (1, 1, 1, 0.5f);
			dayFill.color = dayFillColor - new Color (0, 0, 0, .5f);
		}
	}

	public void SetSliderMaxValues(){
		dayCoreSlider.maxValue = Game.control.player.special.coreCap;
		nightCoreSlider.maxValue = Game.control.player.special.coreCap;
	}


	public void EmptyCores(){
		dayCoreSlider.value = 0;
		nightCoreSlider.value = 0;
		UpdatePower("Day", 0);
		UpdatePower("Night", 0);
	}

	public void UpdatePower(string core, int power){
		Transform textContainer = null;
		if(core == "Day") textContainer = dayMultiplierTexts;
		if(core == "Night") textContainer = nightMultiplierTexts;
		
		TextMeshProUGUI[] texts = textContainer.GetComponentsInChildren<TextMeshProUGUI>();
		foreach(TextMeshProUGUI text in texts){
			 text.color = new Color(1,1,1,.2f);
		}

		for(int i = 0; i < power; i++){
			textContainer.GetChild(power - 1).GetComponent<TextMeshProUGUI>().color = new Color(1,1,1,1);
		}
	}

	public void UpdateCoreCharge(string core, int gainedCharge){
		if 		(core == "Day")   dayCoreSlider.value   += gainedCharge;
		else if (core == "Night") nightCoreSlider.value += gainedCharge;
	}

    public void DepleteCoreCharge(string core, float specialAttackTime, int current, int threshold){
		depleteCoreChargeRoutine = DepleteCoreChargeRoutine(core, specialAttackTime, current, threshold);
		StartCoroutine(depleteCoreChargeRoutine);
	}

	public IEnumerator DepleteCoreChargeRoutine(string core, float specialAttackTime, int current, int threshold){
		Slider slider = null;
		if (core == "Day") 			slider = dayCoreSlider;
		else if (core == "Night")	slider = nightCoreSlider;
		
		for (float i = current; i > threshold; i -= 1f) {
			slider.value = i;
			yield return new WaitForSeconds (0.008f * specialAttackTime);
		}
	}
}
