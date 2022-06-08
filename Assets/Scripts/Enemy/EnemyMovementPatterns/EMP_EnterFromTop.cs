using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_EnterFromTop : EnemyMovementPattern
{
   // public EMP_EnterFromTop(){}

    public EMP_EnterFromTop(){
        spawnPosition = Game.control.vectorLib.centerTopOOB;
        enterDir = Game.control.vectorLib.centerTop;
        speed = 3f;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("EnterFromTop", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        UpdateDirection(enterDir.x, enterDir.y);
		yield return new WaitUntil (() => HasReachedDestination (m) == true);
    }
}
