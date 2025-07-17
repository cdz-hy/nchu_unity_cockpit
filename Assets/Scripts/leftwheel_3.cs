using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWheel_3 : MonoBehaviour
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
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 5, -45), 0.5f*Time.deltaTime);
        if (isKeyPressed == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 5, 0), 0.5f*Time.deltaTime);
    }
}
