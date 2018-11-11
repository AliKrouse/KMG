using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDCamera : MonoBehaviour
{
    private Camera c;
    public GameObject p1, p2, p3, p4;
    private SDController p1c, p2c, p3c, p4c;
    private Vector2 mp1, mp2, midPoint;
    private float newX, newY;
    private Vector3 centerPoint;
    private Vector3 p1v, p2v, p3v, p4v;
    
    public float d1, d2, d3, d4;
    public float minPlayerDistance, maxPlayerDistance;
    public float OneTwo, OneThree, OneFour, TwoThree, TwoFour, ThreeFour;
    public float zoomSpeed;

    void Start()
    {
        c = GetComponent<Camera>();
        p1c = p1.GetComponent<SDController>();
        p2c = p2.GetComponent<SDController>();
        p3c = p3.GetComponent<SDController>();
        p4c = p4.GetComponent<SDController>();

        centerPoint = Vector3.zero;
        centerPoint += p1.transform.position;
        centerPoint += p2.transform.position;
        centerPoint += p3.transform.position;
        centerPoint += p4.transform.position;
    }

    void Update()
    {
        if (!p1c.fallen && SDOptions.playerIsInGame[0])
            p1v = p1.transform.position;
        else
            p1v = Vector3.zero;
        if (!p2c.fallen && SDOptions.playerIsInGame[1])
            p2v = p2.transform.position;
        else
            p2v = Vector3.zero;
        if (!p3c.fallen && SDOptions.playerIsInGame[2])
            p3v = p3.transform.position;
        else
            p3v = Vector3.zero;
        if (!p4c.fallen && SDOptions.playerIsInGame[3])
            p4v = p4.transform.position;
        else
            p4v = Vector3.zero;
        
        newX = (p1v.x + p2v.x + p3v.x + p4v.x) / 4;
        newY = (p1v.y + p2v.y + p3v.y + p4v.y) / 4;
        centerPoint = new Vector3(newX, newY, -10);
        transform.position = Vector3.MoveTowards(transform.position, centerPoint, Time.deltaTime * zoomSpeed);

        if (!p1c.fallen && !p2c.fallen && SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[1])
            OneTwo = Vector2.Distance(p1.transform.position, p2.transform.position);
        else
            OneTwo = 0;

        if (!p1c.fallen && !p3c.fallen && SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[2])
            OneThree = Vector2.Distance(p1.transform.position, p3.transform.position);
        else
            OneThree = 0;

        if (!p1c.fallen && !p4c.fallen && SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[3])
            OneFour = Vector2.Distance(p1.transform.position, p4.transform.position);
        else
            OneFour = 0;

        if (!p2c.fallen && !p3c.fallen && SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[3])
            TwoThree = Vector2.Distance(p2.transform.position, p3.transform.position);
        else
            TwoThree = 0;

        if (!p2c.fallen && !p4c.fallen && SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[3])
            TwoFour = Vector2.Distance(p2.transform.position, p4.transform.position);
        else
            TwoFour = 0;

        if (!p3c.fallen && !p4c.fallen && SDOptions.playerIsInGame[2] && SDOptions.playerIsInGame[3])
            ThreeFour = Vector2.Distance(p3.transform.position, p4.transform.position);
        else
            ThreeFour = 0;

        if (OneTwo > maxPlayerDistance || OneThree > maxPlayerDistance || OneFour > maxPlayerDistance || TwoThree > maxPlayerDistance || TwoFour > maxPlayerDistance || ThreeFour > maxPlayerDistance)
        {
            if (c.orthographicSize < 16)
                c.orthographicSize += Time.deltaTime * zoomSpeed;
        }
        if (OneTwo < minPlayerDistance && OneThree < minPlayerDistance && OneFour < minPlayerDistance && TwoThree < minPlayerDistance && TwoFour < minPlayerDistance && ThreeFour < minPlayerDistance)
        {
            if (c.orthographicSize > 5)
                c.orthographicSize -= Time.deltaTime * zoomSpeed;
        }
        if (OneTwo > minPlayerDistance && OneTwo < maxPlayerDistance && OneThree > minPlayerDistance && OneThree < maxPlayerDistance && OneFour > minPlayerDistance && OneFour < maxPlayerDistance && TwoThree > minPlayerDistance && TwoThree < maxPlayerDistance && TwoFour > minPlayerDistance && TwoFour < maxPlayerDistance && ThreeFour > minPlayerDistance && ThreeFour < maxPlayerDistance)
        {
            if (c.orthographicSize < 10)
                c.orthographicSize += Time.deltaTime * zoomSpeed;
            if (c.orthographicSize > 10.25)
                c.orthographicSize -= Time.deltaTime * zoomSpeed;
        }
    }
}
