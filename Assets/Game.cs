using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public static Game instance;
    public static int Save = 0;
    public int Wave = 1;
    public Enemy Pusher;
    public Enemy PusherBig;
    public Enemy Swarmling;
    public Enemy Catapult;
    public Animator FadeOnDAmage;
    public Text WaveAnnouncer; 
    public Dictionary<string, Enemy> Beasteary;
  
    public List<Transform> MeleeSpawns = new List<Transform>();
    public List<Transform> RangeSpawns = new List<Transform>();

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

    public void Damage()
    {
        FadeOnDAmage.SetTrigger("Fade");
        if (timer <= 0)
        {
            timer = 4f;
        }
        else
        {
            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
        Wave = Save;
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
        yield return StartCoroutine(AnnounceText("Prepare"));
        yield return  StartCoroutine(AnnounceText("Wave " + Wave));
        
        foreach (var UnitAndAmount in Waves.ElementAt(Wave))
        {
            for (var i = 0; i < UnitAndAmount.Value; i++)
            {
                var position = (Beasteary[UnitAndAmount.Key] == Catapult
                        ? RangeSpawns[Random.Range(0, RangeSpawns.Count - 1)]
                        : MeleeSpawns[Random.Range(0, MeleeSpawns.Count - 1)]
                    ).position;

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
        Save = Wave;
        GenerateWave();

        IEnumerator AnnounceText(string text)
        {
            WaveAnnouncer.text = text;
            animator.SetTrigger("Show");
            yield return new WaitForSeconds(5);
        }
    }

    private float timer;
    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha0))
            CheatAndLoad(0);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CheatAndLoad(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            CheatAndLoad(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            CheatAndLoad(3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            CheatAndLoad(4);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            CheatAndLoad(5);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            CheatAndLoad(6);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            CheatAndLoad(7);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            CheatAndLoad(8);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            CheatAndLoad(9);
    }

    private void CheatAndLoad(int i)
    {
        Save = i;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void UnregisterEnemy(Enemy enemy)
    {
        if (instance.units.Contains(enemy))
            instance.units.Remove(enemy);
    }
}
