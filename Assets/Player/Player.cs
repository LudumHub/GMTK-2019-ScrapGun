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
    private Animator myAnimator;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
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

        var magn = movement.magnitude;
        if (magn >  InputMultiplicator)
            movement = movement.normalized * InputMultiplicator;
        
        myAnimator.SetFloat("Movement", magn);
            
        transform.position += movement*Time.deltaTime;
    }

    public ParticleSystem leftLeg;
    public ParticleSystem rightLeg;
    public void LeftStepTrigger()
    {
        MusicBox.Play("Steps");
        leftLeg.Play();
    }

    public void RightStepTrigger()
    {
        MusicBox.Play("Steps");
        rightLeg.Play();
    }
    
    public float MaxEulerPerFrame = 130;
    private void FixedUpdate()
    {
        Visual.transform.rotation = Quaternion.RotateTowards(
            Visual.transform.rotation, 
            Gun.transform.rotation, 
            MaxEulerPerFrame * Time.deltaTime); 
    }

    public void GetHit(Vector3 direction)
    {
        Shaker.instance.ShakeContinuously();
        Game.instance.Damage();
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
