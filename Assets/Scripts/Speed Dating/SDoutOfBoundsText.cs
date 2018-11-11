using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SDoutOfBoundsText : MonoBehaviour
{
    public SDController sdc;
    private Text t;
    
	void Start ()
    {
        t = GetComponent<Text>();
	}
	
	void Update ()
    {
        if (sdc.inBounds || sdc.fallen)
            t.text = "";
        else
            t.text = "You're out of bounds!";
	}
}
