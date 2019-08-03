using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        var player = other.GetComponent<Player>();
        var gun = player.Gun;
        gun.GetScrap(GetComponent<SpriteRenderer>().sprite);
        
        Destroy(gameObject);
    }
}
