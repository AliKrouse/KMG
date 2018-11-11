using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.position.x > transform.position.x)
                collision.gameObject.GetComponent<CharController>().canMoveLeft = false;
            if (collision.transform.position.x < transform.position.x)
                collision.gameObject.GetComponent<CharController>().canMoveRight = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CharController>().canMoveRight = true;
            collision.gameObject.GetComponent<CharController>().canMoveLeft = true;
        }
    }
}
