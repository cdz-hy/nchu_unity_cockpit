using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWheel_1 : MonoBehaviour
{
    // Start is called before the first frame update
    public int isKeyPressed = 1;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (isKeyPressed == 2)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1, 5, 79), Time.deltaTime);
        if (isKeyPressed == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1, 5, 0), Time.deltaTime);
    }
}
