using UnityEngine;

public class CamreaFree : MonoBehaviour
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

        eulerAngles = transform.rotation.eulerAngles; // 使用世界旋转获取初始角度
        eulerAngles.x = Mathf.Repeat(eulerAngles.x + 180, 360) - 180;
        eulerAngles.y = Mathf.Repeat(eulerAngles.y + 180, 360) - 180;
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
    }

    void Update()
    {
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
                rotationY = Mathf.Clamp(rotationY, -90, 90);
                // 世界旋转，锁定滚转(z轴)
                transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
            }
        }
        else
        {
            rotationX += mx;
            rotationY -= my;
            rotationY = Mathf.Clamp(rotationY, -90, 90);
            // 世界旋转，锁定滚转(z轴)
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            camera.fieldOfView -= scrollInput * zoomSpeed;
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength);
        }

        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = 0;

        if (Input.GetKey(KeyCode.Space))
            moveVertical = verticalSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.LeftShift))
            moveVertical = -verticalSpeed * Time.deltaTime;

        Vector3 move = transform.right * moveSide + transform.forward * moveForward + transform.up * moveVertical;
        if (!isCollision || !Physics.Raycast(transform.position, move.normalized, move.magnitude * raycastDistance, LayerMask.GetMask("Wall")))
        {
            transform.position += move;
        }
    }

    public void Reset()
    {
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
        // 世界旋转，锁定滚转(z轴)
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        camera.fieldOfView = 60.0f;
    }
}