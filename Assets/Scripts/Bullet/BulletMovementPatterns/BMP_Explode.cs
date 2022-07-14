using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_Explode : BulletMovementPattern
{
    public BMP_Explode(){}

    public BMP_Explode(Pattern p, float _movementSpeed){
        pattern = p;
        movementSpeed = _movementSpeed;
    }

    public BMP_Explode(Pattern p, float _movementSpeed, bool _rotateOnAxis, bool _moveWhileRotates, bool _holdShape){
        pattern = p;
        movementSpeed = _movementSpeed;
        rotateOnAxis = _rotateOnAxis;
        moveWhileRotates = _moveWhileRotates;
        holdShape = _holdShape;
        randomForcedXDir = Random.Range(-1, 2);   
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern _bmp){
       return CopyValues(_bmp, new BMP_Explode());
    }

    public override IEnumerator ExecuteRoutine(){
        bulletMovement = bullet.GetComponent<BulletMovement>();
        
        StartMoving (14);
        rotation = bullet.transform.rotation;
        if(startHoming) FindPlayer(bullet);
        if(rotateOnAxis) {
            if(pattern.enemyShoot!= null) centerPoint = pattern.enemyShoot.transform.position;
            if(rotationDir == 1 || rotationDir == -1)  RotateOnAxis (rotationDir, axisRotateSpeed);
            else RotateOnAxis (-1, axisRotateSpeed);
        }
        yield return null;
    }
}
