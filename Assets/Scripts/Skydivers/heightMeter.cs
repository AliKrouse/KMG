using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heightMeter : MonoBehaviour
{
    private Scrollbar sb;

    public GameObject player, plane;
    
	void Start ()
    {
        sb = GetComponent<Scrollbar>();
	}
	
	void Update ()
    {
        sb.size = player.transform.position.y / plane.transform.position.y;
	}
}
