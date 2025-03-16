using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheel_2 : MonoBehaviour
{

    private float targetRotationX = -53f; // 目标旋转角度
    private float speed = 100f; // 旋转速度
    private Quaternion targetRotation; // 目标四元数旋转

    void Start()
    {
        // 设置目标四元数旋转，只改变X轴旋转，保持其他轴不变
        targetRotation = Quaternion.Euler(targetRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void Update()
    {
        // 如果当前的旋转角度与目标旋转角度不同，则逐步旋转
        if (transform.localRotation != targetRotation)
        {
            // 使用 Quaternion.RotateTowards 来平滑过渡到目标角度
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, speed * Time.deltaTime);
        }
    }
}
