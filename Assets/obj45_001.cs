using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj45_001 : MonoBehaviour
{
    private float screenHeight;
    private float screenWidth;
    private float previousMouseX;
    private bool isInLeftHalf;
    private float initialRotation;  // 记录初始旋转角度
    
    // 旋转角度限制
    private const float MAX_ROTATION = 15f;
    private const float MIN_ROTATION = -15f;
    
    // 中线区域的宽度范围（左右各50像素）
    private const float MIDDLE_ZONE = 50f;

    // 旋转速度系数（值越小旋转越慢）
    private const float ROTATION_SPEED = 0.13f;

    // Start is called before the first frame update
    void Start()
    {
        // 获取屏幕尺寸
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        previousMouseX = Input.mousePosition.x;

        // 记录初始X轴旋转角度
        initialRotation = transform.localEulerAngles.x;
        if (initialRotation > 180)
        {
            initialRotation -= 360;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标位置
        Vector3 mousePos = Input.mousePosition;
        
        // 检查鼠标是否在屏幕左半部分
        isInLeftHalf = mousePos.x < screenWidth / 2;

        // 检查是否在中线区域
        bool isInMiddleZone = Mathf.Abs(mousePos.x - screenWidth / 2) < MIDDLE_ZONE;

        // 获取当前X轴旋转角度并确保在正确的范围内
        float currentXRotation = transform.localEulerAngles.x;
        if (currentXRotation > 180)
        {
            currentXRotation -= 360;
        }

        // 计算相对于初始角度的偏移量
        float relativeRotation = currentXRotation - initialRotation;

        if (isInMiddleZone)
        {
            // 在中线区域时，逐渐回正到初始角度
            if (Mathf.Abs(relativeRotation) > 0.1f)
            {
                float resetSpeed = 2f; // 回正的速度
                float resetAmount = -relativeRotation * Time.deltaTime * resetSpeed;
                transform.Rotate(resetAmount, 0, 0, Space.Self);
            }
        }
        else if (isInLeftHalf) // 只在左半部分响应鼠标移动
        {
            // 计算鼠标X轴移动差值
            float deltaX = mousePos.x - previousMouseX;
            
            // 如果鼠标左右移动了，则旋转物体
            if (deltaX != 0)
            {
                // 在左半部分，向右移动时向上转（负deltaX会使物体向上转）
                float rotationAmount = -deltaX*ROTATION_SPEED;
                
                // 检查是否会超出限制
                float newRotation = currentXRotation + rotationAmount;
                if (newRotation >= MIN_ROTATION && newRotation <= MAX_ROTATION)
                {
                    transform.Rotate(rotationAmount, 0, 0, Space.Self);
                }
            }
        }

        // 更新前一帧的鼠标X位置
        previousMouseX = mousePos.x;

        // Debug绘制屏幕分区线（仅在Scene视图中可见）
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(screenWidth/2, 0, 10)), 
                      Camera.main.ScreenToWorldPoint(new Vector3(screenWidth/2, screenHeight, 10)), Color.red);
    }
}
