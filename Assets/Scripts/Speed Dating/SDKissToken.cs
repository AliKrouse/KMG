using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDKissToken : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<SDController>().kisses < 3)
            {
                collision.GetComponent<SDController>().kisses++;
                Destroy(this.gameObject);
            }
        }
    }
}
