using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    // 可选：旋转平滑速度
    public float smoothSpeed = 2f;

    void Update()
    {
        // 确保DataCenter已初始化
        if (DataCenter.Instance != null)
        {
            // 从DataCenter获取最新数据
            float pitch = DataCenter.Instance.pitchAngle;
            float roll = DataCenter.Instance.rollAngle;
            float yaw = DataCenter.Instance.rotationAngle; // 这里假定rotationAngle作为yaw

            // 根据数据构造目标旋转（可根据需要调整轴对应关系）
            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, roll);

            // 可选：平滑过渡到目标旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
    }
}
