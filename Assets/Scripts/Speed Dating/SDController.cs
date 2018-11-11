using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class SDController : MonoBehaviour
{
    public int PLAYERNUMBER;
    private Player p;
    private Rigidbody2D rb;
    public float speed;
    private float angle;

    private bool goingFast;

    public bool inBounds = true;
    private bool bothInBounds;

    private bool canKiss = true;
    private bool canBeHit = true;

    private int hitNumber;
    public bool fallen;

    private Coroutine stabilizer;

    private bool goingUp, goingLeft, goingDown, goingRight;

    private AudioSource source;
    public AudioClip kiss, hit, fall;

    public GameObject heart;

    private Animator anim;

    public int kisses = 3;

    private SDController collidingPlayer;

    public GameObject blueLight, pinkLight;
    public float maxDistance;
    private bool hasMultiplier;
    
    public float whatPercentageIsFast;

    private bool isContactingPlayer;

    private SpriteRenderer sr;
    
	void Start ()
    {
        p = ReInput.players.GetPlayer(PLAYERNUMBER);

        rb = GetComponent<Rigidbody2D>();

        source = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();

        if (SDOptions.playerIsUsingController[PLAYERNUMBER])
        {
            p.controllers.AddController(ReInput.controllers.GetController<Joystick>(SDOptions.controllerChoice[PLAYERNUMBER]), true);
            p.controllers.maps.SetAllMapsEnabled(true);
        }

        anim.SetInteger("character", SDOptions.characterChoices[PLAYERNUMBER]);

        ReInput.ControllerConnectedEvent += OnControllerConnected;

        sr = GetComponent<SpriteRenderer>();
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (SDOptions.playerIsUsingController[PLAYERNUMBER])
            if (args.controllerId == SDOptions.controllerChoice[PLAYERNUMBER])
                p.controllers.AddController(ReInput.controllers.GetController(args.controllerType, args.controllerId), true);
    }
	
	void Update ()
    {
        if (!fallen)
        {
            if (p.GetAxis("Vertical") > float.Epsilon && !goingDown)
            {
                rb.AddForce(Vector2.up * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (p.GetAxis("Vertical") < -float.Epsilon && !goingUp)
            {
                rb.AddForce(Vector2.down * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (p.GetAxis("Horizontal") > float.Epsilon && !goingLeft)
            {
                rb.AddForce(Vector2.right * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (p.GetAxis("Horizontal") < -float.Epsilon && !goingRight)
            {
                rb.AddForce(Vector2.left * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (Mathf.Abs(p.GetAxis("Horizontal")) > float.Epsilon || Mathf.Abs(p.GetAxis("Vertical")) > float.Epsilon)
                anim.SetBool("moving", true);
            else
                anim.SetBool("moving", false);

            if (rb.velocity.x > speed)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            if (rb.velocity.x < -speed)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (rb.velocity.y > speed)
                rb.velocity = new Vector2(rb.velocity.x, speed);
            if (rb.velocity.y < -speed)
                rb.velocity = new Vector2(rb.velocity.x, -speed);

            if (Mathf.Abs(rb.velocity.x) >= (speed * whatPercentageIsFast) || Mathf.Abs(rb.velocity.y) >= (speed * whatPercentageIsFast))
                goingFast = true;
            else
            {
                if (goingFast)
                    StartCoroutine(stopGoingFast());
            }

            float d1 = Vector2.Distance(transform.position, pinkLight.transform.position);
            float d2 = Vector2.Distance(transform.position, blueLight.transform.position);
            if (d1 <= maxDistance && d2 <= maxDistance)
                hasMultiplier = true;
            else
                hasMultiplier = false;

            if (isContactingPlayer)
            {
                if (p.GetButtonDown("Kiss"))
                    Kiss();
            }
        }
        if (hitNumber == 0)
            sr.color = Color.white;
        if (hitNumber == 1)
            sr.color = new Color(1, 0.66f, 0.66f);
        if (hitNumber == 2)
            sr.color = new Color(1, 0.33f, 0.33f);
        if (fallen)
            sr.color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (inBounds == true && collision.gameObject.GetComponent<SDController>().inBounds == true)
                bothInBounds = true;

            if (collision.gameObject.GetComponent<SDController>().fallen == false)
            {
                collidingPlayer = collision.gameObject.GetComponent<SDController>();
                isContactingPlayer = true;
            }
        }

        if (collision.gameObject.CompareTag("Team 2"))
        {
            if (goingFast)
            {
                source.PlayOneShot(hit);

                AIBehavior ai = collision.gameObject.GetComponent<AIBehavior>();
                ai.StartCoroutine(ai.isHit());

                if (stabilizer != null)
                    StopCoroutine(stabilizer);

                if (canBeHit)
                {
                    hitNumber++;
                }

                if (hitNumber == SDOptions.hitsToFall)
                    StartCoroutine(Fall());
                else
                    stabilizer = StartCoroutine(reStabilize());
            }
        }

        if (collision.gameObject.CompareTag("Untagged"))
            StartCoroutine(Fall());
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isContactingPlayer = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
            inBounds = true;
        if (collision.gameObject.CompareTag("right"))
            goingUp = true;
        if (collision.gameObject.CompareTag("top"))
            goingLeft = true;
        if (collision.gameObject.CompareTag("left"))
            goingDown = true;
        if (collision.gameObject.CompareTag("bottom"))
            goingRight = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
            inBounds = false;
        if (collision.gameObject.CompareTag("right"))
            goingUp = false;
        if (collision.gameObject.CompareTag("top"))
            goingLeft = false;
        if (collision.gameObject.CompareTag("left"))
            goingDown = false;
        if (collision.gameObject.CompareTag("bottom"))
            goingRight = false;
    }

    void Kiss()
    {
        if (kisses > 0)
        {
            source.PlayOneShot(kiss);
            if (bothInBounds)
            {
                if (hasMultiplier)
                {
                    if (PLAYERNUMBER == 0)
                        SDScoreboard.p1s += 2;
                    if (PLAYERNUMBER == 1)
                        SDScoreboard.p2s += 2;
                    if (PLAYERNUMBER == 2)
                        SDScoreboard.p3s += 2;
                    if (PLAYERNUMBER == 3)
                        SDScoreboard.p4s += 2;
                }
                else
                {
                    if (PLAYERNUMBER == 0)
                        SDScoreboard.p1s++;
                    if (PLAYERNUMBER == 1)
                        SDScoreboard.p2s++;
                    if (PLAYERNUMBER == 2)
                        SDScoreboard.p3s++;
                    if (PLAYERNUMBER == 3)
                        SDScoreboard.p4s++;
                }
            }
            Instantiate(heart, transform.position, Quaternion.identity);
            kisses--;
            cooldownKiss();
        }
    }

    private IEnumerator stopGoingFast()
    {
        yield return new WaitForSeconds(0.5f);
        goingFast = false;
    }

    private IEnumerator cooldownKiss()
    {
        canKiss = false;
        yield return new WaitForSeconds(5);
        canKiss = true;
    }

    private IEnumerator reStabilize()
    {
        canBeHit = false;
        yield return new WaitForSeconds(0.1f);
        canBeHit = true;
        while (hitNumber > 0)
        {
            yield return new WaitForSeconds(2f);
            hitNumber--;
        }
    }

    private IEnumerator Fall()
    {
        anim.SetBool("fall", true);
        source.PlayOneShot(fall);
        fallen = true;
        yield return new WaitForSeconds(5);
        anim.SetBool("fall", false);
        fallen = false;
        hitNumber = 0;
    }
}
