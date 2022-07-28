using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMP_3DRotation : BulletMovementPattern
{
   public BMP_3DRotation(){}

    //FOR SETTING UP IN WAVE INITS
    public BMP_3DRotation(Pattern p, bool _xrot, bool _yrot){
        pattern = p;
        xAxisRotation = _xrot;
        yAxisRotation = _yrot;
    }

    public override BulletMovementPattern GetNewBulletMovement(BulletMovementPattern bmp){
        
        return CopyValues(bmp, new BMP_3DRotation());
    }

    public override IEnumerator ExecuteRoutine(){
        bulletMovement = bullet.GetComponent<BulletMovement>();
        movementSpeed = 5f;

        while(true){
            if(xAxisRotation)  rotation.eulerAngles +=  new Vector3(0.5f, 0, 0); //Quaternion.Euler(Time.deltaTime * movementSpeed * 10, bullet.transform.rotation.y, 0);
            if(yAxisRotation)  rotation.eulerAngles +=  new Vector3(0, 0.5f, 0); 
            yield return null;
        }
        /*
        SmoothAcceleration ();
        
        yield return new WaitUntil(() => bulletMovement.CheckBounds() == true);
        dontDestroy = false;

        if(xAxisRotation)  rotation = Quaternion.Euler(Time.deltaTime * movementSpeed * 10, bullet.transform.rotation.y, 0);
        //if(yAxisRotation)  rotation = Quaternion.Euler(bullet.transform.rotation.x, Time.deltaTime * movementSpeed * 10, 0);
        
        //rotation = bullet.transform.rotation;
        */
        yield return null;
    }
}
