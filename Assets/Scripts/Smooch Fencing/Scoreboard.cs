using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class Scoreboard : MonoBehaviour
{
    public float kissDistance;
    public float kissTimer;
    
    public int pinkPoints, bluePoints;

    public bool pinkKiss, blueKiss;
    public Animator PAnim, BAnim;
    
    public Text winText, resetText;

    public bool gameEnded;

    private Player[] players = new Player[2];

    public CharController pink, blue;

    public AudioSource source;
    public AudioClip win;

    void Start()
    {
        Time.timeScale = 1;

        for (int i = 0; i < 2; i++)
            players[i] = ReInput.players.GetPlayer(i);

        source.loop = true;
    }

    void Update()
    {
        if (resetText.gameObject.activeSelf == true)
        {
            foreach (Player p in players)
            {
                if (p.GetButtonDown("Start"))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                if (p.GetButtonDown("Quit"))
                    SceneManager.LoadScene("smooch league menu");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("smooch league menu");
            }
        }
    }

    public IEnumerator scoreKiss()
    {
        if (gameOptions.mode != "ALL OR NOTHING")
            yield return new WaitForSeconds(kissTimer);
        if (pinkKiss && blueKiss)
        {
            makeOut();

            PAnim.SetBool("kissing", true);
            BAnim.SetBool("kissing", true);
        }
        else
        {
            if (pinkKiss)
            {
                pinkPoints++;
                if (gameOptions.mode == "POINT THIEF")
                    if (bluePoints > 0)
                        bluePoints--;
                if (gameOptions.mode == "ALL OR NOTHING")
                    bluePoints = 0;

                PAnim.SetBool("kissing", true);
                BAnim.SetBool("kissed", true);
            }
            if (blueKiss)
            {
                bluePoints++;
                if (gameOptions.mode == "POINT THIEF")
                    if (pinkPoints > 0)
                        pinkPoints--;
                if (gameOptions.mode == "ALL OR NOTHING")
                    pinkPoints = 0;

                BAnim.SetBool("kissing", true);
                PAnim.SetBool("kissed", true);
            }
        }
        pinkKiss = false; blueKiss = false;

        if (pinkPoints == gameOptions.ptw || bluePoints == gameOptions.ptw)
            gameEnd();
        else if (!gameEnded)
            StartCoroutine(returnAfterKiss());

        yield return null;
    }

    public IEnumerator returnAfterKiss()
    {
        pink.canMove = false;
        blue.canMove = false;
        CharController.canKiss = false;

        yield return new WaitForSeconds(2);

        PAnim.SetBool("kissing", false);
        PAnim.SetBool("kissed", false);
        PAnim.SetBool("returnedControl", true);
        BAnim.SetBool("kissing", false);
        BAnim.SetBool("kissed", false);
        BAnim.SetBool("returnedControl", true);
        PAnim.SetBool("moving", false);
        BAnim.SetBool("moving", false);

        StartCoroutine(kissRecharge());
        pink.canMove = true;
        blue.canMove = true;
        CharController.canKiss = true;

        yield return new WaitForSeconds(0.25f);

        PAnim.SetBool("returnedControl", false);
        BAnim.SetBool("returnedControl", false);
    }

    void gameEnd()
    {
        pink.canMove = false;
        blue.canMove = false;
        CharController.canKiss = false;

        gameEnded = true;

        if (pinkPoints == gameOptions.ptw)
        {
            winText.gameObject.SetActive(true);
            winText.color = new Color(0.96f, 0.66f, 0.72f);
            winText.text = gameOptions.pinkName + " WINS!";
        }
        if (bluePoints == gameOptions.ptw)
        {
            winText.gameObject.SetActive(true);
            winText.color = new Color(0.46f, 0.66f, 0.98f);
            winText.text = gameOptions.blueName + " WINS!";
        }

        source.Stop();
        source.clip = win;
        source.loop = false;
        source.Play();
        resetText.gameObject.SetActive(true);
    }

    void makeOut()
    {
        pink.canMove = false;
        blue.canMove = false;
        CharController.canKiss = false;

        gameEnded = true;

        winText.gameObject.SetActive(true);
        winText.color = new Color(0.92f, 0.28f, 0.28f);
        winText.text = "EVERYBODY WINS WHEN YOU MAKE OUT!!";

        source.Stop();
        source.clip = win;
        source.loop = false;
        source.Play();
        resetText.gameObject.SetActive(true);
    }

    private IEnumerator kissRecharge()
    {
        CharController.canKiss = false;
        yield return new WaitForSeconds(1.5f);
        CharController.canKiss = true;
    }
}
