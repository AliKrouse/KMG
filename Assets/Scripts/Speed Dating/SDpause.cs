using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class SDpause : MonoBehaviour
{
    private Player[] players = new Player[4];

    private GameObject pause, inst;
    private bool acceptInput = true;

    public string sceneToLoad;

    private Text[] names = new Text[4];
    private GameObject[] keys = new GameObject[4];
    private GameObject[] controls = new GameObject[4];
    private GameObject[] inGoal = new GameObject[4];
    private Image[] kiss = new Image[4];
    public Sprite[] kissOptions;

    public GameObject reset;

    void Start()
    {
        pause = transform.GetChild(0).gameObject;
        inst = transform.GetChild(1).gameObject;

        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);

            names[i] = inst.transform.GetChild(i + 1).GetComponent<Text>();
            keys[i] = names[i].transform.GetChild(0).gameObject;
            controls[i] = names[i].transform.GetChild(1).gameObject;
            if (sceneToLoad == "speed dating menu")
                kiss[i] = controls[i].transform.GetChild(1).GetComponent<Image>();
            if (sceneToLoad == "tonsil hockey menu")
                inGoal[i] = names[i].transform.GetChild(2).gameObject;
        }

        SetControlText();
    }

    void Update()
    {
        if (pause.activeSelf == true)
        {
            foreach (Player i in players)
            {
                if (i.GetButtonDown("Start") && acceptInput)
                {
                    StartCoroutine(waitBeforeInput());
                    pause.SetActive(false);
                    Time.timeScale = 1;
                }
                if (i.GetButtonDown("Instruct"))
                {
                    pause.SetActive(false);
                    inst.SetActive(true);
                    StartCoroutine(waitBeforeInput());
                }
                if (i.GetButtonDown("Quit") && acceptInput)
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                pause.SetActive(false);
                Time.timeScale = 1;
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                pause.SetActive(false);
                inst.SetActive(true);
                StartCoroutine(waitBeforeInput());
            }
            if (Input.GetKeyDown(KeyCode.Escape) && acceptInput)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        if (inst.activeSelf == true)
        {
            foreach (Player i in players)
            {
                if (i.GetButtonDown("Quit") && acceptInput)
                {
                    pause.SetActive(true);
                    inst.SetActive(false);
                    StartCoroutine(waitBeforeInput());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) && acceptInput)
            {
                pause.SetActive(true);
                inst.SetActive(false);
                StartCoroutine(waitBeforeInput());
            }
        }
        if (reset.activeSelf == false)
        {
            if (pause.activeSelf == false)
            {
                foreach (Player i in players)
                {
                    if (i.GetButtonDown("Start") && acceptInput)
                    {
                        StartCoroutine(waitBeforeInput());
                        pause.SetActive(true);
                        Time.timeScale = 0;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    StartCoroutine(waitBeforeInput());
                    pause.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
    }

    private IEnumerator waitBeforeInput()
    {
        acceptInput = false;
        yield return new WaitForSecondsRealtime(0.15f);
        acceptInput = true;
    }

    void SetControlText()
    {
        if (sceneToLoad == "speed dating menu")
        {
            names[0].text = SDOptions.p1Name;
            names[1].text = SDOptions.p2Name;
            names[2].text = SDOptions.p3Name;
            names[3].text = SDOptions.p4Name;

            for (int i = 0; i < 4; i++)
            {
                if (!SDOptions.playerIsInGame[i])
                {
                    controls[i].SetActive(false);
                    keys[i].SetActive(false);
                }
                else
                {
                    if (SDOptions.playerIsUsingController[i])
                    {
                        controls[i].SetActive(true);
                        keys[i].SetActive(false);
                        if (SDOptions.controllerType[i] == "Sony DualShock 4")
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
        if (sceneToLoad == "tonsil hockey menu")
        {
            names[0].text = THOptions.p1Name;
            names[1].text = THOptions.p2Name;
            names[2].text = THOptions.p3Name;
            names[3].text = THOptions.p4Name;

            for (int i = 0; i < 4; i++)
            {
                if (!THOptions.playerIsInGame[i])
                {
                    controls[i].SetActive(false);
                    keys[i].SetActive(false);
                    inGoal[i].SetActive(false);
                }
                else
                {
                    if (THOptions.playerIsUsingController[i])
                    {
                        controls[i].SetActive(true);
                        keys[i].SetActive(false);
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
}
