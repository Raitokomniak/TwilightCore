using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniToast : MonoBehaviour
{
    Canvas fxCanvas;
    public Object miniToastPrefab;
    public Sprite scoreSprite;
    public Sprite dayCoreSprite;
    public Sprite nightCoreSprite;
    public Sprite XPSprite;

    string previousType;

    bool readyForToast = true;
    float toastTimer;
    float toastCD = .2f;


    void Awake(){
        fxCanvas = GetComponent<Canvas>();
    }
    void Update(){
        if(!readyForToast) ToastTimer();

    }

    void ToastTimer(){
        if(toastTimer >= toastCD) readyForToast = true;
        else {
            readyForToast = false;
            toastTimer+=Time.deltaTime;
        }
    }


    public void PlayToast(string type){
        previousType = type;

        if(!readyForToast) return;

        GameObject o = InstantiateToast();
        SpriteRenderer spriteRenderer = o.GetComponent<SpriteRenderer>();
        
        if     (type == "Score")     spriteRenderer.sprite = scoreSprite;
        else if(type == "XP")        spriteRenderer.sprite = XPSprite;
        else if(type == "DayCore")   spriteRenderer.sprite = dayCoreSprite;
        else if(type == "NightCore") spriteRenderer.sprite = nightCoreSprite;
        
        IEnumerator toastRoutine = ShowToastRoutine(o);
        StartCoroutine(toastRoutine);
    }

    GameObject InstantiateToast(){
        GameObject o = Instantiate(miniToastPrefab) as GameObject;
        SpriteRenderer spriteRenderer = o.GetComponent<SpriteRenderer>();
        o.transform.SetParent(fxCanvas.transform);
        o.transform.position = transform.position + new Vector3(Random.Range(-1, 2),Random.Range(-1, 1),0);
       
        //INSTEAD OF SCALING, MAKE THE SPRITES THE SIZE YOU WANT THEM TO BE
        if(previousType == "Score") o.transform.localScale = new Vector3(1.5f, 1.5f,1.5f);
        else o.transform.localScale = new Vector3(2,2,2);
        return o;
    }

    IEnumerator ShowToastRoutine(GameObject toastObject){
        toastTimer = 0;
        readyForToast = false;

        SpriteRenderer r = toastObject.GetComponent<SpriteRenderer>();
         for(float i = 1; i > 0; i-=Time.deltaTime * 2){
            toastObject.transform.position += new Vector3(0,Time.deltaTime * 2,0);
            yield return new WaitForSeconds(Time.deltaTime);
            r.color = new Color(1,1,1,i);
        }
        Destroy(toastObject);
    }
}
