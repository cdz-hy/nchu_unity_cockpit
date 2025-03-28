using UnityEngine;

public class Accelerator : MonoBehaviour
{
    [Header("��ת����")]
    [Range(0, 180)] public float maxAngle = 45f;   // �����ת�Ƕ�
    [Range(0.1f, 2)] public float sensitivity = 1f; // ����������
    [Tooltip("�Ʊ���X����ת")] public bool useLocalX = true;

    private Plane interactionPlane;     // ����ƽ��
    private Vector3 rotationAxis;      // ʵ����ת��
    private Vector3 initialMouseDir;   // ��ʼ��������
    private float currentAngle;        // ��ǰ��ת�Ƕ�
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

    // ��ʼ����תϵͳ
    void InitializeRotationSystem()
    {
        // ��������ѡ����ת��
        rotationAxis = useLocalX ? transform.right : transform.forward;

        // ��������ת�ᴹֱ�Ľ���ƽ��
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

            // ����Ƕȱ仯
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
        // ���ƽǶȷ�Χ
        float targetAngle = Mathf.Clamp(
            currentAngle + delta,
            -maxAngle,
            maxAngle
        );

        // ����ʵ����ת��
        float actualRotation = targetAngle - currentAngle;

        // Ӧ�ñ�����ת
        transform.Rotate(rotationAxis, actualRotation, Space.Self);

        currentAngle = targetAngle;
    }

    // ��ȡ��ǰ���ؽǶȣ��淶����-180~180��
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
        // ��ѡ����ӵ��Իص�Ч��
        // StartCoroutine(SmoothReturn());
    }

    private Camera GetActiveCamera()
    {
        foreach (Camera cam in Camera.allCameras)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam; // ���ص�һ������������
            }
        }
        return null; // ���û�м��������������� null
    }
}