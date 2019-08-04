using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Destroyable : MonoBehaviour
{
    private int hp = 1;
    public int MaxHp = 1;
    public Scrap ScrapPrefab;
    public float lootSpread = .5f;
    public int ExtraDrop = 0;

    private void Awake()
    {
        hp = MaxHp;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Bullet"))
            return;

        GetHit();
        Destroy(other.gameObject);
    }

    public void GetHit()
    {
        if (--hp > 0) return;

        for (var i = 0; i < MaxHp + ExtraDrop; i++)
            SpawnScrap();

        Destroy(gameObject);

        void SpawnScrap()
        {
            Vector3 pos;
            do
            {
                pos = transform.position +
                      new Vector3(Random.Range(-1, 1) * lootSpread, Random.Range(-1, 1) * lootSpread);
            } while (Physics2D.OverlapPoint(pos, 1 << 4) != null);

            Instantiate<Scrap>(
                ScrapPrefab,
                pos,
                Quaternion.identity);
        }
    }

    public void GetHit(Vector3 lastVector)
    {
        StartCoroutine(HitState(lastVector));
    }

    public bool isStunned = false;

    private IEnumerator HitState(Vector3 direction)
    {
        var rBody = GetComponent<Rigidbody2D>();
        rBody.AddForce(direction.normalized * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rBody.velocity = Vector2.zero;
    }
}