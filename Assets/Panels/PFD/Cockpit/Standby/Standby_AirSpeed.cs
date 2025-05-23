using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standby_Airspeed : MonoBehaviour
{
    public float MoveSpeed = 360f; // 每秒旋转的角度  
    public float airSpeed;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        //airSpeed = DataCenter.Instance.AirSpeed;
        //airSpeed++;
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        Control(airSpeed);
        //绕 Z 轴旋转
        if (airSpeed < 460 && airSpeed > 45)
        {
            Control(airSpeed);
        }
        else if (airSpeed > 460)
        {
            Control(460f);
        }
        else if (airSpeed < 45)
        {
            Control(45f);
        }

    }

    void Control(float speed)
    {
        speed -= 40f;
        speed *= -0.00059f;
        Vector3 targetPosition = initialPosition + new Vector3(0, speed, 0);
        StartCoroutine(Move(targetPosition, initialRotation));
    }

    public IEnumerator Move(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * MoveSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // 确保完成后设置到目标位置  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}

