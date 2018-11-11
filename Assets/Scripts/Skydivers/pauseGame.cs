using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class pauseGame : MonoBehaviour
{
    private Player[] players = new Player[4];

    public static bool paused;
    public bool canQuit;
    private GameObject pauseMenu, inst;

    private Text[] names = new Text[4];
    private GameObject[] keys = new GameObject[4];
    private GameObject[] controls = new GameObject[4];
    private Image[] drop = new Image[4];
    private Image[] slow = new Image[4];
    public Sprite[] dropOptions, slowOptions;

    public GameObject restart;
    
	void Start ()
    {
        pauseMenu = transform.GetChild(0).gameObject;
        inst = transform.GetChild(1).gameObject;

        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);

            names[i] = inst.transform.GetChild(i + 1).GetComponent<Text>();
            keys[i] = names[i].transform.GetChild(0).gameObject;
            controls[i] = names[i].transform.GetChild(1).gameObject;
            drop[i] = controls[i].transform.GetChild(1).GetComponent<Image>();
            slow[i] = controls[i].transform.GetChild(2).GetComponent<Image>();
        }

        SetControlsText();

        canQuit = true;
        paused = false;
    }
	
	void Update ()
    {
        if (restart.activeSelf == false)
        {
            if (!paused)
            {
                foreach (Player i in players)
                {
                    if (i.GetButtonDown("Start") && canQuit)
                    {
                        pauseMenu.SetActive(true);
                        Time.timeScale = 0;
                        StartCoroutine(makeQuittable());
                        paused = true;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;
                    StartCoroutine(makeQuittable());
                    paused = true;
                }
            }
        }

        if (paused)
        {
            if (pauseMenu.activeSelf == true)
            {
                foreach (Player i in players)
                {
                    if (i.GetButtonDown("Start") && canQuit)
                    {
                        pauseMenu.SetActive(false);
                        Time.timeScale = 1;
                        StartCoroutine(makeQuittable());
                        paused = false;
                    }
                    if (i.GetButtonDown("Instruct"))
                    {
                        pauseMenu.SetActive(false);
                        inst.SetActive(true);
                        StartCoroutine(makeQuittable());
                    }
                    if (i.GetButtonDown("Quit") && canQuit)
                    {
                        SceneManager.LoadScene("skydivers menu");
                    }
                }

                if (Input.GetKeyDown(KeyCode.Space) && canQuit)
                {
                    pauseMenu.SetActive(false);
                    Time.timeScale = 1;
                    paused = false;
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
                    pauseMenu.SetActive(false);
                    inst.SetActive(true);
                    StartCoroutine(makeQuittable());
                }
                if (Input.GetKeyDown(KeyCode.Escape) && canQuit)
                {
                    SceneManager.LoadScene("skydivers menu");
                }
            }
            if (inst.activeSelf == true)
            {
                foreach (Player i in players)
                {
                    if (i.GetButtonDown("Quit"))
                    {
                        pauseMenu.SetActive(true);
                        inst.SetActive(false);
                        StartCoroutine(makeQuittable());
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    pauseMenu.SetActive(true);
                    inst.SetActive(false);
                    StartCoroutine(makeQuittable());
                }
            }
        }
	}

    private IEnumerator makeQuittable()
    {
        canQuit = false;
        yield return new WaitForSecondsRealtime(0.15f);
        canQuit = true;
    }

    void SetControlsText()
    {
        names[0].text = SkydiversOptions.p1Name;
        names[1].text = SkydiversOptions.p2Name;
        names[2].text = SkydiversOptions.p3Name;
        names[3].text = SkydiversOptions.p4Name;

        for (int i = 0; i < 4; i++)
        {
            if (SkydiversOptions.playerIsUsingController[i])
            {
                controls[i].SetActive(true);
                keys[i].SetActive(false);
                if (SkydiversOptions.controllerTypes[i] == "Sony DualShock 4")
                {
                    drop[i].sprite = dropOptions[0];
                    slow[i].sprite = slowOptions[0];
                }
                else
                {
                    drop[i].sprite = dropOptions[1];
                    slow[i].sprite = slowOptions[1];
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
