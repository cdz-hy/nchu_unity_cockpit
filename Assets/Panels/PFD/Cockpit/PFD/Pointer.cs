using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float rotationSpeed = 360f; // 每秒旋转的角度  
    public float pointer;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }
    void Update()
    {
        // 绕 Z 轴旋转  
        Control(pointer);
        pointer += 0.1f;
    }
    void Control(float angle)
    {      
        if(angle > 20)
        {
            angle = 20;
            angle = angle * 9;
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, angle);
            StartCoroutine(Move(initialPosition, targetRotation));
        }
        else
        {
            angle = angle * 9;
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, angle);
            StartCoroutine(Move(initialPosition, targetRotation));
        }
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