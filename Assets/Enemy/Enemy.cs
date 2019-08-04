﻿using System;
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
    bool isSleep = true;
    public float waitingTime = 1f;
    private void Start()
    {
        player = Player.instance.transform;
    }

    private void OnDestroy()
    {
        Game.UnregisterEnemy(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
            other.gameObject.GetComponent<Player>()
                .GetHit(lastVector);
        
        StartCoroutine(OppositeMovement());
    }

    private int directionMult = 1;
    private IEnumerator OppositeMovement()
    {
        directionMult = -1;
        yield return new WaitForSeconds(0.1f);
        isHorisontalMove = !isHorisontalMove;
        currentPause = waitingTime/2;
        directionMult = 1;
    }

    private Vector3 lastVector;
    private float currentPause = 0;
    void FixedUpdate()
    {
        if (currentPause > 0)
        {
            currentPause -= Time.deltaTime;
            return;
        }
        
        if (isSleep)
            return;
        
        var targetDelta = isHorisontalMove
            ? player.position.x - transform.position.x
            : player.position.y - transform.position.y;
        
        var distance = Mathf.Abs(targetDelta);
        if (distance < 0.1f)
        {
            isHorisontalMove = !isHorisontalMove;
            currentPause = waitingTime;
            return;
        }

        targetDelta *= Time.deltaTime * Speed * directionMult; 
        if (Mathf.Abs(targetDelta) > maxSpeed)
            targetDelta = maxSpeed * Mathf.Sign(targetDelta);

        lastVector = targetDelta *
                         (isHorisontalMove ? Vector3.right : Vector3.up);
        transform.position += lastVector;
    }

    public void WakeUp()
    {
        isSleep = false;
    }
}
