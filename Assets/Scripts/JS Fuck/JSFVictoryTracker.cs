using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class JSFVictoryTracker : MonoBehaviour
{
    public static int[] targetChoices = new int[8];
    public static JSFController[] playersInGame = new JSFController[8];
    public static int playersLeft;
    public static int playersLeftInCircle;

    private JSFController remaining1, remaining2;
    private int r1Choice, r2Choice;

    public GameObject[] eyesHolders;
    private Image[] victoryEyes = new Image[2];
    public Sprite[] vEyesSprites;
    private Sprite[] vEyesWinners = new Sprite[2];
    public Text victoryText;
    private string[] winnerNames = new string[2];
    public GameObject reset;

    public float eyesSpeed;

    private bool UIActive;

    private Camera cam;
    private float newSize;

    private Player[] players = new Player[8];

    public AudioSource source;
    public AudioClip win, loss;

    private bool gameOver;
    
	void Start ()
    {
        for (int i = 0; i < 8; i++)
            if (JSFOptions.playerIsInGame[i])
                playersLeft++;
        playersLeftInCircle = 8;

        cam = Camera.main;

        victoryEyes[0] = eyesHolders[0].transform.GetChild(1).GetComponent<Image>();
        victoryEyes[1] = eyesHolders[1].transform.GetChild(1).GetComponent<Image>();

        for (int i = 0; i < 8; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }

        Time.timeScale = 1;

        source.loop = true;
    }
	
	void Update ()
    {
        if (playersLeft <= 2 && !gameOver)
        {
            StartCoroutine(endGame());
            source.volume -= Time.deltaTime;
        }

        if (UIActive)
        {
            if (playersLeft == 2)
            {
                FindRemainingPlayers();

                if (r1Choice == remaining2.PLAYERNUMBER && r2Choice == remaining1.PLAYERNUMBER)
                {
                    eyesHolders[0].SetActive(true);
                    victoryEyes[0].sprite = vEyesWinners[0];
                    eyesHolders[1].SetActive(true);
                    victoryEyes[1].sprite = vEyesWinners[1];

                    if (eyesHolders[0].transform.localPosition.y > 75)
                        eyesHolders[0].transform.Translate(Vector2.down * eyesSpeed);
                    if (eyesHolders[1].transform.localPosition.y < -75)
                        eyesHolders[1].transform.Translate(Vector2.up * eyesSpeed);

                    victoryText.gameObject.SetActive(true);
                    victoryText.text = winnerNames[0] + " AND " + winnerNames[1] + " WIN!";

                    if (!gameOver)
                    {
                        source.volume = 0.5f;
                        source.loop = false;
                        source.clip = win;
                        source.Play();

                        gameOver = true;
                    }
                }
                else 
                {
                    if (!gameOver)
                    {
                        victoryText.gameObject.SetActive(true);
                        victoryText.text = "NODOBY GETS TO MAKE OUT :(";

                        source.volume = 0.5f;
                        source.loop = false;
                        source.clip = loss;
                        source.Play();

                        gameOver = true;
                    }
                }

                reset.SetActive(true);
            }
            if (playersLeft < 2 && !gameOver)
            {
                victoryText.gameObject.SetActive(true);
                victoryText.text = "NODOBY GETS TO MAKE OUT :(";

                source.volume = 0.5f;
                source.loop = false;
                source.clip = loss;
                source.Play();

                gameOver = true;

                reset.SetActive(true);
            }
        }

        newSize = (playersLeftInCircle * 1.0f) / 2 + 1;
        if (cam.orthographicSize > newSize)
            cam.orthographicSize -= Time.deltaTime;

        if (reset.activeSelf)
        {
            foreach (Player p in players)
            {
                if (p.GetButtonDown("Start"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                if (p.GetButtonDown("Quit"))
                    SceneManager.LoadScene("js fuck menu");
            }
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("js fuck menu");
        }
    }

    void FindRemainingPlayers()
    {
        for (int i = 0; i < 8; i++)
        {
            if (playersInGame[i] != null)
            {
                if (remaining1 == null)
                {
                    remaining1 = playersInGame[i];
                    r1Choice = targetChoices[i];
                    vEyesWinners[0] = vEyesSprites[i];
                    winnerNames[0] = playersInGame[i].gameObject.name;
                }
                else if (remaining2 == null)
                {
                    remaining2 = playersInGame[i];
                    r2Choice = targetChoices[i];
                    vEyesWinners[1] = vEyesSprites[i];
                    winnerNames[1] = playersInGame[i].gameObject.name;
                }
            }
        }
    }

    private IEnumerator endGame()
    {
        yield return new WaitForSeconds(1.5f);
        UIActive = true;
        JSFTurnController.runningTurns = false;
        Time.timeScale = 1;
    }
}
