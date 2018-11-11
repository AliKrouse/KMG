using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class THScoreboard : MonoBehaviour
{
    private int ptw;
    public int pinkPoints, bluePoints;
    private Text pt, bt;

    public string color;
    public GameObject p1, p2, b1, b2;
    private Vector2 p1Start, p2Start, b1Start, b2Start;
    public Image cover;

    private bool fadeIn, fadeOut, gameEnd;
    private float a;

    public GameObject heartSpawner;

    public Image vImage1, vImage2, cup;
    public Sprite[] victors;
    public Text vText;
    public GameObject reset;

    private Player[] players = new Player[4];

    public GameObject[] markers;

    public AudioSource source;
    public AudioClip win;
    
	void Start ()
    {
        pt = transform.GetChild(0).GetComponent<Text>();
        bt = transform.GetChild(1).GetComponent<Text>();
        ptw = THOptions.pointsToWin;

        p1Start = p1.transform.position;
        b1Start = b1.transform.position;

        if (THOptions.fourPlayer)
        {
            p2Start = p2.transform.position;
            b2Start = b2.transform.position;
        }

        ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(true);
        ReInput.players.GetPlayer(1).controllers.maps.SetAllMapsEnabled(true);
        ReInput.players.GetPlayer(2).controllers.maps.SetAllMapsEnabled(true);
        ReInput.players.GetPlayer(3).controllers.maps.SetAllMapsEnabled(true);

        for (int i = 0; i < 4; i++)
            players[i] = ReInput.players.GetPlayer(i);

        source.loop = true;
    }
	
	void Update ()
    {
        pt.text = "PINK\n" + pinkPoints;
        bt.text = "BLUE\n" + bluePoints;

        if (fadeIn)
        {
            cover.gameObject.SetActive(true);
            a += Time.deltaTime;
            cover.color = new Color(0, 0, 0, a);

            if (a >= 1)
            {
                fadeOut = true;
                fadeIn = false;

                heartSpawner.SetActive(false);
            }
        }
        if (fadeOut)
        {
            p1.transform.position = p1Start;
            b1.transform.position = b1Start;
            if (THOptions.fourPlayer)
            {
                p2.transform.position = p2Start;
                b2.transform.position = b2Start;
            }

            a -= Time.deltaTime;
            cover.color = new Color(0, 0, 0, a);

            if (a <= 0)
            {
                ReInput.players.GetPlayer(0).controllers.maps.SetAllMapsEnabled(true);
                ReInput.players.GetPlayer(1).controllers.maps.SetAllMapsEnabled(true);
                ReInput.players.GetPlayer(2).controllers.maps.SetAllMapsEnabled(true);
                ReInput.players.GetPlayer(3).controllers.maps.SetAllMapsEnabled(true);

                cover.gameObject.SetActive(false);
                fadeOut = false;
            }
        }

        if ((pinkPoints == ptw || bluePoints == ptw) && !gameEnd)
            source.volume -= Time.deltaTime;

        if (gameEnd)
        {
            cup.gameObject.SetActive(true);
            vImage1.gameObject.SetActive(true);
            if (THOptions.fourPlayer)
                vImage2.gameObject.SetActive(true);
            vText.gameObject.SetActive(true);
            reset.SetActive(true);

            if (pinkPoints == ptw)
            {
                vImage1.sprite = victors[THOptions.characterChoices[0]];
                if (THOptions.fourPlayer)
                    vImage2.sprite = victors[THOptions.characterChoices[1]];
                vText.text = "PINK TEAM WINS!\n" + pinkPoints + " - " + bluePoints;
                vText.color = new Color(0.96f, 0.66f, 0.72f);
            }
            if (bluePoints == ptw)
            {
                vImage1.sprite = victors[THOptions.characterChoices[2]];
                if (THOptions.fourPlayer)
                    vImage2.sprite = victors[THOptions.characterChoices[3]];
                vText.text = "BLUE TEAM WINS!\n" + bluePoints + " - " + pinkPoints;
                vText.color = new Color(0.46f, 0.66f, 0.98f);
            }

            foreach (Player p in players)
            {
                if (p.GetButtonDown("Start"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                if (p.GetButtonDown("Quit"))
                    SceneManager.LoadScene("tonsil hockey menu");
            }
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene("tonsil hockey menu");

            foreach (GameObject m in markers)
                m.SetActive(false);
        }
	}

    public IEnumerator Score()
    {
        heartSpawner.SetActive(true);
        
        p1.GetComponent<THController>().canMove = false;
        p2.GetComponent<THController>().canMove = false;
        b1.GetComponent<THController>().canMove = false;
        b2.GetComponent<THController>().canMove = false;

        if (color == "pink")
            bluePoints++;
        if (color == "blue")
            pinkPoints++;

        if (pinkPoints == ptw || bluePoints == ptw)
        {
            yield return new WaitForSeconds(2f);
            gameEnd = true;
            source.volume = 0.5f;
            source.clip = win;
            source.loop = false;
            source.Play();
        }
        else
        {
            yield return new WaitForSeconds(2f);
            a = 0;
            fadeIn = true;
        }
    }
}
