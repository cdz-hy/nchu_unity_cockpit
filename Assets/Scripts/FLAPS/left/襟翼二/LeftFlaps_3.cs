using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFlaps_3 : MonoBehaviour
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00175f, 0.00022f, 0.00471f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(5.792f, 2.317f, 0.401f), Time.deltaTime);
        }
        if (flaps == 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00179f, 0.00019f, 0.0050275f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(9.434f, 2.817f, 0.5325f), Time.deltaTime);
        }
        if (flaps == 2)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00181f, 0.00016f, 0.005345f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(13.076f, 3.317f, 0.665f), Time.deltaTime);
        }
        if (flaps == 3)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00183f, 0.00013f, 0.0056625f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(16.718f, 3.817f, 0.7975f), Time.deltaTime);
        }
        if (flaps == 4)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00185f, 0.00010f, 0.00598f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(20.36f, 4.317f, 0.7975f), Time.deltaTime);
        }
        if (flaps == 5)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00187f, 0.00007f, 0.0062975f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(24.002f, 4.817f, 0.7975f), Time.deltaTime);
        }
        if (flaps == 6)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00189f, 0.00004f, 0.006615f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(27.644f, 5.317f, 0.7975f), Time.deltaTime);
        }
        if (flaps == 7)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00191f, 0.00001f, 0.0069325f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(31.286f, 5.817f, 1.3275f), Time.deltaTime);
        }
        if (flaps == 8)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.00193f, 0f, 0.00725f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(34.93f, 6.337f, 1.46f), Time.deltaTime);
        }

    }
}
