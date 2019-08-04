using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Force;
    public int MaxReflects = 2;
    public int reflects = 0;
    public Scrap ScrapPrefab;
    public int Amount = 1;

    private float DistanceAlive = 0;
    private float DistanceNeeded;

    private Vector3 prevPosition;
    IEnumerator Start()
    {
        prevPosition = transform.position;
        var rBody = GetComponent<Rigidbody2D>();
        rBody.AddRelativeForce(Force, ForceMode2D.Impulse);
        DistanceNeeded = Gun.GetBulletDistance(Amount);
        transform.localScale *= Gun.GetBulletSize(Amount);
        
        while (DistanceAlive < DistanceNeeded)
        {
            yield return new WaitForFixedUpdate();
            DistanceAlive += Vector3.Distance(prevPosition, transform.position);
            prevPosition = transform.position;
        }
        
        while (Physics2D.OverlapPoint(transform.position, 1 << 4) != null)
            yield return new WaitForFixedUpdate();
        
        rBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(.2f);
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
