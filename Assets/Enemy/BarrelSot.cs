using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSot : MonoBehaviour
{
    public Vector3 target;
    public Destroyable LandedVersion;

    public float movSpeed = 30;
    public float rotationSpeed = 2f;

    void Update()
    {
        transform.Translate((target - transform.position) * Time.deltaTime * movSpeed, 
            Space.World);
        if (Vector3.Distance(transform.position,target) < 0.1f)
        {
            Destroy(gameObject);
            Instantiate(LandedVersion, target, Quaternion.identity)
                .ExtraDrop = -1;
        }
        
        transform.Rotate(0,0,Time.deltaTime * rotationSpeed
                             ,Space.Self);        
    }
}
