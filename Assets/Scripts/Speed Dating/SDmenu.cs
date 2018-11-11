using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class SDmenu : MonoBehaviour
{
    private Player[] p = new Player[4];
    private Player allControllers;

    public GameObject screen1, screen2, screen3, screen4;

    public Image[] players;
    public Sprite[] ready;
    public Sprite[] unready;
    public Text[] readyText;
    public Text countdown;
    private bool inGame1, inGame2, inGame3, inGame4;
    private bool ready1, ready2, ready3, ready4;

    private bool countdownIsRunning;

    public GameObject selector;
    public Transform[] selectorPoints;
    public Text AINumber, hitsNumber, timeNumber;

    private bool acceptInput = true;

    private AudioSource source;
    public AudioClip confirm, go, back;
    
    private int selectorNumber;
    public Text[] controls;
    public GameObject[] controllerInputs;
    public Image[] kissButton;
    public Sprite[] kissButtonOptions;

    public InputField p1Input, p2Input, p3Input, p4Input;

    public GameObject[] highlights;
    public Color[] playerColors;

    private Joystick j;

    private int playersInGame;

    void Start ()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);

        source = GetComponent<AudioSource>();

        Time.timeScale = 1;

        for (int i = 0; i < 4; i++)
        {
            p[i] = ReInput.players.GetPlayer(i);
            players[i].color = Color.black;
        }
        allControllers = ReInput.players.GetPlayer(4);

        foreach (Joystick j in ReInput.controllers.Joysticks)
            allControllers.controllers.AddController(j, false);

        SDOptions.characterChoices[0] = 0;
        SDOptions.characterChoices[1] = 1;
        SDOptions.characterChoices[2] = 2;
        SDOptions.characterChoices[3] = 3;

        foreach (GameObject obj in highlights)
            obj.SetActive(false);

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnected;

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
                    players[i].color = Color.black;
                    players[i].sprite = unready[SDOptions.characterChoices[i]];
                    SDOptions.playerIsUsingController[i] = false;

                    controls[i].text = "";

                    if (i == 0)
                    {
                        readyText[0].text = "Press W or X / A on a controller to join game";
                        ready1 = false;
                        inGame1 = false;
                    }
                    if (i == 1)
                    {
                        readyText[1].text = "Press T or X / A on a controller to join game";
                        ready2 = false;
                        inGame2 = false;
                    }
                    if (i == 2)
                    {
                        readyText[2].text = "Press I or X / A on a controller to join game";
                        ready3 = false;
                        inGame3 = false;
                    }
                    if (i == 3)
                    {
                        readyText[3].text = "Press ▲ or X / A on a controller to join game";
                        ready4 = false;
                        inGame4 = false;
                    }

                    playersInGame--;
                    SDOptions.playerIsInGame[i] = false;
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

                source.PlayOneShot(confirm);
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
                StartCoroutine(waitBeforeInput());

                source.PlayOneShot(confirm);
            }
            if (allControllers.GetButtonDown("Back") && acceptInput)
            {
                screen1.SetActive(true);
                screen2.SetActive(false);
                StartCoroutine(waitBeforeInput());

                source.PlayOneShot(back);
            }
        }
        if (screen3.activeSelf == true && screen4.activeSelf == false)
        {
            if (!inGame1)
            {
                controls[0].text = "";
                readyText[0].text = "Press W or X / A on a controller to join game";
                players[0].color = Color.black;
                if (Input.GetKeyDown(KeyCode.W) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SDOptions.playerIsUsingController[0] = false;
                    inGame1 = true;
                    SDOptions.playerIsInGame[0] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Assign"))
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[0].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[0].controllers.maps.SetAllMapsEnabled(true);

                        SDOptions.controllerChoice[0] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SDOptions.playerIsUsingController[0] = true;
                        SDOptions.controllerType[0] = ReInput.controllers.GetLastActiveController().name;

                        inGame1 = true;
                        SDOptions.playerIsInGame[0] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame2)
            {
                controls[1].text = "";
                readyText[1].text = "Press T or X / A on a controller to join game";
                players[0].color = Color.black;
                if (Input.GetKeyDown(KeyCode.T) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SDOptions.playerIsUsingController[1] = false;
                    inGame2 = true;
                    SDOptions.playerIsInGame[1] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Assign") && inGame1)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[1].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[1].controllers.maps.SetAllMapsEnabled(true);

                        SDOptions.controllerChoice[1] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SDOptions.playerIsUsingController[1] = true;
                        SDOptions.controllerType[1] = ReInput.controllers.GetLastActiveController().name;

                        inGame2 = true;
                        SDOptions.playerIsInGame[1] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame3)
            {
                controls[2].text = "";
                readyText[2].text = "Press I or X / A on a controller to join game";
                players[2].color = Color.black;
                if (Input.GetKeyDown(KeyCode.I) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SDOptions.playerIsUsingController[2] = false;
                    inGame3 = true;
                    SDOptions.playerIsInGame[2] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Assign") && inGame2)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[2].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[2].controllers.maps.SetAllMapsEnabled(true);

                        SDOptions.controllerChoice[2] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SDOptions.playerIsUsingController[2] = true;
                        SDOptions.controllerType[2] = ReInput.controllers.GetLastActiveController().name;

                        inGame3 = true;
                        SDOptions.playerIsInGame[2] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame4)
            {
                controls[3].text = "";
                readyText[3].text = "Press ▲ or X / A on a controller to join game";
                players[3].color = Color.black;
                if (Input.GetKeyDown(KeyCode.UpArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    SDOptions.playerIsUsingController[3] = false;
                    inGame4 = true;
                    SDOptions.playerIsInGame[3] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Assign") && inGame3)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[3].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[3].controllers.maps.SetAllMapsEnabled(true);

                        SDOptions.controllerChoice[3] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        SDOptions.playerIsUsingController[3] = true;
                        SDOptions.controllerType[3] = ReInput.controllers.GetLastActiveController().name;

                        inGame4 = true;
                        SDOptions.playerIsInGame[3] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }

            if (inGame1)
            {
                players[0].color = Color.white;
                if (!SDOptions.playerIsUsingController[0])
                {
                    controls[0].text = "WASD - MOVE\nE - KISS";
                    if (!ready1)
                        readyText[0].text = "Press UP to ready!";
                    if (Input.GetKeyDown(KeyCode.W) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !ready1)
                    {
                        ready1 = true;
                        players[0].sprite = ready[SDOptions.characterChoices[0]];
                        readyText[0].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.A) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SDOptions.characterChoices[0]--;
                        if (SDOptions.characterChoices[0] < 0)
                            SDOptions.characterChoices[0] = 7;
                        while (SDOptions.characterChoices[0] == SDOptions.characterChoices[1] || SDOptions.characterChoices[0] == SDOptions.characterChoices[2] || SDOptions.characterChoices[0] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[0]--;
                            if (SDOptions.characterChoices[0] < 0)
                                SDOptions.characterChoices[0] = 3;
                        }

                        if (!ready1)
                            players[0].sprite = unready[SDOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[SDOptions.characterChoices[0]];

                        highlights[0].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.placeholder.color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.textComponent.color = playerColors[SDOptions.characterChoices[0]];
                    }
                    if (Input.GetKeyDown(KeyCode.D) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        SDOptions.characterChoices[0]++;
                        if (SDOptions.characterChoices[0] > 7)
                            SDOptions.characterChoices[0] = 0;
                        while (SDOptions.characterChoices[0] == SDOptions.characterChoices[1] || SDOptions.characterChoices[0] == SDOptions.characterChoices[2] || SDOptions.characterChoices[0] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[0]++;
                            if (SDOptions.characterChoices[0] > 7)
                                SDOptions.characterChoices[0] = 0;
                        }

                        if (!ready1)
                            players[0].sprite = unready[SDOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[SDOptions.characterChoices[0]];

                        highlights[0].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.placeholder.color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.textComponent.color = playerColors[SDOptions.characterChoices[0]];
                    }
                }
                else
                {
                    //controls[0].text = "LEFT STICK - MOVE\nBUMP PLAYER - KISS";
                    controls[0].gameObject.SetActive(false);
                    controllerInputs[0].SetActive(true);
                    if (SDOptions.controllerType[0] == "Sony DualShock 4")
                        kissButton[0].sprite = kissButtonOptions[0];
                    else
                        kissButton[0].sprite = kissButtonOptions[1];

                    if (!ready1)
                        readyText[0].text = "Press UP to ready!";
                    if (p[0].GetAxis("Vertical") > 0.5 && !ready1)
                    {
                        ready1 = true;
                        players[0].sprite = ready[0];
                        readyText[0].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[0].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[0]--;
                        if (SDOptions.characterChoices[0] < 0)
                            SDOptions.characterChoices[0] = 7;
                        while (SDOptions.characterChoices[0] == SDOptions.characterChoices[1] || SDOptions.characterChoices[0] == SDOptions.characterChoices[2] || SDOptions.characterChoices[0] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[0]--;
                            if (SDOptions.characterChoices[0] < 0)
                                SDOptions.characterChoices[0] = 7;
                        }

                        if (!ready1)
                            players[0].sprite = unready[SDOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[SDOptions.characterChoices[0]];

                        StartCoroutine(waitBeforeInput());

                        highlights[0].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.placeholder.color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.textComponent.color = playerColors[SDOptions.characterChoices[0]];
                    }
                    if (p[0].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[0]++;
                        if (SDOptions.characterChoices[0] > 7)
                            SDOptions.characterChoices[0] = 0;
                        while (SDOptions.characterChoices[0] == SDOptions.characterChoices[1] || SDOptions.characterChoices[0] == SDOptions.characterChoices[2] || SDOptions.characterChoices[0] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[0]++;
                            if (SDOptions.characterChoices[0] > 7)
                                SDOptions.characterChoices[0] = 0;
                        }

                        if (!ready1)
                            players[0].sprite = unready[SDOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[SDOptions.characterChoices[0]];

                        StartCoroutine(waitBeforeInput());

                        highlights[0].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.placeholder.color = playerColors[SDOptions.characterChoices[0]];
                        p1Input.textComponent.color = playerColors[SDOptions.characterChoices[0]];
                    }
                    if (p[0].GetButtonDown("Highlight"))
                    {
                        highlights[0].SetActive(true);
                        highlights[0].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame2)
            {
                players[1].color = Color.white;
                if (!SDOptions.playerIsUsingController[1])
                {
                    controls[1].text = "TFGH - MOVE\nY - KISS";
                    if (!ready2)
                        readyText[1].text = "Press UP to ready!";
                    if (Input.GetKeyDown(KeyCode.T) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !ready2)
                    {
                        ready2 = true;
                        players[1].sprite = ready[SDOptions.characterChoices[1]];
                        readyText[1].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.F) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput)
                    {
                        SDOptions.characterChoices[1]--;
                        if (SDOptions.characterChoices[1] < 0)
                            SDOptions.characterChoices[1] = 7;
                        while (SDOptions.characterChoices[1] == SDOptions.characterChoices[0] || SDOptions.characterChoices[1] == SDOptions.characterChoices[2] || SDOptions.characterChoices[1] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[1]--;
                            if (SDOptions.characterChoices[1] < 0)
                                SDOptions.characterChoices[1] = 7;
                        }

                        if (!ready2)
                            players[1].sprite = unready[SDOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[SDOptions.characterChoices[1]];

                        highlights[1].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.placeholder.color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.textComponent.color = playerColors[SDOptions.characterChoices[1]];
                    }
                    if (Input.GetKeyDown(KeyCode.H) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput)
                    {
                        SDOptions.characterChoices[1]++;
                        if (SDOptions.characterChoices[1] > 7)
                            SDOptions.characterChoices[1] = 0;
                        while (SDOptions.characterChoices[1] == SDOptions.characterChoices[0] || SDOptions.characterChoices[1] == SDOptions.characterChoices[2] || SDOptions.characterChoices[1] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[1]++;
                            if (SDOptions.characterChoices[1] > 7)
                                SDOptions.characterChoices[1] = 0;
                        }

                        if (!ready2)
                            players[1].sprite = unready[SDOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[SDOptions.characterChoices[1]];

                        highlights[1].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.placeholder.color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.textComponent.color = playerColors[SDOptions.characterChoices[1]];
                    }
                }
                else
                {
                    //controls[1].text = "LEFT STICK - MOVE\nBUMP PLAYER - KISS";
                    controls[1].gameObject.SetActive(false);
                    controllerInputs[1].SetActive(true);
                    if (SDOptions.controllerType[1] == "Sony DualShock 4")
                        kissButton[1].sprite = kissButtonOptions[0];
                    else
                        kissButton[1].sprite = kissButtonOptions[1];

                    if (!ready2)
                        readyText[1].text = "Press UP to ready!";
                    if (p[1].GetAxis("Vertical") > 0.5 && !ready2)
                    {
                        ready2 = true;
                        players[1].sprite = ready[SDOptions.characterChoices[1]];
                        readyText[1].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[1].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[1]--;
                        if (SDOptions.characterChoices[1] < 0)
                            SDOptions.characterChoices[1] = 7;
                        while (SDOptions.characterChoices[1] == SDOptions.characterChoices[0] || SDOptions.characterChoices[1] == SDOptions.characterChoices[2] || SDOptions.characterChoices[1] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[1]--;
                            if (SDOptions.characterChoices[1] < 0)
                                SDOptions.characterChoices[1] = 7;
                        }

                        if (!ready2)
                            players[1].sprite = unready[SDOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[SDOptions.characterChoices[1]];

                        StartCoroutine(waitBeforeInput());

                        highlights[1].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.placeholder.color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.textComponent.color = playerColors[SDOptions.characterChoices[1]];
                    }
                    if (p[1].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[1]++;
                        if (SDOptions.characterChoices[1] > 7)
                            SDOptions.characterChoices[1] = 0;
                        while (SDOptions.characterChoices[1] == SDOptions.characterChoices[0] || SDOptions.characterChoices[1] == SDOptions.characterChoices[2] || SDOptions.characterChoices[1] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[1]++;
                            if (SDOptions.characterChoices[1] > 7)
                                SDOptions.characterChoices[1] = 0;
                        }

                        if (!ready2)
                            players[1].sprite = unready[SDOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[SDOptions.characterChoices[1]];

                        StartCoroutine(waitBeforeInput());

                        highlights[1].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.placeholder.color = playerColors[SDOptions.characterChoices[1]];
                        p2Input.textComponent.color = playerColors[SDOptions.characterChoices[1]];
                    }
                    if (p[1].GetButtonDown("Highlight"))
                    {
                        highlights[1].SetActive(true);
                        highlights[1].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame3)
            {
                players[2].color = Color.white;
                if (!SDOptions.playerIsUsingController[2])
                {
                    controls[2].text = "IJKL - MOVE\nU - KISS";
                    if (!ready3)
                        readyText[2].text = "Press UP to ready!";
                    if (Input.GetKeyDown(KeyCode.I) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !ready3)
                    {
                        ready3 = true;
                        players[2].sprite = ready[SDOptions.characterChoices[2]];
                        readyText[2].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.J) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput)
                    {
                        SDOptions.characterChoices[2]--;
                        if (SDOptions.characterChoices[2] < 0)
                            SDOptions.characterChoices[2] = 7;
                        while (SDOptions.characterChoices[2] == SDOptions.characterChoices[0] || SDOptions.characterChoices[2] == SDOptions.characterChoices[1] || SDOptions.characterChoices[2] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[2]--;
                            if (SDOptions.characterChoices[2] < 0)
                                SDOptions.characterChoices[2] = 7;
                        }

                        if (!ready3)
                            players[2].sprite = unready[SDOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[SDOptions.characterChoices[2]];

                        highlights[2].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.placeholder.color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.textComponent.color = playerColors[SDOptions.characterChoices[2]];
                    }
                    if (Input.GetKeyDown(KeyCode.L) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput)
                    {
                        SDOptions.characterChoices[2]++;
                        if (SDOptions.characterChoices[2] > 7)
                            SDOptions.characterChoices[2] = 0;
                        while (SDOptions.characterChoices[2] == SDOptions.characterChoices[0] || SDOptions.characterChoices[2] == SDOptions.characterChoices[1] || SDOptions.characterChoices[2] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[2]++;
                            if (SDOptions.characterChoices[2] > 7)
                                SDOptions.characterChoices[2] = 0;
                        }

                        if (!ready3)
                            players[2].sprite = unready[SDOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[SDOptions.characterChoices[2]];

                        highlights[2].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.placeholder.color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.textComponent.color = playerColors[SDOptions.characterChoices[2]];
                    }
                }
                else
                {
                    //controls[2].text = "LEFT STICK - MOVE\nBUMP PLAYER - KISS";
                    controls[2].gameObject.SetActive(false);
                    controllerInputs[2].SetActive(true);
                    if (SDOptions.controllerType[2] == "Sony DualShock 4")
                        kissButton[2].sprite = kissButtonOptions[0];
                    else
                        kissButton[2].sprite = kissButtonOptions[1];

                    if (!ready3)
                        readyText[2].text = "Press UP to ready!";
                    if (p[2].GetAxis("Vertical") > 0.5 && !ready3)
                    {
                        ready3 = true;
                        players[2].sprite = ready[SDOptions.characterChoices[2]];
                        readyText[2].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[2].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[2]--;
                        if (SDOptions.characterChoices[2] < 0)
                            SDOptions.characterChoices[2] = 7;
                        while (SDOptions.characterChoices[2] == SDOptions.characterChoices[0] || SDOptions.characterChoices[2] == SDOptions.characterChoices[1] || SDOptions.characterChoices[2] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[2]--;
                            if (SDOptions.characterChoices[2] < 0)
                                SDOptions.characterChoices[2] = 7;
                        }

                        if (!ready3)
                            players[2].sprite = unready[SDOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[SDOptions.characterChoices[2]];

                        StartCoroutine(waitBeforeInput());

                        highlights[2].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.placeholder.color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.textComponent.color = playerColors[SDOptions.characterChoices[2]];
                    }
                    if (p[2].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[2]++;
                        if (SDOptions.characterChoices[2] > 7)
                            SDOptions.characterChoices[2] = 0;
                        while (SDOptions.characterChoices[2] == SDOptions.characterChoices[0] || SDOptions.characterChoices[2] == SDOptions.characterChoices[1] || SDOptions.characterChoices[2] == SDOptions.characterChoices[3])
                        {
                            SDOptions.characterChoices[2]++;
                            if (SDOptions.characterChoices[2] > 7)
                                SDOptions.characterChoices[2] = 0;
                        }

                        if (!ready3)
                            players[2].sprite = unready[SDOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[SDOptions.characterChoices[2]];

                        StartCoroutine(waitBeforeInput());

                        highlights[2].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.placeholder.color = playerColors[SDOptions.characterChoices[2]];
                        p3Input.textComponent.color = playerColors[SDOptions.characterChoices[2]];
                    }
                    if (p[2].GetButtonDown("Highlight"))
                    {
                        highlights[2].SetActive(true);
                        highlights[2].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame4)
            {
                players[3].color = Color.white;
                if (!SDOptions.playerIsUsingController[3])
                {
                    controls[3].text = "ARROWS - MOVE\nSHIFT - KISS";
                    if (!ready4)
                        readyText[3].text = "Press UP to ready!";
                    if (Input.GetKeyDown(KeyCode.UpArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !ready4)
                    {
                        ready4 = true;
                        players[3].sprite = ready[SDOptions.characterChoices[3]];
                        readyText[3].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput)
                    {
                        SDOptions.characterChoices[3]--;
                        if (SDOptions.characterChoices[3] < 0)
                            SDOptions.characterChoices[3] = 7;
                        while (SDOptions.characterChoices[3] == SDOptions.characterChoices[0] || SDOptions.characterChoices[3] == SDOptions.characterChoices[1] || SDOptions.characterChoices[3] == SDOptions.characterChoices[2])
                        {
                            SDOptions.characterChoices[3]--;
                            if (SDOptions.characterChoices[3] < 0)
                                SDOptions.characterChoices[3] = 7;
                        }

                        if (!ready4)
                            players[3].sprite = unready[SDOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[SDOptions.characterChoices[3]];

                        highlights[3].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.placeholder.color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.textComponent.color = playerColors[SDOptions.characterChoices[3]];
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput)
                    {
                        SDOptions.characterChoices[3]++;
                        if (SDOptions.characterChoices[3] > 7)
                            SDOptions.characterChoices[3] = 0;
                        while (SDOptions.characterChoices[3] == SDOptions.characterChoices[0] || SDOptions.characterChoices[3] == SDOptions.characterChoices[1] || SDOptions.characterChoices[3] == SDOptions.characterChoices[2])
                        {
                            SDOptions.characterChoices[3]++;
                            if (SDOptions.characterChoices[3] > 7)
                                SDOptions.characterChoices[3] = 0;
                        }

                        if (!ready4)
                            players[3].sprite = unready[SDOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[SDOptions.characterChoices[3]];

                        highlights[3].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.placeholder.color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.textComponent.color = playerColors[SDOptions.characterChoices[3]];
                    }
                }
                else
                {
                    //controls[3].text = "LEFT STICK - MOVE\nBUMP PLAYER - KISS";
                    controls[3].gameObject.SetActive(false);
                    controllerInputs[3].SetActive(true);
                    if (SDOptions.controllerType[3] == "Sony DualShock 4")
                        kissButton[3].sprite = kissButtonOptions[0];
                    else
                        kissButton[3].sprite = kissButtonOptions[1];

                    if (!ready4)
                        readyText[3].text = "Press UP to ready!";
                    if (p[3].GetAxis("Vertical") > 0.5 && !ready4)
                    {
                        ready4 = true;
                        players[3].sprite = ready[SDOptions.characterChoices[3]];
                        readyText[3].text = "Ready!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[3].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[3]--;
                        if (SDOptions.characterChoices[3] < 0)
                            SDOptions.characterChoices[3] = 7;
                        while (SDOptions.characterChoices[3] == SDOptions.characterChoices[0] || SDOptions.characterChoices[3] == SDOptions.characterChoices[1] || SDOptions.characterChoices[3] == SDOptions.characterChoices[2])
                        {
                            SDOptions.characterChoices[3]--;
                            if (SDOptions.characterChoices[3] < 0)
                                SDOptions.characterChoices[3] = 7;
                        }

                        if (!ready4)
                            players[3].sprite = unready[SDOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[SDOptions.characterChoices[3]];

                        StartCoroutine(waitBeforeInput());

                        highlights[3].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.placeholder.color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.textComponent.color = playerColors[SDOptions.characterChoices[3]];
                    }
                    if (p[3].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        SDOptions.characterChoices[3]++;
                        if (SDOptions.characterChoices[3] > 7)
                            SDOptions.characterChoices[3] = 0;
                        while (SDOptions.characterChoices[3] == SDOptions.characterChoices[0] || SDOptions.characterChoices[3] == SDOptions.characterChoices[1] || SDOptions.characterChoices[3] == SDOptions.characterChoices[2])
                        {
                            SDOptions.characterChoices[3]++;
                            if (SDOptions.characterChoices[3] > 7)
                                SDOptions.characterChoices[3] = 0;
                        }

                        if (!ready4)
                            players[3].sprite = unready[SDOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[SDOptions.characterChoices[3]];

                        StartCoroutine(waitBeforeInput());

                        highlights[3].GetComponent<Image>().color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.placeholder.color = playerColors[SDOptions.characterChoices[3]];
                        p4Input.textComponent.color = playerColors[SDOptions.characterChoices[3]];
                    }
                    if (p[3].GetButtonDown("Highlight"))
                    {
                        highlights[3].SetActive(true);
                        highlights[3].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }

            if ((Input.GetKey(KeyCode.O) || allControllers.GetButtonDown("Start")) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
            {
                screen4.SetActive(true);
                StartCoroutine(waitBeforeInput());

                if (countdownIsRunning)
                {
                    StopAllCoroutines();
                    ready1 = false;
                    ready2 = false;
                    ready3 = false;
                    ready4 = false;

                    for (int i = 0; i < 4; i++)
                    {
                        players[i].sprite = unready[i];
                        readyText[i].text = "Press UP to ready!";
                    }

                    countdown.gameObject.SetActive(false);

                    StartCoroutine(waitBeforeInput());
                }
            }
            if (allControllers.GetButtonDown("Back") && countdownIsRunning)
            {
                StopAllCoroutines();
                ready1 = false;
                ready2 = false;
                ready3 = false;
                ready4 = false;

                for (int i = 0; i < 4; i++)
                {
                    players[i].sprite = unready[SDOptions.characterChoices[i]];
                    readyText[i].text = "Press UP to ready!";
                }

                countdown.gameObject.SetActive(false);

                StartCoroutine(waitBeforeInput());

                source.PlayOneShot(back);
                countdownIsRunning = false;
            }
            if (allControllers.GetButtonDown("Back") && !countdownIsRunning && acceptInput)
            {
                inGame1 = false;
                inGame2 = false;
                inGame3 = false;
                inGame4 = false;
                ready1 = false;
                ready2 = false;
                ready3 = false;
                ready4 = false;

                for (int i = 0; i < 4; i++)
                {
                    players[i].sprite = unready[SDOptions.characterChoices[i]];
                    players[i].color = Color.black;

                    if (SDOptions.playerIsUsingController[i])
                    {
                        SDOptions.playerIsUsingController[i] = false;
                        p[i].controllers.GetLastActiveController().tag = null;
                        p[i].controllers.RemoveController(p[i].controllers.GetLastActiveController());
                    }
                }

                controls[0].text = "";
                readyText[0].text = "Press W or X / A on a controller to join game";
                controls[1].text = "";
                readyText[1].text = "Press T or X / A on a controller to join game";
                controls[2].text = "";
                readyText[2].text = "Press I or X / A on a controller to join game";
                controls[3].text = "";
                readyText[3].text = "Press ▲ or X / A on a controller to join game";

                foreach (Text t in controls)
                    t.gameObject.SetActive(true);
                foreach (GameObject obj in controllerInputs)
                    obj.SetActive(false);

                playersInGame = 0;
                SDOptions.playerIsInGame[0] = false;
                SDOptions.playerIsInGame[1] = false;
                SDOptions.playerIsInGame[2] = false;
                SDOptions.playerIsInGame[3] = false;

                screen1.SetActive(true);
                screen3.SetActive(false);

                StartCoroutine(waitBeforeInput());

                source.PlayOneShot(back);
            }

            if (playersInGame == 2)
            {
                if (SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[1])
                    if (ready1 && ready2 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
                if (SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[2])
                    if (ready1 && ready3 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
                if (SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[3])
                    if (ready1 && ready4 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());

                if (SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[2])
                    if (ready2 && ready3 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
                if (SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[3])
                    if (ready2 && ready4 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());

                if (SDOptions.playerIsInGame[2] && SDOptions.playerIsInGame[3])
                    if (ready3 && ready4 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
            }
            if (playersInGame == 3)
            {
                if (SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[2])
                    if (ready1 && ready2 && ready3 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
                if (SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[3])
                    if (ready1 && ready2 && ready4 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
                if (SDOptions.playerIsInGame[0] && SDOptions.playerIsInGame[2] && SDOptions.playerIsInGame[3])
                    if (ready1 && ready3 && ready4 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
                if (SDOptions.playerIsInGame[1] && SDOptions.playerIsInGame[2] && SDOptions.playerIsInGame[3])
                    if (ready2 && ready3 && ready4 && !countdownIsRunning)
                        StartCoroutine(StartCountdown());
            }
            if (playersInGame == 4)
            {
                if (ready1 && ready2 && ready3 && ready4 && !countdownIsRunning)
                {
                    StartCoroutine(StartCountdown());
                }
            }


            if (p1Input.isFocused)
            {
                p1Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SDOptions.p1Name = p1Input.text;
            }
            else
                p1Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p2Input.isFocused)
            {
                p2Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SDOptions.p2Name = p2Input.text;
            }
            else
                p2Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p3Input.isFocused)
            {
                p3Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SDOptions.p3Name = p3Input.text;
            }
            else
                p3Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p4Input.isFocused)
            {
                p4Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                SDOptions.p4Name = p4Input.text;
            }
            else
                p4Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        }
        if (screen4.activeSelf == true)
        {
            AINumber.text = SDOptions.AINumber.ToString();
            hitsNumber.text = SDOptions.hitsToFall.ToString();

            if (allControllers.GetAxis("Vertical") < -float.Epsilon && acceptInput)
            {
                selectorNumber++;
                source.PlayOneShot(confirm);

                if (selectorNumber > 2)
                    selectorNumber = 0;

                StartCoroutine(waitBeforeInput());
            }
            if (allControllers.GetAxis("Vertical") > float.Epsilon && acceptInput)
            {
                selectorNumber--;
                source.PlayOneShot(confirm);

                if (selectorNumber < 0)
                    selectorNumber = 2;

                StartCoroutine(waitBeforeInput());
            }

            selector.transform.position = selectorPoints[selectorNumber].position;

            if (selectorNumber == 0)
            {
                if (allControllers.GetAxis("Horizontal") < -float.Epsilon && SDOptions.AINumber > 30 && acceptInput)
                {
                    SDOptions.AINumber -= 10;
                    source.PlayOneShot(confirm);
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetAxis("Horizontal") > float.Epsilon && SDOptions.AINumber < 60 && acceptInput)
                {
                    SDOptions.AINumber += 10;
                    source.PlayOneShot(confirm);
                    StartCoroutine(waitBeforeInput());
                }
            }
            if (selectorNumber == 1)
            {
                if (allControllers.GetAxis("Horizontal") < -float.Epsilon && SDOptions.hitsToFall > 1 && acceptInput)
                {
                    SDOptions.hitsToFall--;
                    source.PlayOneShot(confirm);
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetAxis("Horizontal") > float.Epsilon && SDOptions.hitsToFall < 3 && acceptInput)
                {
                    SDOptions.hitsToFall++;
                    source.PlayOneShot(confirm);
                    StartCoroutine(waitBeforeInput());
                }
            }
            if (selectorNumber == 2)
            {
                if (allControllers.GetAxis("Horizontal") < -float.Epsilon)
                {
                    if (SDOptions.timer == 180 && acceptInput)
                    {
                        SDOptions.timer = 120;
                        timeNumber.text = "2 minutes";
                        StartCoroutine(waitBeforeInput());
                    }
                    if (SDOptions.timer == 120 && acceptInput)
                    {
                        SDOptions.timer = 60;
                        timeNumber.text = "1 minute";
                        StartCoroutine(waitBeforeInput());
                    }
                }
                if (allControllers.GetAxis("Horizontal") > float.Epsilon)
                {
                    if (SDOptions.timer == 60 && acceptInput)
                    {
                        SDOptions.timer = 120;
                        timeNumber.text = "2 minutes";
                        StartCoroutine(waitBeforeInput());
                    }
                    if (SDOptions.timer == 120 && acceptInput)
                    {
                        SDOptions.timer = 180;
                        timeNumber.text = "3 minutes";
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }

            if (allControllers.GetButton("Back"))
            {
                screen4.SetActive(false);
                StartCoroutine(waitBeforeInput());
            }
        }
	}

    private IEnumerator waitBeforeInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.15f);
        acceptInput = true;
    }

    private IEnumerator StartCountdown()
    {
        countdownIsRunning = true;
        countdown.gameObject.SetActive(true);
        countdown.text = "GAME BEGINS IN 3...";
        source.PlayOneShot(confirm);
        yield return new WaitForSeconds(1);
        countdown.text = "GAME BEGINS IN 2...";
        source.PlayOneShot(confirm);
        yield return new WaitForSeconds(1);
        countdown.text = "GAME BEGINS IN 1...";
        source.PlayOneShot(confirm);
        yield return new WaitForSeconds(1);
        countdown.text = "GAME BEGINNING...";
        source.PlayOneShot(go);
        SceneManager.LoadScene("speed dating arena");
    }
}
