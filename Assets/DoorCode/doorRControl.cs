using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorR1code : MonoBehaviour
{
    public GameObject nob;
    [Header("Door Movement Settings")]
    [SerializeField] private float openSpeed = 0.8f;          // 舱门运动速度  
    [SerializeField] private float innerPullDistance = 0.1f;  // 向内拉的距离  
    [SerializeField] private float outerPushAngle = 180f;     // 向外推的角度（180度）    
    [SerializeField] private float finalOffset = 1f;          // 最终位置偏移量  
    [SerializeField] private float extraLeftOffset = 0.5f;    // 向左额外平移的距离  

    private Vector3 initialPosition;    // 舱门初始位置  
    private Quaternion initialRotation;  // 舱门初始旋转  
    public bool isDoorOpened = false;   // 舱门状态  
    public bool isDoorMoving = false;   // 舱门是否正在移动  

    public BoxCollider[] colliders; // 存储所有Box Collider组件  
    public Texture2D handCursor;    // 自定义手型光标  
    public Texture2D clickCursor;   // 点击时的光标  
    public Texture2D defaultCursor; // 默认光标  

    void Start()
    {
        // 确保光标是可见的  
        Cursor.visible = true;

        colliders = GetComponentsInChildren<BoxCollider>();

        // 自动赋值为当前物体的 Transform  
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        doorL1Nob com = nob.GetComponent<doorL1Nob>();

        // 处理门的开关逻辑  
        if (!isDoorMoving)
        {
            if ((CheckLeftButton(colliders[0]) || CheckLeftButton(colliders[2])) && !isDoorOpened)
            {
                StartCoroutine(OpenDoorCoroutine());

                if (com != null)
                {
                    com.OpenDoor();
                }
            }
            else if (CheckLeftButton(colliders[1]) && isDoorOpened)
            {
                StartCoroutine(CloseDoorCoroutine());

                if (com != null)
                {
                    com.CloseDoor();
                }
            }
        }

        // 处理鼠标交互  
        HandleMouseInteraction();
    }

    // 处理鼠标交互的逻辑  
    private void HandleMouseInteraction()
    {
        // 创建射线
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 检测鼠标是否在子物体的碰撞箱上
        bool mouseOver = false;

        foreach (var collider in colliders)
        {
            if (collider.Raycast(ray, out hit, Mathf.Infinity))
            {
                mouseOver = true;
                // 如果鼠标进入子物体的碰撞箱
                if (!collider.gameObject.GetComponent<CursorChange>().isMouseOver)
                {
                    collider.gameObject.GetComponent<CursorChange>().isMouseOver = true;
                    collider.gameObject.SendMessage("OnMouseEnter");
                }
                break; // 找到一个碰撞箱后可以退出循环
            }
        }

        // 如果鼠标不在任何子物体的碰撞箱上
        if (!mouseOver)
        {
            foreach (var collider in colliders)
            {
                if (collider.gameObject.GetComponent<CursorChange>().isMouseOver)
                {
                    collider.gameObject.GetComponent<CursorChange>().isMouseOver = false;
                    collider.gameObject.SendMessage("OnMouseExit");
                }
            }
        }

        // 检测鼠标左键按下状态，改变光标为点击状态  
        if (Input.GetMouseButton(0) && IsMouseOverAnyBoxCollider()) // 如果左键按下  
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // 使用点击时的光标  
        }
        else if (Input.GetMouseButtonUp(0)) // 如果鼠标未在BoxCollider区域内，恢复默认光标  
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // 检查鼠标是否在任何BoxCollider区域内  
    private bool IsMouseOverAnyBoxCollider()
    {
        foreach (BoxCollider collider in colliders)
        {
            if (IsMouseOverBoxCollider(collider))
            {
                return true; // 鼠标在某个BoxCollider区域内  
            }
        }
        return false; // 鼠标不在任何BoxCollider区域内  
    }

    // 检查鼠标是否在BoxCollider区域内  
    private bool IsMouseOverBoxCollider(BoxCollider boxCollider)
    {
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
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

    /// <summary>  
    /// 打开门的协程  
    /// </summary>  
    private IEnumerator OpenDoorCoroutine()
    {
        isDoorMoving = true;

        // 阶段1：向内拉动  
        Vector3 targetInnerPosition = initialPosition + transform.InverseTransformDirection(transform.right) * innerPullDistance;
        yield return MoveDoor(targetInnerPosition, initialRotation); // 向内拉动  

        // 阶段2：以门的中心为旋转中心旋转到与原位置垂直（90度，围绕自身中轴旋转）  
        Quaternion verticalRotation = initialRotation * Quaternion.Euler(0, -90, 0); // 旋转90度  
        yield return MoveDoor(targetInnerPosition, verticalRotation); // 保持在90度  

        // 阶段3：以门的边缘为旋转中心向外旋转到180度并平移  
        Quaternion targetRotation = verticalRotation * Quaternion.Euler(0, -90, 0); // 从90度变化到180度  
        Vector3 outerPosition = targetInnerPosition - transform.right * 0.3f + transform.forward * 0.35f;
        yield return MoveDoor(outerPosition, targetRotation); // 向外推动  

        // 阶段4：向左平移一段距离  
        Vector3 leftOffsetPosition = outerPosition - transform.right * 0.02f + transform.forward * 0.1f;
        yield return MoveDoor(leftOffsetPosition, targetRotation); // 继续保持旋转，向左平移  

        isDoorOpened = true;
        isDoorMoving = false;
    }

    // 关门的协程（反向操作）  
    private IEnumerator CloseDoorCoroutine()
    {
        isDoorMoving = true;

        // 阶段1：以门的边缘为旋转中心回到180度引导到内侧  
        Quaternion verticalRotation = initialRotation * Quaternion.Euler(0, -90, 0);
        // 先到边缘位置   
        Vector3 positionAfterPush = initialPosition + transform.right * 0.02f - transform.forward * 0.1f;
        yield return MoveDoor(positionAfterPush, verticalRotation);

        // 阶段2：旋转回原位置  
        yield return MoveDoor(initialPosition, initialRotation); // 回到起始位置  

        isDoorOpened = false;
        isDoorMoving = false;
    }

    // 通用移动方法  
    private IEnumerator MoveDoor(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * openSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // 确保完成后设置到目标位置  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
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