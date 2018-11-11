using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScreenCrowd : MonoBehaviour
{
    public float waverAmmount, speed;
    private Vector2 bottom, top;
    private bool goingUp, goingDown;
    
	void Start ()
    {
        bottom = transform.position;
        top = new Vector2(transform.position.x, transform.position.y + waverAmmount);
        goingUp = true;
	}
	
	void Update ()
    {
        if (goingUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, top, Time.deltaTime * speed);

            float d = Vector2.Distance(transform.position, top);
            if (d < 0.1)
            {
                goingDown = true;
                goingUp = false;
            }
        }
        if (goingDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, bottom, Time.deltaTime * speed);

            float d = Vector2.Distance(transform.position, bottom);
            if (d < 0.1)
            {
                goingUp = true;
                goingDown = false;
            }
        }
	}
}
