using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderKillZone : MonoBehaviour
{
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }
}
