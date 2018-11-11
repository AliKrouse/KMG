using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class menuControl : MonoBehaviour
{
    private Player[] p = new Player[4];
    private Player allControllers;

    public GameObject screen1, screen2, screen3;

    public Image[] images;
    public Sprite[] readied;
    public Sprite[] unReadied;
    public Sprite redLight, greenLight;
    public Text p1r, p2r, p3r, p4r;
    public Text countdown;
    private bool p1InGame, p2InGame, p3InGame, p4InGame;
    private bool p1Ready, p2Ready, p3Ready, p4Ready, counting;

    private bool acceptInput = true;

    private AudioSource source;
    public AudioClip confirm, back, go;

    public Text[] controls;
    public GameObject[] controllerInputs;
    public Image[] dropButton, slowButton;
    public Sprite[] dropButtonOptions, slowButtonOptions;

    public InputField p1Input, p2Input, p3Input, p4Input;

    public GameObject[] highlights;

    private Joystick j;

    void Start ()
    {
        source = GetComponent<AudioSource>();

        for (int i = 0; i < 4; i++)
        {
            p[i] = ReInput.players.GetPlayer(i);

            images[i].color = Color.black;
        }
        allControllers = ReInput.players.GetPlayer(4);

        foreach (Joystick j in ReInput.controllers.Joysticks)
            allControllers.controllers.AddController(j, false);

        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);

        SkydiversOptions.characterChoices[0] = 0;
        SkydiversOptions.characterChoices[1] = 1;
        SkydiversOptions.characterChoices[2] = 4;
        SkydiversOptions.characterChoices[3] = 5;

        foreach (GameObject obj in highlights)
            obj.SetActive(false);

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnected;

        Time.timeScale = 1;

        foreach (Text t in controls)
            t.gameObject.SetActive(true);
        foreach (GameObject obj in controllerInputs)
            obj.SetActive(false);
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        j = ReInput.controllers.GetJoystick(args.controllerId);
        allControllers.controllers.AddController(j, true);
        j.tag = null;
    }
    void OnControllerPreDisconnected(ControllerStatusChangedEventArgs args)
    {
        j = ReInput.controllers.GetJoystick(args.controllerId);
        if (j.tag == "assigned")
        {
            for (int i = 0; i < 4; i++)
            {
                if (p[i].controllers.ContainsController(j))
                {
                    p[i].controllers.RemoveController(j);
                    images[i].color = Color.black;
                    images[i].sprite = unReadied[SkydiversOptions.characterChoices[i]];
                    images[i + 4].sprite = redLight;
                    SkydiversOptions.playerIsUsingController[i] = false;

                    if (i == 0)
                    {
                        controls[0].text = "Press S or X / A\non a controller\nto join game";
                        p1r.text = "";
                        p1Ready = false;
                        p1InGame = false;
                    }
                    if (i == 1)
                    {
                        controls[1].text = "Press G or X / A\non a controller\nto join game";
                        p2r.text = "";
                        p2Ready = false;
                        p2InGame = false;
                    }
                    if (i == 2)
                    {
                        controls[2].text = "Press K or X / A\non a controller\nto join game";
                        p3r.text = "";
                        p3Ready = false;
                        p3InGame = false;
                    }
                    if (i == 3)
                    {
                        controls[3].text = "Press ▼ or X / A\non a controller\nto join game";
                        p4r.text = "";
                        p4Ready = false;
                        p4InGame = false;
                    }
                }
            }
        }
    }
	
	void Update ()
    {
        if (screen1.activeSelf == true)
        {
            if (allControllers.GetButtonDown("Start"))
            {
                screen2.SetActive(true);
                screen1.SetActive(false);
                StartCoroutine(waitBeforeInput());

                source.clip = confirm;
                source.Play();
            }
            if (allControllers.GetButtonDown("Back") && acceptInput)
            {
                SceneManager.LoadScene("kmg menu");
            }
        }

        if (screen2.activeSelf == true)
        {
            if (allControllers.GetButtonDown("Start") && acceptInput)
            {
                screen3.SetActive(true);
                screen2.SetActive(false);

                source.clip = confirm;
                source.Play();

                StartCoroutine(waitBeforeInput());
            }
            if (allControllers.GetButtonDown("Back") && acceptInput)
            {
                screen1.SetActive(true);
                screen2.SetActive(false);
                StartCoroutine(waitBeforeInput());

                source.clip = back;
                source.Play();
            }
        }

        if (screen3.activeSelf == true)
        {
            if (!p1InGame)
            {
                controls[0].text = "Press S or X / A\non a controller\nto join game";
                p1r.text = "";
                images[0].color = Color.black;
                if (Input.GetKeyDown(KeyCode.S) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SkydiversOptions.playerIsUsingController[0] = false;
                    p1InGame = true;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Drop"))
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[0].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[0].controllers.maps.SetAllMapsEnabled(true);

                        SkydiversOptions.controllerChoice[0] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SkydiversOptions.playerIsUsingController[0] = true;
                        SkydiversOptions.controllerTypes[0] = ReInput.controllers.GetLastActiveController().name;

                        p1InGame = true;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!p2InGame)
            {
                controls[1].text = "Press G or X / A\non a controller\nto join game";
                p2r.text = "";
                images[1].color = Color.black;
                if (Input.GetKeyDown(KeyCode.G) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SkydiversOptions.playerIsUsingController[1] = false;
                    p2InGame = true;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Drop") && p1InGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[1].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[1].controllers.maps.SetAllMapsEnabled(true);

                        SkydiversOptions.controllerChoice[1] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SkydiversOptions.playerIsUsingController[1] = true;
                        SkydiversOptions.controllerTypes[1] = ReInput.controllers.GetLastActiveController().name;

                        p2InGame = true;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!p3InGame)
            {
                controls[2].text = "Press K or X / A\non a controller\nto join game";
                p3r.text = "";
                images[2].color = Color.black;
                if (Input.GetKeyDown(KeyCode.K) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SkydiversOptions.playerIsUsingController[2] = false;
                    p3InGame = true;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Drop") && p2InGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[2].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[2].controllers.maps.SetAllMapsEnabled(true);

                        SkydiversOptions.controllerChoice[2] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SkydiversOptions.playerIsUsingController[2] = true;
                        SkydiversOptions.controllerTypes[2] = ReInput.controllers.GetLastActiveController().name;

                        p3InGame = true;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!p4InGame)
            {
                controls[3].text = "Press ▼ or X / A\non a controller\nto join game";
                p4r.text = "";
                images[3].color = Color.black;
                if (Input.GetKeyDown(KeyCode.DownArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SkydiversOptions.playerIsUsingController[3] = false;
                    p4InGame = true;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Drop") && p3InGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[3].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[3].controllers.maps.SetAllMapsEnabled(true);

                        SkydiversOptions.controllerChoice[3] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SkydiversOptions.playerIsUsingController[3] = true;
                        SkydiversOptions.controllerTypes[3] = ReInput.controllers.GetLastActiveController().name;

                        p4InGame = true;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }

            if (p1InGame)
            {
                images[0].color = Color.white;
                if (!p1Ready)
                    p1r.text = "PRESS DROP TO READY";
                if (!SkydiversOptions.playerIsUsingController[0])
                {
                    controls[0].text = "W - Slow Down\nS - Drop\nA - Move Left\nD - Move Right";
                    if (Input.GetKeyDown(KeyCode.S) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p1Ready && acceptInput)
                    {
                        p1Ready = true;
                        images[0].sprite = readied[SkydiversOptions.characterChoices[0]];
                        images[4].sprite = greenLight;
                        p1r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.A) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[0]--;
                        if (SkydiversOptions.characterChoices[0] < 0)
                            SkydiversOptions.characterChoices[0] = 3;
                        if (SkydiversOptions.characterChoices[0] == SkydiversOptions.characterChoices[1])
                            SkydiversOptions.characterChoices[0]--;
                        if (SkydiversOptions.characterChoices[0] < 0)
                            SkydiversOptions.characterChoices[0] = 3;

                        if (!p1Ready)
                            images[0].sprite = unReadied[SkydiversOptions.characterChoices[0]];
                        else
                            images[0].sprite = readied[SkydiversOptions.characterChoices[0]];
                    }
                    if (Input.GetKeyDown(KeyCode.D) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[0]++;
                        if (SkydiversOptions.characterChoices[0] > 3)
                            SkydiversOptions.characterChoices[0] = 0;
                        if (SkydiversOptions.characterChoices[0] == SkydiversOptions.characterChoices[1])
                            SkydiversOptions.characterChoices[0]++;
                        if (SkydiversOptions.characterChoices[0] > 3)
                            SkydiversOptions.characterChoices[0] = 0;

                        if (!p1Ready)
                            images[0].sprite = unReadied[SkydiversOptions.characterChoices[0]];
                        else
                            images[0].sprite = readied[SkydiversOptions.characterChoices[0]];
                    }
                }
                else
                {
                    controls[0].gameObject.SetActive(false);
                    controllerInputs[0].gameObject.SetActive(true);
                    if (SkydiversOptions.controllerTypes[0] == "Sony DualShock 4")
                    {
                        dropButton[0].sprite = dropButtonOptions[0];
                        slowButton[0].sprite = slowButtonOptions[0];
                    }
                    else
                    {
                        dropButton[0].sprite = dropButtonOptions[1];
                        slowButton[0].sprite = slowButtonOptions[1];
                    }

                    if (p[0].GetButtonDown("Drop") && !p1Ready && acceptInput)
                    {
                        p1Ready = true;
                        images[0].sprite = readied[SkydiversOptions.characterChoices[0]];
                        images[4].sprite = greenLight;
                        p1r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[0].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[0]--;
                        if (SkydiversOptions.characterChoices[0] < 0)
                            SkydiversOptions.characterChoices[0] = 3;
                        if (SkydiversOptions.characterChoices[0] == SkydiversOptions.characterChoices[1])
                            SkydiversOptions.characterChoices[0]--;
                        if (SkydiversOptions.characterChoices[0] < 0)
                            SkydiversOptions.characterChoices[0] = 3;

                        if (!p1Ready)
                            images[0].sprite = unReadied[SkydiversOptions.characterChoices[0]];
                        else
                            images[0].sprite = readied[SkydiversOptions.characterChoices[0]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[0].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[0]++;
                        if (SkydiversOptions.characterChoices[0] > 3)
                            SkydiversOptions.characterChoices[0] = 0;
                        if (SkydiversOptions.characterChoices[0] == SkydiversOptions.characterChoices[1])
                            SkydiversOptions.characterChoices[0]++;
                        if (SkydiversOptions.characterChoices[0] > 3)
                            SkydiversOptions.characterChoices[0] = 0;

                        if (!p1Ready)
                            images[0].sprite = unReadied[SkydiversOptions.characterChoices[0]];
                        else
                            images[0].sprite = readied[SkydiversOptions.characterChoices[0]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[0].GetButtonDown("Highlight"))
                    {
                        highlights[0].SetActive(true);
                        highlights[0].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (p2InGame)
            {
                images[1].color = Color.white;
                if (!p2Ready)
                    p2r.text = "PRESS DROP TO READY";
                if (!SkydiversOptions.playerIsUsingController[1])
                {
                    controls[1].text = "T - slow down\nG - drop\nF - move left\nH - move right";
                    if (Input.GetKeyDown(KeyCode.G) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p2Ready && acceptInput)
                    {
                        p2Ready = true;
                        images[1].sprite = readied[SkydiversOptions.characterChoices[1]];
                        images[5].sprite = greenLight;
                        p2r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.F) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[1]--;
                        if (SkydiversOptions.characterChoices[1] < 0)
                            SkydiversOptions.characterChoices[1] = 3;
                        if (SkydiversOptions.characterChoices[1] == SkydiversOptions.characterChoices[0])
                            SkydiversOptions.characterChoices[1]--;
                        if (SkydiversOptions.characterChoices[1] < 0)
                            SkydiversOptions.characterChoices[1] = 3;

                        if (!p2Ready)
                            images[1].sprite = unReadied[SkydiversOptions.characterChoices[1]];
                        else
                            images[1].sprite = readied[SkydiversOptions.characterChoices[1]];
                    }
                    if (Input.GetKeyDown(KeyCode.H) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[1]++;
                        if (SkydiversOptions.characterChoices[1] > 3)
                            SkydiversOptions.characterChoices[1] = 0;
                        if (SkydiversOptions.characterChoices[1] == SkydiversOptions.characterChoices[0])
                            SkydiversOptions.characterChoices[1]++;
                        if (SkydiversOptions.characterChoices[1] > 3)
                            SkydiversOptions.characterChoices[1] = 0;

                        if (!p2Ready)
                            images[1].sprite = unReadied[SkydiversOptions.characterChoices[1]];
                        else
                            images[1].sprite = readied[SkydiversOptions.characterChoices[1]];
                    }
                }
                else
                {
                    controls[1].gameObject.SetActive(false);
                    controllerInputs[1].gameObject.SetActive(true);
                    if (SkydiversOptions.controllerTypes[1] == "Sony DualShock 4")
                    {
                        dropButton[1].sprite = dropButtonOptions[0];
                        slowButton[1].sprite = slowButtonOptions[0];
                    }
                    else
                    {
                        dropButton[1].sprite = dropButtonOptions[1];
                        slowButton[1].sprite = slowButtonOptions[1];
                    }

                    if (p[1].GetButtonDown("Drop") && !p2Ready && acceptInput)
                    {
                        p2Ready = true;
                        images[1].sprite = readied[SkydiversOptions.characterChoices[1]];
                        images[5].sprite = greenLight;
                        p2r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[1].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[1]--;
                        if (SkydiversOptions.characterChoices[1] < 0)
                            SkydiversOptions.characterChoices[1] = 3;
                        if (SkydiversOptions.characterChoices[1] == SkydiversOptions.characterChoices[0])
                            SkydiversOptions.characterChoices[1]--;
                        if (SkydiversOptions.characterChoices[1] < 0)
                            SkydiversOptions.characterChoices[1] = 3;

                        if (!p2Ready)
                            images[1].sprite = unReadied[SkydiversOptions.characterChoices[1]];
                        else
                            images[1].sprite = readied[SkydiversOptions.characterChoices[1]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[1].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[1]++;
                        if (SkydiversOptions.characterChoices[1] > 3)
                            SkydiversOptions.characterChoices[1] = 0;
                        if (SkydiversOptions.characterChoices[1] == SkydiversOptions.characterChoices[0])
                            SkydiversOptions.characterChoices[1]++;
                        if (SkydiversOptions.characterChoices[1] > 3)
                            SkydiversOptions.characterChoices[1] = 0;

                        if (!p2Ready)
                            images[1].sprite = unReadied[SkydiversOptions.characterChoices[1]];
                        else
                            images[1].sprite = readied[SkydiversOptions.characterChoices[1]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[1].GetButtonDown("Highlight"))
                    {
                        highlights[1].SetActive(true);
                        highlights[1].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (p3InGame)
            {
                images[2].color = Color.white;
                if (!p3Ready)
                    p3r.text = "PRESS DROP TO READY";
                if (!SkydiversOptions.playerIsUsingController[2])
                {
                    controls[2].text = "I - slow down\nK - drop\nJ - move left\nL - move right";
                    if (Input.GetKeyDown(KeyCode.K) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p3Ready && acceptInput)
                    {
                        p3Ready = true;
                        images[2].sprite = readied[SkydiversOptions.characterChoices[2]];
                        images[6].sprite = greenLight;
                        p3r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.J) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[2]--;
                        if (SkydiversOptions.characterChoices[2] < 4)
                            SkydiversOptions.characterChoices[2] = 7;
                        if (SkydiversOptions.characterChoices[2] == SkydiversOptions.characterChoices[3])
                            SkydiversOptions.characterChoices[2]--;
                        if (SkydiversOptions.characterChoices[2] < 4)
                            SkydiversOptions.characterChoices[2] = 7;

                        if (!p3Ready)
                            images[2].sprite = unReadied[SkydiversOptions.characterChoices[2]];
                        else
                            images[2].sprite = readied[SkydiversOptions.characterChoices[2]];
                    }
                    if (Input.GetKeyDown(KeyCode.L) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[2]++;
                        if (SkydiversOptions.characterChoices[2] > 7)
                            SkydiversOptions.characterChoices[2] = 4;
                        if (SkydiversOptions.characterChoices[2] == SkydiversOptions.characterChoices[3])
                            SkydiversOptions.characterChoices[2]++;
                        if (SkydiversOptions.characterChoices[2] > 7)
                            SkydiversOptions.characterChoices[2] = 4;

                        if (!p3Ready)
                            images[2].sprite = unReadied[SkydiversOptions.characterChoices[2]];
                        else
                            images[2].sprite = readied[SkydiversOptions.characterChoices[2]];
                    }
                }
                else
                {
                    controls[2].gameObject.SetActive(false);
                    controllerInputs[2].gameObject.SetActive(true);
                    if (SkydiversOptions.controllerTypes[2] == "Sony DualShock 4")
                    {
                        dropButton[2].sprite = dropButtonOptions[0];
                        slowButton[2].sprite = slowButtonOptions[0];
                    }
                    else
                    {
                        dropButton[2].sprite = dropButtonOptions[1];
                        slowButton[2].sprite = slowButtonOptions[1];
                    }

                    if (p[2].GetButtonDown("Drop") && !p3Ready && acceptInput)
                    {
                        p3Ready = true;
                        images[2].sprite = readied[SkydiversOptions.characterChoices[2]];
                        images[6].sprite = greenLight;
                        p3r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[2].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[2]--;
                        if (SkydiversOptions.characterChoices[2] < 4)
                            SkydiversOptions.characterChoices[2] = 7;
                        if (SkydiversOptions.characterChoices[2] == SkydiversOptions.characterChoices[3])
                            SkydiversOptions.characterChoices[2]--;
                        if (SkydiversOptions.characterChoices[2] < 4)
                            SkydiversOptions.characterChoices[2] = 7;

                        if (!p3Ready)
                            images[2].sprite = unReadied[SkydiversOptions.characterChoices[2]];
                        else
                            images[2].sprite = readied[SkydiversOptions.characterChoices[2]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[2].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[2]++;
                        if (SkydiversOptions.characterChoices[2] > 7)
                            SkydiversOptions.characterChoices[2] = 4;
                        if (SkydiversOptions.characterChoices[2] == SkydiversOptions.characterChoices[3])
                            SkydiversOptions.characterChoices[2]++;
                        if (SkydiversOptions.characterChoices[2] > 7)
                            SkydiversOptions.characterChoices[2] = 4;

                        if (!p3Ready)
                            images[2].sprite = unReadied[SkydiversOptions.characterChoices[2]];
                        else
                            images[2].sprite = readied[SkydiversOptions.characterChoices[2]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[2].GetButtonDown("Highlight"))
                    {
                        highlights[2].SetActive(true);
                        highlights[2].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (p4InGame)
            {
                images[3].color = Color.white;
                if (!p4Ready)
                    p4r.text = "PRESS DROP TO READY";
                if (!SkydiversOptions.playerIsUsingController[3])
                {
                    controls[3].text = "▲ - slow down\n▼ -drop\n◄ -move left\n► -move right";
                    if (Input.GetKeyDown(KeyCode.DownArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p4Ready && acceptInput)
                    {
                        p4Ready = true;
                        images[3].sprite = readied[SkydiversOptions.characterChoices[3]];
                        images[7].sprite = greenLight;
                        p4r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[3]--;
                        if (SkydiversOptions.characterChoices[3] < 4)
                            SkydiversOptions.characterChoices[3] = 7;
                        if (SkydiversOptions.characterChoices[3] == SkydiversOptions.characterChoices[2])
                            SkydiversOptions.characterChoices[3]--;
                        if (SkydiversOptions.characterChoices[3] < 4)
                            SkydiversOptions.characterChoices[3] = 7;

                        if (!p4Ready)
                            images[3].sprite = unReadied[SkydiversOptions.characterChoices[3]];
                        else
                            images[3].sprite = readied[SkydiversOptions.characterChoices[3]];
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SkydiversOptions.characterChoices[3]++;
                        if (SkydiversOptions.characterChoices[3] > 7)
                            SkydiversOptions.characterChoices[3] = 4;
                        if (SkydiversOptions.characterChoices[3] == SkydiversOptions.characterChoices[2])
                            SkydiversOptions.characterChoices[3]++;
                        if (SkydiversOptions.characterChoices[3] > 7)
                            SkydiversOptions.characterChoices[3] = 4;

                        if (!p4Ready)
                            images[3].sprite = unReadied[SkydiversOptions.characterChoices[3]];
                        else
                            images[3].sprite = readied[SkydiversOptions.characterChoices[3]];
                    }
                }
                else
                {
                    controls[3].gameObject.SetActive(false);
                    controllerInputs[3].gameObject.SetActive(true);
                    if (SkydiversOptions.controllerTypes[3] == "Sony DualShock 4")
                    {
                        dropButton[3].sprite = dropButtonOptions[0];
                        slowButton[3].sprite = slowButtonOptions[0];
                    }
                    else
                    {
                        dropButton[3].sprite = dropButtonOptions[1];
                        slowButton[3].sprite = slowButtonOptions[1];
                    }

                    if (p[3].GetButtonDown("Drop") && !p4Ready && acceptInput)
                    {
                        p4Ready = true;
                        images[3].sprite = readied[SkydiversOptions.characterChoices[3]];
                        images[7].sprite = greenLight;
                        p4r.text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[3].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[3]--;
                        if (SkydiversOptions.characterChoices[3] < 4)
                            SkydiversOptions.characterChoices[3] = 7;
                        if (SkydiversOptions.characterChoices[3] == SkydiversOptions.characterChoices[2])
                            SkydiversOptions.characterChoices[3]--;
                        if (SkydiversOptions.characterChoices[3] < 4)
                            SkydiversOptions.characterChoices[3] = 7;

                        if (!p4Ready)
                            images[3].sprite = unReadied[SkydiversOptions.characterChoices[3]];
                        else
                            images[3].sprite = readied[SkydiversOptions.characterChoices[3]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[3].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SkydiversOptions.characterChoices[3]++;
                        if (SkydiversOptions.characterChoices[3] > 7)
                            SkydiversOptions.characterChoices[3] = 4;
                        if (SkydiversOptions.characterChoices[3] == SkydiversOptions.characterChoices[2])
                            SkydiversOptions.characterChoices[3]++;
                        if (SkydiversOptions.characterChoices[3] > 7)
                            SkydiversOptions.characterChoices[3] = 4;

                        if (!p4Ready)
                            images[3].sprite = unReadied[SkydiversOptions.characterChoices[3]];
                        else
                            images[3].sprite = readied[SkydiversOptions.characterChoices[3]];

                        StartCoroutine(waitBeforeInput());
                    }
                    if (p[3].GetButtonDown("Highlight"))
                    {
                        highlights[3].SetActive(true);
                        highlights[3].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }

            if (p1Ready && p2Ready && p3Ready && p4Ready && !counting)
            {
                StartCoroutine(startCountdown());
            }
            if (counting)
            {
                if (allControllers.GetButtonDown("Back"))
                {
                    StopAllCoroutines();
                    countdown.gameObject.SetActive(false);
                    counting = false;

                    p1Ready = false;
                    p1r.text = "Press DROP to ready";
                    images[0].sprite = unReadied[SkydiversOptions.characterChoices[0]];
                    images[4].sprite = redLight;
                    p2Ready = false;
                    p2r.text = "Press DROP to ready";
                    images[1].sprite = unReadied[SkydiversOptions.characterChoices[1]];
                    images[5].sprite = redLight;
                    p3Ready = false;
                    p3r.text = "Press DROP to ready";
                    images[2].sprite = unReadied[SkydiversOptions.characterChoices[2]];
                    images[6].sprite = redLight;
                    p4Ready = false;
                    p4r.text = "Press DROP to ready";
                    images[3].sprite = unReadied[SkydiversOptions.characterChoices[3]];
                    images[7].sprite = redLight;

                    source.clip = back;
                    source.Play();

                    for (int i = 0; i < 4; i++)
                    {
                        SkydiversOptions.playerIsUsingController[i] = false;
                    }

                    StartCoroutine(waitBeforeInput());
                }
            }
            if (allControllers.GetButtonDown("Back") && !counting)
            {
                p1Ready = false; p2Ready = false;
                p3Ready = false; p4Ready = false;
                p1InGame = false; p2InGame = false;
                p3InGame = false; p4InGame = false;

                for (int i = 0; i < 4; i++)
                {
                    images[i].sprite = unReadied[SkydiversOptions.characterChoices[i]];
                    images[i].color = Color.black;

                    if (SkydiversOptions.playerIsUsingController[i])
                    {
                        SkydiversOptions.playerIsUsingController[i] = false;
                        p[i].controllers.GetLastActiveController().tag = null;
                        p[i].controllers.RemoveController(p[i].controllers.GetLastActiveController());
                    }
                }
                for (int i = 4; i < 8; i++)
                {
                    images[i].sprite = redLight;
                }

                controls[0].text = "Press S or X / A\non a controller\nto join game";
                controls[1].text = "Press G or X / A\non a controller\nto join game";
                controls[2].text = "Press K or X / A\non a controller\nto join game";
                controls[3].text = "Press ▼ or X / A\non a controller\nto join game";
                p1r.text = "";
                p2r.text = "";
                p3r.text = "";
                p4r.text = "";

                foreach (Text t in controls)
                    t.gameObject.SetActive(true);
                foreach (GameObject obj in controllerInputs)
                    obj.SetActive(false);

                screen1.SetActive(true);
                screen3.SetActive(false);
                StartCoroutine(waitBeforeInput());

                source.clip = back;
                source.Play();
            }

            if (p1Input.isFocused)
            {
                p1Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SkydiversOptions.p1Name = p1Input.text;
            }
            else
                p1Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p2Input.isFocused)
            {
                p2Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SkydiversOptions.p2Name = p2Input.text;
            }
            else
                p2Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p3Input.isFocused)
            {
                p3Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SkydiversOptions.p3Name = p3Input.text;
            }
            else
                p3Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p4Input.isFocused)
            {
                p4Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SkydiversOptions.p4Name = p4Input.text;
            }
            else
                p4Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        }
	}

    private IEnumerator startCountdown()
    {
        counting = true;
        countdown.gameObject.SetActive(true);
        countdown.text = "GAME BEGINS IN 3...";
        source.clip = confirm;
        source.Play();
        yield return new WaitForSeconds(1);
        countdown.text = "GAME BEGINS IN 2...";
        source.clip = confirm;
        source.Play();
        yield return new WaitForSeconds(1);
        countdown.text = "GAME BEGINS IN 1...";
        source.clip = confirm;
        source.Play();
        yield return new WaitForSeconds(1);
        countdown.text = "GAME BEGINNING...";
        source.clip = go;
        source.Play();
        SceneManager.LoadScene("skydivers arena");
    }

    private IEnumerator waitBeforeInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.15f);
        acceptInput = true;
    }
}
