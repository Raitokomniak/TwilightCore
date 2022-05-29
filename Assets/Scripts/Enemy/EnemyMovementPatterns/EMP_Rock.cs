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
        //check for infinity bool ?
        UpdateDirection(-2f, 8f);
        yield return new WaitUntil (() => CheckIfReachedDestination (m) == true);
        yield return new WaitForSeconds(1f);
		UpdateDirection (-11f, 8f);
		yield return new WaitUntil (() => CheckIfReachedDestination (m) == true);
    }
}
