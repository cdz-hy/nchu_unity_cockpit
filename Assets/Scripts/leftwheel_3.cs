using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftwheel_3 : MonoBehaviour
{
    private int isKeyPressed = 1;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            if (isKeyPressed == 1)
                isKeyPressed = 2;
            else if (isKeyPressed == 2)
                isKeyPressed = 1;
        }
        if (isKeyPressed == 2)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 5, 236), 0.5f*Time.deltaTime);
        if (isKeyPressed == 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 5, 0), 0.5f*Time.deltaTime);
    }
}
