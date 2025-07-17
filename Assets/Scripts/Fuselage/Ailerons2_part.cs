using UnityEngine;

public class Ailerons2_part : MonoBehaviour
{
    private float initialRotation;  // 初始旋转角度
    
    // 旋转参数配置
    [SerializeField] private float maxRotation = 15f;
    [SerializeField] private float rotationSmoothness = 5f;
    
    void Start()
    {
        // 记录初始X轴旋转角度并规范化到[-180, 180]范围
        initialRotation = NormalizeAngle(transform.localEulerAngles.x);
    }

    void Update()
    {
        // 获取DataCenter中的rollControl值（范围-1到1）
        float rollControl = DataCenter.Instance.rollControl;
        
        // 计算目标旋转角度（基于初始角度）
        // rollControl为-1时对应最小旋转角度，为1时对应最大旋转角度
        float targetRotation = initialRotation + rollControl * maxRotation;
        
        // 获取当前规范化后的旋转角度
        float currentRotation = NormalizeAngle(transform.localEulerAngles.x);
        
        // 使用平滑插值计算新的旋转角度
        float newRotation = Mathf.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSmoothness);
        
        // 应用旋转（仅修改X轴）
        Vector3 eulerAngles = transform.localEulerAngles;
        eulerAngles.x = newRotation;
        transform.localEulerAngles = eulerAngles;
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

