using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public Transform steeringWheel1; // 主驾方向盘的 Transform
    public Transform steeringWheel2; // 副驾方向盘的 Transform
    public Transform rotatingObject; // 配平手轮的 Transform
    public Camera pilotCamera; // 自定义摄像机引用
    public BoxCollider boxCollider; // 获取碰撞箱
    public float stickRotationSpeed = 0.5f; // 操纵杆旋转速度
    public float wheelRotationSpeed = 0.5f; // 方向盘旋转速度
    public float maxStickAngle = 10f; // 操纵杆最大旋转角度
    public float maxWheelAngle = 110f; // 方向盘最大旋转角度
    public float maxObjectRotationAngle = 90f; // 配平手轮最大旋转角度
    public float maxRotationSpeed = 40f; // 配平手轮最大旋转速度
    private float currentObjectRotation = 0f; // 配平手轮现在的旋转角

    private Vector3 initialMousePosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
            if (boxCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                isDragging = true; // 直接设置为拖动状态
                initialMousePosition = Input.mousePosition; // 记录初始鼠标位置
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            float stickRotation = Mathf.Clamp(mouseDelta.y * stickRotationSpeed, -maxStickAngle, maxStickAngle);
            float wheelRotation = Mathf.Clamp(mouseDelta.x * wheelRotationSpeed, -maxWheelAngle, maxWheelAngle);

            // 旋转操纵杆
            transform.localRotation = Quaternion.Euler(-stickRotation, 0, 0);

            // 旋转方向盘
            steeringWheel1.localRotation = Quaternion.Euler(0, 0, wheelRotation);
            steeringWheel2.localRotation = Quaternion.Euler(0, 0, wheelRotation);

            // 计算目标旋转角度
            float objectRotation = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
            Quaternion targetRotation = Quaternion.Euler(objectRotation, rotatingObject.localRotation.eulerAngles.y, rotatingObject.localRotation.eulerAngles.z);

            // 使用 RotateTowards 限制最大旋转速度
            rotatingObject.localRotation = Quaternion.RotateTowards(rotatingObject.localRotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        }
    }
}