using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_ZigZag : EnemyMovementPattern
{
    public EMP_ZigZag(){}

    public EMP_ZigZag(Vector3 _spawnPosition, float _stayTime, int direction){
        spawnPosition = _spawnPosition;
        movementDirection = direction;
        stayTime = _stayTime;
        speed = 3f;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("ZigZag", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        UpdateDirection(enterDir.x, enterDir.y);
		yield return new WaitUntil (() => HasReachedDestination (m) == true);
        if(movementDirection == 1){
			UpdateDirection (-14f, 6f);
			yield return new WaitUntil (() => HasReachedDestination (m) == true);
			UpdateDirection (4f, 2f);
		}
		else if(movementDirection == -1){
			UpdateDirection (4f, 6f);
			yield return new WaitUntil (() => HasReachedDestination (m) == true);
			UpdateDirection (-14f, 2f);
		}
		else if(movementDirection == 2){
			UpdateDirection (-14f, 6f);
			yield return new WaitUntil (() => HasReachedDestination (m) == true);
			UpdateDirection (4f, 2f);
			yield return new WaitUntil (() => HasReachedDestination (m) == true);
			UpdateDirection (-14f, 2f);
			yield return new WaitUntil (() => HasReachedDestination (m) == true);
			UpdateDirection (4f, 6f);
		}
		
        yield return new WaitUntil (() => HasReachedDestination (m) == true);
		
        UpdateDirection (leaveDir.x, leaveDir.y);
    }
}
