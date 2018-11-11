using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public GameObject player;
    public float wallDistance;

    private Vector3 pos;
    
	void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        if (player.transform.position.x > wallDistance)
        {
            pos = new Vector3(wallDistance, player.transform.position.y, -10);
        }
        if (player.transform.position.x < -wallDistance)
        {
            pos = new Vector3(-wallDistance, player.transform.position.y, -10);
        }
        if (player.transform.position.x < wallDistance && player.transform.position.x > -wallDistance)
        {
            pos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        }

        transform.position = pos;
	}
}
