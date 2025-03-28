using UnityEngine;

public class Accelerator : MonoBehaviour
{
    [Header("旋转参数")]
    [Range(0, 180)] public float maxAngle = 45f;   // 最大旋转角度
    [Range(0.1f, 2)] public float sensitivity = 1f; // 操作灵敏度
    [Tooltip("绕本地X轴旋转")] public bool useLocalX = true;

    private Plane interactionPlane;     // 交互平面
    private Vector3 rotationAxis;      // 实际旋转轴
    private Vector3 initialMouseDir;   // 初始方向向量
    private float currentAngle;        // 当前旋转角度
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetActiveCamera();
        InitializeRotationSystem();
    }

    void Update()
    {
        HandleDragInteraction();
    }

    // 初始化旋转系统
    void InitializeRotationSystem()
    {
        // 根据设置选择旋转轴
        rotationAxis = useLocalX ? transform.right : transform.forward;

        // 创建与旋转轴垂直的交互平面
        interactionPlane = new Plane(rotationAxis, transform.position);
        currentAngle = GetCurrentLocalAngle();
    }

    void HandleDragInteraction()
    {
        if (Input.GetMouseButtonDown(0)) TryStartDrag();
        if (Input.GetMouseButton(0)) UpdateDrag();
        if (Input.GetMouseButtonUp(0)) EndDrag();
    }

    void TryStartDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
        {
            InitializeDrag(ray);
        }
    }

    void InitializeDrag(Ray initialRay)
    {
        if (interactionPlane.Raycast(initialRay, out float enter))
        {
            Vector3 hitPoint = initialRay.GetPoint(enter);
            initialMouseDir = (hitPoint - transform.position).normalized;
        }
    }

    void UpdateDrag()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (interactionPlane.Raycast(ray, out float enter))
        {
            Vector3 currentHit = ray.GetPoint(enter);
            Vector3 currentDir = (currentHit - transform.position).normalized;

            // 计算角度变化
            float angleDelta = Vector3.SignedAngle(
                initialMouseDir,
                currentDir,
                rotationAxis
            ) * sensitivity;

            ApplyRotation(angleDelta);
        }
    }

    void ApplyRotation(float delta)
    {
        // 限制角度范围
        float targetAngle = Mathf.Clamp(
            currentAngle + delta,
            -maxAngle,
            maxAngle
        );

        // 计算实际旋转量
        float actualRotation = targetAngle - currentAngle;

        // 应用本地旋转
        transform.Rotate(rotationAxis, actualRotation, Space.Self);

        currentAngle = targetAngle;
    }

    // 获取当前本地角度（规范化到-180~180）
    float GetCurrentLocalAngle()
    {
        Vector3 localEuler = transform.localEulerAngles;
        float rawAngle = useLocalX ? localEuler.x : localEuler.z;
        return NormalizeAngle(rawAngle);
    }

    float NormalizeAngle(float angle)
    {
        angle %= 360;
        return angle > 180 ? angle - 360 : angle;
    }

    void EndDrag()
    {
        // 可选：添加弹性回弹效果
        // StartCoroutine(SmoothReturn());
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