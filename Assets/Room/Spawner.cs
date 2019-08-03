using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy EnemyPrefab;
    public int WaveToSpawn = 1;

    private void Start()
    {
        transform.parent.GetComponent<Room>().RegisterSpawner(this);
    }

    public void Wave(int i)
    {
        if (i != WaveToSpawn) return;

        Room.ActiveRoom
            .RegisterEnemy(Instantiate(EnemyPrefab, transform.position, Quaternion.identity, transform.parent));
        Room.UnregisterSpawner(this);
        
        Destroy(gameObject);
    }
}
