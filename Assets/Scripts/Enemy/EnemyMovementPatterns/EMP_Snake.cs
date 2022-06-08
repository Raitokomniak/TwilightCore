using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Snake : EnemyMovementPattern
{
    //THIS IS THE DOUBLE SNAKE, NORMAL SNAKE IS JUST ENTERLEAVE

    public bool doubleSnake; //THIS IS TRIPLESNAKE
    
    public EMP_Snake(){}

    public EMP_Snake(Vector3 _spawnPosition, float _stayTime, int _direction){
        spawnPosition = _spawnPosition;
        stayTime = _stayTime;
        speed = 3f;
        movementDirection = _direction; //1 = right  to left,,,  -1 = left to right
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
       return CopyValues("Snake", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        UpdateDirection(enterDir.x, enterDir.y);
		yield return new WaitUntil (() => HasReachedDestination (m) == true);
        if(movementDirection == 1) UpdateDirection (4f, 6f);
        else UpdateDirection (-14f, 6f);

		yield return new WaitForSeconds(stayTime);

        if(doubleSnake){
            if(movementDirection == 1) UpdateDirection (-14f, 6f);
            else UpdateDirection (4f, 6f);
        }

        yield return new WaitUntil (() => HasReachedDestination (m) == true);
		UpdateDirection(leaveDir.x, leaveDir.y);
    }
}
