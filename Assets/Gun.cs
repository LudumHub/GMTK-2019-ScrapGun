using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public TextMesh AmmoText;
    public Transform TextAncor;
    public int Ammo = 0;
    public SpriteRenderer BulletPrefab;
    private void Awake()
    {
        UpdateAmmo(0);
        collider = GetComponent<Collider2D>();
    }

    Collider2D collider;
    public float degMovementIsAttack = 2f;
    private float prevAngle = 0;
    private void UpdateAmmo(int ammo)
    {
        Ammo = ammo;
        AmmoText.text = Ammo.ToString();
    }

    public float meleePushPower = 100;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var vector = other.transform.position - transform.position;
            other.GetComponent<Rigidbody2D>().AddForce(vector * meleePushPower, ForceMode2D.Impulse);
        }
        else if (other.CompareTag("Box"))
        {
            other.GetComponent<Destroyable>().GetHit();
        }
    }

    void Update()
    {
        var diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
 
        var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        var eulerZ = rot_z - 90;
        transform.rotation = Quaternion.Euler(0f, 0f, eulerZ); 
        
        if (eulerZ < 65 && eulerZ > -65)
            transform.localPosition = Vector3.forward;
        else
            transform.localPosition= Vector3.back;

        AmmoText.transform.position = TextAncor.transform.position;

        if (Input.GetMouseButtonDown(0))
            StartCoroutine(Shoot());

        collider.enabled =
            Mathf.Abs(prevAngle - eulerZ) > degMovementIsAttack;
        prevAngle = eulerZ;
    }

    private IEnumerator Shoot()
    {
        if (Ammo == 0)
            yield break;

        var randomShift = 0;
        var shootPosition = TextAncor.position;
        var shootRotation = transform.rotation.eulerAngles;
        var amount = Ammo;
        
        var degPerBulletMult = 20;
        var degrees = degPerBulletMult * Ammo;
        foreach (var sprite in CollectedScrap)
        {
            var rotation = shootRotation;
            if (amount>1)
                rotation.z += -(degrees/2) + (amount - Ammo) * degPerBulletMult;
            
            var b = Instantiate(BulletPrefab, shootPosition, Quaternion.Euler(rotation));
            b.sprite = sprite;
            b.GetComponent<Bullet>().Amount = amount;
            UpdateAmmo(--Ammo);
            randomShift++;
        }
        
        CollectedScrap.Clear();
    }

    List<Sprite> CollectedScrap = new List<Sprite>();
    public void GetScrap(Sprite sprite)
    {
        if (sprite == null)
            sprite = BulletPrefab.sprite;
        CollectedScrap.Add(sprite);
        UpdateAmmo(++Ammo);
    }
}
