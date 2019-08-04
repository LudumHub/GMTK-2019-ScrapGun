using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public bool UseSmoothMovement = false;
    public float InputMultiplicator = 0.4f;
    public Gun Gun;
    public Transform Visual; 
    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(isStunned)
            return;
        
        var movement = Vector3.zero;

        if (UseSmoothMovement)
        {
            if (Input.GetKey(KeyCode.S))
                movement += Vector3.down;
            if (Input.GetKey(KeyCode.W))
                movement += Vector3.up;
            if (Input.GetKey(KeyCode.A))
                movement += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                movement += Vector3.right;

            movement *= InputMultiplicator;
        }
        else
        {
            movement = InputMultiplicator *
                       new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        }

        if (movement.magnitude > InputMultiplicator)
            movement = movement.normalized * InputMultiplicator;
            
        transform.position += movement*Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Visual.transform.rotation = Gun.transform.rotation;
    }
    
    public void GetHit(Vector3 direction)
    {
        StartCoroutine(HitState(direction));
    }

    public bool isStunned = false;
    private IEnumerator HitState(Vector3 direction)
    {
        if(isStunned)
            yield break;
        
        isStunned = true;
        foreach (var line in Gun.activeHelpers)
            line.gameObject.SetActive(false);

        var rBody = GetComponent<Rigidbody2D>();
        rBody.AddForce(direction.normalized * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        rBody.velocity = Vector2.zero;
        
        isStunned = false;
        foreach (var line in Gun.activeHelpers)
            line.gameObject.SetActive(true);
    }
    
    public void CatchBullet(Collider2D other)
    {
        if (other.gameObject.GetComponent<Bullet>().reflects == 0)
            return;
            
        Destroy(other.gameObject);
        Gun.GetScrap(other.gameObject.GetComponent<SpriteRenderer>().sprite);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
