using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerePosition : MonoBehaviour
{
    public Transform Player;
    public Transform Target;
    private float CameraZPosition;
    public Collider2D CameraBounds;
    private Vector3 CameraMinShift;
    private Vector3 CameraMaxShift;
    public Vector3 camShift = Vector3.zero;
    
    private void Awake()
    {
        CameraZPosition = transform.position.z;
        lastTarget = transform.position;
    }

    private void Start()
    {
        var minShift = Camera.main.ScreenToWorldPoint(Vector3.zero) - transform.position;
        var maxShift = Camera.main.ScreenToWorldPoint(
                           new Vector3(Screen.width, Screen.height, 0)
                       ) - transform.position;

        CameraMinShift = (CameraBounds.bounds.center + CameraBounds.bounds.min) - minShift;
        CameraMaxShift = (CameraBounds.bounds.center + CameraBounds.bounds.max) - maxShift;
    }

    public float smoothTime = 1;
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastTarget;
    public bool isShaking = false;

    void LateUpdate()
    {
        if (isShaking)
            return;
        
        var target = new Vector3(
            (Player.position.x + Target.position.x)/2,
            (Player.position.y + Target.position.y)/2, 
            CameraZPosition)
            + camShift;
        
        if (target.x < CameraMinShift.x ||target.x > CameraMaxShift.x)
            target.x = lastTarget.x;
        if (target.y < CameraMinShift.y ||target.y > CameraMaxShift.y)
            target.y = lastTarget.y;
        
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
        lastTarget = target;
    }
}
