using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightFlaps_4 : MonoBehaviour
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.857f, -1.513f, 20.299f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0.2f), Time.deltaTime);
        }
        if (flaps == 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.857f, -1.513f, 20.32f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(2.9585f, -0.4785f, 0.16f), Time.deltaTime);
        }
        if (flaps == 2)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.857f, -1.513f, 20.34f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(5.917f, -0.957f,0.12f), Time.deltaTime);
        }
        if (flaps == 3)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.86f, -1.516f, 20.37f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(8.8755f, -1.4355f, 0.08f), Time.deltaTime);
        }
        if (flaps == 4)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.86f, -1.516f, 20.40f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(11.834f, -1.914f, 0.04f), Time.deltaTime);
        }
        if (flaps == 5)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.865f, -1.518f, 20.43f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(14.7925f, -2.3925f, -0.02f), Time.deltaTime);
        }
        if (flaps == 6)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.865f, -1.518f, 20.46f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(17.751f, -2.871f, -0.06f), Time.deltaTime);
        }
        if (flaps == 7)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.865f, -1.518f, 20.49f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(20.7095f,- 3.3495f, -0.12f), Time.deltaTime);
        }
        if (flaps == 8)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-1.87f, -1.52f, 20.52f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(23.668f, -3.828f,- 0.157f), Time.deltaTime);
        }

    }
}
