using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Force;
    IEnumerator Start()
    {
        GetComponent<Rigidbody2D>().AddRelativeForce(Force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
