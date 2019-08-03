using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private bool isHorisontalMove = true;
    private Vector3 TargetPosition;
    public float maxSpeed = 7f;
    public float Speed = 3f;
    public bool isSleep = false;
    public float movingForwardAt = 1f;
    private void Start()
    {
        player = Player.instance.transform;
        transform.parent.GetComponent<Room>().RegisterEnemy(this);
    }

    private void OnDestroy()
    {
        Room.UnregisterEnemy(this);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        isHorisontalMove = !isHorisontalMove;
    }

    void FixedUpdate()
    {
        if (isSleep)
            return;
        
        var targetDelta = isHorisontalMove
            ? player.position.x - transform.position.x
            : player.position.y - transform.position.y;
        
        var distance = Mathf.Abs(targetDelta);
        if (distance < 0.1f)
        {
            isHorisontalMove = !isHorisontalMove;
            return;
        }

        targetDelta *= Time.deltaTime * Speed; 
        if (Mathf.Abs(targetDelta) > maxSpeed)
            targetDelta = maxSpeed * Mathf.Sign(targetDelta);

        transform.position += targetDelta * 
                              (isHorisontalMove ? Vector3.right : Vector3.up);
    }

    public void Sleep()
    {
        isSleep = true;
    }
    
    public void WakeUp()
    {
        isSleep = false;
    }
}
