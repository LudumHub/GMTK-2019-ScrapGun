using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Force;
    public int MaxReflects = 2;
    private int reflects = 0;
    public Scrap ScrapPrefab;
    public int Amount = 1;

    public float maxLife = 3f;
    public float minLife = 0.2f;
    public int maxBullets = 6;
    IEnumerator Start()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(Force, ForceMode2D.Impulse);

        var t = (maxLife - minLife) / maxBullets;
        t = Mathf.Max(maxLife - t * Amount, minLife);
        yield return new WaitForSeconds(t);
        TurnIntoScrap();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Wall"))
            return;
        
        reflects++;
        if (reflects == MaxReflects)
            TurnIntoScrap();
    }

    void TurnIntoScrap()
    {
        var s = Instantiate(ScrapPrefab, transform.position, Quaternion.identity);
        s.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        Destroy(gameObject);
    }
}
