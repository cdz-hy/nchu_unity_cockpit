// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Ailerons1 : MonoBehaviour
// {
//     private float screenHeight;
//     private float screenWidth;
//     private float previousMouseX;
//     private bool isInLeftHalf;
//     private float initialRotation;  // 记录初始旋转角度
//     private float targetRotation;   // 目标旋转角度
    
//     // 旋转角度限制
//     private const float MAX_ROTATION = 15f;
//     private const float MIN_ROTATION = -15f;
    
//     // 中线区域的宽度范围（左右各50像素）
//     private const float MIDDLE_ZONE = 50f;

//     // 旋转速度系数（值越小旋转越慢）
//     private const float ROTATION_SPEED = 0.05f;

//     // 最大旋转速度（每帧最大旋转角度）
//     private const float MAX_ROTATION_SPEED = 0.17f;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // 获取屏幕尺寸
//         screenHeight = Screen.height;
//         screenWidth = Screen.width;
//         previousMouseX = Input.mousePosition.x;

//         // 记录初始X轴旋转角度
//         initialRotation = transform.localEulerAngles.x;
//         if (initialRotation > 180)
//         {
//             initialRotation -= 360;
//         }
//         targetRotation = initialRotation;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // 获取鼠标位置
//         Vector3 mousePos = Input.mousePosition;
        
//         // 检查鼠标是否在屏幕左半部分
//         isInLeftHalf = mousePos.x < screenWidth / 2;

//         // 检查是否在中线区域
//         bool isInMiddleZone = Mathf.Abs(mousePos.x - screenWidth / 2) < MIDDLE_ZONE;

//         // 获取当前X轴旋转角度并确保在正确的范围内
//         float currentXRotation = transform.localEulerAngles.x;
//         if (currentXRotation > 180)
//         {
//             currentXRotation -= 360;
//         }

//         // 计算相对于初始角度的偏移量
//         float relativeRotation = currentXRotation - initialRotation;

//         if (isInMiddleZone)
//         {
//             // 在中线区域时，逐渐回正到初始角度
//             if (Mathf.Abs(relativeRotation) > 0.1f)
//             {
//                 float resetSpeed = 2f; // 回正的速度
//                 float resetAmount = -relativeRotation * Time.deltaTime * resetSpeed;
//                 transform.Rotate(resetAmount, 0, 0, Space.Self);
//                 targetRotation = initialRotation;
//             }
//         }
//         else if (isInLeftHalf) // 只在左半部分响应鼠标移动
//         {
//             // 计算鼠标X轴移动差值
//             float deltaX = mousePos.x - previousMouseX;
            
//             // 如果鼠标左右移动了，则更新目标旋转角度
//             if (deltaX != 0)
//             {
//                 // 在左半部分，向右移动时向上转（负deltaX会使物体向上转）
//                 float rotationAmount = deltaX * ROTATION_SPEED;
                
//                 // 更新目标旋转角度
//                 float newTargetRotation = targetRotation + rotationAmount;
//                 if (newTargetRotation >= MIN_ROTATION && newTargetRotation <= MAX_ROTATION)
//                 {
//                     targetRotation = newTargetRotation;
//                 }
//             }
//         }

//         // 如果当前角度与目标角度不同，则继续旋转
//         if (Mathf.Abs(currentXRotation - targetRotation) > 0.1f)
//         {
//             float direction = Mathf.Sign(targetRotation - currentXRotation);
//             float rotationAmount = direction * MAX_ROTATION_SPEED;
            
//             // 如果剩余距离小于最大旋转速度，则直接旋转到目标角度
//             if (Mathf.Abs(targetRotation - currentXRotation) < MAX_ROTATION_SPEED)
//             {
//                 rotationAmount = targetRotation - currentXRotation;
//             }
            
//             transform.Rotate(rotationAmount, 0, 0, Space.Self);
//         }

//         // 更新前一帧的鼠标X位置
//         previousMouseX = mousePos.x;
//     }
// }



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Ailerons1 : MonoBehaviour
// {
//     // 旋转角度限制
//     private const float MAX_ROTATION = 20f;
//     private const float MIN_ROTATION = -20f;

//     // 最大旋转速度（每帧最大旋转角度）
//     private const float MAX_ROTATION_SPEED = 0.2f;

//     // 当前目标旋转角度增量
//     private float targetRotationDelta;

//     // Start is called before the first frame update
//     void Start()
//     {
//         // 初始化目标旋转角度增量为 0
//         targetRotationDelta = 0f;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         // 获取 DataCenter 的输入值
//         float rollInput = DataCenter.Instance.rollControl; // 范围 -1 到 1
//         float pitchInput = DataCenter.Instance.pitchControl; // 范围 -1 到 1

//         // 将 rollInput 映射到旋转角度范围
//         float rotationDelta = Mathf.Lerp(MIN_ROTATION, MAX_ROTATION, (rollInput + 1) / 2);

//         // 更新目标旋转角度增量
//         targetRotationDelta = rotationDelta;

//         // 如果目标角度增量不为 0，则继续旋转
//         if (Mathf.Abs(targetRotationDelta) > 0.1f)
//         {
//             float direction = Mathf.Sign(targetRotationDelta);
//             float rotationAmount = direction * MAX_ROTATION_SPEED;

//             // 如果剩余距离小于最大旋转速度，则直接旋转到目标角度
//             if (Mathf.Abs(targetRotationDelta) < MAX_ROTATION_SPEED)
//             {
//                 rotationAmount = targetRotationDelta;
//             }

//             // 在局部空间中应用旋转
//             transform.Rotate(rotationAmount, 0, 0, Space.Self);

//             // 更新目标旋转角度增量
//             targetRotationDelta -= rotationAmount;
//         }
//     }
// }


using UnityEngine;

public class Ailerons1 : MonoBehaviour
{
    private float initialRotation;  // 初始旋转角度
    private float targetRotation;   // 目标旋转角度
    
    // 旋转参数配置
    private const float MAX_ROTATION = 15f;
    private const float ROTATION_SMOOTHING = 8f;
    private const float ROTATION_DEADZONE = 0.1f;

    void Start()
    {
        // 记录初始X轴旋转角度并规范化到[-180, 180]范围
        initialRotation = NormalizeAngle(transform.localEulerAngles.x);
        targetRotation = initialRotation;
    }

    void Update()
    {
        // 获取滚转控制输入（-1到1）
        float rollControl = DataCenter.Instance.rollControl;

        // 计算目标旋转角度（基于初始角度）
        float newTarget = initialRotation + rollControl * MAX_ROTATION;
        
        // 使用线性插值平滑过渡
        targetRotation = Mathf.Lerp(targetRotation, newTarget, Time.deltaTime * ROTATION_SMOOTHING);

        // 获取当前规范化后的旋转角度
        float currentRot = NormalizeAngle(transform.localEulerAngles.x);

        // 当角度差值大于死区时执行旋转
        if (Mathf.Abs(currentRot - targetRotation) > ROTATION_DEADZONE)
        {
            // 计算旋转差值
            float rotationDelta = targetRotation - currentRot;
            
            // 应用旋转（仅修改X轴）
            transform.localRotation = Quaternion.Euler(
                currentRot + rotationDelta * Time.deltaTime * ROTATION_SMOOTHING,
                transform.localEulerAngles.y,
                transform.localEulerAngles.z
            );
        }
    }

    // 角度规范化方法
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) 
            angle -= 360f;
        return angle;
    }
}