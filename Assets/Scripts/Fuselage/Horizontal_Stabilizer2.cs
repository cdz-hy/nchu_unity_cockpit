using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horizontal_Stabilizer2 : MonoBehaviour
{
    private float screenHeight;
    private float screenWidth;
    private float previousMouseY;
    private bool isInTopHalf;
    private float initialRotation;  // 记录初始旋转角度
    private float targetRotation;   // 目标旋转角度
    
    // 旋转角度限制
    private const float MAX_ROTATION = 15f;
    private const float MIN_ROTATION = -15f;
    
    // 中线区域的高度范围（上下各50像素）
    private const float MIDDLE_ZONE = 50f;

    // 旋转速度系数（值越小旋转越慢）
    private const float ROTATION_SPEED = 0.3f;

    // 最大旋转速度（每帧最大旋转角度）
    private const float MAX_ROTATION_SPEED = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        // 获取屏幕尺寸
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        previousMouseY = Input.mousePosition.y;

        // 记录初始X轴旋转角度
        initialRotation = transform.localEulerAngles.x;
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
        
        // 检查鼠标是否在屏幕上半部分
        isInTopHalf = mousePos.y > screenHeight / 2;

        // 检查是否在中线区域
        bool isInMiddleZone = Mathf.Abs(mousePos.y - screenHeight / 2) < MIDDLE_ZONE;

        // 获取当前X轴旋转角度
        float currentXRotation = transform.localEulerAngles.x;
        if (currentXRotation > 180)
        {
            currentXRotation -= 360;
        }

        if (isInMiddleZone)
        {
            // 在中线区域时，逐渐回正到初始角度
            if (Mathf.Abs(currentXRotation - initialRotation) > 0.1f)
            {
                float resetSpeed = 2f; // 回正的速度
                float resetAmount = -(currentXRotation - initialRotation) * Time.deltaTime * resetSpeed;
                transform.Rotate(resetAmount, 0, 0, Space.Self);
                targetRotation = initialRotation;
            }
        }
        else
        {
            // 计算鼠标Y轴移动差值
            float deltaY = mousePos.y - previousMouseY;
            
            // 如果鼠标移动了，则更新目标旋转角度
            if (deltaY != 0)
            {
                // 计算旋转方向：上半部分鼠标向上(正deltaY)物体向上转(正旋转)
                // 下半部分鼠标向上(正deltaY)物体向上转(正旋转)
                float rotationAmount = deltaY * ROTATION_SPEED;
                
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
        if (Mathf.Abs(currentXRotation - targetRotation) > 0.1f)
        {
            float direction = Mathf.Sign(targetRotation - currentXRotation);
            float rotationAmount = direction * MAX_ROTATION_SPEED;
            
            // 如果剩余距离小于最大旋转速度，则直接旋转到目标角度
            if (Mathf.Abs(targetRotation - currentXRotation) < MAX_ROTATION_SPEED)
            {
                rotationAmount = targetRotation - currentXRotation;
            }
            
            transform.Rotate(rotationAmount, 0, 0, Space.Self);
        }

        // 更新前一帧的鼠标Y位置
        previousMouseY = mousePos.y;
    }
}

