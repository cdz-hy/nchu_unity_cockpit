using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWheel_3 : MonoBehaviour
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 5, 121), 0.5f * Time.deltaTime);
        if (isKeyPressed == 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 5, 0), 0.5f * Time.deltaTime);
    }
}
