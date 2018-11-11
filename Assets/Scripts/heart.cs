using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    public float maxSize;
    public float speed;
    private SpriteRenderer sr;
    private Color c;
    private float a;
    
	void Start ()
    {
        sr = GetComponent<SpriteRenderer>();
        c = sr.color;
        a = c.a;
	}
	
	void Update ()
    {
        transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * speed);

        a -= Time.deltaTime * speed;
        sr.color = new Color(c.r, c.g, c.b, a);

        if (a <= 0)
            Destroy(this.gameObject);
	}
}
