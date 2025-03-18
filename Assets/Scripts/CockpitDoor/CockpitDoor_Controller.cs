using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitDoor_Controller : MonoBehaviour
{
    public float targetRotationY = 0f;  // 目标Y旋转角度（门关闭的位置）
    public float rotationSpeed = 50f;  // 旋转速度
    private bool shouldRotate = false;  // 控制是否开始旋转

    private BoxCollider[] boxColliders;  // 物体的BoxCollider

    public Texture2D handCursor;    // 如果你要使用自定义光标
    public Texture2D clickCursor;   // 点击时的光标
    public Texture2D defaultCursor; // 默认光标

    void Start()
    {
        // 获取物体的BoxCollider组件
        boxColliders = GetComponents<BoxCollider>();

        // 确保光标是可见的
        Cursor.visible = true;

        // 初始设置默认光标
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    // 使用OnMouseDown来处理点击
    void OnMouseDown()
    {
        // 判断门是否已经到达目标位置，如果到达则切换目标位置
        if (targetRotationY == 80.463f)
        {
            targetRotationY = 0f;  // 设置目标为关闭位置
        }
        else
        {
            targetRotationY = 80.463f;  // 设置目标为打开位置
        }

        // 开始旋转
        shouldRotate = true;

        Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // 使用点击时的光标
    }

    void OnMouseUp()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        // 如果应该开始旋转，进行旋转操作
        if (shouldRotate)
        {
            RotateSmoothly();
        }
    }

    // 平滑旋转到目标Y轴角度
    void RotateSmoothly()
    {
        // 获取当前物体的Y轴旋转角度
        float currentRotationY = transform.rotation.eulerAngles.y;

        // 使用Mathf.MoveTowardsAngle进行平滑旋转
        float newRotationY = Mathf.MoveTowardsAngle(currentRotationY, targetRotationY, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, newRotationY, 0f);

        // 如果旋转已经接近目标角度，停止旋转
        if (Mathf.Approximately(newRotationY, targetRotationY))
        {
            shouldRotate = false;
        }
    }

    // 当鼠标进入BoxCollider区域时，改变鼠标光标为手形
    void OnMouseEnter()
    {
        // 如果鼠标进入BoxCollider区域，且没有按下左键，显示手形光标
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 当鼠标离开BoxCollider区域时，恢复默认光标
    void OnMouseExit()
    {
        // 离开BoxCollider区域时，如果没有按下左键，恢复默认光标
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 检查鼠标是否在BoxCollider区域内
    private bool IsMouseOverBoxCollider()
    {
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        foreach (var boxCollider in boxColliders) 
        {
            if (boxCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                return true;  // 鼠标在BoxCollider区域内
            }
            return false;  // 鼠标不在BoxCollider区域内
        }
        return false;
    }

    private Camera GetActiveCamera()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam; // 返回第一个激活的摄像机
            }
        }
        return null; // 如果没有激活的摄像机，返回 null
    }
}
