using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public static Game instance;
    public int Wave = 1;
    public Enemy Pusher;
    public Enemy PusherBig;
    public Enemy Swarmling;
    public Enemy Catapult;

    public Text WaveAnnouncer; 
    public Dictionary<string, Enemy> Beasteary;
    public Collider2D SpawnBounds;
    
    public List<Dictionary<string, int>> Waves = new List<Dictionary<string, int>>()
    {
        new Dictionary<string, int>(),
        new Dictionary<string, int>(){{"Pusher", 1}},
        new Dictionary<string, int>(){{"Pusher", 5}},
        new Dictionary<string, int>(){{"Catapult", 1}},
        new Dictionary<string, int>(){{"Catapult", 3}, {"Pusher", 1}},
        new Dictionary<string, int>(){{"Swarmling", 4}},
        new Dictionary<string, int>(){{"Swarmling", 2},{"Catapult", 1}},
        new Dictionary<string, int>(){{"Pusher", 3},{"Catapult", 3}},
        new Dictionary<string, int>(){{"Swarmling", 9}},
        new Dictionary<string, int>(){{"Pusher", 3},{"Catapult", 3},{"Swarmling", 5}}
    };
    
    private void Awake()
    {
        Beasteary = new Dictionary<string, Enemy>()
        {
            {"Pusher", Pusher},
            {"PusherBig", PusherBig},
            {"Swarmling", Swarmling},
            {"Catapult", Catapult}
        };
        
        instance = this;
        GenerateWave();
    }

    private void GenerateWave()
    {
        StartCoroutine(WaveAction());
    }

    List<Enemy> units = new List<Enemy>(); 
    IEnumerator WaveAction()
    {
        var animator = WaveAnnouncer.GetComponent<Animator>();
        StartCoroutine(AnnounceText("Prepare"));   
        yield return new WaitForSeconds(1);
        
        var bounds = SpawnBounds.bounds;
        yield return  StartCoroutine(AnnounceText("Wave " + Wave));
        
        foreach (var UnitAndAmount in Waves.ElementAt(Wave))
        {
            for (var i = 0; i < UnitAndAmount.Value; i++)
            {
                var position = Random.Range(0, 2) == 0
                    ? new Vector2(Random.Range(bounds.min.x, bounds.max.x),
                        Random.Range(0, 2) == 0 ? bounds.min.y : bounds.max.y)
                    : new Vector2(Random.Range(0, 2) == 0 ? bounds.min.x : bounds.max.x,
                        Random.Range(bounds.min.y, bounds.max.y));

                var unit = Instantiate(
                    Beasteary[UnitAndAmount.Key],
                    position,
                    Quaternion.identity,
                    transform);
                
                units.Add(unit);
                unit.WakeUp();
            }
            yield return new WaitForSeconds(4f);
        }
        
        while (units.Count > 0)
            yield return new WaitForFixedUpdate();

        Wave++;
        GenerateWave();

        IEnumerator AnnounceText(string text)
        {
            WaveAnnouncer.text = text;
            animator.SetTrigger("Show");
            yield return new WaitForSeconds(5);
        }
    }
    
    public static void UnregisterEnemy(Enemy enemy)
    {
        if (instance.units.Contains(enemy))
            instance.units.Remove(enemy);
    }
}
