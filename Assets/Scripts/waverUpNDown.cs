using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waverUpNDown : MonoBehaviour
{
    public float waver, waverSpeed;
    private float up, down;
    private Vector2 upPos, downPos;
    private bool movingUp, movingDown;
    
	void Start ()
    {
        up = transform.position.y + waver;
        down = transform.position.y - waver;
        movingUp = true;
	}
	
	void Update ()
    {
        if (movingUp)
        {
            upPos = new Vector2(transform.position.x, up);

            transform.position = Vector2.MoveTowards(transform.position, upPos, Time.deltaTime * waverSpeed);

            float d = Vector2.Distance(transform.position, upPos);
            if (d < float.Epsilon)
            {
                movingUp = false;
                movingDown = true;
            }
        }
        if (movingDown)
        {
            downPos = new Vector2(transform.position.x, down);

            transform.position = Vector2.MoveTowards(transform.position, downPos, Time.deltaTime * waverSpeed);

            float d = Vector2.Distance(transform.position, downPos);
            if (d < float.Epsilon)
            {
                movingDown = false;
                movingUp = true;
            }
        }
	}
}
