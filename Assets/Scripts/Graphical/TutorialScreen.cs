using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    public Image tutImage;

    public Sprite[] tutImages;
    int tutIndex = 0;
    public bool tutorialOn;

   void LateUpdate(){
    if(tutorialOn){
        if(Input.GetKeyDown(KeyCode.Z)){
            NextImage();
        }
    }
   }

   void NextImage(){
        tutIndex++;
        if(tutIndex >= tutImages.Length) {
            tutorialOn = false;
            tutIndex = 0;
            if(Game.control.GetCurrentScene() == "MainMenu") Game.control.menu.Menu("MainMenu");
            else Game.control.menu.Menu("PauseMenu");
        }
        else {
            tutImage.sprite = tutImages[tutIndex];
        }
   }
}
