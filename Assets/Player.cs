using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public bool UseSmoothMovement = false;
    public float InputMultiplicator = 0.4f;
    public Gun Gun;
   private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //CheckPoint
    }

    void Update()
    {
        var movement = Vector3.zero;

        if (UseSmoothMovement)
        {
            if (Input.GetKey(KeyCode.S))
                movement += Vector3.down;
            if (Input.GetKey(KeyCode.W))
                movement += Vector3.up;
            if (Input.GetKey(KeyCode.A))
                movement += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                movement += Vector3.right;

            movement *= InputMultiplicator;
        }
        else
        {
            movement = InputMultiplicator *
                       new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }

        if (movement.magnitude > InputMultiplicator)
            movement = movement.normalized * InputMultiplicator;
            
        transform.position += movement*Time.deltaTime;
    }
}
