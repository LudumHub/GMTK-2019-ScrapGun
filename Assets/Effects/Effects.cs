﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private static Effects instance;
    public Animator Explosion;
    public ParticleSystem Dust;
    private void Awake()
    {
        instance = this;
    }

    public static void Explode(Vector3 coords, float scale = 1)
    {
        Instantiate(instance.Explosion, coords, Quaternion.identity)
            .transform.localScale*=scale;
    }

    public static void Smoke(Vector3 coords, float scale = 1)
    {
        var d = Instantiate(instance.Dust, coords, Quaternion.identity);
        d.transform.localScale *= scale;
        d.Play();
    }
}