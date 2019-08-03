using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Collider2D collider;

    public List<Door> Doors = new List<Door>();
    private List<Enemy> Bots = new List<Enemy>();
    private List<Spawner> Spawners = new List<Spawner>();
    private bool isCompleted = false;
    public static Room ActiveRoom;
    
    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        foreach (var Door in Doors)
            Door.SetOwner(this);
    }

    public static void UnregisterEnemy(Enemy e)
    {
        if (ActiveRoom == null)
            return;
        
        if (ActiveRoom.Bots.Contains(e))
            ActiveRoom.Bots.Remove(e);
    }
    public static void UnregisterSpawner(Spawner e)
    {
        if (ActiveRoom.Spawners.Contains(e))
            ActiveRoom.Spawners.Remove(e);
    }
    
    IEnumerator Action()
    {
        foreach (var door in Doors)
            door.Close();
        
        yield return new WaitForSeconds(.5f);

        var wave = 0;
        do {
            foreach (var bot in Bots)
                bot.WakeUp();
            
            while (Bots.Count > 0)
                yield return new WaitForFixedUpdate();

            wave++;
            for (var i = Spawners.Count-1; i > -1; i--)
                Spawners.ElementAt(i).Wave(wave);
            
        } while (Spawners.Count > 0 || Bots.Count > 0);
        
        foreach (var door in Doors.Where(d => d != entarance))
            door.Open();
        
        isCompleted = true;
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!Bots.Contains(enemy))
            Bots.Add(enemy);
    }
    
    public void RegisterSpawner(Spawner spawner)
    {
        Spawners.Add(spawner);
    }

    private Door entarance;
    public void Enter(Door sender)
    {
        if (isCompleted || ActiveRoom == this)
            return;

        entarance = sender;
        Debug.Log("Enter room: " + gameObject.name);

        ActiveRoom = this;
        StartCoroutine(Action());
    }
}
