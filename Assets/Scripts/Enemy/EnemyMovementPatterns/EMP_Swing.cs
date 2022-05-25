using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Swing : EnemyMovementPattern
{
    public EMP_Swing(){ }

    public EMP_Swing(float _speed, int _direction){
        speed = _speed;
        movementDirection = _direction;
    }

    public override EnemyMovementPattern GetNewEnemyMovement(EnemyMovementPattern _emp){
        return CopyValues("Swing", _emp);
    }

    public override IEnumerator ExecuteRoutine(EnemyMovement m){
        UpdateDirection(Game.control.enemyLib.enterLeft.x, 8);
		yield return new WaitForSeconds (1f);
		RotateOnAxis (50f * speed);
		yield return new WaitForSeconds (4f);
		rotateOnAxis = false;
		
    }
}
