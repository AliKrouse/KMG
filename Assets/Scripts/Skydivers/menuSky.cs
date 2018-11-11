using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuSky : MonoBehaviour
{
    public float rotationSpeed, waverSpeed, waverAmount;
    private Vector2 top, bottom;
    private bool moveUp, moveDown;
    
	void Start ()
    {
        top = new Vector2(transform.position.x, transform.position.y + waverAmount);
        bottom = new Vector2(transform.position.x, transform.position.y - waverAmount);
        moveUp = true;
	}
	
	void Update ()
    {
        transform.Rotate(Vector2.up * Time.deltaTime * rotationSpeed);

        if (moveUp)
        {
            transform.position = Vector2.MoveTowards(transform.position, top, Time.deltaTime * waverSpeed);

            float dist = Vector2.Distance(transform.position, top);
            if (dist < float.Epsilon)
            {
                moveUp = false;
                moveDown = true;
            }
        }
        if (moveDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, bottom, Time.deltaTime * waverSpeed);

            float dist = Vector2.Distance(transform.position, bottom);
            if (dist < float.Epsilon)
            {
                moveDown = false;
                moveUp = true;
            }
        }
	}
}
