using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {
    public float smoothDampTime = 0.2f;

    Vector3 smoothDampVelocity;
    Vector3 destination;

    public Vector3 deviation = Vector3.zero;
    float timer = 0;
    public float deviationChangeTimer = 4f;
    public float deviationSize = 2f;
    Vector3 StartPosition;
    float ShakeTimer = 0f;
    private bool preserveTimer;

    public static Shaker instance;

    public void Shake(float seconds)
    {
        camPos.isShaking = true;
        StartPosition = transform.position;
        if (!preserveTimer)
            ShakeTimer = seconds;
    }

    public void ShakeContinuously(float power = 1, float seconds = .3f)
    {
        deviationSize = power;
        Shake(seconds);
        preserveTimer = true;
    }

    private CamerePosition camPos;
    private void Awake()
    {
        camPos = GetComponent<CamerePosition>();
        instance = this;
    }

    private void Update()
    {
        if (!camPos.isShaking)
            return;
        
        if (ShakeTimer < 0f)
        {
            preserveTimer = false;
            camPos.isShaking = false;
        }
        else 
        {
            ShakeTimer -= Time.deltaTime;
            destination = StartPosition + deviation;
            GenerateDeviation();
        }
        
    }

    private void GenerateDeviation()
    {
        timer += Time.deltaTime;
        if (timer < deviationChangeTimer) return;
        timer = 0;

        deviation = new Vector3(Random.value - .5f, Random.value - .5f, 0) * deviationSize;
    }

    void LateUpdate()
    {
        if (!camPos.isShaking)
            return;
        
        transform.position =
            Vector3.SmoothDamp(transform.position, destination, ref smoothDampVelocity, smoothDampTime);
    }
}