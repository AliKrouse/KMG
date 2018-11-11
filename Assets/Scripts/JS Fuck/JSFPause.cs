using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class JSFPause : MonoBehaviour
{
    private Player[] players = new Player[8];

    private GameObject pause, inst;
    private Text[] names = new Text[8];
    private GameObject[] keys = new GameObject[8];
    private GameObject[] controls = new GameObject[8];
    private Image[] kiss = new Image[8];
    public Sprite[] kissOptions;

    private bool waitForInput;

    public static bool paused;

    public GameObject reset;
    
	void Start ()
    {
        pause = transform.GetChild(0).gameObject;
        inst = transform.GetChild(1).gameObject;

        for (int i = 0; i < 8; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);

            names[i] = inst.transform.GetChild(i + 1).GetComponent<Text>();
            keys[i] = names[i].transform.GetChild(0).gameObject;
            controls[i] = names[i].transform.GetChild(1).gameObject;
            kiss[i] = controls[i].transform.GetChild(1).GetComponent<Image>();
        }

        SetControlsText();
    }
	
	void Update ()
    {
        if (reset.activeSelf == false)
        {
            if (pause.activeSelf == false)
            {
                foreach (Player i in players)
                {
                    if (i.GetButtonDown("Start") && !waitForInput)
                    {
                        pause.SetActive(true);
                        Time.timeScale = 0;
                        paused = true;
                        StartCoroutine(waitBeforeInput());
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pause.SetActive(true);
                    Time.timeScale = 0;
                    paused = true;
                    StartCoroutine(waitBeforeInput());
                }
            }
        }

        if (pause.activeSelf == true)
        {
            foreach (Player i in players)
            {
                if (i.GetButtonDown("Start") && !waitForInput)
                {
                    pause.SetActive(false);
                    Time.timeScale = 1;
                    paused = false;
                    StartCoroutine(waitBeforeInput());
                }
                if (i.GetButtonDown("Instruct"))
                {
                    pause.SetActive(false);
                    inst.SetActive(true);
                    StartCoroutine(waitBeforeInput());
                }
                if (i.GetButtonDown("Quit") && !waitForInput)
                {
                    SceneManager.LoadScene("js fuck menu");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !waitForInput)
            {
                pause.SetActive(false);
                Time.timeScale = 1;
                paused = false;
                StartCoroutine(waitBeforeInput());
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                pause.SetActive(false);
                inst.SetActive(true);
                StartCoroutine(waitBeforeInput());
            }
            if (Input.GetKeyDown(KeyCode.Escape) && !waitForInput)
            {
                SceneManager.LoadScene("js fuck menu");
            }
        }

        if (inst.activeSelf == true)
        {
            foreach (Player i in players)
            {
                if (i.GetButtonDown("Quit"))
                {
                    pause.SetActive(true);
                    inst.SetActive(false);
                    StartCoroutine(waitBeforeInput());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pause.SetActive(true);
                inst.SetActive(false);
                StartCoroutine(waitBeforeInput());
            }
        }
	}

    private IEnumerator waitBeforeInput()
    {
        waitForInput = true;
        yield return new WaitForSecondsRealtime(0.15f);
        waitForInput = false;
    }

    private void SetControlsText()
    {
        names[0].text = JSFOptions.p1Name;
        names[1].text = JSFOptions.p2Name;
        names[2].text = JSFOptions.p3Name;
        names[3].text = JSFOptions.p4Name;
        names[4].text = JSFOptions.p5Name;
        names[5].text = JSFOptions.p6Name;
        names[6].text = JSFOptions.p7Name;
        names[7].text = JSFOptions.p8Name;

        for (int i = 0; i < 8; i++)
        {
            if (!JSFOptions.playerIsInGame[i])
            {
                controls[i].SetActive(false);
                keys[i].SetActive(false);
            }
            else
            {
                if (JSFOptions.playerIsUsingController[i])
                {
                    controls[i].SetActive(true);
                    keys[i].SetActive(false);
                    if (JSFOptions.controllerTypes[i] == "Sony DualShock 4")
                        kiss[i].sprite = kissOptions[0];
                    else
                        kiss[i].sprite = kissOptions[1];
                }
                else
                {
                    keys[i].SetActive(true);
                    controls[i].SetActive(false);
                }
            }
        }
    }
}
