using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class THController : MonoBehaviour
{
    private Player p;
    public int PLAYERNUMBER;
    private Rigidbody2D rb;
    public float speed;
    public bool inGoal;
    public string goalColor;

    public GameObject opponent1, opponent2;
    public float kissDistance;
    public THScoreboard sb;

    public bool isHit, isBeingPushed;
    public float stunTime = 3;

    private bool goingFast;

    private GameObject rotator;
    private Transform point1, point2;
    private GameObject star1, star2;

    private float angle;

    public AudioSource source;
    public AudioClip hit, kiss;

    private Animator anim;

    private GameObject stick;
    private THStick stickScript;
    public float stickSpeed;
    public Sprite[] arms;

    public float starSpeed;

    public bool canMove;
    
	void Start ()
    {
        p = ReInput.players.GetPlayer(PLAYERNUMBER);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rotator = transform.GetChild(0).gameObject;
        point1 = rotator.transform.GetChild(0);
        point2 = rotator.transform.GetChild(1);
        star1 = transform.GetChild(1).gameObject;
        star2 = transform.GetChild(2).gameObject;

        source = GetComponent<AudioSource>();

        if (THOptions.playerIsUsingController[PLAYERNUMBER])
        {
            p.controllers.AddController(ReInput.controllers.GetController<Joystick>(THOptions.controllerChoice[PLAYERNUMBER]), true);
            p.controllers.maps.SetAllMapsEnabled(true);
        }

        stick = transform.GetChild(3).gameObject;
        stickScript = stick.GetComponent<THStick>();

        anim.SetInteger("characterChoice", THOptions.characterChoices[PLAYERNUMBER]);
        stick.GetComponent<SpriteRenderer>().sprite = arms[THOptions.characterChoices[PLAYERNUMBER]];

        ReInput.ControllerConnectedEvent += OnControllerConnected;

        canMove = true;
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (THOptions.playerIsUsingController[PLAYERNUMBER])
        {
            if (args.controllerId == THOptions.controllerChoice[PLAYERNUMBER])
                p.controllers.AddController(ReInput.controllers.GetController(args.controllerType, args.controllerId), true);
        }
    }
	
	void Update ()
    {
        if (!isHit && canMove)
        {
            if (p.GetAxis("Vertical") > float.Epsilon)
            {
                rb.AddForce(Vector2.up * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (p.GetAxis("Vertical") < -float.Epsilon)
            {
                rb.AddForce(Vector2.down * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (p.GetAxis("Horizontal") > float.Epsilon)
            {
                rb.AddForce(Vector2.right * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (p.GetAxis("Horizontal") < -float.Epsilon)
            {
                rb.AddForce(Vector2.left * speed);
                angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            if (Mathf.Abs(p.GetAxis("Horizontal")) > float.Epsilon || Mathf.Abs(p.GetAxis("Vertical")) > float.Epsilon)
            {
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
                stick.transform.localPosition = Vector2.zero;
            }

            if (rb.velocity.x > speed)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            if (rb.velocity.x < -speed)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (rb.velocity.y > speed)
                rb.velocity = new Vector2(rb.velocity.x, speed);
            if (rb.velocity.y < -speed)
                rb.velocity = new Vector2(rb.velocity.x, -speed);

            if (!inGoal)
            {
                if (p.GetButton("Stick Right"))
                {
                    if (stickScript.canTurnRight)
                    {
                        stick.transform.Rotate(Vector3.forward * Time.deltaTime * -stickSpeed);
                        stickScript.inMotion = true;
                    }
                }
                if (p.GetButton("Stick Left"))
                {
                    if (stickScript.canTurnLeft)
                    {
                        stick.transform.Rotate(Vector3.forward * Time.deltaTime * stickSpeed);
                        stickScript.inMotion = true;
                    }
                }
                if (!p.GetButton("Stick Left") && !p.GetButton("Stick Right"))
                {
                    stick.GetComponent<THStick>().inMotion = false;
                }
            }

            if (inGoal)
            {
                stick.GetComponent<PolygonCollider2D>().enabled = false;

                if (opponent1.GetComponent<THController>().inGoal)
                {
                    Vector2 dir = opponent1.transform.position - transform.position;
                    angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
                if (opponent2.GetComponent<THController>().inGoal)
                {
                    Vector2 dir = opponent2.transform.position - transform.position;
                    angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }

                float d1 = Vector2.Distance(transform.position, opponent1.transform.position);
                if (d1 <= kissDistance && opponent1.GetComponent<THController>().inGoal)
                {
                    if (p.GetButtonDown("Kiss"))
                    {
                        anim.SetBool("kissing", true);
                        opponent1.GetComponent<THController>().anim.SetBool("kissing", true);
                        sb.heartSpawner.transform.position = transform.position;
                        sb.color = goalColor;
                        sb.StartCoroutine(sb.Score());

                        source.PlayOneShot(kiss);
                    }
                }

                if (THOptions.fourPlayer)
                {
                    float d2 = Vector2.Distance(transform.position, opponent2.transform.position);
                    if (d2 <= kissDistance && opponent2.GetComponent<THController>().inGoal)
                    {
                        if (p.GetButtonDown("Kiss"))
                        {
                            anim.SetBool("kissing", true);
                            opponent2.GetComponent<THController>().anim.SetBool("kissing", true);
                            sb.heartSpawner.transform.position = transform.position;
                            sb.color = goalColor;
                            sb.StartCoroutine(sb.Score());

                            source.PlayOneShot(kiss);
                        }
                    }
                }
            }
            else
            {
                anim.SetBool("kissing", false);
                stick.GetComponent<PolygonCollider2D>().enabled = true;
            }
        }
        if (isHit)
        {
            anim.SetBool("moving", false);
            stick.transform.localPosition = Vector2.zero;
            stunTime -= Time.deltaTime;
            if (stunTime <= 0)
                isHit = false;

            rb.drag = 0;
            rb.angularDrag = 0;
            rb.mass = 1;

            rotator.SetActive(true);
            star1.SetActive(true);
            star2.SetActive(true);

            rotator.transform.Rotate(Vector3.forward * Time.deltaTime * starSpeed);
            star1.transform.position = point1.position;
            star2.transform.position = point2.position;
        }
        else if (isBeingPushed)
        {
            rb.drag = 0.5f;
            rb.angularDrag = 0.5f;
            rb.mass = 0.5f;
        }
        else
        {
            rb.drag = 1;
            rb.angularDrag = 1;
            rb.mass = 1;

            rotator.SetActive(false);
            star1.SetActive(false);
            star2.SetActive(false);
        }

        if (Mathf.Abs(rb.velocity.x) >= (speed * 0.9f) || Mathf.Abs(rb.velocity.y) >= (speed * 0.9f))
            goingFast = true;
        else
        {
            if (goingFast)
                StartCoroutine(stopGoingFast());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (goingFast)
            {
                Vector2 targetDir = collision.gameObject.transform.position - transform.position;
                float angle = Vector2.Angle(targetDir, transform.right);

                if (angle <= 45)
                {
                    collision.gameObject.GetComponent<THController>().stunTime = 3;
                    collision.gameObject.GetComponent<THController>().isHit = true;
                    
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(targetDir * (speed * 1.5f));

                    source.PlayOneShot(hit);
                }
            }
        }
    }

    private IEnumerator stopGoingFast()
    {
        yield return new WaitForSeconds(0.5f);
        goingFast = false;
    }

    private void moveStickForward()
    {
        stick.transform.localPosition = new Vector2(0.1f, 0);
    }

    private void moveStickBackward()
    {
        stick.transform.localPosition = Vector2.zero;
    }
}
