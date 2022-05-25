using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Teleport : EnemyMovementPattern
{
    public EMP_Teleport(){
        teleport = true;
        speed = 3f;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("Teleport", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        UpdateDirection(targetPos.x, targetPos.y);
        m.teleporting = true;
	    m.EnableSprite(false);
		yield return new WaitUntil (() => CheckIfReachedDestination (m) == true);
		m.EnableSprite(true);
		m.teleporting = false;
    }
}
