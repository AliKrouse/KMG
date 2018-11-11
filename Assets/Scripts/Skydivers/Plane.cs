using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public PlayerController[] players;
    public float speed, tooFar;
    private bool dropped;
    private bool going = true;
	
	void Update ()
    {
        if (!pauseGame.paused)
        {
            if (going)
                transform.Translate(Vector2.right * speed);

            if (transform.position.x >= tooFar && !dropped)
            {
                foreach (PlayerController player in players)
                {
                    if (!player.dropped)
                        player.DropFromPlane();
                }
                players = null;
                dropped = true;
                StartCoroutine(stopPlane());
            }
        }
	}

    private IEnumerator stopPlane()
    {
        yield return new WaitForSeconds(2);
        going = false;
    }
}
