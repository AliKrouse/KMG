using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSFactivatePlayer : MonoBehaviour
{
    public GameObject p1, p2, p3, p4, p5, p6, p7, p8;

    private void OnEnable()
    {
        if (JSFOptions.playerIsInGame[0])
            p1.SetActive(true);
        else
            p1.SetActive(false);

        if (JSFOptions.playerIsInGame[1])
            p2.SetActive(true);
        else
            p2.SetActive(false);

        if (JSFOptions.playerIsInGame[2])
            p3.SetActive(true);
        else
            p3.SetActive(false);

        if (JSFOptions.playerIsInGame[3])
            p4.SetActive(true);
        else
            p4.SetActive(false);

        if (JSFOptions.playerIsInGame[4])
            p5.SetActive(true);
        else
            p5.SetActive(false);

        if (JSFOptions.playerIsInGame[5])
            p6.SetActive(true);
        else
            p6.SetActive(false);

        if (JSFOptions.playerIsInGame[6])
            p7.SetActive(true);
        else
            p7.SetActive(false);

        if (JSFOptions.playerIsInGame[7])
            p8.SetActive(true);
        else
            p8.SetActive(false);
    }
}
