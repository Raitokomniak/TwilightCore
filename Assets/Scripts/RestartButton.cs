using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void Restart(){
        Game.control.stage.StartStage(false, "Level1");
    }
}
