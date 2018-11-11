using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goal : MonoBehaviour
{
    public string color;
    public PhysicsMaterial2D bouncyMaterial;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<THController>().inGoal = true;
            collision.gameObject.GetComponent<THController>().stunTime = 6;
            collision.gameObject.GetComponent<THController>().goalColor = color;
            collision.GetComponent<CircleCollider2D>().sharedMaterial = null;
            collision.GetComponent<Rigidbody2D>().sharedMaterial = null;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<THController>().inGoal = false;
            collision.GetComponent<CircleCollider2D>().sharedMaterial = bouncyMaterial;
            collision.GetComponent<Rigidbody2D>().sharedMaterial = bouncyMaterial;
        }
    }
}
