using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Destroyable : MonoBehaviour
{
    private int hp = 1;
    public int MaxHp = 1;
    public Scrap ScrapPrefab;
    public float lootSpread = .5f;

    private void Awake()
    {
        hp = MaxHp;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Bullet"))
            return;
        
        GetHit();
        Destroy(other.gameObject);
    }

    public void GetHit()
    {
        if (--hp > 0) return;

        if (MaxHp > 1)
            for (var i = 0; i < MaxHp / 2; i++)
                SpawnScrap();
        else
            SpawnScrap();

        Destroy(gameObject);

        void SpawnScrap()
        {
            Instantiate<Scrap>(
                ScrapPrefab,
                transform.position +
                new Vector3(Random.Range(-1, 1) * lootSpread, Random.Range(-1, 1) * lootSpread),
                Quaternion.identity);
        }
    }
}
