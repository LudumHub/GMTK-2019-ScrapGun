using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform DoorBlock;

    private void Awake()
    {
        Open();
    }

    public void Open()
    {
        DoorBlock.gameObject.SetActive(false);
    }
    
    public void Close()
    {
        DoorBlock.gameObject.SetActive(true);
    }

    private Room owner;
    public void SetOwner(Room room)
    {
        owner = room;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (owner != null)
            owner.Enter(this);
    }
}
