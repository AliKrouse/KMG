using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSFMarker : MonoBehaviour
{
    public int playerNumber;
    public GameObject player;
    private JSFController controller;
    private Vector2 point;
    public float yPlus;
    private Text t;
    private string livesString;

    private GameObject livesContainer;
    private GameObject[] lives = new GameObject[4];

    void Start()
    {
        if (player.activeSelf != true)
            gameObject.SetActive(false);

        t = GetComponent<Text>();

        controller = player.GetComponent<JSFController>();

        if (player.activeSelf == false)
            this.gameObject.SetActive(false);

        livesContainer = this.gameObject.transform.GetChild(0).gameObject;
        for (int i = 0; i < 4; i++)
            lives[i] = livesContainer.transform.GetChild(i).gameObject;
    }

    void Update()
    {
        if (player == null)
            Destroy(this.gameObject);

        point = new Vector2(player.transform.position.x, player.transform.position.y + yPlus);
        transform.position = Camera.main.WorldToScreenPoint(point);

        if (controller.lives == 4)
        {
            foreach (GameObject obj in lives)
                obj.SetActive(true);
            livesContainer.transform.localPosition = new Vector2(0, -15);
        }
        if (controller.lives == 3)
        {
            lives[0].SetActive(true);
            lives[1].SetActive(true);
            lives[2].SetActive(true);
            lives[3].SetActive(false);
            livesContainer.transform.localPosition = new Vector2(7.5f, -15);
        }
        if (controller.lives == 2)
        {
            lives[0].SetActive(true);
            lives[1].SetActive(true);
            lives[2].SetActive(false);
            lives[3].SetActive(false);
            livesContainer.transform.localPosition = new Vector2(15, -15);
        }
        if (controller.lives == 1)
        {
            lives[0].SetActive(true);
            lives[1].SetActive(false);
            lives[2].SetActive(false);
            lives[3].SetActive(false);
            livesContainer.transform.localPosition = new Vector3(22.5f, -15);
        }
        if (controller.lives <= 0)
        {
            foreach (GameObject obj in lives)
                obj.SetActive(false);
        }

        if (playerNumber == 1)
            t.text = JSFOptions.p1Name;
        if (playerNumber == 2)
            t.text = JSFOptions.p2Name;
        if (playerNumber == 3)
            t.text = JSFOptions.p3Name;
        if (playerNumber == 4)
            t.text = JSFOptions.p4Name;
        if (playerNumber == 5)
            t.text = JSFOptions.p5Name;
        if (playerNumber == 6)
            t.text = JSFOptions.p6Name;
        if (playerNumber == 7)
            t.text = JSFOptions.p7Name;
        if (playerNumber == 8)
            t.text = JSFOptions.p8Name;
    }
}
