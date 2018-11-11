using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SDkissesUI : MonoBehaviour
{
    public SDController player;
    public int kissNumber;
    private Image i;
    
	void Start ()
    {
        i = GetComponent<Image>();
	}
	
	void Update ()
    {
        if (player.kisses < kissNumber)
            i.enabled = false;
        else
            i.enabled = true;
	}
}
