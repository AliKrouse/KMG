using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDactivatePlayers : MonoBehaviour
{
    public GameObject p1, p2, p3, p4;

    private void OnEnable()
    {
        if (SDOptions.playerIsInGame[0])
            p1.SetActive(true);
        else
            p1.SetActive(false);

        if (SDOptions.playerIsInGame[1])
            p2.SetActive(true);
        else
            p2.SetActive(false);

        if (SDOptions.playerIsInGame[2])
            p3.SetActive(true);
        else
            p3.SetActive(false);

        if (SDOptions.playerIsInGame[3])
            p4.SetActive(true);
        else
            p4.SetActive(false);
    }
}
