using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // 目标物体
    public float distance = 30.0f; // 固定半径
    public float horizontalSpeed = 20.0f; // 水平方向旋转速度
    public float verticalSpeed = 20.0f; // 竖直方向旋转速度
    public float zoomSpeed = 25.0f; // 缩放速度
    public float minFocalLength = 10.0f; // 最小焦距
    public float maxFocalLength = 60.0f; // 最大焦距

    private float horizontalAngle = -140.0f; // 水平方向角度
    private float verticalAngle = 0.0f; // 竖直方向角度
    private Camera camera; // 摄像头组件

    void Start()
    {
        // 获取摄像头组件
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        // 获取键盘输入
        float horizontalInput = -Input.GetAxis("Horizontal");
        float verticalInput = -Input.GetAxis("Vertical");

        // 更新角度
        horizontalAngle += horizontalInput * horizontalSpeed * Time.deltaTime;
        verticalAngle -= verticalInput * verticalSpeed * Time.deltaTime;

        // 右键拖拽视角移动
        if (Input.GetMouseButton(1)) // 右键按下
        {
            horizontalAngle += Input.GetAxis("Mouse X") * 30 * horizontalSpeed * Time.deltaTime;
            verticalAngle -= Input.GetAxis("Mouse Y") * 30 * verticalSpeed * Time.deltaTime;
        }


        // 限制竖直方向的角度
        verticalAngle = Mathf.Clamp(verticalAngle, -89.9f, 89.9f);

        // 检测鼠标滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // 根据滚轮输入调整焦距
            camera.fieldOfView -= scrollInput * zoomSpeed; // 使用 fieldOfView 来实现缩放
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength); // 限制焦距范围
        }

        // 计算摄像头的位置
        Vector3 offset = new Vector3(
            distance * Mathf.Cos(verticalAngle * Mathf.Deg2Rad) * Mathf.Sin(horizontalAngle * Mathf.Deg2Rad),
            distance * Mathf.Sin(verticalAngle * Mathf.Deg2Rad),
            distance * Mathf.Cos(verticalAngle * Mathf.Deg2Rad) * Mathf.Cos(horizontalAngle * Mathf.Deg2Rad)
        );

        // 设置摄像头的位置和朝向
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }

    public void Reset()
    {
        horizontalAngle = -140.0f;
        verticalAngle = 0.0f;
        camera.fieldOfView = 60.0f;
    }
}