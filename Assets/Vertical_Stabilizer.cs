using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertical_Stabilizer : MonoBehaviour
{
    private float screenHeight;
    private float screenWidth;
    private float previousMouseX;  // 改为跟踪鼠标X坐标
    private bool isInRightHalf;    // 改为检测左右半部分
    private float initialRotation;  // 记录初始旋转角度
    private float targetRotation;   // 目标旋转角度
    
    // 旋转角度限制
    private const float MAX_ROTATION = 15f;
    private const float MIN_ROTATION = -15f;
    
    // 中线区域的宽度范围（左右各50像素）
    private const float MIDDLE_ZONE = 50f;

    // 旋转速度系数（值越小旋转越慢）
    private const float ROTATION_SPEED = 0.05f;

    // 最大旋转速度（每帧最大旋转角度）
    private const float MAX_ROTATION_SPEED = 0.09f;

    // Start is called before the first frame update
    void Start()
    {
        // 获取屏幕尺寸
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        previousMouseX = Input.mousePosition.x;  // 记录鼠标X坐标

        // 记录初始Y轴旋转角度
        initialRotation = transform.localEulerAngles.y;
        if (initialRotation > 180)
        {
            initialRotation -= 360;
        }
        targetRotation = initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标位置
        Vector3 mousePos = Input.mousePosition;
        
        // 检查鼠标是否在屏幕右半部分
        isInRightHalf = mousePos.x > screenWidth / 2;

        // 检查是否在中线区域
        bool isInMiddleZone = Mathf.Abs(mousePos.x - screenWidth / 2) < MIDDLE_ZONE;

        // 获取当前Y轴旋转角度
        float currentYRotation = transform.localEulerAngles.y;
        if (currentYRotation > 180)
        {
            currentYRotation -= 360;
        }

        if (isInMiddleZone)
        {
            // 在中线区域时，逐渐回正
            if (Mathf.Abs(currentYRotation - initialRotation) > 0.1f)
            {
                float resetSpeed = 2f; // 回正的速度
                float resetAmount = -(currentYRotation - initialRotation) * Time.deltaTime * resetSpeed;
                transform.Rotate(0, resetAmount, 0, Space.Self);
                targetRotation = initialRotation;
            }
        }
        else
        {
            // 计算鼠标X轴移动差值
            float deltaX = mousePos.x - previousMouseX;
            
            // 如果鼠标移动了，则更新目标旋转角度
            if (deltaX != 0)
            {
                // 计算旋转方向：右半部分鼠标向右(正deltaX)物体向右转(正旋转)
                // 左半部分鼠标向右(正deltaX)物体向右转(正旋转)
                float rotationAmount = -deltaX * ROTATION_SPEED;
                
                // 更新目标旋转角度
                float newTargetRotation = targetRotation + rotationAmount;
                // 检查是否会超出限制
                if (newTargetRotation >= MIN_ROTATION && newTargetRotation <= MAX_ROTATION)
                {
                    targetRotation = newTargetRotation;
                }
            }
        }

        // 如果当前角度与目标角度不同，则继续旋转
        if (Mathf.Abs(currentYRotation - targetRotation) > 0.1f)
        {
            float direction = Mathf.Sign(targetRotation - currentYRotation);
            float rotationAmount = direction * MAX_ROTATION_SPEED;
            
            // 如果剩余距离小于最大旋转速度，则直接旋转到目标角度
            if (Mathf.Abs(targetRotation - currentYRotation) < MAX_ROTATION_SPEED)
            {
                rotationAmount = targetRotation - currentYRotation;
            }
            
            transform.Rotate(0, rotationAmount, 0, Space.Self);
        }

        // 更新前一帧的鼠标X位置
        previousMouseX = mousePos.x;

        // Debug绘制屏幕分区线（仅在Scene视图中可见）
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(screenWidth/2, 0, 10)), 
                      Camera.main.ScreenToWorldPoint(new Vector3(screenWidth/2, screenHeight, 10)), Color.red);
    }
}

