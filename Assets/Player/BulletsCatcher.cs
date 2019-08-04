using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsCatcher : MonoBehaviour
{
    public Player player;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
            player.CatchBullet(other);
    }
}
