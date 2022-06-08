using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Rock : EnemyMovementPattern
{
    public EMP_Rock(){
        speed = 3f;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("Rock", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        VectorLib lib = Game.control.vectorLib;
        //check for infinity bool ?
        UpdateDirection(lib.GetVector("D3"));
        yield return new WaitUntil (() => HasReachedDestination (m) == true);
        yield return new WaitForSeconds(1f);
		UpdateDirection(lib.GetVector("H3"));
		yield return new WaitUntil (() => HasReachedDestination (m) == true);
    }
}
