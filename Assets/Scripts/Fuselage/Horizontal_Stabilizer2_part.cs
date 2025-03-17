using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horizontal_Stabilizer2_part : MonoBehaviour
{
    private float screenHeight;
    private float screenWidth;
    private float previousMouseY;
    private bool isInTopHalf;
    private float initialRotation;  // 记录初始旋转角度
    
    // 旋转角度限制
    private const float MAX_ROTATION = 15f;
    private const float MIN_ROTATION = -15f;
    
    // 中线区域的高度范围（上下各50像素）
    private const float MIDDLE_ZONE = 50f;

    // Start is called before the first frame update
    void Start()
    {
        // 获取屏幕尺寸
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        previousMouseY = Input.mousePosition.y;

        // 记录初始Z轴旋转角度
        initialRotation = transform.localEulerAngles.z;
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
        
        // 检查鼠标是否在屏幕上半部分
        isInTopHalf = mousePos.y > screenHeight / 2;

        // 检查是否在中线区域
        bool isInMiddleZone = Mathf.Abs(mousePos.y - screenHeight / 2) < MIDDLE_ZONE;

        // 获取当前Z轴旋转角度并确保在正确的范围内
        float currentZRotation = transform.localEulerAngles.z;
        if (currentZRotation > 180)
        {
            currentZRotation -= 360;
        }

        // 计算相对于初始角度的偏移量
        float relativeRotation = currentZRotation - initialRotation;

        if (isInMiddleZone)
        {
            // 在中线区域时，逐渐回正到初始角度
            if (Mathf.Abs(relativeRotation) > 0.1f)
            {
                float resetSpeed = 2f; // 回正的速度
                float resetAmount = -relativeRotation * Time.deltaTime * resetSpeed;
                transform.Rotate(0, 0, resetAmount, Space.Self);
            }
        }
        else
        {
            // 计算鼠标Y轴移动差值
            float deltaY = mousePos.y - previousMouseY;
            
            // 如果鼠标移动了，则旋转物体
            if (deltaY != 0)
            {
                // 计算旋转方向：与obj7_002相反
                float rotationAmount = -deltaY;
                
                // 检查是否会超出限制（相对于初始角度）
                float newRelativeRotation = relativeRotation + rotationAmount;
                if (newRelativeRotation >= MIN_ROTATION && newRelativeRotation <= MAX_ROTATION)
                {
                    transform.Rotate(0, 0, rotationAmount, Space.Self);
                }
            }
        }

        // 更新前一帧的鼠标Y位置
        previousMouseY = mousePos.y;

        // Debug绘制屏幕分区线（仅在Scene视图中可见）
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(screenWidth/2, 0, 10)), 
                      Camera.main.ScreenToWorldPoint(new Vector3(screenWidth/2, screenHeight, 10)), Color.red);
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(0, screenHeight/2, 10)), 
                      Camera.main.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight/2, 10)), Color.red);
    }
}
