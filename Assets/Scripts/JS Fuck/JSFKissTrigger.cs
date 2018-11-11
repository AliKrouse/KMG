using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSFKissTrigger : MonoBehaviour
{
    private JSFController mom;
    
	void Start ()
    {
        mom = transform.parent.GetComponent<JSFController>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.name == "left kiss")
                mom.playerOnLeft = collision.gameObject;
            if (this.name == "right kiss")
                mom.playerOnRight = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (this.name == "left kiss")
                mom.playerOnLeft = null;
            if (this.name == "right kiss")
                mom.playerOnRight = null;
        }
    }
}
