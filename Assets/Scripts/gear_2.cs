using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear_2 : MonoBehaviour
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
            transform.position = Vector3.Lerp(transform.position, new Vector3(-0.1667627f, 0.28f, 1.837f), 0.4f/7 + 0.005f);
        if (isKeyPressed == 1)
            transform.position = Vector3.Lerp(transform.position, new Vector3(-0.1667627f, 0.36f, 1.806424f), 0.4f/7 + 0.005f);
    }
}
