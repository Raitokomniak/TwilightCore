using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_INTRO : MonoBehaviour
{
    public Image bg;
    public GameObject container;
    public TextMeshProUGUI[] paragraphs;
    int paraIndex = -1;

    public void Init(){
        bg.color = new Color (0, 0, 0, 1);
        foreach(TextMeshProUGUI t in paragraphs) t.text = "";
        container.SetActive(true);
    }

    public void UpdateParagraph(string text){
        paraIndex++;
        if(paraIndex > 3) {
            paraIndex = 0;
            foreach(TextMeshProUGUI t in paragraphs) t.text = "";
        }
        paragraphs[paraIndex].text = text;
        IEnumerator animateRoutine = AnimateParagraph(paragraphs[paraIndex]);
        StartCoroutine(animateRoutine);
    }

    public void ForceFadeIn(){
        if(paraIndex == -1) return;
        paragraphs[paraIndex].color = new Color (1, 1, 1, 1);
        StopAllCoroutines();
    }

    IEnumerator AnimateParagraph(TextMeshProUGUI para){
        para.color = new Color (1, 1, 1, 0);

		for (float i = 0; i < 1; i+=.01f) {
			para.color = new Color (1, 1, 1, i);
			yield return new WaitForSeconds(Time.deltaTime * 5);
		}
        yield return null;
    }

    public void Hide(){
        foreach(TextMeshProUGUI t in paragraphs)t.text = "";
        IEnumerator fadeRoutine = FadeContainer();
        StartCoroutine(fadeRoutine);
    }

    IEnumerator FadeContainer(){
        for (float i = 1; i > 0; i-=.1f) {
			bg.color = new Color (0, 0, 0, i);
			yield return new WaitForSeconds(Time.deltaTime * 10);
		}
        yield return new WaitForSeconds(.3f);
        Game.control.stageHandler.intro.EndIntro();
        container.SetActive(false);
    }

}
