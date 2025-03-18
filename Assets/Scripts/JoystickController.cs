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
    public MeshCollider[] colliders; // �洢����Mesh Collider���  

    private Vector3 initialMousePosition;
    private bool isDragging = false;

    public Texture2D handCursor;    // �Զ������͹��  
    public Texture2D clickCursor;   // ���ʱ�Ĺ��  
    public Texture2D defaultCursor; // Ĭ�Ϲ��  

    void Start()
    {
        colliders = GetComponentsInChildren<MeshCollider>();
    }

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
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // ʹ�õ��ʱ�Ĺ�� 
        }

        if (GetActiveCamera() == pilotCamera && Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
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

    // ������꽻�����߼�  
    private void HandleMouseInteraction()
    {
        // �������Ƿ������������ײ����
        bool mouseOver = false;

        foreach (var collider in colliders)
        {
            if (IsMouseOverMeshCollider(collider))
            {
                mouseOver = true;
                // ������������������ײ��
                if (!collider.gameObject.GetComponent<CursorChange>().isMouseOver)
                {
                    collider.gameObject.GetComponent<CursorChange>().isMouseOver = true;
                    collider.gameObject.SendMessage("OnMouseEnter");
                }
                break; // �ҵ�һ����ײ�������˳�ѭ��
            }
        }

        // �����겻���κ����������ײ����
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
    }
 
    // �������Ƿ���MeshCollider������  
    private bool IsMouseOverMeshCollider(MeshCollider collider)
    {
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
        return collider.Raycast(ray, out _, Mathf.Infinity); // �������Ƿ���MeshCollider������  
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