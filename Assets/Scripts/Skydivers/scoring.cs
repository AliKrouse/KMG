using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class scoring : MonoBehaviour
{
    public GameObject[] team1;
    public GameObject[] team2;
    public Animator[] anims;
    public float maxTeamDistance;

    public Text[] d, f;
    public Text v;
    public Image winner1, winner2;
    public Sprite[] vImages;
    public GameObject restart;

    private bool t1IsTogether, t2IsTogether;
    private float t1Distance, t2Distance;

    private bool p1Landed, p2Landed, p3Landed, p4Landed;

    private bool gameOver;

    private bool coroutineIsRunning = false;

    private Player[] players = new Player[4];

    private bool noWinner;

    public AudioSource source;
    public AudioClip win, loss;

    void Start ()
    {
        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }

        Time.timeScale = 1;

        source.loop = true;
        source.volume = 0.5f;
    }
	
	void Update ()
    {
        if (p1Landed && p2Landed && p3Landed && p4Landed )
        {
            if (!coroutineIsRunning)
                FinalizeScoring();
            if (!gameOver)
                source.volume -= Time.deltaTime;
        }

        if (gameOver)
        {
            foreach (Player p in players)
            {
                if (p.GetButtonDown("Start"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                if (p.GetButtonDown("Quit"))
                    SceneManager.LoadScene("skydivers menu");
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("skydivers menu");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "p1")
            p1Landed = true;
        if (collision.gameObject.name == "p2")
            p2Landed = true;
        if (collision.gameObject.name == "p3")
            p3Landed = true;
        if (collision.gameObject.name == "p4")
            p4Landed = true;
    }

    void FinalizeScoring()
    {
        foreach (Text t in d)
            t.gameObject.SetActive(true);

        d[0].text = Vector2.Distance(team1[0].transform.position, transform.position).ToString("F2") + " ft off";
        d[1].text = Vector2.Distance(team1[1].transform.position, transform.position).ToString("F2") + " ft off";
        d[2].text = Vector2.Distance(team2[0].transform.position, transform.position).ToString("F2") + " ft off";
        d[3].text = Vector2.Distance(team2[1].transform.position, transform.position).ToString("F2") + " ft off";

        float t1Dist = Vector2.Distance(team1[0].transform.position, team1[1].transform.position);
        if (t1Dist > maxTeamDistance)
        {
            t1IsTogether = false;
            f[0].text = "YOU'RE TOO FAR FROM YOUR TEAMMATE";
            f[1].text = "YOU'RE TOO FAR FROM YOUR TEAMMATE";
            f[0].gameObject.SetActive(true);
            f[1].gameObject.SetActive(true);
        }
        else
        {
            t1IsTogether = true;
        }

        float t2Dist = Vector2.Distance(team2[0].transform.position, team2[1].transform.position);
        if (t2Dist > maxTeamDistance)
        {
            t2IsTogether = false;
            f[2].text = "YOU'RE TOO FAR FROM YOUR TEAMMATE";
            f[3].text = "YOU'RE TOO FAR FROM YOUR TEAMMATE";
            f[2].gameObject.SetActive(true);
            f[3].gameObject.SetActive(true);
        }
        else
        {
            t2IsTogether = true;
        }

        if (t1IsTogether && !t2IsTogether && !coroutineIsRunning)
        {
            v.text = "P I N K   T E A M\nW I N S !";
            v.color = new Color(0.96f, 0.66f, 0.72f);
            anims[0].SetBool("victory", true);
            anims[1].SetBool("victory", true);
            
            winner1.sprite = vImages[SkydiversOptions.characterChoices[0]];
            winner2.sprite = vImages[SkydiversOptions.characterChoices[1]];

            StartCoroutine(waitForVictory());

            noWinner = false;
        }
        if (t2IsTogether && !t1IsTogether && !coroutineIsRunning)
        {
            v.text = "B L U E   T E A M\nW I N S !";
            v.color = new Color(0.46f, 0.66f, 0.98f);
            anims[2].SetBool("victory", true);
            anims[3].SetBool("victory", true);
            
            winner1.sprite = vImages[SkydiversOptions.characterChoices[2]];
            winner2.sprite = vImages[SkydiversOptions.characterChoices[3]];

            StartCoroutine(waitForVictory());

            noWinner = false;
        }

        if (t1IsTogether && t2IsTogether)
        {
            Vector2 mp1 = team1[1].transform.position + (team1[0].transform.position - team1[1].transform.position) * 0.5f;
            t1Distance = Vector2.Distance(mp1, transform.position);

            Vector2 mp2 = team2[1].transform.position + (team2[0].transform.position - team2[1].transform.position) * 0.5f;
            t2Distance = Vector2.Distance(mp2, transform.position);

            if (t1Distance < t2Distance && !coroutineIsRunning)
            {
                v.text = "P I N K   T E A M\nW I N S !";
                v.color = new Color(0.96f, 0.66f, 0.72f);
                anims[0].SetBool("victory", true);
                anims[1].SetBool("victory", true);
                
                winner1.sprite = vImages[SkydiversOptions.characterChoices[0]];
                winner2.sprite = vImages[SkydiversOptions.characterChoices[1]];

                StartCoroutine(waitForVictory());

                noWinner = false;
            }

            if (t2Distance < t1Distance && !coroutineIsRunning)
            {
                v.text = "B L U E   T E A M\nW I N S !";
                v.color = new Color(0.46f, 0.66f, 0.98f);
                anims[2].SetBool("victory", true);
                anims[3].SetBool("victory", true);
                
                winner1.sprite = vImages[SkydiversOptions.characterChoices[2]];
                winner2.sprite = vImages[SkydiversOptions.characterChoices[3]];

                StartCoroutine(waitForVictory());

                noWinner = false;
            }

            if (t1Distance == t2Distance && !coroutineIsRunning)
            {
                v.text = "I T S   A   T I E !";
                v.color = Color.white;

                StartCoroutine(waitForVictory());

                noWinner = true;
            }
        }

        if (!t1IsTogether && !t2IsTogether && !coroutineIsRunning)
        {
            v.text = "N O B O D Y   W I N S   : (";
            v.color = Color.white;
            
            StartCoroutine(waitForVictory());

            noWinner = true;
        }
    }

    private IEnumerator waitForVictory()
    {
        coroutineIsRunning = true;
        yield return new WaitForSeconds(2);

        v.gameObject.SetActive(true);
        if (!noWinner)
        {
            winner1.gameObject.SetActive(true);
            winner2.gameObject.SetActive(true);
            source.clip = win;
        }
        else
            source.clip = loss;
        restart.SetActive(true);
        gameOver = true;

        source.loop = false;
        source.Stop();
        source.volume = 0.5f;
        source.Play();
    }
}
