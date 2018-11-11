using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDTokenSpawner : MonoBehaviour
{
    public float maxX, maxY;
    public Vector2 point;
    public GameObject token;
    public float interval, maxInterval, minInterval;
    private PolygonCollider2D pc;
    private bool pointIsSet;
    
	void Start ()
    {
        pc = GetComponent<PolygonCollider2D>();

        StartCoroutine(spawnToken());
	}

    private IEnumerator spawnToken()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            while (!pointIsSet)
            {
                point = new Vector2(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY));
                if (pc.OverlapPoint(point))
                    pointIsSet = true;
            }
            Instantiate(token, point, Quaternion.identity);
            interval = Random.Range(minInterval, maxInterval);
            pointIsSet = false;
        }
    }
}
