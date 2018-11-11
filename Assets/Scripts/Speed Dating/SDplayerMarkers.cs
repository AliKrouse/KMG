using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SDplayerMarkers : MonoBehaviour
{
    public int playerNumber;
    public GameObject player;
    private SDController sdc;
    public float height;
    private Text t;

    public Color[] colorOptions;
    
	void Start ()
    {
        sdc = player.GetComponent<SDController>();
        t = GetComponent<Text>();

        t.color = colorOptions[SDOptions.characterChoices[playerNumber - 1]];
    }
	
	void Update ()
    {
		transform.position = Camera.main.WorldToScreenPoint(new Vector2(player.transform.position.x, player.transform.position.y + height));

        if (playerNumber == 1)
            t.text = SDOptions.p1Name + "\n▼";
        if (playerNumber == 2)
            t.text = SDOptions.p2Name + "\n▼";
        if (playerNumber == 3)
            t.text = SDOptions.p3Name + "\n▼";
        if (playerNumber == 4)
            t.text = SDOptions.p4Name + "\n▼";

        if (player.activeSelf == false)
            this.gameObject.SetActive(false);
    }
}
