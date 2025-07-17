using UnityEngine;

public class Spoiler4 : MonoBehaviour
{
    private float initialRotation;  // 初始旋转角度
    
    // 旋转参数配置
    [SerializeField] private float maxRotation = 50f;
    [SerializeField] private float rotationSpeed = 0.5f;  // 最大旋转速度
    
    void Start()
    {
        // 记录初始X轴旋转角度并规范化到[-180, 180]范围
        initialRotation = NormalizeAngle(transform.localEulerAngles.x);
    }

    void Update()
    {
        // 获取DataCenter中的rollControl值（范围-1到1）
        float rollControl = DataCenter.Instance.rollControl;
        
        // 将rollControl映射到0到1范围（只在rollControl为正值时激活扰流板）
        float mappedControl = Mathf.Max(rollControl, 0);
        
        // 计算目标旋转角度（基于初始角度）
        float targetRotation = initialRotation - mappedControl * maxRotation;
        
        // 获取当前规范化后的旋转角度
        float currentXRotation = NormalizeAngle(transform.localEulerAngles.x);
        
        // 如果当前角度与目标角度不同，则继续旋转
        if (Mathf.Abs(currentXRotation - targetRotation) > 0.1f)
        {
            // 计算旋转方向和旋转量
            float direction = Mathf.Sign(targetRotation - currentXRotation);
            float rotationAmount = direction * rotationSpeed;
            
            // 如果剩余距离小于最大旋转速度，则直接旋转到目标角度
            if (Mathf.Abs(targetRotation - currentXRotation) < rotationSpeed)
            {
                rotationAmount = targetRotation - currentXRotation;
            }
            
            // 使用transform.Rotate方法应用旋转
            transform.Rotate(rotationAmount, 0, 0, Space.Self);
        }
    }

    // 角度规范化方法，将角度转换到[-180, 180]范围
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) 
            angle -= 360f;
        return angle;
    }
}

