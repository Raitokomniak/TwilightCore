using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_TestPattern :EnemyMovementPattern
{
    public EMP_TestPattern(){}

    public EMP_TestPattern(Vector3 _spawnPosition){
        spawnPosition = _spawnPosition;
        //smoothArc = true;
        speed = 3f;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("TestPattern", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        VectorLib lib = Game.control.vectorLib;
        previousPoint = m.transform.position;
        smoothArc = true;
        speed = 10f;

        UpdateDirection(lib.GetVector("I7"));
        yield return new WaitUntil (() => HasReachedDestination (m) == true);
        UpdateSlerpDirection(lib.GetVector("I7"), -1);
        yield return new WaitUntil (() => HasReachedDestination (m) == true);
        Debug.Log("reach");
        UpdateSlerpDirection(lib.GetVector("H5"), 1);
        yield return new WaitUntil (() => HasReachedDestination (m) == true);

        UpdateSlerpDirection(lib.GetVector("D3"), -1);
        yield return new WaitUntil (() => HasReachedDestination (m) == true);

        yield return null;
    }
}
