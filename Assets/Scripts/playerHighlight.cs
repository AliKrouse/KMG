using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHighlight : MonoBehaviour
{
    private Image i;
    private Color c;
    private float a;
    public float fadeInSpeed, fadeOutSpeed;
    private bool fadingIn, fadingOut;
    
	void OnEnable ()
    {
        ResetHighlight();
	}

    public void ResetHighlight()
    {
        i = GetComponent<Image>();
        c = i.color;
        a = c.a;
        fadingIn = true;
        fadingOut = false;
    }
	
	void Update ()
    {
        if (fadingIn)
        {
            a += Time.deltaTime * fadeInSpeed;
            if (a >= 1)
            {
                fadingIn = false;
                fadingOut = true;
            }
        }
        if (fadingOut)
        {
            a -= Time.deltaTime * fadeOutSpeed;
            if (a <= 0)
                this.gameObject.SetActive(false);
        }
        i.color = new Color(c.r, c.g, c.b, a);
	}
}
