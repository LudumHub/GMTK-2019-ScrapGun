using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private bool isHorisontalMove = true;
    public float maxSpeed = 0.001f;

    private void Start()
    {
        player = Player.instance.transform;
    }

    void Update()
    {
        var targetDelta = isHorisontalMove
            ? player.position.x - transform.position.x
            : player.position.y - transform.position.y;
        
        var distance = Mathf.Abs(targetDelta);
        if (distance < 0.1f)
        {
            isHorisontalMove = !isHorisontalMove;
            return;
        }

        if (Mathf.Abs(targetDelta) > maxSpeed)
            targetDelta = maxSpeed * Mathf.Sign(targetDelta);

        transform.position += (isHorisontalMove ? Vector3.right : Vector3.up) * targetDelta;
    }
}
