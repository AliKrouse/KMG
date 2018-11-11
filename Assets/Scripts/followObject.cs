using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followObject : MonoBehaviour
{
    public GameObject objectToFollow;
    public float speed;
    
	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.position = Vector2.MoveTowards(transform.position, objectToFollow.transform.position, Time.deltaTime * speed);
	}
}
