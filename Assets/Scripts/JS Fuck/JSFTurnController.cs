using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JSFTurnController : MonoBehaviour
{
    public Text t;
    public static bool runningTurns = true;

    private Coroutine turnRoutine;
    
	void Start ()
    {
        turnRoutine = StartCoroutine(turn());
	}

    private void Update()
    {
        if (!runningTurns || JSFPause.paused)
        {
            t.gameObject.SetActive(false);
            if (turnRoutine != null)
            {
                StopCoroutine(turnRoutine);
                turnRoutine = null;
            }
        }
        if (runningTurns && !JSFPause.paused)
        {
            t.gameObject.SetActive(true);
            if (turnRoutine == null)
                turnRoutine = StartCoroutine(turn());
        }
    }

    private IEnumerator turn()
    {
        while (runningTurns && !JSFPause.paused)
        {
            Time.timeScale = 0;
            t.text = "WAIT...";
            yield return new WaitForSecondsRealtime(3);
            t.text = "MOVE IN 3...";
            yield return new WaitForSecondsRealtime(1);
            t.text = "MOVE IN 2...";
            yield return new WaitForSecondsRealtime(1);
            t.text = "MOVE IN 1...";
            yield return new WaitForSecondsRealtime(1);
            t.text = "MOVE!";
            Time.timeScale = 1;
            yield return new WaitForSecondsRealtime(3);
        }
    }
}
