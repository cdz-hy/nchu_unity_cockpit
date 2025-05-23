using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizon_above_Standby : MonoBehaviour
{
    public float rotationSpeed = 360f; // 每秒旋转的角度  
    public float roll;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        roll = 0;
    }
    void Update()
    {
        // 绕 Z 轴旋转  
        Control(roll);
        //roll = DataCenter.Instance.Roll;
    }
    void Control(float angle)
    {
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, angle);
        StartCoroutine(Move(initialPosition, targetRotation));
    }

    public IEnumerator Move(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * rotationSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // 确保完成后设置到目标位置  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}