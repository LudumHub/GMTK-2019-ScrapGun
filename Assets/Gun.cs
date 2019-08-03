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
    }

    private void UpdateAmmo(int ammo)
    {
        Ammo = ammo;
        AmmoText.text = Ammo.ToString();
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
    }

    private IEnumerator Shoot()
    {
        if (Ammo == 0)
            yield break;

        var randomShift = 0;
        var shootPosition = TextAncor.position;
        var shootRotation = transform.rotation.eulerAngles;
        foreach (var sprite in CollectedScrap)
        {
            var rotation = shootRotation;
            rotation.z += Random.Range(-randomShift, randomShift)*3;
            
            var b = Instantiate(BulletPrefab, shootPosition, Quaternion.Euler(rotation));
            b.sprite = sprite;
            UpdateAmmo(--Ammo);
            yield return new WaitForFixedUpdate();
            randomShift++;
        }
        
        CollectedScrap.Clear();
    }

    List<Sprite> CollectedScrap = new List<Sprite>();
    public void GetScrap(Sprite sprite)
    {
        CollectedScrap.Add(sprite);
        UpdateAmmo(++Ammo);
    }
}
