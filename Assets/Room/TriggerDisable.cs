using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDisable : MonoBehaviour
{
    public Door door;

    private void Start()
    {
        door.Close();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            door.Open();
    }
}
