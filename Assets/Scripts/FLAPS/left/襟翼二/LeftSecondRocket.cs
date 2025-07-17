using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Left : MonoBehaviour
{
    private int flaps = 0;
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (flaps < 8)
                flaps++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (flaps > 0)
                flaps--;
        }
        if (flaps == 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1f, 0, 0), Time.deltaTime);
        }
        if (flaps == 1)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(5.625f, 0, 0), Time.deltaTime);
        }
        if (flaps == 2)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(6.25f, 0, 0), Time.deltaTime);
        }
        if (flaps == 3)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(8.875f, 0, 0), Time.deltaTime);
        }
        if (flaps == 4)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(11.5f, 0, 0), Time.deltaTime);
        }
        if (flaps == 5)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(14.125f, 0, 0), Time.deltaTime);
        }
        if (flaps == 6)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(16.75f, 0, 0), Time.deltaTime);
        }
        if (flaps == 7)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(19.375f, 0, 0), Time.deltaTime);
        }
        if (flaps == 8)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(22f, 0, 0), Time.deltaTime);
        }

    }
}
