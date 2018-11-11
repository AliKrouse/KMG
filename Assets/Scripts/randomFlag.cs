using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class randomFlag : MonoBehaviour
{
    public bool UI;
    private Image i;
    private SpriteRenderer sr;
    public Sprite[] flags;
    private int r;
    
	void Start ()
    {
        r = Random.Range(0, flags.Length);

        if (UI)
        {
            i = GetComponent<Image>();
            i.sprite = flags[r];
        }
        if (!UI)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.sprite = flags[r];
        }
	}
}
