using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camScript : MonoBehaviour
{
    private Camera c;
    public GameObject p1, p2, b1, b2;
    private Vector2 mp1, mp2, midPoint;
    private float newX, newY;
    public float XBoundary, YBoundary;

    public float Border;
    public float d1, d2, d3, d4;
    public float maxPlayerDistance;
    public float p1p2, p1b1, p1b2, p2b1, p2b2, b1b2;
    public float zoomSpeed;

    void Start()
    {
        c = GetComponent<Camera>();
    }

    void Update()
    {
        mp1 = p1.transform.position + (b1.transform.position - p1.transform.position) / 2;
        if (THOptions.fourPlayer)
        {
            mp2 = p2.transform.position + (b2.transform.position - p2.transform.position) / 2;
            midPoint = mp1 + (mp2 - mp1) / 2;
        }
        else
            midPoint = mp1;

        if (midPoint.y < YBoundary && midPoint.y > -YBoundary)
            newY = midPoint.y;
        if (midPoint.x < XBoundary && midPoint.x > -XBoundary)
            newX = midPoint.x;

        transform.position = new Vector3(newX, newY, -10);

        p1b1 = Vector2.Distance(p1.transform.position, b1.transform.position);
        if (THOptions.fourPlayer)
        {
            p1p2 = Vector2.Distance(p1.transform.position, p2.transform.position);
            p1b2 = Vector2.Distance(p1.transform.position, b2.transform.position);
            p2b1 = Vector2.Distance(p2.transform.position, b1.transform.position);
            p2b2 = Vector2.Distance(p2.transform.position, p2.transform.position);
            b1b2 = Vector2.Distance(b1.transform.position, b2.transform.position);
        }

        if (p1b1 > maxPlayerDistance || p1p2 > maxPlayerDistance || p1b2 > maxPlayerDistance || p2b1 > maxPlayerDistance || p2b2 > maxPlayerDistance || b1b2 > maxPlayerDistance)
        {
            if (c.orthographicSize < 8.75)
                c.orthographicSize += Time.deltaTime * zoomSpeed;
        }
        else
        {
            if (c.orthographicSize > 5)
                c.orthographicSize -= Time.deltaTime * zoomSpeed;
        }
    }
}
