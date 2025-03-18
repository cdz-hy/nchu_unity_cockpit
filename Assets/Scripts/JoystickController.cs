using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public Transform steeringWheel1; // ���ݷ����̵� Transform
    public Transform steeringWheel2; // ���ݷ����̵� Transform
    public Transform rotatingObject; // ��ƽ���ֵ� Transform
    public Camera pilotCamera; // �Զ������������
    public MeshCollider meshCollider; // ��ȡ��ײ��
    public float stickRotationSpeed = 0.1f; // ���ݸ���ת�ٶ�
    public float wheelRotationSpeed = 0.5f; // ��������ת�ٶ�
    public float maxStickAngle = 10f; // ���ݸ������ת�Ƕ�
    public float maxWheelAngle = 110f; // �����������ת�Ƕ�
    public float maxObjectRotationAngle = 90f; // ��ƽ���������ת�Ƕ�
    public float maxRotationSpeed = 40f; // ��ƽ���������ת�ٶ�

    private Vector3 initialMousePosition;
    private bool isDragging = false;

    void Update()
    {
        if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonDown(0))
        {
            Ray ray = pilotCamera.ScreenPointToRay(Input.mousePosition);
            if (meshCollider.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                isDragging = true; // ֱ������Ϊ�϶�״̬
                initialMousePosition = Input.mousePosition; // ��¼��ʼ���λ��
            }
        }

        if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            float stickRotation = Mathf.Clamp(mouseDelta.y * stickRotationSpeed, -maxStickAngle, maxStickAngle);
            float wheelRotation = Mathf.Clamp(mouseDelta.x * wheelRotationSpeed, -maxWheelAngle, maxWheelAngle);

            // ��ת���ݸ�
            transform.localRotation = Quaternion.Euler(-stickRotation, 0, 0);

            // ��ת������
            steeringWheel1.localRotation = Quaternion.Euler(0, 0, wheelRotation);
            steeringWheel2.localRotation = Quaternion.Euler(0, 0, wheelRotation);

            // ����Ŀ����ת�Ƕ�
            float objectRotation = (wheelRotation / maxWheelAngle) * maxObjectRotationAngle;
            Quaternion targetRotation = Quaternion.Euler(objectRotation, rotatingObject.localRotation.eulerAngles.y, rotatingObject.localRotation.eulerAngles.z);

            // ʹ�� RotateTowards ���������ת�ٶ�
            rotatingObject.localRotation = Quaternion.RotateTowards(rotatingObject.localRotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        }
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