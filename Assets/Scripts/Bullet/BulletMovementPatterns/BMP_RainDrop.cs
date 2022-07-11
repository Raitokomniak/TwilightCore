using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_RainDrop : BulletMovementPattern
{
    public BMP_RainDrop(){}

    //FOR SETTING UP IN WAVE INITS
    public BMP_RainDrop(Pattern p, float _speed){
        pattern = p;
        movementSpeed = _speed;
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
        return CopyValues(bmp, new BMP_RainDrop());
    }

    public override IEnumerator ExecuteRoutine(){
        bulletMovement = bullet.GetComponent<BulletMovement>();

        SmoothAcceleration ();
        
        //SPAWNS OOB SO WAIT UNTIL IS IB AND THEN SET TO BE DESTROYED 
        
        yield return new WaitUntil(() => bulletMovement.CheckBounds() == true);
        dontDestroy = false;

        rotation = bullet.transform.rotation;
        isMoving = true;
        
        yield return null;
    }
}
