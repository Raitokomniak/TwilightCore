using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_LeftSidePanel : MonoBehaviour
{
   
	public Slider dayCoreSlider;
	public Slider nightCoreSlider;

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

	public void UpdateCoreCharge(string core, int updatedCharge){
		if (core == "Day") {
			dayCoreSlider.value = updatedCharge;
		} else if (core == "Night") {
			nightCoreSlider.value = updatedCharge;
		}
	}

    	public void DepleteCoreCharge(string core, float specialAttackTime, int current, int threshold){
		depleteCoreChargeRoutine = DepleteCoreChargeRoutine(core, specialAttackTime, current, threshold);
		StartCoroutine(depleteCoreChargeRoutine);
	}

	public IEnumerator DepleteCoreChargeRoutine(string core, float specialAttackTime, int current, int threshold){
		Slider slider = null;
		if (core == "Day")
			slider = dayCoreSlider;
		else if (core == "Night")
			slider = nightCoreSlider;
		
		for (float i = current; i > threshold; i -= 1f) {
			slider.value = i;
			yield return new WaitForSeconds (0.008f * specialAttackTime);
		}
	}
}
