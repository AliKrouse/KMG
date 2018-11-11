using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THStick : MonoBehaviour
{
    public bool inMotion;
    public float pushSpeed;

    private THController mom;
    private THController thc;

    public bool canTurnLeft, canTurnRight;

    public string rot;

    private void Start()
    {
        mom = transform.parent.GetComponent<THController>();

        canTurnLeft = true;
        canTurnRight = true;
    }

    private void Update()
    {
        if (transform.localEulerAngles.z < 70 || transform.localEulerAngles.z > 290)
        {
            canTurnLeft = true;
            canTurnRight = true;
        }
        if (transform.localEulerAngles.z >= 70 && transform.localEulerAngles.z < 80)
        {
            canTurnLeft = false;
            canTurnRight = true;
        }
        if (transform.localEulerAngles.z <= 290 && transform.localEulerAngles.z > 280)
        {
            canTurnLeft = true;
            canTurnRight = false;
        }

        rot = transform.localEulerAngles.z.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thc = collision.gameObject.GetComponent<THController>();

            if (inMotion)
            {
                Vector2 dir = (Vector2)collision.gameObject.transform.position - collision.contacts[0].point;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(dir * pushSpeed);
                thc.stunTime = 3;
                thc.isHit = true;
                thc.isBeingPushed = false;

                mom.source.PlayOneShot(mom.hit);
            }
            if (!inMotion)
            {
                thc.isBeingPushed = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thc = collision.gameObject.GetComponent<THController>();
            thc.isBeingPushed = false;
        }
    }
}
