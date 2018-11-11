using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class CharController : MonoBehaviour
{
    public int PLAYER;
    private Player p;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private AudioSource source;

    private GameObject cam;

    public float speed;
    public float jumpPower;

    public bool isGrounded, canMoveRight, canMoveLeft;

    public Scoreboard scoring;
    public GameObject opponent;

    public AudioClip jump, kiss;

    public static bool canKiss;

    public GameObject heart;
    private Vector2 heartPoint;
    public float heartYIncrease;

    public bool canMove;
    
	void Start ()
    {
        p = ReInput.players.GetPlayer(PLAYER);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        cam = Camera.main.gameObject;

        canMoveLeft = true;
        canMoveRight = true;
        canKiss = true;

        if (gameOptions.playerIsUsingController[PLAYER])
        {
            p.controllers.AddController(ReInput.controllers.GetController<Joystick>(gameOptions.controllerChoice[PLAYER]), true);
            p.controllers.maps.SetAllMapsEnabled(true);
        }

        anim.SetInteger("characterChoice", gameOptions.characterChoices[PLAYER]);

        ReInput.ControllerConnectedEvent += OnControllerConnected;

        canMove = true;
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (gameOptions.playerIsUsingController[PLAYER])
            if (args.controllerId == gameOptions.controllerChoice[PLAYER])
                p.controllers.AddController(ReInput.controllers.GetController(args.controllerType, args.controllerId), true);
    }

	void Update ()
    {
        float camDistance = Vector2.Distance(transform.position, cam.transform.position);

        if (canMove)
        {
            if (p.GetAxis("Move") > float.Epsilon && canMoveRight)
            {
                rb.velocity = new Vector2(speed * p.GetAxis("Move"), rb.velocity.y);

                sr.flipX = false;
                anim.SetBool("moving", true);
            }

            if (p.GetAxis("Move") < -float.Epsilon && canMoveLeft)
            {
                rb.velocity = new Vector2(speed * p.GetAxis("Move"), rb.velocity.y);

                sr.flipX = true;
                anim.SetBool("moving", true);
            }

            if (p.GetAxis("Move") < float.Epsilon && p.GetAxis("Move") > -float.Epsilon)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetBool("moving", false);
            }

            if (p.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpPower);
                isGrounded = false;

                anim.SetBool("jumped", true);

                if (Time.timeScale == 1)
                    source.PlayOneShot(jump);
            }
        }

        if (!isGrounded && rb.velocity.y > 0)
        {
            anim.SetBool("goingUp", true);
            anim.SetBool("goingDown", false);
        }
        if (!isGrounded && rb.velocity.y < 0)
        {
            anim.SetBool("goingDown", true);
            anim.SetBool("goingUp", false);
        }
        if (isGrounded)
        {
            anim.SetBool("jumped", false);
            anim.SetBool("goingUp", false);
            anim.SetBool("goingDown", false);
        }

        if (p.GetButtonDown("Kiss") && canKiss)
        {
            float kissDistance = Vector2.Distance(transform.position, opponent.transform.position);
            if (kissDistance <= scoring.kissDistance)
                Kiss();
            if (kissDistance > scoring.kissDistance)
                StartCoroutine(TryYourBest());
        }
	}

    void Kiss()
    {
        source.PlayOneShot(kiss);

        Vector2 midpoint = transform.position + (opponent.transform.position - transform.position) / 2;
        heartPoint = new Vector2(midpoint.x, midpoint.y + heartYIncrease);
        Instantiate(heart, heartPoint, Quaternion.identity);

        if (PLAYER == 0)
            scoring.pinkKiss = true;
        if (PLAYER == 1)
            scoring.blueKiss = true;

        scoring.StartCoroutine(scoring.scoreKiss());
    }

    private IEnumerator TryYourBest()
    {
        p.controllers.maps.SetAllMapsEnabled(false);
        anim.SetBool("kissing", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("kissing", false);
        anim.SetBool("returnedControl", true);
        p.controllers.maps.SetAllMapsEnabled(true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("returnedControl", false);
    }
}
