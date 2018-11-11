using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkydiversPlayerMarker : MonoBehaviour
{
    public int playerNumber;
    private Text t;

	void Start ()
    {
        t = GetComponent<Text>();

        if (playerNumber == 1)
            t.text = SkydiversOptions.p1Name + "\n▼";
        if (playerNumber == 2)
            t.text = SkydiversOptions.p2Name + "\n▼";
        if (playerNumber == 3)
            t.text = SkydiversOptions.p3Name + "\n▼";
        if (playerNumber == 4)
            t.text = SkydiversOptions.p4Name + "\n▼";
    }
	
	void Update ()
    {

    }
}
