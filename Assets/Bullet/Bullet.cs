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
    IEnumerator Start()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(Force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(10);
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
