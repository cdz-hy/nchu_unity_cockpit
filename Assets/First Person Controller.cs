using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalSpeed = 3.0f;
    public float zoomSpeed = 25.0f; // 缩放速度
    public float minFocalLength = 10.0f; // 最小焦距
    public float maxFocalLength = 60.0f; // 最大焦距

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Camera camera; // 摄像头组件
    private Vector3 eulerAngles;

    void Start()
    {
        // 获取摄像头组件
        camera = GetComponent<Camera>();

        // 将光标锁定在屏幕中心
        Cursor.lockState = CursorLockMode.Locked;

        eulerAngles = transform.localRotation.eulerAngles;
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
    }

    void Update()
    {
        // 如果鼠标右键被按下，则允许通过鼠标移动视角
        if (Input.GetMouseButton(1))
        {
            // 根据鼠标的X轴输入调整水平旋转角度
            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
            // 根据鼠标的Y轴输入调整垂直旋转角度，并限制垂直旋转范围
            rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90); // 限制垂直旋转角度在-90到90度之间

            // 应用旋转
            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        // 计算前后左右移动的距离
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = 0;

        // 按下空格键向上移动
        if (Input.GetKey(KeyCode.Space))
        {
            moveVertical = verticalSpeed * Time.deltaTime;
        }
        // 按下左Shift键向下移动
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVertical = -verticalSpeed * Time.deltaTime;
        }

        // 计算总的移动向量并更新位置
        Vector3 move = transform.right * moveSide + transform.forward * moveForward + transform.up * moveVertical;
        transform.position += move;
    }

    public void Reset()
    {
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
        camera.fieldOfView = 60.0f;
    }
}