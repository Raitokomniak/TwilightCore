using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_RainDrop : BulletMovementPattern
{
    public BMP_RainDrop(){}

    //FOR SETTING UP IN WAVE INITS
    public BMP_RainDrop(Pattern p){
        pattern = p;
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
        return CopyValues("RainDrop", bmp);
    }

    public override IEnumerator ExecuteRoutine(){
        movementSpeed = 5f;
        SmoothAcceleration ();
        
        //SPAWNS OOB SO WAIT UNTIL IS IB AND THEN SET TO BE DESTROYED 
        
        yield return new WaitUntil(() => bullet.GetComponent<BulletMovement>().CheckBounds() == true);
        dontDestroy = false;

        rotation = bullet.transform.rotation;
        isMoving = true;
        
        yield return null;
    }
}
