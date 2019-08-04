using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public TextMesh AmmoText;
    public Transform TextAncor;
    public int Ammo = 0;
    public SpriteRenderer BulletPrefab;
    
    public List<BulletsHelper> shootingGuide = new List<BulletsHelper>();
    private void Awake()
    {
        UpdateAmmo(0);
        collider = GetComponent<Collider2D>(); ;
    }

    Collider2D collider;
    public float degMovementIsAttack = 2f;
    private float prevAngle = 0;
    private Transform activeHelper;
    private void UpdateAmmo(int ammo)
    {
        if (activeHelper != null)
        {
            activeHelper.gameObject.SetActive(false);
            activeHelper = null;
        }

        if (ammo > 0)
        {
            activeHelper = shootingGuide.ElementAt(ammo > shootingGuide.Count - 1 ? shootingGuide.Count - 1 : ammo)
                .transform;
            activeHelper.gameObject.SetActive(true);
        }

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
        
        var guide = shootingGuide.ElementAt(
            shootingGuide.Count - 1 < Ammo ? 
            Ammo :
            shootingGuide.Count - 1);

        var degPerBulletMult = 20;
        var degrees = degPerBulletMult * Ammo;
        foreach (var sprite in CollectedScrap)
        {
            var rotation = shootRotation;
            if (amount > shootingGuide.Count - 1)
                rotation.z += -(degrees / 2) + (amount - Ammo) * degPerBulletMult;
            else
            {
                var diff = guide.bullets[amount - Ammo].position - transform.position;
                diff.Normalize();
 
                var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                rotation.z = rot_z - 90;
            }

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
