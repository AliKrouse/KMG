using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoringVisual : MonoBehaviour
{
    public bool bg;
    public string color;
    public int pointNumber;
    public Scoreboard scoreboard;

    private int points;

    private Image i;

    private void Start()
    {
        i = GetComponent<Image>();
    }

    void Update ()
    {
        if (!bg)
        {
            if (color == "pink")
                points = scoreboard.pinkPoints;
            if (color == "blue")
                points = scoreboard.bluePoints;

            if (points >= pointNumber)
                i.enabled = true;
            else
                i.enabled = false;
        }
        if (bg)
        {
            if (pointNumber > gameOptions.ptw)
                this.gameObject.SetActive(false);
        }
	}
}
