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
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, -0.000132f, 0.001195f), 0.4f/7 + 0.03f);
        if (isKeyPressed == 1)
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0.0007199239f, 0.0008676908f), 0.4f/7 + 0.03f);
    }
}
