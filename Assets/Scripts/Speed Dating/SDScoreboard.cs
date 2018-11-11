using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class SDScoreboard : MonoBehaviour
{
    public static int p1s, p2s, p3s, p4s;
    public Text p1name, p2name, p3name, p4name, p1t, p2t, p3t, p4t;
    public Text p1breakdown, p2breakdown, p3breakdown, p4breakdown;

    public SDController p1, p2, p3, p4;
    public static int fallen;
    public int pointBonus;
    
    public Text vText;
    public Image[] vImages;
    public Sprite[] vSprites;
    public GameObject victorParent;
    public GameObject reset;

    private bool victor;

    private bool coroutineIsRunning;

    private Player[] players = new Player[4];

    private float timeRemaining;
    public Text timerText;

    public AudioSource source;
    public AudioClip win;
    
	void Start ()
    {
        Time.timeScale = 1;
        fallen = 0;
        p1s = 0;
        p2s = 0;
        p3s = 0;
        p4s = 0;

        timeRemaining = SDOptions.timer;

        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }

        p1name.text = SDOptions.p1Name;
        p2name.text = SDOptions.p2Name;
        p3name.text = SDOptions.p3Name;
        p4name.text = SDOptions.p4Name;

        source.loop = true;
    }
	
	void Update ()
    {
        p1t.text = p1s.ToString();
        p2t.text = p2s.ToString();
        p3t.text = p3s.ToString();
        p4t.text = p4s.ToString();

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString("F0");
        }

        if (timeRemaining <= 0)
        {
            if (!coroutineIsRunning)
                StartCoroutine(scoreGame());
            if (reset.activeSelf == false)
                source.volume -= Time.deltaTime;
        }

        if (reset.activeSelf == true)
        {
            foreach (Player p in players)
            {
                if (p.GetButtonDown("Start"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                if (p.GetButtonDown("Quit"))
                    SceneManager.LoadScene("speed dating menu");
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("speed dating menu");
            }
        }
	}

    public IEnumerator scoreGame()
    {
        coroutineIsRunning = true;

        yield return new WaitForSeconds(2);

        vImages[0].sprite = vSprites[SDOptions.characterChoices[0]];
        vImages[1].sprite = vSprites[SDOptions.characterChoices[1]];
        vImages[2].sprite = vSprites[SDOptions.characterChoices[2]];
        vImages[3].sprite = vSprites[SDOptions.characterChoices[3]];

        if (p1s > p2s && p1s > p3s && p1s > p4s)
        {
            victor = true;
            vText.text = SDOptions.p1Name + " WINS!";
            vImages[0].gameObject.transform.localScale = Vector2.one;
            vImages[0].gameObject.transform.localPosition = Vector2.zero;
            vImages[0].transform.SetParent(victorParent.transform);
            vImages[0].color = Color.white;
        }
        if (p2s > p1s && p2s > p3s && p2s > p4s)
        {
            victor = true;
            vText.text = SDOptions.p2Name + " WINS!";
            vImages[1].gameObject.transform.localScale = Vector2.one;
            vImages[1].gameObject.transform.localPosition = Vector2.zero;
            vImages[1].transform.SetParent(victorParent.transform);
            vImages[1].color = Color.white;
        }
        if (p3s > p1s && p3s > p2s && p3s > p4s)
        {
            victor = true;
            vText.text = SDOptions.p3Name + " WINS!";
            vImages[2].gameObject.transform.localScale = Vector2.one;
            vImages[2].gameObject.transform.localPosition = Vector2.zero;
            vImages[2].transform.SetParent(victorParent.transform);
            vImages[2].color = Color.white;
        }
        if (p4s > p1s && p4s > p2s && p4s > p3s)
        {
            victor = true;
            vText.text = SDOptions.p4Name + " WINS!";
            vImages[3].gameObject.transform.localScale = Vector2.one;
            vImages[3].gameObject.transform.localPosition = Vector2.zero;
            vImages[3].transform.SetParent(victorParent.transform);
            vImages[3].color = Color.white;
        }
        if (!victor)
        {
            vText.text = "IT'S A TIE!";
            foreach (Image i in vImages)
            {
                i.gameObject.transform.localScale = Vector2.one;
                i.color = Color.white;
            }
        }
        
        vText.gameObject.SetActive(true);
        foreach (Image i in vImages)
            i.gameObject.SetActive(true);

        reset.SetActive(true);

        source.volume = 0.5f;
        source.clip = win;
        source.loop = false;
        source.Play();
    }
}
