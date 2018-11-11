using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateFourPlayer : MonoBehaviour
{
    public GameObject p1, p2, b1, b2;
    
	void OnEnable ()
    {
        //if (THOptions.fourPlayer)
        //{
        //    p2.SetActive(true);
        //    b2.SetActive(true);
        //}
        //else
        //{
        //    p2.SetActive(false);
        //    b2.SetActive(false);
        //}

        if (THOptions.playerIsInGame[0])
            p1.SetActive(true);
        else
            p1.SetActive(false);

        if (THOptions.playerIsInGame[1])
            p2.SetActive(true);
        else
            p2.SetActive(false);

        if (THOptions.playerIsInGame[2])
            b1.SetActive(true);
        else
            b1.SetActive(false);

        if (THOptions.playerIsInGame[3])
            b2.SetActive(true);
        else
            b2.SetActive(false);
	}
}
