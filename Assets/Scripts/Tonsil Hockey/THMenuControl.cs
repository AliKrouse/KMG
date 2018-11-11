using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class THMenuControl : MonoBehaviour
{
    private Player[] p = new Player[4];
    private Player allControllers;

    public GameObject screen1, screen2, screen3, screen4;
    private bool acceptInput = true;

    public Image[] players;
    public Sprite[] unReady;
    public Sprite[] ready;
    public Text[] readyText;
    private bool p1inGame, p2inGame, p3inGame, p4inGame;
    private bool p1r, p2r, b1r, b2r;

    private bool countingDown;
    public Text countdownText;

    public Text pointsText;

    private AudioSource source;
    public AudioClip confirm, go, back;

    public Image[] playerInputs;
    public Sprite keyboard, controller;
    public GameObject selector;
    public Transform[] selectorPoints;
    private int selectorNumber;
    public Text[] controls;
    public GameObject[] controllerInputs;

    public InputField p1Input, p2Input, p3Input, p4Input;

    public GameObject[] highlights;

    private Joystick j;

    private int playersInGame;

    void Start ()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);

        players[1].color = Color.black;
        players[3].color = Color.black;

        source = GetComponent<AudioSource>();

        for (int i = 0; i < 4; i++)
        {
            p[i] = ReInput.players.GetPlayer(i);

            players[i].color = Color.black;
        }
        allControllers = ReInput.players.GetPlayer(4);

        foreach (Joystick j in ReInput.controllers.Joysticks)
            allControllers.controllers.AddController(j, false);

        THOptions.characterChoices[0] = 0;
        THOptions.characterChoices[1] = 1;
        THOptions.characterChoices[2] = 4;
        THOptions.characterChoices[3] = 5;

        foreach (GameObject obj in highlights)
            obj.SetActive(false);

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnected;

        Time.timeScale = 1;

        foreach (Text t in controls)
            t.gameObject.SetActive(true);
        foreach (GameObject obj in controllerInputs)
            obj.gameObject.SetActive(false);
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
                    players[i].sprite = unReady[THOptions.characterChoices[i]];
                    THOptions.playerIsUsingController[i] = false;

                    readyText[i].text = "";

                    if (i == 0)
                    {
                        controls[0].text = "Press W\nor X / A\non a controller\nto join game";
                        p1r = false;
                        p1inGame = false;
                    }
                    if (i == 1)
                    {
                        controls[1].text = "Press T\nor X / A\non a controller\nto join game";
                        p2r = false;
                        p2inGame = false;
                    }
                    if (i == 2)
                    {
                        controls[2].text = "Press I\nor X / A\non a controller\nto join game";
                        b1r = false;
                        p3inGame = false;
                    }
                    if (i == 3)
                    {
                        controls[3].text = "Press ▲\nor X / A\non a controller\nto join game";
                        b2r = false;
                        p4inGame = false;
                    }

                    playersInGame--;
                    THOptions.playerIsInGame[i] = false;
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
                StartCoroutine(freezeInput());

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
                StartCoroutine(freezeInput());

                source.PlayOneShot(confirm);
            }
            if (allControllers.GetButtonDown("Back") && acceptInput)
            {
                screen1.SetActive(true);
                screen2.SetActive(false);
                StartCoroutine(freezeInput());

                source.PlayOneShot(back);
            }
        }

        if (screen3.activeSelf == true && screen4.activeSelf == false)
        {
            if (!p1inGame)
            {
                controls[0].text = "Press W\nor X / A\non a controller\nto join game";
                readyText[0].text = "";
                players[0].color = Color.black;
                if (Input.GetKeyDown(KeyCode.W) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    THOptions.playerIsUsingController[0] = false;
                    p1inGame = true;
                    THOptions.playerIsInGame[0] = true;
                    playersInGame++;
                    StartCoroutine(freezeInput());
                }
                if (allControllers.GetButtonDown("Kiss"))
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[0].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[0].controllers.maps.SetAllMapsEnabled(true);

                        THOptions.controllerChoice[0] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        THOptions.playerIsUsingController[0] = true;

                        p1inGame = true;
                        THOptions.playerIsInGame[0] = true;
                        playersInGame++;
                        StartCoroutine(freezeInput());
                    }
                }
            }
            if (!p3inGame)
            {
                controls[2].text = "Press I\nor X / A\non a controller\nto join game";
                readyText[2].text = "";
                players[2].color = Color.black;
                if (Input.GetKeyDown(KeyCode.I) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    THOptions.playerIsUsingController[2] = false;
                    p3inGame = true;
                    THOptions.playerIsInGame[2] = true;
                    playersInGame++;
                    StartCoroutine(freezeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && p1inGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[2].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[2].controllers.maps.SetAllMapsEnabled(true);

                        THOptions.controllerChoice[2] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        THOptions.playerIsUsingController[2] = true;

                        p3inGame = true;
                        THOptions.playerIsInGame[2] = true;
                        playersInGame++;
                        StartCoroutine(freezeInput());
                    }
                }
            }
            if (!p2inGame)
            {
                controls[1].text = "Press T\nor X / A\non a controller\nto join game";
                readyText[1].text = "";
                players[1].color = Color.black;
                if (Input.GetKeyDown(KeyCode.T) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    THOptions.playerIsUsingController[1] = false;
                    p2inGame = true;
                    THOptions.playerIsInGame[1] = true;
                    playersInGame++;
                    StartCoroutine(freezeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && p3inGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[1].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[1].controllers.maps.SetAllMapsEnabled(true);

                        THOptions.controllerChoice[1] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        THOptions.playerIsUsingController[1] = true;

                        p2inGame = true;
                        THOptions.playerIsInGame[1] = true;
                        playersInGame++;
                        StartCoroutine(freezeInput());
                    }
                }
            }
            if (!p4inGame)
            {
                controls[3].text = "Press ▲\nor X / A\non a controller\nto join game";
                readyText[3].text = "";
                players[3].color = Color.black;
                if (Input.GetKeyDown(KeyCode.UpArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                {
                    THOptions.playerIsUsingController[3] = false;
                    p4inGame = true;
                    THOptions.playerIsInGame[3] = true;
                    playersInGame++;
                    StartCoroutine(freezeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && p2inGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[3].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[3].controllers.maps.SetAllMapsEnabled(true);

                        THOptions.controllerChoice[3] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        THOptions.playerIsUsingController[3] = true;

                        p4inGame = true;
                        THOptions.playerIsInGame[3] = true;
                        playersInGame++;
                        StartCoroutine(freezeInput());
                    }
                }
            }

            if (p1inGame)
            {
                players[0].color = Color.white;
                if (!p1r)
                    readyText[0].text = "PRESS UP TO READY";
                if (!THOptions.playerIsUsingController[0])
                {
                    controls[0].text = "MOVEMENT:\nWASD\n\nMOVE STICK:\nQ/E\n\nKISS:\nQ/E IN GOAL";
                    if (Input.GetKeyDown(KeyCode.W) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !p1r)
                    {
                        p1r = true;
                        players[0].sprite = ready[THOptions.characterChoices[0]];
                        readyText[0].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.A) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[0]--;
                        if (THOptions.characterChoices[0] < 0)
                            THOptions.characterChoices[0] = 3;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[0] == THOptions.characterChoices[1])
                                THOptions.characterChoices[0]--;
                            if (THOptions.characterChoices[0] < 0)
                                THOptions.characterChoices[0] = 3;
                        }

                        if (!p1r)
                            players[0].sprite = unReady[THOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[THOptions.characterChoices[0]];
                    }
                    if (Input.GetKeyDown(KeyCode.D) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[0]++;
                        if (THOptions.characterChoices[0] > 3)
                            THOptions.characterChoices[0] = 0;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[0] == THOptions.characterChoices[1])
                                THOptions.characterChoices[0]++;
                            if (THOptions.characterChoices[0] > 3)
                                THOptions.characterChoices[0] = 0;
                        }

                        if (!p1r)
                            players[0].sprite = unReady[THOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[THOptions.characterChoices[0]];
                    }
                }
                else
                {
                    //controls[0].text = "MOVEMENT:\nLEFT STICK\n\nMOVE STICK:L1/R1\n\nKISS:\nL1/R1 IN GOAL";
                    controls[0].gameObject.SetActive(false);
                    controllerInputs[0].gameObject.SetActive(true);

                    if (p[0].GetAxis("Vertical") > 0.5 && acceptInput && !p1r)
                    {
                        p1r = true;
                        players[0].sprite = ready[THOptions.characterChoices[0]];
                        readyText[0].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[0].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        THOptions.characterChoices[0]--;
                        if (THOptions.characterChoices[0] < 0)
                            THOptions.characterChoices[0] = 3;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[0] == THOptions.characterChoices[1])
                                THOptions.characterChoices[0]--;
                            if (THOptions.characterChoices[0] < 0)
                                THOptions.characterChoices[0] = 3;
                        }

                        if (!p1r)
                            players[0].sprite = unReady[THOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[THOptions.characterChoices[0]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[0].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        THOptions.characterChoices[0]++;
                        if (THOptions.characterChoices[0] > 3)
                            THOptions.characterChoices[0] = 0;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[0] == THOptions.characterChoices[1])
                                THOptions.characterChoices[0]++;
                            if (THOptions.characterChoices[0] > 3)
                                THOptions.characterChoices[0] = 0;
                        }

                        if (!p1r)
                            players[0].sprite = unReady[THOptions.characterChoices[0]];
                        else
                            players[0].sprite = ready[THOptions.characterChoices[0]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[0].GetButtonDown("Highlight"))
                    {
                        highlights[0].SetActive(true);
                        highlights[0].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (p2inGame)
            {
                players[1].color = Color.white;
                if (!p2r)
                    readyText[1].text = "PRESS UP TO READY";
                if (!THOptions.playerIsUsingController[1])
                {
                    controls[1].text = "MOVEMENT:\nTFGH\n\nMOVE STICK:R/Y\n\nKISS:\nR/Y IN GOAL";
                    if (Input.GetKeyDown(KeyCode.T) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !p2r)
                    {
                        p2r = true;
                        players[1].sprite = ready[THOptions.characterChoices[1]];
                        readyText[1].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.F) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[1]--;
                        if (THOptions.characterChoices[1] < 0)
                            THOptions.characterChoices[1] = 3;
                        if (THOptions.characterChoices[1] == THOptions.characterChoices[0])
                            THOptions.characterChoices[1]--;
                        if (THOptions.characterChoices[1] < 0)
                            THOptions.characterChoices[1] = 3;

                        if (!p2r)
                            players[1].sprite = unReady[THOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[THOptions.characterChoices[1]];
                    }
                    if (Input.GetKeyDown(KeyCode.H) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[1]++;
                        if (THOptions.characterChoices[1] > 3)
                            THOptions.characterChoices[1] = 0;
                        if (THOptions.characterChoices[1] == THOptions.characterChoices[0])
                            THOptions.characterChoices[1]++;
                        if (THOptions.characterChoices[1] > 3)
                            THOptions.characterChoices[1] = 0;

                        if (!p2r)
                            players[1].sprite = unReady[THOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[THOptions.characterChoices[1]];
                    }
                }
                else
                {
                    //controls[1].text = "MOVEMENT:\nLEFT STICK\n\nMOVE STICK:L1/R1\n\nKISS:\nL1/R1 IN GOAL";
                    controls[1].gameObject.SetActive(false);
                    controllerInputs[1].SetActive(true);

                    if (p[1].GetAxis("Vertical") > 0.5 && acceptInput && !p2r)
                    {
                        p2r = true;
                        players[1].sprite = ready[THOptions.characterChoices[1]];
                        readyText[1].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[1].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        THOptions.characterChoices[1]--;
                        if (THOptions.characterChoices[1] < 0)
                            THOptions.characterChoices[1] = 3;
                        if (THOptions.characterChoices[1] == THOptions.characterChoices[0])
                            THOptions.characterChoices[1]--;
                        if (THOptions.characterChoices[1] < 0)
                            THOptions.characterChoices[1] = 3;

                        if (!p2r)
                            players[1].sprite = unReady[THOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[THOptions.characterChoices[1]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[1].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        THOptions.characterChoices[1]++;
                        if (THOptions.characterChoices[1] > 3)
                            THOptions.characterChoices[1] = 0;
                        if (THOptions.characterChoices[1] == THOptions.characterChoices[0])
                            THOptions.characterChoices[1]++;
                        if (THOptions.characterChoices[1] > 3)
                            THOptions.characterChoices[1] = 0;

                        if (!p2r)
                            players[1].sprite = unReady[THOptions.characterChoices[1]];
                        else
                            players[1].sprite = ready[THOptions.characterChoices[1]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[1].GetButtonDown("Highlight"))
                    {
                        highlights[1].SetActive(true);
                        highlights[1].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (p3inGame)
            {
                players[2].color = Color.white;
                if (!b1r)
                    readyText[2].text = "PRESS UP TO READY";
                if (!THOptions.playerIsUsingController[2])
                {
                    controls[2].text = "MOVEMENT:\nIJKL\n\nMOVE STICK:U/O\n\nKISS:\nU/O IN GOAL";
                    if (Input.GetKeyDown(KeyCode.I) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !b1r)
                    {
                        b1r = true;
                        players[2].sprite = ready[THOptions.characterChoices[2]];
                        readyText[2].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.J) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[2]--;
                        if (THOptions.characterChoices[2] < 4)
                            THOptions.characterChoices[2] = 7;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[2] == THOptions.characterChoices[3])
                                THOptions.characterChoices[2]--;
                            if (THOptions.characterChoices[2] < 4)
                                THOptions.characterChoices[2] = 7;
                        }

                        if (!b1r)
                            players[2].sprite = unReady[THOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[THOptions.characterChoices[2]];
                    }
                    if (Input.GetKeyDown(KeyCode.L) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[2]++;
                        if (THOptions.characterChoices[2] > 7)
                            THOptions.characterChoices[2] = 4;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[2] == THOptions.characterChoices[3])
                                THOptions.characterChoices[2]++;
                            if (THOptions.characterChoices[2] > 7)
                                THOptions.characterChoices[2] = 4;
                        }

                        if (!b1r)
                            players[2].sprite = unReady[THOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[THOptions.characterChoices[2]];
                    }
                }
                else
                {
                    //controls[2].text = "MOVEMENT:\nLEFT STICK\n\nMOVE STICK:L1/R1\n\nKISS:\nL1/R1 IN GOAL";
                    controls[2].gameObject.SetActive(false);
                    controllerInputs[2].SetActive(true);

                    if (p[2].GetAxis("Vertical") > 0.5 && acceptInput && !b1r)
                    {
                        b1r = true;
                        players[2].sprite = ready[THOptions.characterChoices[2]];
                        readyText[2].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[2].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        THOptions.characterChoices[2]--;
                        if (THOptions.characterChoices[2] < 4)
                            THOptions.characterChoices[2] = 7;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[2] == THOptions.characterChoices[3])
                                THOptions.characterChoices[2]--;
                            if (THOptions.characterChoices[2] < 4)
                                THOptions.characterChoices[2] = 7;
                        }

                        if (!b1r)
                            players[2].sprite = unReady[THOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[THOptions.characterChoices[2]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[2].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        THOptions.characterChoices[2]++;
                        if (THOptions.characterChoices[2] > 7)
                            THOptions.characterChoices[2] = 4;
                        if (THOptions.fourPlayer)
                        {
                            if (THOptions.characterChoices[2] == THOptions.characterChoices[3])
                                THOptions.characterChoices[2]++;
                            if (THOptions.characterChoices[2] > 7)
                                THOptions.characterChoices[2] = 4;
                        }

                        if (!b1r)
                            players[2].sprite = unReady[THOptions.characterChoices[2]];
                        else
                            players[2].sprite = ready[THOptions.characterChoices[2]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[2].GetButtonDown("Highlight"))
                    {
                        highlights[2].SetActive(true);
                        highlights[2].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (p4inGame)
            {
                players[3].color = Color.white;
                if (!b2r)
                    readyText[3].text = "PRESS UP TO READY";
                if (!THOptions.playerIsUsingController[3])
                {
                    controls[3].text = "MOVEMENT:\nARROW KEYS\n\nMOVE STICK:SHIFT/0\n\nKISS:\nSHIFT/0 IN GOAL";
                    if (Input.GetKeyDown(KeyCode.UpArrow) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && acceptInput && !b2r)
                    {
                        b2r = true;
                        players[3].sprite = ready[THOptions.characterChoices[3]];
                        readyText[3].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[3]--;
                        if (THOptions.characterChoices[3] < 4)
                            THOptions.characterChoices[3] = 7;
                        if (THOptions.characterChoices[3] == THOptions.characterChoices[2])
                            THOptions.characterChoices[3]--;
                        if (THOptions.characterChoices[3] < 4)
                            THOptions.characterChoices[3] = 7;

                        if (!b2r)
                            players[3].sprite = unReady[THOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[THOptions.characterChoices[3]];
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
                    {
                        THOptions.characterChoices[3]++;
                        if (THOptions.characterChoices[3] > 7)
                            THOptions.characterChoices[3] = 4;
                        if (THOptions.characterChoices[3] == THOptions.characterChoices[2])
                            THOptions.characterChoices[3]++;
                        if (THOptions.characterChoices[3] > 7)
                            THOptions.characterChoices[3] = 4;

                        if (!b2r)
                            players[3].sprite = unReady[THOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[THOptions.characterChoices[3]];
                    }
                }
                else
                {
                    //controls[3].text = "MOVEMENT:\nLEFT STICK\n\nMOVE STICK:L1/R1\n\nKISS:\nL1/R1 IN GOAL";
                    controls[3].gameObject.SetActive(false);
                    controllerInputs[3].gameObject.SetActive(true);

                    if (p[3].GetAxis("Vertical") > 0.5 && acceptInput && !b2r)
                    {
                        b2r = true;
                        players[3].sprite = ready[THOptions.characterChoices[3]];
                        readyText[3].text = "READY!";

                        source.PlayOneShot(confirm);
                    }
                    if (p[3].GetAxis("Horizontal") < -0.5 && acceptInput)
                    {
                        THOptions.characterChoices[3]--;
                        if (THOptions.characterChoices[3] < 4)
                            THOptions.characterChoices[3] = 7;
                        if (THOptions.characterChoices[3] == THOptions.characterChoices[2])
                            THOptions.characterChoices[3]--;
                        if (THOptions.characterChoices[3] < 4)
                            THOptions.characterChoices[3] = 7;

                        if (!b2r)
                            players[3].sprite = unReady[THOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[THOptions.characterChoices[3]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[3].GetAxis("Horizontal") > 0.5 && acceptInput)
                    {
                        THOptions.characterChoices[3]++;
                        if (THOptions.characterChoices[3] > 7)
                            THOptions.characterChoices[3] = 4;
                        if (THOptions.characterChoices[3] == THOptions.characterChoices[2])
                            THOptions.characterChoices[3]++;
                        if (THOptions.characterChoices[3] > 7)
                            THOptions.characterChoices[3] = 4;

                        if (!b2r)
                            players[3].sprite = unReady[THOptions.characterChoices[3]];
                        else
                            players[3].sprite = ready[THOptions.characterChoices[3]];

                        StartCoroutine(freezeInput());
                    }
                    if (p[3].GetButtonDown("Highlight"))
                    {
                        highlights[3].SetActive(true);
                        highlights[3].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }

            if (playersInGame > 2)
                THOptions.fourPlayer = true;
            else
                THOptions.fourPlayer = false;

            if ((Input.GetKeyDown(KeyCode.O) || allControllers.GetButtonDown("Start")) && acceptInput && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused)
            {
                screen4.SetActive(true);

                source.PlayOneShot(confirm);

                if (countingDown)
                {
                    StopAllCoroutines();
                    StartCoroutine(freezeInput());
                    countingDown = false;

                    p1r = false; p2r = false;
                    b1r = false; b2r = false;
                    players[0].sprite = unReady[0];
                    players[1].sprite = unReady[1];
                    players[2].sprite = unReady[2];
                    players[3].sprite = unReady[3];
                    readyText[0].text = "PRESS UP TO READY";
                    readyText[1].text = "PRESS UP TO READY";
                    readyText[2].text = "PRESS UP TO READY";
                    readyText[3].text = "PRESS UP TO READY";

                    StartCoroutine(freezeInput());
                }
            }
            if (allControllers.GetButtonDown("Back") && acceptInput && !countingDown)
            {
                screen1.SetActive(true);
                screen3.SetActive(false);
                StartCoroutine(freezeInput());

                p1r = false; p2r = false;
                b1r = false; b2r = false;
                p1inGame = false; p2inGame = false;
                p3inGame = false; p4inGame = false;

                for (int i = 0; i < 4; i++)
                {
                    players[i].sprite = unReady[THOptions.characterChoices[i]];
                    players[i].color = Color.black;

                    if (THOptions.playerIsUsingController[i])
                    {
                        THOptions.playerIsUsingController[i] = false;
                        p[i].controllers.GetLastActiveController().tag = null;
                        p[i].controllers.RemoveController(p[i].controllers.GetLastActiveController());
                    }
                }

                controls[0].text = "Press W\nor X / A\non a controller\nto join game";
                readyText[0].text = "";
                controls[2].text = "Press I\nor X / A\non a controller\nto join game";
                readyText[2].text = "";
                controls[1].text = "Press T\nor X / A\non a controller\nto join game";
                readyText[1].text = "";
                controls[3].text = "Press ▲\nor X / A\non a controller\nto join game";
                readyText[3].text = "";

                foreach (Text t in controls)
                    t.gameObject.SetActive(true);
                foreach (GameObject obj in controllerInputs)
                    obj.SetActive(false);

                playersInGame = 0;
                THOptions.playerIsInGame[0] = false;
                THOptions.playerIsInGame[1] = false;
                THOptions.playerIsInGame[2] = false;
                THOptions.playerIsInGame[3] = false;

                source.PlayOneShot(back);
            }
            if (allControllers.GetButtonDown("Back") && countingDown)
            {
                StopAllCoroutines();
                StartCoroutine(freezeInput());
                countingDown = false;
                countdownText.gameObject.SetActive(false);

                p1r = false; p2r = false;
                b1r = false; b2r = false;
                players[0].sprite = unReady[THOptions.characterChoices[0]];
                players[1].sprite = unReady[THOptions.characterChoices[1]];
                players[2].sprite = unReady[THOptions.characterChoices[2]];
                players[3].sprite = unReady[THOptions.characterChoices[3]];

                source.PlayOneShot(back);
            }

            if (THOptions.fourPlayer)
            {
                if (p1r && p2r && b1r && b2r && !countingDown)
                {
                    StartCoroutine(countDown());
                }
            }
            else
            {
                if (THOptions.playerIsInGame[0] && THOptions.playerIsInGame[2])
                    if (p1r && b1r && !countingDown)
                        StartCoroutine(countDown());
                if (THOptions.playerIsInGame[0] && THOptions.playerIsInGame[3])
                    if (p1r && b2r && !countingDown)
                        StartCoroutine(countDown());
                if (THOptions.playerIsInGame[1] && THOptions.playerIsInGame[2])
                    if (p2r && b1r && !countingDown)
                        StartCoroutine(countDown());
                if (THOptions.playerIsInGame[1] && THOptions.playerIsInGame[3])
                    if (p2r && b2r && !countingDown)
                        StartCoroutine(countDown());
            }

            if (p1Input.isFocused)
            {
                p1Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                THOptions.p1Name = p1Input.text;
            }
            else
                p1Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p2Input.isFocused)
            {
                p2Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                THOptions.p2Name = p2Input.text;
            }
            else
                p2Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p3Input.isFocused)
            {
                p3Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                THOptions.p3Name = p3Input.text;
            }
            else
                p3Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p4Input.isFocused)
            {
                p4Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                THOptions.p4Name = p4Input.text;
            }
            else
                p4Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        }

        if (screen4.activeSelf == true)
        {
            //if (allControllers.GetAxis("Vertical") < -float.Epsilon && acceptInput)
            //{
            //    selectorNumber++;
            //    source.PlayOneShot(confirm);

            //    if (selectorNumber > 4)
            //        selectorNumber = 0;

            //    StartCoroutine(freezeInput());
            //}
            //if (allControllers.GetAxis("Vertical") > float.Epsilon && acceptInput)
            //{
            //    selectorNumber--;
            //    source.PlayOneShot(confirm);

            //    if (selectorNumber < 0)
            //        selectorNumber = 4;

            //    StartCoroutine(freezeInput());
            //}
            selector.transform.position = selectorPoints[selectorNumber].position;

            if (selectorNumber == 0)
            {
                if (allControllers.GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                {
                    if (THOptions.pointsToWin > 1)
                    {
                        THOptions.pointsToWin--;
                        pointsText.text = THOptions.pointsToWin.ToString();

                        source.PlayOneShot(confirm);

                        StartCoroutine(freezeInput());
                    }
                }
                if (allControllers.GetAxis("Horizontal") > float.Epsilon && acceptInput)
                {
                    if (THOptions.pointsToWin < 10)
                    {
                        THOptions.pointsToWin++;
                        pointsText.text = THOptions.pointsToWin.ToString();

                        source.PlayOneShot(confirm);

                        StartCoroutine(freezeInput());
                    }
                }
            }

            if (allControllers.GetButtonDown("Back") && acceptInput)
            {
                screen4.SetActive(false);
                StartCoroutine(freezeInput());

                source.PlayOneShot(back);
            }
        }
	}

    private IEnumerator freezeInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.15f);
        acceptInput = true;
    }

    private IEnumerator countDown()
    {
        countingDown = true;
        countdownText.gameObject.SetActive(true);
        countdownText.text = "GAME BEGINS IN 3...";
        source.PlayOneShot(confirm);
        yield return new WaitForSeconds(1);
        countdownText.text = "GAME BEGINS IN 2...";
        source.PlayOneShot(confirm);
        yield return new WaitForSeconds(1);
        countdownText.text = "GAME BEGINS IN 1...";
        source.PlayOneShot(confirm);
        yield return new WaitForSeconds(1);
        countdownText.text = "GAME BEGINNING...";
        source.PlayOneShot(go);
        SceneManager.LoadScene("tonsil hockey arena");
    }
}
