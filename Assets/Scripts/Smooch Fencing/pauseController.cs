using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class pauseController : MonoBehaviour
{
    private Player[] players = new Player[2];

    public Animator p, b;

    private GameObject pause, inst;
    private bool waitBeforeInput;

    private Text[] names = new Text[2];
    private GameObject[] keys = new GameObject[2];
    private GameObject[] controls = new GameObject[2];
    private Image[] jump = new Image[2];
    private Image[] kiss = new Image[2];
    public Sprite[] jumpOptions, kissOptions;

    public GameObject reset;
    
	void Start ()
    {
        pause = transform.GetChild(0).gameObject;
        inst = transform.GetChild(1).gameObject;

        for (int i = 0; i < 2; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);

            names[i] = inst.transform.GetChild(i + 1).GetComponent<Text>();
            keys[i] = names[i].transform.GetChild(0).gameObject;
            controls[i] = names[i].transform.GetChild(1).gameObject;
            jump[i] = controls[i].transform.GetChild(1).GetComponent<Image>();
            kiss[i] = controls[i].transform.GetChild(2).GetComponent<Image>();
        }

        SetControlText();
    }
	
	void Update ()
    {
        if (reset.activeSelf == false)
        {
            if (pause.activeSelf == false && inst.activeSelf == false)
            {
                foreach (Player i in players)
                {
                    if (i.GetButtonDown("Start") && !waitBeforeInput)
                    {
                        Time.timeScale = 0;
                        pause.SetActive(true);
                        StartCoroutine(inputDelay());
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Time.timeScale = 0;
                    pause.SetActive(true);
                    StartCoroutine(inputDelay());
                }
            }
        }
        if (pause.activeSelf == true)
        {
            foreach (Player i in players)
            {
                if (i.GetButtonDown("Start") && !waitBeforeInput)
                {
                    Time.timeScale = 1;
                    pause.SetActive(false);
                    StartCoroutine(inputDelay());
                }
                if (i.GetButtonDown("Instruct"))
                {
                    pause.SetActive(false);
                    inst.SetActive(true);
                    StartCoroutine(inputDelay());
                }
                if (i.GetButtonDown("Quit") && !waitBeforeInput)
                {
                    SceneManager.LoadScene("smooch league menu");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && !waitBeforeInput)
            {
                Time.timeScale = 1;
                pause.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                pause.SetActive(false);
                inst.SetActive(true);
                StartCoroutine(inputDelay());
            }
            if (Input.GetKeyDown(KeyCode.Escape) && !waitBeforeInput)
            {
                SceneManager.LoadScene("smooch league menu");
            }
        }
        if (inst.activeSelf == true)
        {
            foreach (Player i in players)
            {
                if (i.GetButtonDown("Quit") && !waitBeforeInput)
                {
                    pause.SetActive(true);
                    inst.SetActive(false);
                    StartCoroutine(inputDelay());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !waitBeforeInput)
            {
                pause.SetActive(true);
                inst.SetActive(false);
                StartCoroutine(inputDelay());
            }
        }
	}

    private IEnumerator inputDelay()
    {
        waitBeforeInput = true;
        yield return new WaitForSecondsRealtime(0.15f);
        waitBeforeInput = false;
    }

    void SetControlText()
    {
        names[0].text = gameOptions.pinkName;
        names[1].text = gameOptions.blueName;

        for (int i = 0; i < 2; i++)
        {
            if (gameOptions.playerIsUsingController[i])
            {
                controls[i].SetActive(true);
                keys[i].SetActive(false);
                if (gameOptions.controllerTypes[i] == "Sony DualShock 4")
                {
                    jump[i].sprite = jumpOptions[0];
                    kiss[i].sprite = kissOptions[0];
                }
                else
                {
                    jump[i].sprite = jumpOptions[1];
                    kiss[i].sprite = kissOptions[1];
                }
            }
            else
            {
                keys[i].SetActive(true);
                controls[i].SetActive(false);
            }
        }
    }
}
