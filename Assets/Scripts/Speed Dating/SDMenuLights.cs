using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDMenuLights : MonoBehaviour
{
    private RectTransform rt;

    public float maxRot, minRot, newRot, speed;
    public float n;
    
	void Start ()
    {
        rt = GetComponent<RectTransform>();

        MakeRotation();
	}
	
	void Update ()
    {
        if (newRot > n)
        {
            n += Time.deltaTime * speed;
        }
        if (newRot < n)
        {
            n -= Time.deltaTime * speed;
        }
        rt.rotation = Quaternion.Euler(rt.rotation.x, rt.rotation.y, n);
        
        if (Mathf.Abs(n - newRot) < 2)
            MakeRotation();
    }

    void MakeRotation()
    {
        newRot = Random.Range(minRot, maxRot);
    }
}
