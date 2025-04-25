using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFirstRocket : MonoBehaviour
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
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0.5f, 0, 0), Time.deltaTime);
        if (flaps == 1)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(2.875f, 0, 0), Time.deltaTime);
        if (flaps == 2)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(5.25f, 0, 0), Time.deltaTime);
        if (flaps == 3)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(7.625f, 0, 0), Time.deltaTime);
        if (flaps == 4)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(10f, 0, 0), Time.deltaTime);
        if (flaps == 5)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(12.375f, 0, 0), Time.deltaTime);
        if (flaps == 6)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(14.75f, 0, 0), Time.deltaTime);
        if (flaps == 7)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(17.125f, 0, 0), Time.deltaTime);
        if (flaps == 8)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(19.5f, 0, 0), Time.deltaTime);

    }
}
