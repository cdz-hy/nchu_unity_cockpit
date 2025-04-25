using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightFlaps_5 : MonoBehaviour
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003761465f, 0.004607658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);
        }
        if (flaps == 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003731465f, 0.004907658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0.25f,0.07f, 0.03f), Time.deltaTime);
        }
        if (flaps == 2)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003671465f, 0.005207658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0.5f, 0.14f, 0.07f), Time.deltaTime);
        }
        if (flaps == 3)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003651465f, 0.005507658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0.75f, 0.20f, 0.11f), Time.deltaTime);
        }
        if (flaps == 4)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003631465f, 0.005807658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1, 0.27f, 0.15f), Time.deltaTime);
        }
        if (flaps == 5)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003601465f, 0.006107658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1.25f, 0.33f, 0.19f), Time.deltaTime);
        }
        if (flaps == 6)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003571465f, 0.006407658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1.5f, 0.39f, 0.23f), Time.deltaTime);
        }
        if (flaps == 7)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.001983722f, 0.003541465f, 0.006707658f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(1.75f, 0.45f, 0.27f), Time.deltaTime);
        }
        if (flaps == 8)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-0.00199f, 0.00361f, 0.00707f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(2.031f, 0.52f, 0.309f), Time.deltaTime);
        }

    }
}
