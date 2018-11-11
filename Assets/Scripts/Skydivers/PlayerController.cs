using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    private Player p;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator a;
    public float fallSpeed0, fallSpeed1, fallSpeed2, moveSpeed, moveSpeedCap, curFallSpeed, force;
    public bool dropped = false;
    public bool hit = false;
    public bool landed = false;

    public GameObject heart;
    private GameObject teamMate;
    private Coroutine heartSpawner;
    private bool runningCoroutine;

    private AudioSource source;
    public AudioClip jump, hitPlayer, kiss, land;

    private GameObject plane;

    private GameObject marker;
    
	void Awake ()
    {
        p = ReInput.players.GetPlayer(playerNumber);

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        a = GetComponent<Animator>();

        curFallSpeed = fallSpeed0;

        source = GetComponent<AudioSource>();

        if (SkydiversOptions.playerIsUsingController[playerNumber])
        {
            p.controllers.AddController(ReInput.controllers.GetController<Joystick>(SkydiversOptions.controllerChoice[playerNumber]), true);
            p.controllers.maps.SetAllMapsEnabled(true);
        }

        a.SetInteger("character", SkydiversOptions.characterChoices[playerNumber]);

        ReInput.ControllerConnectedEvent += OnControllerConnected;

        plane = transform.parent.gameObject;

        marker = transform.GetChild(1).transform.GetChild(0).gameObject;
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (SkydiversOptions.playerIsUsingController[playerNumber])
            if (args.controllerId == SkydiversOptions.controllerChoice[playerNumber])
                p.controllers.AddController(ReInput.controllers.GetController(args.controllerType, args.controllerId), true);
    }
	
	void Update ()
    {
        if (!landed)
        {
            if (!dropped)
            {
                transform.position = transform.parent.position;
                rb.velocity = Vector2.zero;

                if (p.GetButtonDown("Fall"))
                {
                    if (plane.transform.position.x > -44 && plane.transform.position.x < 44)
                        DropFromPlane();
                }
            }

            if (dropped)
            {
                if (!hit)
                {
                    if (p.GetButtonDown("Fall"))
                    {
                        if (curFallSpeed == fallSpeed0)
                        {
                            curFallSpeed = fallSpeed1;
                            a.SetBool("moderate", true);
                            a.SetBool("fast", false);
                            a.SetBool("slow", false);
                            return;
                        }
                        if (curFallSpeed == fallSpeed1)
                        {
                            curFallSpeed = fallSpeed2;
                            a.SetBool("fast", true);
                            a.SetBool("moderate", false);
                            a.SetBool("slow", false);
                            return;
                        }
                    }
                    if (p.GetButtonDown("Slow"))
                    {
                        if (curFallSpeed == fallSpeed2)
                        {
                            curFallSpeed = fallSpeed1;
                            a.SetBool("moderate", true);
                            a.SetBool("fast", false);
                            a.SetBool("slow", false);
                            return;
                        }
                        if (curFallSpeed == fallSpeed1)
                        {
                            curFallSpeed = fallSpeed0;
                            a.SetBool("slow", true);
                            a.SetBool("moderate", false);
                            a.SetBool("fast", false);
                            return;
                        }
                    }
                    if (p.GetAxis("Move") > float.Epsilon)
                    {
                        rb.AddForce(Vector2.right * moveSpeed);
                        sr.flipX = true;
                        a.SetBool("motion", true);
                        a.SetBool("down", false);
                    }
                    if (p.GetAxis("Move") < -float.Epsilon)
                    {
                        rb.AddForce(Vector2.left * moveSpeed);
                        sr.flipX = false;
                        a.SetBool("motion", true);
                        a.SetBool("down", false);
                    }
                    if (p.GetAxis("Move") < float.Epsilon && p.GetAxis("Move") > -float.Epsilon)
                    {
                        a.SetBool("down", true);
                        a.SetBool("motion", false);
                    }

                    rb.velocity = new Vector2(rb.velocity.x, curFallSpeed);

                    if (rb.velocity.x > moveSpeedCap)
                        rb.velocity = new Vector2(moveSpeedCap, curFallSpeed);
                    if (rb.velocity.x < -moveSpeedCap)
                        rb.velocity = new Vector2(-moveSpeedCap, curFallSpeed);
                }
            }
        }

        if (landed)
        {
            rb.velocity = Vector2.zero;
            GetComponent<CircleCollider2D>().enabled = false;
        }
	}

    public void DropFromPlane()
    {
        transform.parent = null;
        dropped = true;
        sr.enabled = true;
        a.enabled = true;

        source.volume = 1;
        source.clip = jump;
        source.Play();

        marker.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (dropped && !landed)
        {
            if ((this.gameObject.tag == "Team 1" && collision.gameObject.tag == "Team 2") || (this.gameObject.tag == "Team 2" && collision.gameObject.tag == "Team 1"))
            {
                if (!hit)
                {
                    Rigidbody2D crb = collision.gameObject.GetComponent<Rigidbody2D>();
                    PlayerController cpc = collision.gameObject.GetComponent<PlayerController>();

                    if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(crb.velocity.x))
                    {
                        source.Stop();
                        source.volume = 1;
                        source.clip = hitPlayer;
                        source.Play();

                        cpc.hit = true;
                        cpc.StartCoroutine(cpc.regainControl());

                        if (transform.position.x > collision.gameObject.transform.position.x)
                            crb.AddForce(Vector2.left * force);
                        if (transform.position.x < collision.gameObject.transform.position.x)
                            crb.AddForce(Vector2.right * force);
                    }
                }
            }
            if ((this.gameObject.tag == "Team 1" && collision.gameObject.tag == "Team 1") || (this.gameObject.tag == "Team 2" && collision.gameObject.tag == "Team 2"))
            {
                source.Stop();
                source.clip = kiss;
                source.volume = 0.5f;
                source.Play();
            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                landed = true;
                rb.velocity = Vector2.zero;
                a.SetBool("landed", true);

                source.Stop();
                source.volume = 1;
                source.clip = land;
                source.Play();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dropped)
        {
            if ((this.gameObject.tag == "Team 1" && collision.gameObject.tag == "Team 1") || (this.gameObject.tag == "Team 2" && collision.gameObject.tag == "Team 2"))
            {
                if (runningCoroutine == false)
                {
                    teamMate = collision.gameObject;
                    heartSpawner = StartCoroutine(spawnHearts());
                    runningCoroutine = true;
                }
            }
        }
        if (landed)
        {
            if (collision.gameObject.tag == "Team 1" || collision.gameObject.tag == "Team 2")
            {
                if (collision.gameObject.transform.position.y > transform.position.y)
                {
                    if (transform.position.x > 0)
                        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);
                    if (transform.position.x < 0)
                        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);
                    if (transform.position.x == 0)
                        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * (force / 2));
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((this.gameObject.tag == "Team 1" && collision.gameObject.tag == "Team 1") || (this.gameObject.tag == "Team 2" && collision.gameObject.tag == "Team 2"))
        {
            if (heartSpawner != null)
            {
                StopCoroutine(heartSpawner);
                runningCoroutine = false;
            }
        }
    }

    private IEnumerator regainControl()
    {
        yield return new WaitForSeconds(2);
        hit = false;
    }

    private IEnumerator spawnHearts()
    {
        while (true)
        {
            Instantiate(heart, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
