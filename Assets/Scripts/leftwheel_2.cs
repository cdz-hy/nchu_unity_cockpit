using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWheel_2 : MonoBehaviour
{
    public int isKeyPressed = 1;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeyPressed == 2)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
        if (isKeyPressed == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-7, 0, 79), Time.deltaTime);
    }

}

