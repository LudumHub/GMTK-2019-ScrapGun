using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
    public int Ammo = 0;
    public SpriteRenderer BulletPrefab;

    public LineRenderer HelperLine;
    private void Awake()
    {
        UpdateAmmo(0);
        collider = GetComponent<Collider2D>(); 
    }

    Collider2D collider;
    public Transform ShootingPoint;

    public float degMovementIsAttack = 2f;
    private Quaternion prevAngle = Quaternion.identity;
    public List<LineRenderer> activeHelpers = new List<LineRenderer>();
    private List<float> eulerHelpers = new List<float>();
    public List<Transform> HaveScrapVisual;

    static List<float> DistancesPerBulletsAmount = new List<float>()
    {
        99, 35, 12, 7, 5.8f, 5.5f, 5, 4.5f, 4, 3.5f, 3, 2.5f, 2.2f, 2f, 1.9f, 1.8f, 1.7f, 1.6f, 1.5f, 1.4f
    };

    public static float GetBulletDistance(int bulletsAmount)
    {
        return DistancesPerBulletsAmount[
            Mathf.Min(DistancesPerBulletsAmount.Count - 1, bulletsAmount)
        ];
    }

    static List<float> DegreeSpreadPerBulletsAmount = new List<float>()
    {
        0, 5, 7, 10, 12, 15, 20, 25, 30, 35
    };

    public static float GetDegreeSpread(int bulletsAmount)
    {
        return DegreeSpreadPerBulletsAmount[
            Mathf.Min(DegreeSpreadPerBulletsAmount.Count - 1, bulletsAmount)
        ];
    }
    
    static List<float> BulletSizePerBulletsAmount = new List<float>()
    {
        1, 1, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 2
    };

    public static float GetBulletSize(int bulletsAmount)
    {
        return BulletSizePerBulletsAmount[
            Mathf.Min(BulletSizePerBulletsAmount.Count - 1, bulletsAmount)
        ];
    }
    
    private void UpdateAmmo(int ammo)
    {
        foreach (var line in activeHelpers)
            Destroy(line.gameObject);
        
        activeHelpers.Clear();
        eulerHelpers.Clear();
        
        var degPerBulletMult = GetDegreeSpread(ammo);
        var degrees = degPerBulletMult * (Ammo-1);
        
        for (var i = 0; i < ammo; i++)
        {
            var line = Instantiate(HelperLine, ShootingPoint.position, Quaternion.identity, transform);
            activeHelpers.Add(line);
            
            var rotation = transform.up;
            var angle = (degrees / 2) - 
                        (ammo - i - 1) * degPerBulletMult;
            
            eulerHelpers.Add(angle);
            rotation = RotateRadians(rotation, angle * (float)(Math.PI/180));
            rotation *= GetBulletDistance(ammo);
            line.startWidth *= GetBulletSize(ammo);
            
            line.SetPosition(1, rotation);
        }

        foreach (var visual in HaveScrapVisual)
            visual.gameObject.SetActive(false);
        
        if (ammo == 1)
            HaveScrapVisual[0].gameObject.SetActive(true);
        else if (ammo > 1 && ammo < 5)
            HaveScrapVisual[1].gameObject.SetActive(true);  
        else if (ammo > 4)
            HaveScrapVisual[2].gameObject.SetActive(true); 
        
        Ammo = ammo;

        Vector3 RotateRadians(Vector3 v, float radians)
        {
            var ca = Mathf.Cos(radians);
            var sa = Mathf.Sin(radians);
            return new Vector3(ca*v.x - sa*v.y, sa*v.x + ca*v.y);
        }
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
    
    public float MaxEulerPerFrame = 0.5f;

    void Update()
    {
        if (Player.instance.isStunned)
            return;
        
        var diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
 
        var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        var eulerZ = rot_z - 90;

        var newRotation = Quaternion.Euler(0f, 0f, eulerZ);

        transform.rotation = newRotation;
        /*
        transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                newRotation, 
                MaxEulerPerFrame * Time.deltaTime);
        */
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(Shoot());
        
        prevAngle = transform.rotation;
    }

    private IEnumerator Shoot()
    {
        if (Ammo == 0)
            yield break;

        var randomShift = 0;
        var shootPosition = ShootingPoint.position;
        var amount = Ammo;
        var gun_rotation = transform.rotation.eulerAngles.z;
        foreach (var sprite in CollectedScrap)
        {
             var rotation_z = gun_rotation +
                             eulerHelpers[amount - Ammo];
             
             var b = Instantiate(
                BulletPrefab, 
                shootPosition, 
                Quaternion.Euler(0f, 0f, rotation_z));
            
            b.sprite = sprite;
            b.GetComponent<Bullet>().Amount = amount;
            Ammo--;
            randomShift++;
        }
        
        UpdateAmmo(0);
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
