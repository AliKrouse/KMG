using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class JSFController : MonoBehaviour
{
    public int PLAYERNUMBER;
    private Player p;

    public float speed;
    public float distance;
    public Transform center;

    private Transform left, right, forward;

    public GameObject playerOnLeft, playerOnRight;
    private GameObject playerToKiss;

    private DistanceJoint2D dj;
    public float newDistance;

    public Animator anim;
    private SpriteRenderer sr;

    public bool outOfGame;

    private bool kissing;

    private AudioSource source;
    public AudioClip kiss;

    public int lives = 3;

    public GameObject heart;

    private bool canKiss;
    
	void Start ()
    {
        p = ReInput.players.GetPlayer(PLAYERNUMBER);
        left = transform.GetChild(1);
        right = transform.GetChild(0);
        forward = transform.GetChild(2);

        JSFVictoryTracker.playersInGame[PLAYERNUMBER] = this;

        dj = GetComponent<DistanceJoint2D>();
        dj.distance = 4;

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        source = GetComponent<AudioSource>();

        if (JSFOptions.playerIsUsingController[PLAYERNUMBER])
        {
            p.controllers.AddController(ReInput.controllers.GetController<Joystick>(JSFOptions.controllerChoice[PLAYERNUMBER]), true);
            p.controllers.maps.SetAllMapsEnabled(true);
        }

        ReInput.ControllerConnectedEvent += OnControllerConnected;

        canKiss = true;
	}

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (JSFOptions.playerIsUsingController[PLAYERNUMBER])
            if (args.controllerId == JSFOptions.controllerChoice[PLAYERNUMBER])
                p.controllers.AddController(ReInput.controllers.GetController(args.controllerType, args.controllerId), true);
    }
	
	void Update ()
    {
        if (!outOfGame)
        {
            Vector2 dir = center.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (p.GetAxis("Movement") < -float.Epsilon)
            {
                transform.position = Vector2.MoveTowards(transform.position, left.position, Time.deltaTime * speed);

                if (p.GetButtonDown("Kiss") && !kissing && canKiss)
                {
                    if (playerOnLeft != null)
                        playerToKiss = playerOnLeft;
                    StartCoroutine(Kiss());
                }
            }
            if (p.GetAxis("Movement") > float.Epsilon)
            {
                transform.position = Vector2.MoveTowards(transform.position, right.position, Time.deltaTime * speed);

                if (p.GetButtonDown("Kiss") && !kissing && canKiss)
                {
                    if (playerOnRight != null)
                        playerToKiss = playerOnRight;
                    sr.flipY = true;
                    StartCoroutine(Kiss());
                }
            }

            newDistance = (JSFVictoryTracker.playersLeftInCircle * 1.0f) / 2;
            if (dj.distance > newDistance)
            {
                dj.distance -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, Time.deltaTime);
            }

            if (lives > 4)
                lives = 4;
            if (lives <= 0)
                outOfGame = true;
        }
        if (outOfGame)
        {
            float a = sr.color.a;

            a -= Time.deltaTime;

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);

            if (a <= 0)
            {
                JSFVictoryTracker.playersInGame[PLAYERNUMBER] = null;
                JSFVictoryTracker.playersLeft--;
                JSFVictoryTracker.playersLeftInCircle--;
                Destroy(this.gameObject);
            }
        }
	}

    private IEnumerator Kiss()
    {
        kissing = true;

        source.PlayOneShot(kiss);

        anim.SetBool("kissing", true);

        if (playerToKiss != null)
        {
            if (playerToKiss.GetComponent<JSFController>().PLAYERNUMBER == JSFVictoryTracker.targetChoices[PLAYERNUMBER] && PLAYERNUMBER == JSFVictoryTracker.targetChoices[playerToKiss.GetComponent<JSFController>().PLAYERNUMBER])
                playerToKiss.GetComponent<JSFController>().lives++;
            else
                playerToKiss.GetComponent<JSFController>().lives--;
            playerToKiss.GetComponent<JSFController>().anim.SetBool("kissed", true);

            Vector2 midpoint = playerToKiss.gameObject.transform.position + (transform.position - playerToKiss.gameObject.transform.position) / 2;
            Instantiate(heart, midpoint, Quaternion.identity);
        }

        p.controllers.maps.SetAllMapsEnabled(false);
        yield return new WaitForSeconds(1f);
        p.controllers.maps.SetAllMapsEnabled(true);

        if (playerToKiss != null)
            playerToKiss.GetComponent<JSFController>().anim.SetBool("kissed", false);

        anim.SetBool("kissing", false);

        kissing = false;
        playerToKiss = null;
        StartCoroutine(kissCooldown());

        yield return new WaitForSeconds(0.1f);
        sr.flipY = false;
    }

    private IEnumerator kissCooldown()
    {
        canKiss = false;
        yield return new WaitForSeconds(1);
        canKiss = true;
    }
}
