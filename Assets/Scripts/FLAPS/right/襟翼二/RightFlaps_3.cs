using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightFlaps_3 : MonoBehaviour
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.175f, 0.022f, 0.471f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(5.792f,- 2.317f, 0.401f), Time.deltaTime);
        }
        if (flaps == 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.179f, 0.019f, 0.50275f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(9.434f, -2.817f, 0.35f), Time.deltaTime);
        }
        if (flaps == 2)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.181f, 0.016f, 0.5345f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(13.076f,- 3.317f, 0.30f), Time.deltaTime);
        }
        if (flaps == 3)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.183f, 0.013f, 0.56625f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(16.718f,- 3.817f,0.25f), Time.deltaTime);
        }
        if (flaps == 4)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.185f, 0.012f, 0.598f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(20.36f, -4.317f,0.20f), Time.deltaTime);
        }
        if (flaps == 5)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.187f, 0.011f, 0.62975f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(24.002f, -4.817f, 0.15f), Time.deltaTime);
        }
        if (flaps == 6)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.189f, 0.007f, 0.6915f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(27.644f,- 5.317f, 0.10f), Time.deltaTime);
        }
        if (flaps == 7)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.191f, 0.001f, 0.72325f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(31.286f,- 5.817f, 0.5f), Time.deltaTime);
        }
        if (flaps == 8)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.193f, 0f, 0.725f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(34.93f,- 6.337f, 0), Time.deltaTime);
        }

    }
}
