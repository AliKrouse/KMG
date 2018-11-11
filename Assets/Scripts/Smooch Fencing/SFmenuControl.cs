using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;

public class SFmenuControl : MonoBehaviour
{
    private Player[] p = new Player[2];
    private Player allControllers;

    public GameObject screen1, screen2, screen3, screen4;
    public Text readyText;
    public Text pointsText, modeText, descripText;
    public GameObject selector;
    private GameObject leftArrow, rightArrow;
    public Transform[] selectorPoints;
    public float[] arrowPoints;
    public string[] modes, descriptions;
    private int i;

    public Image pImage, bImage;
    public Sprite[] ready;
    public Sprite[] unReady;

    private bool pinkInGame, blueInGame;
    private bool pinkReady, blueReady, countdownIsRunning, waitBeforeInput;
    private int optSelection;

    private AudioSource source;
    public AudioClip confirm, back, go;
    
    public Text[] controls;
    public GameObject[] controllerInputs;
    public Image[] jumpButton, kissButton;
    public Sprite[] jumpButtonOptions, kissButtonOptions;

    public InputField pinkInput, blueInput;

    public GameObject pinkHighlight, blueHighlight;
    public Color[] playerColors;

    private Joystick j;

    void Start ()
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
        screen4.SetActive(false);

        source = GetComponent<AudioSource>();

        for (int i = 0; i < 2; i++)
        {
            p[i] = ReInput.players.GetPlayer(i);
        }
        allControllers = ReInput.players.GetPlayer(2);

        foreach (Joystick j in ReInput.controllers.Joysticks)
            allControllers.controllers.AddController(j, false);

        leftArrow = selector.transform.GetChild(0).gameObject;
        rightArrow = selector.transform.GetChild(1).gameObject;

        gameOptions.characterChoices[0] = 0;
        gameOptions.characterChoices[1] = 2;

        pImage.color = Color.black;
        bImage.color = Color.black;

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
            for (int i = 0; i < 2; i++)
            {
                if (p[i].controllers.ContainsController(j))
                {
                    p[i].controllers.RemoveController(j);
                    gameOptions.playerIsUsingController[i] = false;

                    if (i == 0)
                    {
                        controls[0].text = "Press F\nor X / A on a controller to join game";
                        pImage.sprite = unReady[gameOptions.characterChoices[0]];
                        pImage.color = Color.black;
                        pinkReady = false;
                        pinkInGame = false;
                    }
                    if (i == 1)
                    {
                        controls[1].text = "Press SHIFT\nor X / A on a controller to join game";
                        bImage.sprite = unReady[gameOptions.characterChoices[1]];
                        bImage.color = Color.black;
                        blueReady = false;
                        blueInGame = false;
                    }
                }
            }
        }
    }
	
	void Update ()
    {
        if (screen1.activeSelf == true)
        {
            if (allControllers.GetButtonDown("Start") && !waitBeforeInput)
            {
                source.PlayOneShot(confirm);

                screen2.SetActive(true);
                screen1.SetActive(false);
                StartCoroutine(inputDelay());
            }

            if (allControllers.GetButtonDown("Back") && !waitBeforeInput)
            {
                SceneManager.LoadScene("kmg menu");
            }
        }

        if (screen2.activeSelf == true)
        {
            if (allControllers.GetButtonDown("Start") && !waitBeforeInput)
            {
                source.PlayOneShot(confirm);

                screen3.SetActive(true);
                screen2.SetActive(false);
                StartCoroutine(inputDelay());
            }

            if (allControllers.GetButtonDown("Back") && !waitBeforeInput)
            {
                source.PlayOneShot(back);

                screen1.SetActive(true);
                screen2.SetActive(false);
                StartCoroutine(inputDelay());
            }
        }

        if (screen3.activeSelf == true && screen4.activeSelf == false)
        {
            if (!pinkInGame)
            {
                controls[0].text = "Press F\nor X / A on a controller to join game";
                pImage.color = Color.black;
                if (Input.GetKeyDown(KeyCode.F) && !pinkInput.isFocused && !blueInput.isFocused)
                {
                    gameOptions.playerIsUsingController[0] = false;
                    pinkInGame = true;
                    StartCoroutine(inputDelay());
                }
                if (allControllers.GetButtonDown("Kiss"))
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[0].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[0].controllers.maps.SetAllMapsEnabled(true);

                        gameOptions.controllerChoice[0] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        gameOptions.playerIsUsingController[0] = true;
                        gameOptions.controllerTypes[0] = ReInput.controllers.GetLastActiveController().name;

                        pinkInGame = true;
                        StartCoroutine(inputDelay());
                    }
                }
            }
            if (!blueInGame)
            {
                controls[1].text = "Press SHIFT\nor X / A on a controller to join game";
                bImage.color = Color.black;
                if (Input.GetKeyDown(KeyCode.RightShift) && !pinkInput.isFocused && !blueInput.isFocused)
                {
                    gameOptions.playerIsUsingController[1] = false;
                    blueInGame = true;
                    StartCoroutine(inputDelay());
                }
                if (allControllers.GetButtonDown("Kiss") && pinkInGame)
                {
                    if (ReInput.controllers.GetLastActiveController().tag != "assigned")
                    {
                        p[1].controllers.AddController(allControllers.controllers.GetLastActiveController(), true);
                        allControllers.controllers.AddController(ReInput.controllers.GetLastActiveController(), false);
                        p[1].controllers.maps.SetAllMapsEnabled(true);

                        gameOptions.controllerChoice[1] = ReInput.controllers.GetLastActiveController().id;
                        ReInput.controllers.GetLastActiveController().tag = "assigned";
                        gameOptions.playerIsUsingController[1] = true;
                        gameOptions.controllerTypes[1] = ReInput.controllers.GetLastActiveController().name;

                        blueInGame = true;
                        StartCoroutine(inputDelay());
                    }
                }
            }

            if (pinkInGame)
            {
                pImage.color = Color.white;
                if (!gameOptions.playerIsUsingController[0])
                {
                    controls[0].text = "A/D - Move\n\nW - Jump\n\nF - Kiss";
                    if (Input.GetKeyDown(KeyCode.F) && !pinkInput.isFocused && !blueInput.isFocused && !waitBeforeInput && !pinkReady)
                    {
                        source.PlayOneShot(confirm);
                        pImage.sprite = ready[gameOptions.characterChoices[0]];
                        pinkReady = true;
                    }
                    if (Input.GetKeyDown(KeyCode.A) && !pinkInput.isFocused && !blueInput.isFocused)
                    {
                        gameOptions.characterChoices[0]--;
                        if (gameOptions.characterChoices[0] < 0)
                            gameOptions.characterChoices[0] = 7;
                        if (gameOptions.characterChoices[0] == gameOptions.characterChoices[1])
                            gameOptions.characterChoices[0]--;
                        if (gameOptions.characterChoices[0] < 0)
                            gameOptions.characterChoices[0] = 7;

                        if (!pinkReady)
                            pImage.sprite = unReady[gameOptions.characterChoices[0]];
                        else
                            pImage.sprite = ready[gameOptions.characterChoices[0]];

                        pinkHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.placeholder.color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.textComponent.color = playerColors[gameOptions.characterChoices[0]];
                    }
                    if (Input.GetKeyDown(KeyCode.D) && !pinkInput.isFocused && !blueInput.isFocused)
                    {
                        gameOptions.characterChoices[0]++;
                        if (gameOptions.characterChoices[0] > 7)
                            gameOptions.characterChoices[0] = 0;
                        if (gameOptions.characterChoices[0] == gameOptions.characterChoices[1])
                            gameOptions.characterChoices[0]++;
                        if (gameOptions.characterChoices[0] > 7)
                            gameOptions.characterChoices[0] = 0;

                        if (!pinkReady)
                            pImage.sprite = unReady[gameOptions.characterChoices[0]];
                        else
                            pImage.sprite = ready[gameOptions.characterChoices[0]];

                        pinkHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.placeholder.color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.textComponent.color = playerColors[gameOptions.characterChoices[0]];
                    }
                }
                else
                {
                    controls[0].gameObject.SetActive(false);
                    controllerInputs[0].SetActive(true);
                    if (gameOptions.controllerTypes[0] == "Sony DualShock 4")
                    {
                        jumpButton[0].sprite = jumpButtonOptions[0];
                        kissButton[0].sprite = kissButtonOptions[0];
                    }
                    else
                    {
                        jumpButton[0].sprite = jumpButtonOptions[1];
                        kissButton[0].sprite = kissButtonOptions[1];
                    }

                    if (p[0].GetButtonDown("Kiss") && !pinkReady)
                    {
                        source.PlayOneShot(confirm);
                        pImage.sprite = ready[gameOptions.characterChoices[0]];
                        pinkReady = true;
                    }
                    if (p[0].GetAxis("Horizontal") < -0.5 && !waitBeforeInput)
                    {
                        gameOptions.characterChoices[0]--;
                        if (gameOptions.characterChoices[0] < 0)
                            gameOptions.characterChoices[0] = 7;
                        if (gameOptions.characterChoices[0] == gameOptions.characterChoices[1])
                            gameOptions.characterChoices[0]--;
                        if (gameOptions.characterChoices[0] < 0)
                            gameOptions.characterChoices[0] = 7;

                        if (!pinkReady)
                            pImage.sprite = unReady[gameOptions.characterChoices[0]];
                        else
                            pImage.sprite = ready[gameOptions.characterChoices[0]];

                        StartCoroutine(inputDelay());

                        pinkHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.placeholder.color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.textComponent.color = playerColors[gameOptions.characterChoices[0]];
                    }
                    if (p[0].GetAxis("Horizontal") > 0.5 && !waitBeforeInput)
                    {
                        gameOptions.characterChoices[0]++;
                        if (gameOptions.characterChoices[0] > 7)
                            gameOptions.characterChoices[0] = 0;
                        if (gameOptions.characterChoices[0] == gameOptions.characterChoices[1])
                            gameOptions.characterChoices[0]++;
                        if (gameOptions.characterChoices[0] > 7)
                            gameOptions.characterChoices[0] = 0;

                        if (!pinkReady)
                            pImage.sprite = unReady[gameOptions.characterChoices[0]];
                        else
                            pImage.sprite = ready[gameOptions.characterChoices[0]];

                        StartCoroutine(inputDelay());

                        pinkHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.placeholder.color = playerColors[gameOptions.characterChoices[0]];
                        pinkInput.textComponent.color = playerColors[gameOptions.characterChoices[0]];
                    }
                    if (p[0].GetButtonDown("Highlight"))
                    {
                        pinkHighlight.SetActive(true);
                        pinkHighlight.GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }

            if (blueInGame)
            {
                bImage.color = Color.white;
                if (!gameOptions.playerIsUsingController[1])
                {
                    controls[1].text = "◄/► - Move\n\n▲ -Jump\n\nSHFIT - Kiss";
                    if (Input.GetKeyDown(KeyCode.RightShift) && !pinkInput.isFocused && !blueInput.isFocused && !waitBeforeInput)
                    {
                        source.PlayOneShot(confirm);
                        bImage.sprite = ready[gameOptions.characterChoices[1]];
                        blueReady = true;
                    }
                    if (Input.GetKeyDown(KeyCode.LeftArrow) && !pinkInput.isFocused && !blueInput.isFocused)
                    {
                        gameOptions.characterChoices[1]--;
                        if (gameOptions.characterChoices[1] < 0)
                            gameOptions.characterChoices[1] = 7;
                        if (gameOptions.characterChoices[1] == gameOptions.characterChoices[0])
                            gameOptions.characterChoices[1]--;
                        if (gameOptions.characterChoices[1] < 0)
                            gameOptions.characterChoices[1] = 7;

                        if (!blueReady)
                            bImage.sprite = unReady[gameOptions.characterChoices[1]];
                        else
                            bImage.sprite = ready[gameOptions.characterChoices[1]];

                        blueHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.placeholder.color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.textComponent.color = playerColors[gameOptions.characterChoices[1]];
                    }
                    if (Input.GetKeyDown(KeyCode.RightArrow) && !pinkInput.isFocused && !blueInput.isFocused)
                    {
                        gameOptions.characterChoices[1]++;
                        if (gameOptions.characterChoices[1] > 7)
                            gameOptions.characterChoices[1] = 0;
                        if (gameOptions.characterChoices[1] == gameOptions.characterChoices[0])
                            gameOptions.characterChoices[1]++;
                        if (gameOptions.characterChoices[1] > 7)
                            gameOptions.characterChoices[1] = 0;

                        if (!blueReady)
                            bImage.sprite = unReady[gameOptions.characterChoices[1]];
                        else
                            bImage.sprite = ready[gameOptions.characterChoices[1]];

                        blueHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.placeholder.color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.textComponent.color = playerColors[gameOptions.characterChoices[1]];
                    }
                }
                else
                {
                    controls[1].gameObject.SetActive(false);
                    controllerInputs[1].SetActive(true);
                    if (gameOptions.controllerTypes[1] == "Sony DualShock 4")
                    {
                        jumpButton[1].sprite = jumpButtonOptions[0];
                        kissButton[1].sprite = kissButtonOptions[0];
                    }
                    else
                    {
                        jumpButton[1].sprite = jumpButtonOptions[1];
                        kissButton[1].sprite = kissButtonOptions[1];
                    }

                    if (p[1].GetButtonDown("Kiss"))
                    {
                        source.PlayOneShot(confirm);
                        bImage.sprite = ready[gameOptions.characterChoices[1]];
                        blueReady = true;
                    }
                    if (p[1].GetAxis("Horizontal") < -0.5 && !waitBeforeInput)
                    {
                        gameOptions.characterChoices[1]--;
                        if (gameOptions.characterChoices[1] < 0)
                            gameOptions.characterChoices[1] = 7;
                        if (gameOptions.characterChoices[1] == gameOptions.characterChoices[0])
                            gameOptions.characterChoices[1]--;
                        if (gameOptions.characterChoices[1] < 0)
                            gameOptions.characterChoices[1] = 7;

                        if (!blueReady)
                            bImage.sprite = unReady[gameOptions.characterChoices[1]];
                        else
                            bImage.sprite = ready[gameOptions.characterChoices[1]];

                        StartCoroutine(inputDelay());

                        blueHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.placeholder.color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.textComponent.color = playerColors[gameOptions.characterChoices[1]];
                    }
                    if (p[1].GetAxis("Horizontal") > 0.5 && !waitBeforeInput)
                    {
                        gameOptions.characterChoices[1]++;
                        if (gameOptions.characterChoices[1] > 7)
                            gameOptions.characterChoices[1] = 0;
                        if (gameOptions.characterChoices[1] == gameOptions.characterChoices[0])
                            gameOptions.characterChoices[1]++;
                        if (gameOptions.characterChoices[1] > 7)
                            gameOptions.characterChoices[1] = 0;

                        if (!blueReady)
                            bImage.sprite = unReady[gameOptions.characterChoices[1]];
                        else
                            bImage.sprite = ready[gameOptions.characterChoices[1]];

                        StartCoroutine(inputDelay());

                        blueHighlight.GetComponent<Image>().color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.placeholder.color = playerColors[gameOptions.characterChoices[1]];
                        blueInput.textComponent.color = playerColors[gameOptions.characterChoices[1]];
                    }
                    if (p[1].GetButtonDown("Highlight"))
                    {
                        blueHighlight.SetActive(true);
                        blueHighlight.GetComponent<playerHighlight>().ResetHighlight();
                    }
                }
            }

            if (pinkReady && blueReady && !countdownIsRunning)
            {
                StartCoroutine(Countdown());
            }

            if (allControllers.GetButtonDown("Back") && !countdownIsRunning && !waitBeforeInput)
            {
                source.PlayOneShot(back);

                pImage.sprite = unReady[gameOptions.characterChoices[0]];
                pImage.color = Color.black;
                bImage.sprite = unReady[gameOptions.characterChoices[1]];
                bImage.color = Color.black;

                pinkReady = false;
                blueReady = false;
                pinkInGame = false;
                blueInGame = false;

                controls[0].text = "Press F\nor X / A on a controller to join game";
                controls[1].text = "Press SHIFT\nor X / A on a controller to join game";

                for (int i = 0; i < 2; i++)
                {
                    if (gameOptions.playerIsUsingController[i])
                    {
                        gameOptions.playerIsUsingController[i] = false;
                        p[i].controllers.GetLastActiveController().tag = null;
                        p[i].controllers.RemoveController(p[i].controllers.GetLastActiveController());
                    }
                }

                foreach (Text t in controls)
                    t.gameObject.SetActive(true);
                foreach (GameObject obj in controllerInputs)
                    obj.SetActive(false);

                screen1.SetActive(true);
                screen3.SetActive(false);
                StartCoroutine(inputDelay());
            }

            if (allControllers.GetButtonDown("Back") && countdownIsRunning)
            {
                source.PlayOneShot(back);

                StopAllCoroutines();
                readyText.gameObject.SetActive(false);

                pImage.sprite = unReady[gameOptions.characterChoices[0]];
                bImage.sprite = unReady[gameOptions.characterChoices[1]];

                pinkReady = false;
                blueReady = false;
                countdownIsRunning = false;
                StartCoroutine(inputDelay());
            }

            if ((Input.GetKeyDown(KeyCode.O) || allControllers.GetButtonDown("Start")) && !waitBeforeInput && !pinkInput.isFocused && !blueInput.isFocused)
            {
                screen4.SetActive(true);

                source.PlayOneShot(confirm);
            }
            
            if (pinkInput.isFocused)
            {
                pinkInput.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                gameOptions.pinkName = pinkInput.text;
            }
            else
                pinkInput.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
            if (blueInput.isFocused)
            {
                blueInput.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                gameOptions.blueName = blueInput.text;
            }
            else
                blueInput.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        }

        if (screen4.activeSelf == true)
        {
            if (allControllers.GetAxis("Vertical") < -float.Epsilon && !waitBeforeInput)
            {
                source.PlayOneShot(confirm);

                optSelection ++;
                if (optSelection > 1)
                    optSelection = 0;

                StartCoroutine(inputDelay());
            }
            if (allControllers.GetAxis("Vertical") > float.Epsilon && !waitBeforeInput)
            {
                source.PlayOneShot(confirm);

                optSelection --;
                if (optSelection < 0)
                    optSelection = 1;

                StartCoroutine(inputDelay());
            }

            if (optSelection == 0)
            {
                if (allControllers.GetAxis("Horizontal") < -float.Epsilon && !waitBeforeInput)
                {
                    source.PlayOneShot(confirm);

                    if (gameOptions.ptw > 1)
                        gameOptions.ptw--;

                    StartCoroutine(inputDelay());
                }
                if (allControllers.GetAxis("Horizontal") > float.Epsilon && !waitBeforeInput)
                {
                    source.PlayOneShot(confirm);

                    if (gameOptions.ptw < 5)
                        gameOptions.ptw++;

                    StartCoroutine(inputDelay());
                }

                pointsText.text = gameOptions.ptw.ToString();
            }
            if (optSelection == 1)
            {
                if (allControllers.GetAxis("Horizontal") < -float.Epsilon && !waitBeforeInput)
                {
                    source.PlayOneShot(confirm);
                    i--;
                    StartCoroutine(inputDelay());
                }
                if (allControllers.GetAxis("Horizontal") > float.Epsilon && !waitBeforeInput)
                {
                    source.PlayOneShot(confirm);
                    i++;
                    StartCoroutine(inputDelay());
                }
                if (i < 0)
                    i = modes.Length - 1;
                if (i == modes.Length)
                    i = 0;

                gameOptions.mode = modes[i];
                modeText.text = modes[i];
                descripText.text = descriptions[i];
            }

            selector.transform.position = selectorPoints[optSelection].position;
            leftArrow.transform.localPosition = new Vector2(-arrowPoints[optSelection], 0);
            rightArrow.transform.localPosition = new Vector2(arrowPoints[optSelection], 0);

            if (allControllers.GetButtonDown("Back"))
            {
                source.PlayOneShot(back);

                screen4.SetActive(false);
                StartCoroutine(inputDelay());
            }
        }
	}

    private IEnumerator Countdown()
    {
        countdownIsRunning = true;
        readyText.gameObject.SetActive(true);
        source.PlayOneShot(confirm);
        readyText.text = "GAME STARTS IN 3...";
        yield return new WaitForSeconds(1);
        source.PlayOneShot(confirm);
        readyText.text = "GAME STARTS IN 2...";
        yield return new WaitForSeconds(1);
        source.PlayOneShot(confirm);
        readyText.text = "GAME STARTS IN 1...";
        yield return new WaitForSeconds(1);
        source.PlayOneShot(go);
        readyText.text = "GAME STARTING...";
        SceneManager.LoadScene("smooch league arena");
    }

    private IEnumerator inputDelay()
    {
        waitBeforeInput = true;
        yield return new WaitForSeconds(0.15f);
        waitBeforeInput = false;
    }
}
