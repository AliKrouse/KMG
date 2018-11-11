using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    public int AISET;
    public int i;
    public Transform[] waypoints;
    private Vector2 targetPoint;
    public float minSpeed, maxSpeed, speed;
    public float maxDistance = 2.5f;

    private bool hit;
    private SpriteRenderer sr;

    private Animator anim;
    
	void Start ()
    {
        makeTargetPoint();

        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        int i = Random.Range(0, 8);
        anim.SetInteger("aiNumber", i);

        if (AISET > SDOptions.AINumber)
            this.gameObject.SetActive(false);
	}
	
	void Update ()
    {
        if (!hit)
            transform.position = Vector2.MoveTowards(transform.position, targetPoint, Time.deltaTime * speed);

        float d = Vector2.Distance(transform.position, targetPoint);
        if (d <= 0.25)
        {
            i++;
            if (i > 9)
                i = 0;

            makeTargetPoint();
        }

        transform.LookAt(targetPoint);
    }

    void makeTargetPoint()
    {
        float x = Random.Range(-maxDistance, maxDistance);
        float y = Random.Range(-maxDistance, maxDistance);
        targetPoint = new Vector2(waypoints[i].position.x + x, waypoints[i].position.y + y);

        speed = Random.Range(minSpeed, maxSpeed);
    }

    public IEnumerator isHit()
    {
        hit = true;
        anim.SetBool("fallen", true);
        float wait = Random.Range(2, 5);
        yield return new WaitForSeconds(wait);
        anim.SetBool("fallen", false);
        hit = false;
    }
}
