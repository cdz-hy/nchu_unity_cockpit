using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gear_2 : MonoBehaviour
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
        if (isKeyPressed == 1)
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.165f, 0.35f, 1.8f), 0.4f/7 + 0.005f);
        if (isKeyPressed == 2)
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.165f, 0.28f, 1.83f), 0.4f/7 + 0.005f);
    }
}
