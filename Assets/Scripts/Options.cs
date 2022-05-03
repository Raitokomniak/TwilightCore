using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{


    void Start()
    {

    }

    public void AutoScrollOption(){
       Game.control.dialog.ToggleAutoScroll(!Game.control.dialog.autoScroll);
       if(Game.control.dialog.autoScroll) Game.control.ui.UpdateOptionSelection(0,"ON");
       else Game.control.ui.UpdateOptionSelection(0,"OFF");
    }

    public void UpdateOption(bool increase, int index){
        if(index == 0) {
            AutoScrollOption();
        }
    }
}
