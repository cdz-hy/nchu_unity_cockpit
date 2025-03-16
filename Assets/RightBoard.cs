using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBoard : MonoBehaviour
{
    private Quaternion targetRotation;  // 目标旋转，(0, 0, 0)
    public float rotationSpeed = 0.05f;  // 旋转速度
    private bool startRotation = false;  // 控制是否开始旋转

    void Start()
    {
        // 初始化目标旋转为 (0, 0, 0)
        targetRotation = Quaternion.Euler(0f, 0f, 0f);

        // 启动协程，等待 2 秒后开始旋转
        StartCoroutine(StartRotationAfterDelay(1f));
    }

    void Update()
    {
        // 如果已经等待了 2 秒，就开始旋转
        if (startRotation)
        {
            // 使用 Quaternion.RotateTowards 来平滑过渡到目标旋转
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 协程：等待指定时间后开始旋转
    private IEnumerator StartRotationAfterDelay(float delay)
    {
        // 等待指定的时间（延迟 2 秒）
        yield return new WaitForSeconds(delay);

        // 延迟后，设置开始旋转
        startRotation = true;
    }
}
