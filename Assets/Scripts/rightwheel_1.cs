using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightwheel_1 : MonoBehaviour
{
    // Start is called before the first frame update
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(1, 5, 79), Time.deltaTime);
        if (isKeyPressed == 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(1, 5, 0), Time.deltaTime);
    }
}
