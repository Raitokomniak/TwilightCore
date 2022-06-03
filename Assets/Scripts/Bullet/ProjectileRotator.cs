using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRotator : MonoBehaviour
{
     public bool rotate = false;
	float rotationSpeed = 10f;

    void Update(){
       if(rotate) transform.Rotate (0, 0, Time.deltaTime * 300);
       else transform.rotation = Quaternion.Euler(0,0,0);
    }
}
