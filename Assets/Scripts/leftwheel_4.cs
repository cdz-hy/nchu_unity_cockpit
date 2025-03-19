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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 35), 3f * Time.deltaTime);
        if (isKeyPressed == 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 3f * Time.deltaTime);
    }
}
