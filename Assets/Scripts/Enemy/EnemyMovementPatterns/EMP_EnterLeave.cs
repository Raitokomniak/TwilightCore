using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_EnterLeave : EnemyMovementPattern
{
    public EMP_EnterLeave(){}

    public EMP_EnterLeave(Vector3 _spawnPosition, float _stayTime){
        spawnPosition = _spawnPosition;
        //enterDir = Game.control.enemyLib.center;
        stayTime = _stayTime;
        speed = 3f;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("EnterLeave", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        UpdateDirection(enterDir.x, enterDir.y);
		yield return new WaitUntil (() => CheckIfReachedDestination (m) == true);
		yield return new WaitForSeconds (stayTime);
		UpdateDirection (leaveDir.x, leaveDir.y);
    }
}
