using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingWarningLine : MonoBehaviour
{

    // THIS IS NOT USED BUT MIGHT BE A COOL IDEA MAYBE
    // THE BIGGEST PROBLEM WAS THAT THE LINE DOESNT DISAPPEAR WHEN POOLED OR JUST ACTS WONKY

    LineRenderer lineRenderer;
    BulletMovement bulletMovement;

    void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
        bulletMovement = GetComponentInParent<BulletMovement>();
        lineRenderer.enabled = false;
        gameObject.SetActive(false);
    }

    void Update(){
        lineRenderer.SetPosition(0, transform.position);
        if(!bulletMovement.active && gameObject.activeSelf) lineRenderer.enabled = false;
    }
    public void ActivateLine(){
        /*
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, Game.control.player.transform.position);
        IEnumerator animateLine = AnimateLine();
        StartCoroutine(animateLine);*/
    }

    public void DisableLine(){
        //gameObject.SetActive(false);
        lineRenderer.enabled = false;
    }
    IEnumerator AnimateLine(){
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(.3f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(.3f);
        gameObject.SetActive(false);
    }
}
