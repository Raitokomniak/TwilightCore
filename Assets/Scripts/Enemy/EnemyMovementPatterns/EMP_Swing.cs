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
        UpdateDirection("B3");
		yield return new WaitForSeconds (1f);
        m.SmoothAcceleration(3);
		RotateOnAxis (5f * speed);
		yield return new WaitForSeconds (5f);
		rotateOnAxis = false;
    }
}
