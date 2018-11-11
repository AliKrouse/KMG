using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSFArrows : MonoBehaviour
{
    public float rotationAmmount, speed;
    private float currentRot;
    private bool rotateLeft, rotateRight;

	void Start ()
    {
        currentRot = transform.rotation.eulerAngles.z;
        rotateLeft = true;
	}
	
	void Update ()
    {
        if (rotateLeft)
        {
            if (currentRot < rotationAmmount)
            {
                currentRot += Time.deltaTime * speed;
            }
            if (currentRot >= rotationAmmount)
            {
                rotateLeft = false;
                rotateRight = true;
            }
        }
        if (rotateRight)
        {
            if (currentRot > -rotationAmmount)
            {
                currentRot -= Time.deltaTime * speed;
            }
            if (currentRot <= -rotationAmmount)
            {
                rotateRight = false;
                rotateLeft = true;
            }
        }

        transform.eulerAngles = new Vector3(0, 0, currentRot);
	}
}
