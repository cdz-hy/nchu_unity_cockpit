using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftFlaps_1 : MonoBehaviour
{
    // Start is called before the first frame update
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
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.08f, -0.307f, 21.15f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, -19.584f, 0), Time.deltaTime);
        }
        if (flaps == 1)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.08325f, -0.305625f, 21.1935f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(3.0115f, -19.21775f, 0.041125f), Time.deltaTime);
        }
        if (flaps == 2)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.0865f, -0.30425f, 21.237f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(6.023f, -18.8515f, 0.08225f), Time.deltaTime);
        }
        if (flaps == 3)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.08975f, -0.302875f, 21.2805f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(9.0345f, -18.48525f, 0.123375f), Time.deltaTime);
        }
        if (flaps == 4)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.093f, -0.3015f, 21.324f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(12.046f, -18.119f, 0.1645f), Time.deltaTime);
        }
        if (flaps == 5)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.09625f, -0.300125f, 21.3675f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(15.0575f, -17.75275f, 0.205625f), Time.deltaTime);
        }
        if (flaps == 6)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.0995f, -0.29875f, 21.411f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(18.069f, -17.3865f, 0.24675f), Time.deltaTime);
        }
        if (flaps == 7)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.10275f, -0.297375f, 21.4545f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(21.0805f, -17.02025f, 0.287875f), Time.deltaTime);
        }
        if (flaps == 8)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(8.106f, -0.296f, 21.498f), Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(24.092f, -16.654f, 0.329f), Time.deltaTime);
        }
            
    }

}

