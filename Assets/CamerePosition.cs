using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerePosition : MonoBehaviour
{
    public Transform Player;
    public Transform Target;

    private float CameraZPosition;

    private void Awake()
    {
        CameraZPosition = transform.position.z;
    }

    public float smoothTime = 1;
    private Vector3 velocity = Vector3.zero;
    void LateUpdate()
    {
        var target = new Vector3(
            (Player.position.x + Target.position.x)/2,
            (Player.position.y + Target.position.y)/2, 
            CameraZPosition);
        
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }
}
