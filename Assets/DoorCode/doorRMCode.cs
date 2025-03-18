using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorR1Mcode : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float openSpeed = 1.0f; // 调整速度以实现更快的旋转  
    private bool isRotating = false;
    public BoxCollider[] colliders; // 存储所有Box Collider组件  

    public Texture2D handCursor;    // 自定义手型光标  
    public Texture2D clickCursor;   // 点击时的光标  
    public Texture2D defaultCursor; // 默认光标  

    void Start()
    {
        // 确保光标是可见的  
        Cursor.visible = true;

        // 初始设置默认光标  
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        initialPos = transform.localPosition;
        initialRotation = transform.rotation; // 记录初始旋转  
        targetRotation = Quaternion.Euler(0, 0, -135); // 目标旋转  
    }

    void Update()
    {
        if (CheckLeftButton(colliders[0]) && !isRotating)
        {
            isRotating = true;
            StartCoroutine(MoveDoor(initialRotation, targetRotation)); // 开始旋转到目标  
        }
        else if (CheckLeftButton(colliders[0]) && isRotating)
        {
            isRotating = false; // 动画完成后允许再次旋转 
            StartCoroutine(MoveDoor(targetRotation, initialRotation)); // 返回到初始位置  
        }
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

    // 处理鼠标交互的逻辑  
    private void HandleMouseInteraction()
    {
        // 检测鼠标左键按下状态，改变光标为点击状态  
        if (Input.GetMouseButton(0)) // 如果左键按下  
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // 使用点击时的光标  
        }
        else if (!IsMouseOverAnyBoxCollider()) // 如果鼠标未在BoxCollider区域内，恢复默认光标  
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 检查鼠标是否在任何BoxCollider区域内  
    private bool IsMouseOverAnyBoxCollider()
    {
        foreach (var collider in colliders)
        {
            if (IsMouseOverBoxCollider(collider))
            {
                OnMouseEnter();
                return true; // 鼠标在某个BoxCollider区域内  
            }
        }
        OnMouseExit();
        return false; // 鼠标不在任何BoxCollider区域内  
    }

    // 检查鼠标是否在BoxCollider区域内  
    private bool IsMouseOverBoxCollider(BoxCollider boxCollider)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return boxCollider.Raycast(ray, out _, Mathf.Infinity); // 检查鼠标是否在BoxCollider区域内  
    }

    // 检查是否发生鼠标左键点击  
    private bool CheckLeftButton(BoxCollider collider)
    {
        if (Input.GetMouseButtonDown(0)) // 如果左键按下  
        {
            return IsMouseOverBoxCollider(collider); // 检查鼠标是否在BoxCollider范围内  
        }
        return false; // 鼠标没有按下  
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

    // 更新光标状态  
    private void UpdateCursor()
    {
        if (IsMouseOverAnyBoxCollider())
        {
            if (Input.GetMouseButton(0)) // 如果左键按下  
            {
                Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto); // 使用点击时的光标  
            }
            else
            {
                Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto); // 使用手形光标  
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto); // 恢复默认光标  
        }
    }
}