using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gear_1 : MonoBehaviour
{
    private int isKeyPressed = 1;
    // Start is called before the first frame update
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(40, 0, 0), 0.1f);
        if (isKeyPressed == 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.1f);
    }
}
