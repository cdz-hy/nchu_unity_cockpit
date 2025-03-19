using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBoard : MonoBehaviour
{
    private Quaternion targetRotation;  // 目标旋转，(0, 0, 0)
    private Quaternion originalRotation; // 原始旋转四元数
    public float rotationSpeed = 20f;  // 旋转速度
    public int isKeyPressed = 1;

    void Start()
    {
        originalRotation = transform.localRotation; // 存储原始旋转
        // 初始化目标旋转为 (0, 0, 0)
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        if (isKeyPressed == 2)
        {
            // 使用 Quaternion.RotateTowards 来平滑过渡到目标旋转
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // 将物体旋转回原始位置
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
