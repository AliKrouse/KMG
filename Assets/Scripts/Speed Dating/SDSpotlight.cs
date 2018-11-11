using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDSpotlight : MonoBehaviour
{
    private Vector2 point;
    public float speed;
    
	void Start ()
    {
        CreatePoint();
	}
	
	void Update ()
    {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, point, Time.deltaTime * speed);

        float d = Vector2.Distance(transform.localPosition, point);
        if (d < float.Epsilon)
            CreatePoint();
	}

    void CreatePoint()
    {
        float x = Random.Range(-15, 15);
        float y = Random.Range(-15, 15);
        point = new Vector2(x, y);
    }
}
