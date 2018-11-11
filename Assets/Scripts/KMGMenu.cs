using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.Video;

public class KMGMenu : MonoBehaviour
{
    private Player allControllers;

    public int i;
    public GameObject selector;
    public Transform[] selectorPoints;

    private AudioSource source;
    public AudioClip confirm;

    private bool grow, shrink;
    public float selectorWaverSpeed;
    public float sizeChange;
    private Vector2 large, small;

    public Text descriptive, players;

    private bool acceptInput = true;

    private Joystick j;

    public VideoPlayer vp;
    public VideoClip[] clips;

    public AudioSource music;
    public AudioClip[] tracks;
    
	void Start ()
    {
        source = GetComponent<AudioSource>();

        grow = true;

        small = selector.transform.localScale;
        large = new Vector2(selector.transform.localScale.x + sizeChange, selector.transform.localScale.y + sizeChange);

        allControllers = ReInput.players.GetPlayer(0);
        foreach (Joystick j in ReInput.controllers.Joysticks)
            allControllers.controllers.AddController(j, false);

        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnected;

        Time.timeScale = 1;
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        j = ReInput.controllers.GetJoystick(args.controllerId);
        allControllers.controllers.AddController(j, true);
    }
    void OnControllerPreDisconnected(ControllerStatusChangedEventArgs args)
    {
        j = ReInput.controllers.GetJoystick(args.controllerId);
        allControllers.controllers.RemoveController(j);
    }
	
	void Update ()
    {
        if (allControllers.GetAxis("Vertical") > float.Epsilon && acceptInput)
        {
            i--;
            source.PlayOneShot(confirm);
            if (i < 0)
                i = 4;
            
            vp.clip = clips[i];

            StartCoroutine(waitForInput());

            music.Stop();
            music.clip = tracks[i];
            music.Play();
        }
        if (allControllers.GetAxis("Vertical") < -float.Epsilon && acceptInput)
        {
            i++;
            source.PlayOneShot(confirm);
            if (i > 4)
                i = 0;
            
            vp.clip = clips[i];

            StartCoroutine(waitForInput());

            music.Stop();
            music.clip = tracks[i];
            music.Play();
        }
        if (allControllers.GetButtonDown("Select"))
        {
            if (i == 0)
                SceneManager.LoadScene("skydivers menu");
            if (i == 1)
                SceneManager.LoadScene("smooch league menu");
            if (i == 2)
                SceneManager.LoadScene("tonsil hockey menu");
            if (i == 3)
                SceneManager.LoadScene("speed dating menu");
            if (i == 4)
                SceneManager.LoadScene("js fuck menu");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        selector.transform.position = selectorPoints[i].position;

        if (grow)
        {
            selector.transform.localScale = Vector2.MoveTowards(selector.transform.localScale, large, Time.deltaTime * selectorWaverSpeed);
            if (selector.transform.localScale.x >= large.x)
            {
                shrink = true;
                grow = false;
            }
        }
        if (shrink)
        {
            selector.transform.localScale = Vector2.MoveTowards(selector.transform.localScale, small, Time.deltaTime * selectorWaverSpeed);
            if (selector.transform.localScale.x <= small.x)
            {
                grow = true;
                shrink = false;
            }
        }

        if (i == 0)
        {
            descriptive.text = "Jump out of a plane and kiss your friends on the ground";
            players.text = "4 players";
        }
        if (i == 1)
        {
            descriptive.text = "Fencing, but with your mouth";
            players.text = "2 players";
        }
        if (i == 2)
        {
            descriptive.text = "Kiss opposing team members inside their goal to win";
            players.text = "2 or 4 players";
        }
        if (i == 3)
        {
            descriptive.text = "Weave through the crowd to find your friends and kiss 'em";
            players.text = "2 to 4 players";
        }
        if (i == 4)
        {
            descriptive.text = "Try to eliminate all players except you and your crush";
            players.text = "4 to 8 players";
        }
	}

    private IEnumerator waitForInput()
    {
        acceptInput = false;
        yield return new WaitForSeconds(0.15f);
        acceptInput = true;
    }
}
