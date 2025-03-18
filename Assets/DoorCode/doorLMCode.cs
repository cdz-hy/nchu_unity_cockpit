using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorLMCode : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float openSpeed = 1.0f; // 调整速度以实现更快的旋转  
    private bool isRotating = false;

    public Texture2D handCursor;    // 自定义手型光标  
    public Texture2D clickCursor;   // 点击时的光标  
    public Texture2D defaultCursor; // 默认光标  

    void Start()
    {
        // 确保光标是可见的  
        Cursor.visible = true;

        initialPos = transform.localPosition;
        initialRotation = transform.rotation; // 记录初始旋转  
        targetRotation = Quaternion.Euler(0, 0, 135); // 目标旋转  
    }

    void Update()
    {
        
    }

    // 使用OnMouseDown来处理点击
    void OnMouseDown()
    {
        if (!isRotating)
        {
            isRotating = true;
            StartCoroutine(MoveDoor(initialRotation, targetRotation)); // 开始旋转到目标  
        }
        else
        {
            isRotating = false; // 动画完成后允许再次旋转 
            StartCoroutine(MoveDoor(targetRotation, initialRotation)); // 返回到初始位置  
        }

        Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // 使用点击时的光标
    }

    void OnMouseUp()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private IEnumerator MoveDoor(Quaternion startRot, Quaternion targetRot)
    {
        float progress = 0;
        Quaternion currentRot = startRot;

        while (progress < 1)
        {
            progress += Time.deltaTime * openSpeed;
            transform.localRotation = Quaternion.Slerp(currentRot, targetRot, progress);
            yield return null; // 等待下一帧  
        }

        // 确保完成后设置到目标旋转  
        transform.localRotation = targetRot;
    }

    // 当鼠标离开BoxCollider区域时，恢复默认光标  
    void OnMouseExit()
    {
        if (!Input.GetMouseButton(0)) // 如果没有按下左键，恢复默认光标  
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 当鼠标进入BoxCollider区域时，改变鼠标光标为手形  
    void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0)) // 如果鼠标进入BoxCollider区域，且没有按下左键，显示手形光标  
        {
            Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
