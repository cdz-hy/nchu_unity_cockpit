using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRotation1 : MonoBehaviour
{

    // 最大旋转速度（单位：度/秒）
    public float maxRotationSpeed = 360f; // 每秒旋转360度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检查 DataCenter 是否初始化
        if (DataCenter.Instance == null)
        {
            Debug.LogWarning("DataCenter未初始化！");
            return;
        }

        // 获取 ThrottleLever 值并限制其范围为 [0, 1]
        float throttleLever = Mathf.Clamp01(DataCenter.Instance.throttleLever1);

        // 将 ThrottleLever 映射到旋转速度（0 对应 0 度/秒，1 对应 maxRotationSpeed 度/秒）
        float rotationSpeed = throttleLever * maxRotationSpeed;

        // 计算当前帧的旋转角度（基于 Time.deltaTime 确保帧率无关）
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // 绕 Z 轴旋转
        transform.Rotate(0, 0, rotationAngle, Space.Self);
    }
}
