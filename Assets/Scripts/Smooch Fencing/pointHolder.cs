using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pointHolder : MonoBehaviour
{
    private Image i;
    public Sprite s1, s2, s3, s4, s5;
    
	void Start ()
    {
        i = GetComponent<Image>();

        if (gameOptions.ptw == 1)
            i.sprite = s1;
        if (gameOptions.ptw == 2)
            i.sprite = s2;
        if (gameOptions.ptw == 3)
            i.sprite = s3;
        if (gameOptions.ptw == 4)
            i.sprite = s4;
        if (gameOptions.ptw == 5)
            i.sprite = s5;
	}
}
