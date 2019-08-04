using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    public bool isPreseted = false;
    private void Start()
    {
        if (isPreseted)
            return;

        MusicBox.Play("ScrapFall");
        Effects.Smoke(transform.position, 1f);
    }

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
