using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowCenter : MonoBehaviour
{
    private Camera c;
    public GameObject pink, blue;
    public float maxDistanceApart;
    public Scoreboard sb;
    private Vector2 midPoint;
    private float newX, newY;
    public float XBoundary, YBoundary;
    public float zoomSpeed;
    private bool zoomedOut;
    
	void Start ()
    {
        c = GetComponent<Camera>();
	}

	void Update ()
    {
        if (!sb.gameEnded)
        {
            if (!zoomedOut)
            {
                midPoint = pink.transform.position + (blue.transform.position - pink.transform.position) / 2;

                if (midPoint.y > YBoundary)
                    newY = midPoint.y;
                if (midPoint.x < XBoundary && midPoint.x > -XBoundary)
                    newX = midPoint.x;

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(newX, newY, -10), Time.deltaTime * zoomSpeed * 5);
                if (c.orthographicSize > 5)
                    c.orthographicSize -= Time.deltaTime * zoomSpeed * 5;

                float d = Vector2.Distance(pink.transform.position, blue.transform.position);
                if (d >= maxDistanceApart)
                    zoomedOut = true;
            }
            if (zoomedOut)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 8.43f, -10), Time.deltaTime * zoomSpeed * 5);
                if (c.orthographicSize < 13.5)
                    c.orthographicSize += Time.deltaTime * zoomSpeed * 5;

                float d = Vector2.Distance(pink.transform.position, blue.transform.position);
                if (d < maxDistanceApart)
                    zoomedOut = false;
            }
        }
        if (sb.gameEnded)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(midPoint.x, midPoint.y, -10), Time.deltaTime * zoomSpeed);

            if (c.orthographicSize > 1.5)
                c.orthographicSize -= Time.deltaTime * zoomSpeed;
        }
	}
}
