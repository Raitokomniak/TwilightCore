using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_LoadingScreen : MonoBehaviour
{
    public TextMeshProUGUI loadingText;

    public void ShowLoadingScreen(){
        //IEnumerator loadingTextRoutine = LoadingTextRoutine();
        //StartCoroutine(loadingTextRoutine);
    }

    IEnumerator LoadingTextRoutine(){
        loadingText.text = "Loading";
        yield return new WaitForSeconds(0.3f);
        loadingText.text = "Loading.";
        yield return new WaitForSeconds(0.3f);
        loadingText.text = "Loading..";
        yield return new WaitForSeconds(0.3f);
        loadingText.text = "Loading...";
        yield return new WaitForSeconds(0.3f);
    }
}
