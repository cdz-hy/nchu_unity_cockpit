using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWheel_4 : MonoBehaviour
{
    public int isKeyPressed = 1;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeyPressed == 2)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1.185f, -9.027f, 158.719f), Time.deltaTime);
        if (isKeyPressed == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
    }
}
