using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWheel_1 : MonoBehaviour
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(1, -5, 281), Time.deltaTime);
        if (isKeyPressed == 1)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(1, -5, 0), Time.deltaTime);
    }
}
