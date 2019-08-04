using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private bool isHorisontalMove = true;
    private Vector3 TargetPosition;
    public float maxSpeed = 7f;
    public float Speed = 3f;
    bool isSleep = true;
    public float waitingTime = 1f;

    public bool isHoldDistance = false;
    private void Start()
    {
        player = Player.instance.transform;
        if (isHoldDistance)
            targetDistance = 12f;
    }

    private void OnDestroy()
    {
        Game.UnregisterEnemy(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>()
                .GetHit(lastVector);
            MusicBox.Play("Push");
        }

        if (other.collider.CompareTag("Box"))
        {
            other.gameObject.GetComponent<Destroyable>()
                .GetHit(lastVector);
            MusicBox.Play("Push");
        }

        StartCoroutine(OppositeMovement());
    }

    public ParticleSystem Dust;
    private int directionMult = 1;
    private IEnumerator OppositeMovement()
    {
        if (bulletPrefab != null)
            yield break;
        
        MusicBox.Play("Robot");
        Dust.Play();
        
        directionMult = -1;
        currentPause = waitingTime/2;
        yield return new WaitForSeconds(waitingTime/2);
        yield return new WaitForSeconds(0.04f);
        
        Dust.Play();
        isHorisontalMove = !isHorisontalMove;
        directionMult = 1;
        currentPause = waitingTime;
    }

    private Vector3 lastVector;
    private float currentPause = 0;
    private float targetDistance = 0.1f;
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

        if (distance < .1f)
        {
            currentPause = waitingTime;
            isHorisontalMove = !isHorisontalMove;
            Dust.Play();
            return;
        }

        if (isHoldDistance && Vector3.Distance(transform.position, player.position)
            < targetDistance)
        {
            if (spawnTimeout > 0)
                spawnTimeout -= Time.deltaTime;
            else
            {
                ShootingCheck();
                return;
            }
        }

        targetDelta *= Time.deltaTime * Speed * directionMult; 
        if (Mathf.Abs(targetDelta) > maxSpeed)
            targetDelta = maxSpeed * Mathf.Sign(targetDelta);

        lastVector = targetDelta *
                         (isHorisontalMove ? Vector3.right : Vector3.up);
        transform.position += lastVector;
        
        transform.localScale = targetDelta > 0 ? 
            Vector3.one : new Vector3(-1,1,1);
    }

    public BarrelSot bulletPrefab;
    public float shootTimer = 3f;
    private float spawnTimeout = 1.5f;
    private float timer = 0;
    private void ShootingCheck()
    {
        if (bulletPrefab == null)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        Instantiate(bulletPrefab, transform.position, Quaternion.identity)
            .target = player.transform.position;
        
        timer = shootTimer;
    }

    public void WakeUp()
    {
        isSleep = false;
    }
}
