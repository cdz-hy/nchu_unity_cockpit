// using UnityEngine;

// public class FirstPersonController : MonoBehaviour
// {
//     public float speed = 5.0f;
//     public float mouseSensitivity = 2.0f;
//     public float verticalSpeed = 3.0f;
//     public float zoomSpeed = 25.0f; // 缩放速度
//     public float minFocalLength = 10.0f; // 最小焦距
//     public float maxFocalLength = 60.0f; // 最大焦距
//     public float raycastDistance = 1.0f; // 射线检测距离
//     public bool isCollision = false; // 是否可穿墙

//     private bool isFixedViewMode = false; // 是否为固定视角模式
//     private float lastRightClickTime = 0.0f; // 上次右键点击时间
//     private const float doubleClickTime = 0.3f; // 双击时间间隔
//     private float rotationX = 0.0f;
//     private float rotationY = 0.0f;
//     private Camera camera; // 摄像头组件
//     private Vector3 eulerAngles;

//     void Start()
//     {
//         // 获取摄像头组件
//         camera = GetComponent<Camera>();

//         // 将光标锁定在屏幕中心
//         // Cursor.lockState = CursorLockMode.Locked;

//         eulerAngles = transform.localRotation.eulerAngles;
//         eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180, 360) - 180;
//         eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180, 360) - 180;
//         rotationX = eulerAngles.y;
//         rotationY = eulerAngles.x;
//     }

//     void Update()
//     {
//         // 检测右键双击切换模式
//         if (Input.GetMouseButtonDown(1))
//         {
//             if (Time.time - lastRightClickTime < doubleClickTime)
//             {
//                 isFixedViewMode = !isFixedViewMode;
//             }
//             lastRightClickTime = Time.time;
//         }

//         if (isFixedViewMode)
//         {
//             // 固定视角模式下，右键拖拽移动视角
//             if (Input.GetMouseButton(1))
//             {
//                 rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
//                 rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
//                 rotationY = Mathf.Clamp(rotationY, -90, 90);

//                 transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
//             }
//         }
//         else
//         {
//             // 鼠标视角控制
//             rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
//             rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
//             rotationY = Mathf.Clamp(rotationY, -90, 90); // 限制旋转角

//             transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
//         }

//         // 检测鼠标滚轮输入
//         float scrollInput = Input.GetAxis("Mouse ScrollWheel");
//         if (scrollInput != 0)
//         {
//             // 根据滚轮输入调整焦距
//             camera.fieldOfView -= scrollInput * zoomSpeed; // 使用 fieldOfView 来实现缩放
//             camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength); // 限制焦距范围
//         }

//         // 移动
//         float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
//         float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
//         float moveVertical = 0;

//         if (Input.GetKey(KeyCode.Space))
//         {
//             moveVertical = verticalSpeed * Time.deltaTime;
//         }
//         else if (Input.GetKey(KeyCode.LeftShift))
//         {
//             moveVertical = -verticalSpeed * Time.deltaTime;
//         }

//         Vector3 move = transform.right * moveSide + transform.forward * moveForward + transform.up * moveVertical;
//         // 防穿墙检测
//         if (!isCollision || !Physics.Raycast(transform.position, move.normalized, move.magnitude * raycastDistance, LayerMask.GetMask("Wall")))
//         {
//             // 如果没有检测到墙壁，则移动
//             transform.position += move;
//         }
//     }

//     public void Reset()
//     {
//         rotationX = eulerAngles.y;
//         rotationY = eulerAngles.x;
//         // Debug.Log(gameObject.name + rotationY);
//         camera.fieldOfView = 60.0f;
//     }
// }


// using UnityEngine;

// public class FirstPersonController : MonoBehaviour
// {
//     public float speed = 5.0f;
//     public float mouseSensitivity = 2.0f;
//     public float verticalSpeed = 3.0f;
//     public float zoomSpeed = 25.0f; // 缩放速度
//     public float minFocalLength = 10.0f; // 最小焦距
//     public float maxFocalLength = 60.0f; // 最大焦距
//     public float raycastDistance = 1.0f; // 射线检测距离
//     public bool isCollision = false; // 是否可穿墙

//     private bool isFixedViewMode = false; // 是否为固定视角模式
//     private float lastRightClickTime = 0.0f; // 上次右键点击时间
//     private const float doubleClickTime = 0.3f; // 双击时间间隔
//     private float rotationX = 0.0f;
//     private float rotationY = 0.0f;
//     private Camera camera; // 摄像头组件
//     private Vector3 eulerAngles;

//     void Start()
//     {
//         camera = GetComponent<Camera>();

//         eulerAngles = transform.rotation.eulerAngles; // 使用世界旋转获取初始角度
//         eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180, 360) - 180;
//         eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180, 360) - 180;
//         rotationX = eulerAngles.y;
//         rotationY = eulerAngles.x;
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(1))
//         {
//             if (Time.time - lastRightClickTime < doubleClickTime)
//                 isFixedViewMode = !isFixedViewMode;
//             lastRightClickTime = Time.time;
//         }

//         float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
//         float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
//         if (isFixedViewMode)
//         {
//             if (Input.GetMouseButton(1))
//             {
//                 rotationX += mx;
//                 rotationY -= my;
//                 rotationY = Mathf.Clamp(rotationY, -90, 90);
//                 // 世界旋转，锁定滚转(z轴)
//                 transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
//             }
//         }
//         else
//         {
//             rotationX += mx;
//             rotationY -= my;
//             rotationY = Mathf.Clamp(rotationY, -90, 90);
//             // 世界旋转，锁定滚转(z轴)
//             transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
//         }

//         float scrollInput = Input.GetAxis("Mouse ScrollWheel");
//         if (scrollInput != 0)
//         {
//             camera.fieldOfView -= scrollInput * zoomSpeed;
//             camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength);
//         }

//         float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
//         float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
//         float moveVertical = 0;

//         if (Input.GetKey(KeyCode.Space))
//             moveVertical = verticalSpeed * Time.deltaTime;
//         else if (Input.GetKey(KeyCode.LeftShift))
//             moveVertical = -verticalSpeed * Time.deltaTime;

//         Vector3 move = transform.right * moveSide + transform.forward * moveForward + transform.up * moveVertical;
//         if (!isCollision || !Physics.Raycast(transform.position, move.normalized, move.magnitude * raycastDistance, LayerMask.GetMask("Wall")))
//         {
//             transform.position += move;
//         }
//     }

//     public void Reset()
//     {
//         rotationX = eulerAngles.y;
//         rotationY = eulerAngles.x;
//         // 世界旋转，锁定滚转(z轴)
//         transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
//         camera.fieldOfView = 60.0f;
//     }
// }



using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalSpeed = 3.0f;
    public float zoomSpeed = 25.0f; // 缩放速度
    public float minFocalLength = 10.0f; // 最小焦距
    public float maxFocalLength = 60.0f; // 最大焦距
    public float raycastDistance = 1.0f; // 射线检测距离
    public bool isCollision = false; // 是否可穿墙

    private bool isFixedViewMode = false; // 是否为固定视角模式
    private float lastRightClickTime = 0.0f; // 上次右键点击时间
    private const float doubleClickTime = 0.3f; // 双击时间间隔
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Camera camera; // 摄像头组件
    private Vector3 eulerAngles;

    void Start()
    {
        camera = GetComponent<Camera>();

        // 使用相机本地旋转（相对于父物体）获取初始角度
        eulerAngles = transform.localRotation.eulerAngles;
        eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180, 360) - 180;
        eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180, 360) - 180;
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
    }

    void Update()
    {
        // 右键双击切换固定/自由视角
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastRightClickTime < doubleClickTime)
                isFixedViewMode = !isFixedViewMode;
            lastRightClickTime = Time.time;
        }

        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        if (isFixedViewMode)
        {
            if (Input.GetMouseButton(1))
            {
                rotationX += mx;
                rotationY -= my;
                rotationY = Mathf.Clamp(rotationY, -90f, 90f);
                // 本地旋转，锁定滚转(z轴)，保持与父物体XZ面平行
                transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
            }
        }
        else
        {
            rotationX += mx;
            rotationY -= my;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            // 本地旋转，锁定滚转(z轴)，保持与父物体XZ面平行
            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }

        // 缩放视角
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            camera.fieldOfView -= scrollInput * zoomSpeed;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength);
        }

        // 移动逻辑
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = 0f;

        if (Input.GetKey(KeyCode.Space))
            moveVertical = verticalSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.LeftShift))
            moveVertical = -verticalSpeed * Time.deltaTime;

        Vector3 move = transform.right * moveSide + transform.forward * moveForward + transform.up * moveVertical;
        if (!isCollision ||
            !Physics.Raycast(transform.position, move.normalized, move.magnitude * raycastDistance, LayerMask.GetMask("Wall")))
        {
            transform.position += move;
        }
    }

    public void Reset()
    {
        // 重置本地旋转，保持与父物体XZ面平行
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
        camera.fieldOfView = 60.0f;
    }
}