using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMarker : MonoBehaviour
{
    public int playerNumber;
    public GameObject player;
    private Vector2 point;
    public float yPlus;
    private Text t;
    
	void Start ()
    {
        if (player.activeSelf != true)
            gameObject.SetActive(false);

        t = GetComponent<Text>();
	}
	
	void Update ()
    {
        point = new Vector2(player.transform.position.x, player.transform.position.y + yPlus);
        transform.position = Camera.main.WorldToScreenPoint(point);

        if (playerNumber == 1)
            t.text = THOptions.p1Name + "\n▼";
        if (playerNumber == 2)
            t.text = THOptions.p2Name + "\n▼";
        if (playerNumber == 3)
            t.text = THOptions.p3Name + "\n▼";
        if (playerNumber == 4)
            t.text = THOptions.p4Name + "\n▼";
    }
}
