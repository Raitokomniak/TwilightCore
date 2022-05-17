using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniToast : MonoBehaviour
{
    public Canvas fxCanvas;
    public Object miniToastPrefab;
    

    public void PlayScoreToast(int value){
        string toastText = "+" + value.ToString() + " score";
        Toast(toastText);
    }

    public void PlayCorePointToast(int value, string core){
        string toastText = "+" + value.ToString() + " " + core + " point";
        Toast(toastText);
    }

    void Toast(string toastText){
        GameObject miniToastObject = Instantiate(miniToastPrefab) as GameObject;
        miniToastObject.transform.SetParent(fxCanvas.transform);
        miniToastObject.transform.position = transform.position + new Vector3(1f,0,0);
        miniToastObject.GetComponent<TextMeshProUGUI>().text = toastText;
        IEnumerator toastRoutine = ShowToastRoutine(miniToastObject);
        StartCoroutine(toastRoutine);
    }

    IEnumerator ShowToastRoutine(GameObject toastObject){
         for(float i = 1; i > 0; i-=Time.deltaTime){
            toastObject.transform.position += new Vector3(0,Time.deltaTime * 2,0);
            yield return new WaitForSeconds(Time.deltaTime);
            toastObject.GetComponent<TextMeshProUGUI>().color = new Color(1,1,1,i);
        }
        Destroy(toastObject);
    }
}
