using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxDistanceFromPlayer = 2f;
    public Player Player;
    void Update()
    {
        var newCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newCoords.z = 0;

        if (Vector2.Distance(newCoords, Player.transform.position)
            > maxDistanceFromPlayer)
            newCoords = (newCoords - Player.transform.position).normalized
                        * maxDistanceFromPlayer + Player.transform.position;
        
        transform.position = newCoords;
    }
}
