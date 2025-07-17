using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractFrontWheels : MonoBehaviour
{
    public int isKeyPressed = 1;
    private float targetRotationX = 110f; // 目标旋转角度
    private float speed = 70f; // 旋转速度
    private Quaternion targetRotation; // 目标四元数旋转
    private Quaternion originalRotation; // 原始旋转四元数
    private bool isReturning = false; // 标记协程是否正在运行

    void Start()
    {
        originalRotation = transform.localRotation; // 存储原始旋转
        // 设置目标四元数旋转，只改变X轴旋转，保持其他轴不变
        targetRotation = Quaternion.Euler(targetRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void Update()
    {
        if (isKeyPressed == 2)
        {
            // 使用 Quaternion.RotateTowards 来平滑过渡到目标角度
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(ReturnToOriginalPosition(2f));
            if (isReturning) // 只有在协程未运行时才启动
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalRotation, speed * Time.deltaTime);
                isReturning = false;
            }
        }
    }

    private IEnumerator ReturnToOriginalPosition(float delay)
    {
        yield return new WaitForSeconds(delay);

        isReturning = true;
    }
}
