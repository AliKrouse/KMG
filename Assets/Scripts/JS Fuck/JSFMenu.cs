using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class JSFMenu : MonoBehaviour
{
    private Player[] p = new Player[8];
    private Player allControllers;

    public GameObject screen1, screen2, screen3, screen5;

    private bool acceptInput = true;

    private bool inGame1, inGame2, inGame3, inGame4, inGame5, inGame6, inGame7, inGame8;
    private bool[] isReady = new bool[8];
    private bool countingDown;
    private int playerChosing;

    public Text chosingText, countdownText;

    public Image[] players;
    public Sprite[] ready;
    public Sprite[] unReady = new Sprite[8];

    private AudioSource source;
    public AudioClip confirm, back, go, no;

    private KeyCode[] numbers =
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
    };
    
    public Text[] controls;
    public GameObject[] controllerInputs;
    public Image[] kissButton;
    public Sprite[] kissButtonOptions;

    public InputField p1Input, p2Input, p3Input, p4Input, p5Input, p6Input, p7Input, p8Input;

    public GameObject[] highlights;

    private Joystick j;

    private int playersInGame, playersReady;

    public GameObject controlIcons;
    public Text crushKeys;
    public GameObject crushControls;
    public Image kiss;

    public GameObject selector;
    public Image[] selectorPoints;
    private int selectorNumber;
    
	void Start ()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
        //screen4.SetActive(false);
        screen5.SetActive(false);

        playerChosing = 1;

        source = GetComponent<AudioSource>();

        for (int i = 0; i < 8; i++)
        {
            p[i] = ReInput.players.GetPlayer(i);
            unReady[i] = players[i].sprite;
            players[i].color = Color.black;
            JSFOptions.playerIsInGame[i] = false;
        }
        allControllers = ReInput.players.GetPlayer(8);

        foreach (Joystick j in ReInput.controllers.Joysticks)
            allControllers.controllers.AddController(j, false);

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
            for (int i = 0; i < 8; i++)
            {
                if (p[i].controllers.ContainsController(j))
                {
                    p[i].controllers.RemoveController(j);
                    players[i].color = Color.black;
                    players[i].sprite = unReady[i];
                    JSFOptions.playerIsUsingController[i] = false;

                    controls[i].text = "";

                    if (i == 0)
                    {
                        controls[0].text = "Press W on the keyboard\nor X / A on a controller\nto join game";
                        isReady[0] = false;
                        inGame1 = false;
                    }
                    if (i == 1)
                    {
                        controls[1].text = "Press X on the keyboard\nor X / A on a controller\nto join game";
                        isReady[1] = false;
                        inGame2 = false;
                    }
                    if (i == 2)
                    {
                        controls[2].text = "Press T on the keyboard\nor X / A on a controller\nto join game";
                        isReady[2] = false;
                        inGame3 = false;
                    }
                    if (i == 3)
                    {
                        controls[3].text = "Press B on the keyboard\nor X / A on a controller\nto join game";
                        isReady[3] = false;
                        inGame4 = false;
                    }
                    if (i == 4)
                    {
                        controls[4].text = "Press I on the keyboard\nor X / A on a controller\nto join game";
                        isReady[4] = false;
                        inGame5 = false;
                    }
                    if (i == 5)
                    {
                        controls[5].text = "Press , on the keyboard\nor X / A on a controller\nto join game";
                        isReady[5] = false;
                        inGame6 = false;
                    }
                    if (i == 6)
                    {
                        controls[6].text = "Press [ on the keyboard\nor X / A on a controller\nto join game";
                        isReady[6] = false;
                        inGame7 = false;
                    }
                    if (i == 7)
                    {
                        controls[7].text = "Press ▼ on the keyboard\nor X / A on a controller\nto join game";
                        isReady[7] = true;
                        inGame8 = false;
                    }

                    playersInGame--;
                    playersReady--;
                    JSFOptions.playerIsInGame[i] = false;
                }
            }

            if (screen5.activeSelf)
            {
                for (int i = 0; i < 8; i++)
                    isReady[i] = false;
                playersReady = 0;

                for (int i = 0; i < 8; i++)
                {
                    players[i].sprite = unReady[i];
                }

                screen5.SetActive(false);

                source.PlayOneShot(back);
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
                screen2.SetActive(false);
                screen3.SetActive(true);
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
        if (screen3.activeSelf == true && screen5.activeSelf == false)
        {
            if (!inGame1)
            {
                controls[0].text = "Press W on the keyboard\nor X / A on a controller\nto join game";
                players[0].color = Color.black;
                if (Input.GetKeyDown(KeyCode.W) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[0] = false;
                    inGame1 = true;
                    JSFOptions.playerIsInGame[0] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss"))
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[0].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[0].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[0] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[0] = true;
                        JSFOptions.controllerTypes[0] = ReInput.controllers.GetLastActiveController().name;

                        inGame1 = true;
                        JSFOptions.playerIsInGame[0] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame2)
            {
                controls[1].text = "Press X on the keyboard\nor X / A on a controller\nto join game";
                players[1].color = Color.black;
                if (Input.GetKeyDown(KeyCode.X) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[1] = false;
                    inGame2 = true;
                    JSFOptions.playerIsInGame[1] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame1)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[1].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[1].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[1] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[1] = true;
                        JSFOptions.controllerTypes[1] = ReInput.controllers.GetLastActiveController().name;

                        inGame2 = true;
                        JSFOptions.playerIsInGame[1] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame3)
            {
                controls[2].text = "Press T on the keyboard\nor X / A on a controller\nto join game";
                players[2].color = Color.black;
                if (Input.GetKeyDown(KeyCode.T) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[2] = false;
                    inGame3 = true;
                    JSFOptions.playerIsInGame[2] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame2)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[2].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[2].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[2] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[2] = true;
                        JSFOptions.controllerTypes[2] = ReInput.controllers.GetLastActiveController().name;

                        inGame3 = true;
                        JSFOptions.playerIsInGame[2] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame4)
            {
                controls[3].text = "Press B on the keyboard\nor X / A on a controller\nto join game";
                players[3].color = Color.black;
                if (Input.GetKeyDown(KeyCode.B) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[3] = false;
                    inGame4 = true;
                    JSFOptions.playerIsInGame[3] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame3)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[3].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[3].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[3] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[3] = true;
                        JSFOptions.controllerTypes[3] = ReInput.controllers.GetLastActiveController().name;

                        inGame4 = true;
                        JSFOptions.playerIsInGame[3] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame5)
            {
                controls[4].text = "Press I on the keyboard\nor X / A on a controller\nto join game";
                players[4].color = Color.black;
                if (Input.GetKeyDown(KeyCode.I) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[4] = false;
                    inGame5 = true;
                    JSFOptions.playerIsInGame[4] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame4)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[4].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[4].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[4] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[4] = true;
                        JSFOptions.controllerTypes[4] = ReInput.controllers.GetLastActiveController().name;

                        inGame5 = true;
                        JSFOptions.playerIsInGame[4] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame6)
            {
                controls[5].text = "Press , on the keyboard\nor X / A on a controller\nto join game";
                players[5].color = Color.black;
                if (Input.GetKeyDown(KeyCode.Comma) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[5] = false;
                    inGame6 = true;
                    JSFOptions.playerIsInGame[5] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame5)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[5].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[5].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[5] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[5] = true;
                        JSFOptions.controllerTypes[5] = ReInput.controllers.GetLastActiveController().name;

                        inGame6 = true;
                        JSFOptions.playerIsInGame[5] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame7)
            {
                controls[6].text = "Press [ on the keyboard\nor X / A on a controller\nto join game";
                players[6].color = Color.black;
                if (Input.GetKeyDown(KeyCode.LeftBracket) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[6] = false;
                    inGame7 = true;
                    JSFOptions.playerIsInGame[6] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame6)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[6].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[6].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[6] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[6] = true;
                        JSFOptions.controllerTypes[6] = ReInput.controllers.GetLastActiveController().name;

                        inGame7 = true;
                        JSFOptions.playerIsInGame[6] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }
            if (!inGame8)
            {
                controls[7].text = "Press ▼ on the keyboard\nor X / A on a controller\nto join game";
                players[7].color = Color.black;
                if (Input.GetKeyDown(KeyCode.DownArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused)
                {
                    JSFOptions.playerIsUsingController[7] = false;
                    inGame8 = true;
                    JSFOptions.playerIsInGame[7] = true;
                    playersInGame++;
                    StartCoroutine(waitBeforeInput());
                }
                if (allControllers.GetButtonDown("Kiss") && inGame7)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[7].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[7].controllers.maps.SetAllMapsEnabled(true);

                        JSFOptions.controllerChoice[7] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        JSFOptions.playerIsUsingController[7] = true;
                        JSFOptions.controllerTypes[7] = ReInput.controllers.GetLastActiveController().name;

                        inGame8 = true;
                        JSFOptions.playerIsInGame[7] = true;
                        playersInGame++;
                        StartCoroutine(waitBeforeInput());
                    }
                }
            }

            if (inGame1)
            {
                players[0].color = Color.white;
                if (!JSFOptions.playerIsUsingController[0])
                {
                    controls[0].text = "Q - Move Left\nE - Move Right\nW + Q - Kiss Left\nW + E - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.W) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[0])
                    {
                        isReady[0] = true;
                        players[0].sprite = ready[0];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[0].gameObject.SetActive(false);
                    controllerInputs[0].SetActive(true);
                    if (JSFOptions.controllerTypes[0] == "Sony DualShock 4")
                        kissButton[0].sprite = kissButtonOptions[0];
                    else
                        kissButton[0].sprite = kissButtonOptions[1];

                    if (p[0].GetButtonDown("Kiss") && acceptInput && !isReady[0])
                    {
                        isReady[0] = true;
                        players[0].sprite = ready[0];
                        playersReady++;

                        source.PlayOneShot(confirm);
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
                if (!JSFOptions.playerIsUsingController[1])
                {
                    controls[1].text = "Z - Move Left\nC - Move Right\nX + Z - Kiss Left\nX + C - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.X) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[1])
                    {
                        isReady[1] = true;
                        players[1].sprite = ready[1];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[1].gameObject.SetActive(false);
                    controllerInputs[1].SetActive(true);
                    if (JSFOptions.controllerTypes[1] == "Sony DualShock 4")
                        kissButton[1].sprite = kissButtonOptions[0];
                    else
                        kissButton[1].sprite = kissButtonOptions[1];

                    if (p[1].GetButtonDown("Kiss") && acceptInput && !isReady[1])
                    {
                        isReady[1] = true;
                        players[1].sprite = ready[1];
                        playersReady++;

                        source.PlayOneShot(confirm);
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
                if (!JSFOptions.playerIsUsingController[2])
                {
                    controls[2].text = "R - Move Left\nY - Move Right\nT + R - Kiss Left\nT + Y - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.T) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[2])
                    {
                        isReady[2] = true;
                        players[2].sprite = ready[2];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[2].gameObject.SetActive(false);
                    controllerInputs[2].SetActive(true);
                    if (JSFOptions.controllerTypes[2] == "Sony DualShock 4")
                        kissButton[2].sprite = kissButtonOptions[0];
                    else
                        kissButton[2].sprite = kissButtonOptions[1];

                    if (p[2].GetButtonDown("Kiss") && acceptInput && !isReady[2])
                    {
                        isReady[2] = true;
                        players[2].sprite = ready[2];
                        playersReady++;

                        source.PlayOneShot(confirm);
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
                if (!JSFOptions.playerIsUsingController[3])
                {
                    controls[3].text = "V - Move Left\nN - Move Right\nB + V - Kiss Left\nB + N - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.B) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[3])
                    {
                        isReady[3] = true;
                        players[3].sprite = ready[3];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[3].gameObject.SetActive(false);
                    controllerInputs[3].SetActive(true);
                    if (JSFOptions.controllerTypes[3] == "Sony DualShock 4")
                        kissButton[3].sprite = kissButtonOptions[0];
                    else
                        kissButton[3].sprite = kissButtonOptions[1];

                    if (p[3].GetButtonDown("Kiss") && acceptInput && !isReady[3])
                    {
                        isReady[3] = true;
                        players[3].sprite = ready[3];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                    if (p[3].GetButtonDown("Highlight"))
                    {
                        highlights[3].SetActive(true);
                        highlights[3].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame5)
            {
                players[4].color = Color.white;
                if (!JSFOptions.playerIsUsingController[4])
                {
                    controls[4].text = "U - Move Left\nO - Move Right\nI + U - Kiss Left\nI + O - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.I) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[4])
                    {
                        isReady[4] = true;
                        players[4].sprite = ready[4];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[4].gameObject.SetActive(false);
                    controllerInputs[4].SetActive(true);
                    if (JSFOptions.controllerTypes[4] == "Sony DualShock 4")
                        kissButton[4].sprite = kissButtonOptions[0];
                    else
                        kissButton[4].sprite = kissButtonOptions[1];

                    if (p[4].GetButtonDown("Kiss") && acceptInput && !isReady[4])
                    {
                        isReady[4] = true;
                        players[4].sprite = ready[4];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                    if (p[4].GetButtonDown("Highlight"))
                    {
                        highlights[4].SetActive(true);
                        highlights[4].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame6)
            {
                players[5].color = Color.white;
                if (!JSFOptions.playerIsUsingController[5])
                {
                    controls[5].text = "M - Move Left\n. - Move Right\n,+M - Kiss Left\n,+. - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.Comma) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[5])
                    {
                        isReady[5] = true;
                        players[5].sprite = ready[5];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[5].gameObject.SetActive(false);
                    controllerInputs[5].SetActive(true);
                    if (JSFOptions.controllerTypes[5] == "Sony DualShock 4")
                        kissButton[5].sprite = kissButtonOptions[0];
                    else
                        kissButton[5].sprite = kissButtonOptions[1];

                    if (p[5].GetButtonDown("Kiss") && acceptInput && !isReady[5])
                    {
                        isReady[5] = true;
                        players[5].sprite = ready[5];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                    if (p[5].GetButtonDown("Highlight"))
                    {
                        highlights[5].SetActive(true);
                        highlights[5].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame7)
            {
                players[6].color = Color.white;
                if (!JSFOptions.playerIsUsingController[6])
                {
                    controls[6].text = "P - Move Left\n] -Move Right\n[+P - Kiss Left\n[+] - Kiss Right";
                    if (Input.GetKeyDown(KeyCode.LeftBracket) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[6])
                    {
                        isReady[6] = true;
                        players[6].sprite = ready[6];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[6].gameObject.SetActive(false);
                    controllerInputs[6].SetActive(true);
                    if (JSFOptions.controllerTypes[6] == "Sony DualShock 4")
                        kissButton[6].sprite = kissButtonOptions[0];
                    else
                        kissButton[6].sprite = kissButtonOptions[1];

                    if (p[6].GetButtonDown("Kiss") && acceptInput && !isReady[6])
                    {
                        isReady[6] = true;
                        players[6].sprite = ready[6];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                    if (p[6].GetButtonDown("Highlight"))
                    {
                        highlights[6].SetActive(true);
                        highlights[6].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }
            if (inGame8)
            {
                players[7].color = Color.white;
                if (!JSFOptions.playerIsUsingController[7])
                {
                    controls[7].text = "◄ - Move Left\n► -Move Right\n▼+◄ -Kiss Left\n▼+► -Kiss Right";
                    if (Input.GetKeyDown(KeyCode.DownArrow) && !p1Input.isFocused && !p2Input.isFocused && !p3Input.isFocused && !p4Input.isFocused && !p5Input.isFocused && !p6Input.isFocused && !p7Input.isFocused && !p8Input.isFocused && acceptInput && !isReady[7])
                    {
                        isReady[7] = true;
                        players[7].sprite = ready[7];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    controls[7].gameObject.SetActive(false);
                    controllerInputs[7].SetActive(true);
                    if (JSFOptions.controllerTypes[7] == "Sony DualShock 4")
                        kissButton[7].sprite = kissButtonOptions[0];
                    else
                        kissButton[7].sprite = kissButtonOptions[1];

                    if (p[7].GetButtonDown("Kiss") && acceptInput && !isReady[7])
                    {
                        isReady[7] = true;
                        players[7].sprite = ready[7];
                        playersReady++;

                        source.PlayOneShot(confirm);
                    }
                    if (p[7].GetButtonDown("Highlight"))
                    {
                        highlights[7].SetActive(true);
                        highlights[7].GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }

            if (allControllers.GetButtonDown("Back") && acceptInput && !countingDown)
            {
                for (int i = 0; i < 8; i++)
                    isReady[i] = false;
                inGame1 = false; inGame2 = false;
                inGame3 = false; inGame4 = false;
                inGame5 = false; inGame6 = false;
                inGame7 = false; inGame8 = false;

                for (int i = 0; i < 8; i++)
                {
                    players[i].sprite = unReady[i];
                    players[i].color = Color.black;
                    JSFOptions.playerIsInGame[i] = false;

                    if (JSFOptions.playerIsUsingController[i])
                    {
                        JSFOptions.playerIsUsingController[i] = false;
                        p[i].controllers.GetLastActiveController().tag = null;
                        p[i].controllers.RemoveController(p[i].controllers.GetLastActiveController());
                    }
                }

                controls[0].text = "Press W on the keyboard\nor X / A on a controller\nto join game";
                controls[1].text = "Press X on the keyboard\nor X / A on a controller\nto join game";
                controls[2].text = "Press T on the keyboard\nor X / A on a controller\nto join game";
                controls[3].text = "Press B on the keyboard\nor X / A on a controller\nto join game";
                controls[4].text = "Press I on the keyboard\nor X / A on a controller\nto join game";
                controls[5].text = "Press , on the keyboard\nor X / A on a controller\nto join game";
                controls[6].text = "Press [ on the keyboard\nor X / A on a controller\nto join game";
                controls[7].text = "Press ▼ on the keyboard\nor X / A on a controller\nto join game";

                foreach (Text t in controls)
                    t.gameObject.SetActive(true);
                foreach (GameObject obj in controllerInputs)
                    obj.SetActive(false);

                playersInGame = 0;
                playersReady = 0;

                screen1.SetActive(true);
                screen3.SetActive(false);
                StartCoroutine(waitBeforeInput());

                source.PlayOneShot(back);
            }
            if (allControllers.GetButtonDown("Back") && acceptInput && countingDown)
            {
                for (int i = 0; i < 8; i++)
                    isReady[i] = false;

                for (int i = 0; i < 8; i++)
                {
                    players[i].sprite = unReady[i];
                }

                countdownText.text = "PRESS KISS TO READY!";

                StopAllCoroutines();
                countingDown = false;
                StartCoroutine(waitBeforeInput());

                source.PlayOneShot(back);
            }

            if (playersInGame >= 4 && playersReady == playersInGame && !countingDown)
            {
                screen5.SetActive(true);
                StartCoroutine(waitBeforeInput());
            }

            if (p1Input.isFocused)
            {
                p1Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p1Name = p1Input.text;
            }
            else
                p1Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p2Input.isFocused)
            {
                p2Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p2Name = p2Input.text;
            }
            else
                p2Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p3Input.isFocused)
            {
                p3Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p3Name = p3Input.text;
            }
            else
                p3Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p4Input.isFocused)
            {
                p4Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p4Name = p4Input.text;
            }
            else
                p4Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p5Input.isFocused)
            {
                p5Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p5Name = p5Input.text;
            }
            else
                p5Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p6Input.isFocused)
            {
                p6Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p6Name = p6Input.text;
            }
            else
                p6Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p7Input.isFocused)
            {
                p7Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p7Name = p7Input.text;
            }
            else
                p7Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (p8Input.isFocused)
            {
                p8Input.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                JSFOptions.p8Name = p8Input.text;
            }
            else
                p8Input.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        }
        
        if (screen5.activeSelf == true)
        {
            for (int i = 0; i < 8; i++)
            {
                if (!JSFOptions.playerIsInGame[i])
                    selectorPoints[i].color = Color.black;
                else
                    selectorPoints[i].color = Color.white;
            }

            if (playerChosing == 1)
            {
                controlIcons.SetActive(true);

                if (!JSFOptions.playerIsInGame[0])
                    playerChosing++;

                chosingText.text = JSFOptions.p1Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[0])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[0] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[0].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[0].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "Q  W  E";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.Q) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.E) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[0].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.W) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[0] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 2)
            {
                if (!JSFOptions.playerIsInGame[1])
                    playerChosing++;

                chosingText.text = JSFOptions.p2Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[1])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[1] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[1].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[1].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "Z  X  C";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.Z) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.C) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[1].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.X) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[1] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 3)
            {
                if (!JSFOptions.playerIsInGame[2])
                    playerChosing++;

                chosingText.text = JSFOptions.p3Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[2])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[2] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[2].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[2].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "R  T  Y";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.R) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.Y) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[2].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.T) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[2] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 4)
            {
                if (!JSFOptions.playerIsInGame[3])
                    playerChosing++;

                chosingText.text = JSFOptions.p4Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[3])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[3] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[3].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[3].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "V  B  N";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.V) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.N) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[3].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.B) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[3] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 5)
            {
                if (!JSFOptions.playerIsInGame[4])
                    playerChosing++;

                chosingText.text = JSFOptions.p5Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[4])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[4] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[4].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[4].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "U  I  O";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.U) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.O) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[4].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.I) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[4] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 6)
            {
                if (!JSFOptions.playerIsInGame[5])
                    playerChosing++;

                chosingText.text = JSFOptions.p6Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[5])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[5] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[5].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[5].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "M  ,  .";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.M) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.Period) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[5].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.Comma) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[5] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 7)
            {
                if (!JSFOptions.playerIsInGame[6])
                    playerChosing++;

                chosingText.text = JSFOptions.p7Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[6])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[6] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[6].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[6].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "P  [  ]";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.P) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.RightBracket) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[6].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.LeftBracket) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[6] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 8)
            {
                if (!JSFOptions.playerIsInGame[7])
                    playerChosing++;

                chosingText.text = JSFOptions.p8Name + "\n\nChoose your crush";

                if (JSFOptions.playerIsUsingController[7])
                {
                    crushKeys.gameObject.SetActive(false);
                    crushControls.SetActive(true);

                    if (JSFOptions.controllerTypes[7] == "Sony DualShock 4")
                        kiss.sprite = kissButtonOptions[0];
                    else
                        kiss.sprite = kissButtonOptions[1];

                    if (p[7].GetAxis("Horizontal") < -float.Epsilon && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (p[7].GetAxis("Horizontal") > float.Epsilon && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }
                else
                {
                    crushKeys.gameObject.SetActive(true);
                    crushKeys.text = "◄  ▼  ►";
                    crushControls.SetActive(false);

                    if (Input.GetKeyDown(KeyCode.LeftArrow) && acceptInput)
                    {
                        selectorNumber--;
                        if (selectorNumber < 0)
                            selectorNumber = 7;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow) && acceptInput)
                    {
                        selectorNumber++;
                        if (selectorNumber > 7)
                            selectorNumber = 0;
                        StartCoroutine(waitBeforeInput());

                        source.PlayOneShot(confirm);
                    }
                }

                selector.transform.position = selectorPoints[selectorNumber].gameObject.transform.position;

                for (int i = 0; i < 8; i++)
                {
                    if (p[7].GetButtonDown("Kiss") && acceptInput || Input.GetKeyDown(KeyCode.DownArrow) && acceptInput)
                    {
                        if (JSFOptions.playerIsInGame[selectorNumber])
                        {
                            JSFVictoryTracker.targetChoices[7] = selectorNumber;
                            playerChosing++;
                            StartCoroutine(waitBeforeInput());

                            source.PlayOneShot(confirm);

                            StartCoroutine(waitBeforeInput());

                            selectorNumber = 0;
                        }
                        else
                            source.PlayOneShot(no);
                    }
                }
            }
            if (playerChosing == 9)
            {
                chosingText.text = "All crushes chosen!\n\nPress SPACE or START to begin game";

                controlIcons.SetActive(false);
                crushKeys.gameObject.SetActive(false);
                crushControls.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Space) || allControllers.GetButtonDown("Start"))
                {
                    screen5.SetActive(false);
                    StartCoroutine(countDown());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape) || allControllers.GetButtonDown("Back"))
            {
                for (int i = 0; i < 8; i++)
                    isReady[i] = false;
                playersReady = 0;
                playerChosing = 1;

                for (int i = 0; i < 8; i++)
                {
                    players[i].sprite = unReady[i];
                }

                screen5.SetActive(false);

                source.PlayOneShot(back);
            }

            if (JSFOptions.playerIsUsingController[0])
            {
                if (p[0].GetButtonDown("Highlight"))
                {
                    highlights[8].SetActive(true);
                    highlights[8].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[1])
            {
                if (p[1].GetButtonDown("Highlight"))
                {
                    highlights[9].SetActive(true);
                    highlights[9].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[2])
            {
                if (p[2].GetButtonDown("Highlight"))
                {
                    highlights[10].SetActive(true);
                    highlights[10].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[3])
            {
                if (p[3].GetButtonDown("Highlight"))
                {
                    highlights[11].SetActive(true);
                    highlights[11].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[4])
            {
                if (p[4].GetButtonDown("Highlight"))
                {
                    highlights[12].SetActive(true);
                    highlights[12].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[5])
            {
                if (p[5].GetButtonDown("Highlight"))
                {
                    highlights[13].SetActive(true);
                    highlights[13].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[6])
            {
                if (p[6].GetButtonDown("Highlight"))
                {
                    highlights[14].SetActive(true);
                    highlights[14].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
            if (JSFOptions.playerIsUsingController[7])
            {
                if (p[7].GetButtonDown("Highlight"))
                {
                    highlights[15].SetActive(true);
                    highlights[15].GetComponent<playerHighlight>().ResetHighlight();
                }
            }
        }
	}

    private IEnumerator waitBeforeInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.15f);
        acceptInput = true;
    }

    private IEnumerator countDown()
    {
        countingDown = true;
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
        SceneManager.LoadScene("js fuck arena");
    }
}
