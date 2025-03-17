using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalSpeed = 3.0f;
    public float zoomSpeed = 25.0f; // 缩放速度
    public float minFocalLength = 10.0f; // 最小焦距
    public float maxFocalLength = 60.0f; // 最大焦距

    private bool isFixedViewMode = false; // 是否为固定视角模式
    private float lastRightClickTime = 0.0f; // 上次右键点击时间
    private const float doubleClickTime = 0.3f; // 双击时间间隔
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Camera camera; // 摄像头组件
    private Vector3 eulerAngles;

    void Start()
    {
        // 获取摄像头组件
        camera = GetComponent<Camera>();

        // 将光标锁定在屏幕中心
        // Cursor.lockState = CursorLockMode.Locked;

        eulerAngles = transform.localRotation.eulerAngles;
        eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180, 360) - 180;
        eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180, 360) - 180;
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
    }

    void Update()
    {
        // 检测右键双击切换模式
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastRightClickTime < doubleClickTime)
            {
                isFixedViewMode = !isFixedViewMode;
            }
            lastRightClickTime = Time.time;
        }

        if (isFixedViewMode)
        {
            // 固定视角模式下，右键拖拽移动视角
            if (Input.GetMouseButton(1))
            {
                rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
                rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                rotationY = Mathf.Clamp(rotationY, -90, 90);

                transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
            }
        }
        else
        {
            // 鼠标视角控制
            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90); // 限制旋转角

            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        // 检测鼠标滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // 根据滚轮输入调整焦距
            camera.fieldOfView -= scrollInput * zoomSpeed; // 使用 fieldOfView 来实现缩放
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength); // 限制焦距范围
        }

        // 移动
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = 0;

        if (Input.GetKey(KeyCode.Space))
        {
            moveVertical = verticalSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVertical = -verticalSpeed * Time.deltaTime;
        }

        Vector3 move = transform.right * moveSide + transform.forward * moveForward + transform.up * moveVertical;
        transform.position += move;
    }

    public void Reset()
    {
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
        // Debug.Log(gameObject.name + rotationY);
        camera.fieldOfView = 60.0f;
    }
}