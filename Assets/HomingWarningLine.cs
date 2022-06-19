using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingWarningLine : MonoBehaviour
{
    LineRenderer lineRenderer;

    void Awake(){
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update(){
        lineRenderer.SetPosition(0, transform.position);
        if(!GetComponentInParent<BulletMovement>().active && gameObject.activeSelf) gameObject.SetActive(false);
    }
    public void ActivateLine(){
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, Game.control.player.transform.position);
        IEnumerator animateLine = AnimateLine();
        StartCoroutine(animateLine);
    }

    public void DisableLine(){
        gameObject.SetActive(false);
    }
    IEnumerator AnimateLine(){
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(.3f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(.3f);
        gameObject.SetActive(false);
    }
}
