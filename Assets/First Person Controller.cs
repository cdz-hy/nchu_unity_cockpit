using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float verticalSpeed = 3.0f;
    public float zoomSpeed = 25.0f; // �����ٶ�
    public float minFocalLength = 10.0f; // ��С����
    public float maxFocalLength = 60.0f; // ��󽹾�

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Camera camera; // ����ͷ���
    private Vector3 eulerAngles;

    void Start()
    {
        // ��ȡ����ͷ���
        camera = GetComponent<Camera>();

        // �������������Ļ����
        Cursor.lockState = CursorLockMode.Locked;

        eulerAngles = transform.localRotation.eulerAngles;
        rotationX = eulerAngles.y;
        rotationY = eulerAngles.x;
    }

    void Update()
    {
        // �������Ҽ������£�������ͨ������ƶ��ӽ�
        if (Input.GetMouseButton(1))
        {
            // ��������X���������ˮƽ��ת�Ƕ�
            rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
            // ��������Y�����������ֱ��ת�Ƕȣ������ƴ�ֱ��ת��Χ
            rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90); // ���ƴ�ֱ��ת�Ƕ���-90��90��֮��

            // Ӧ����ת
            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        // ����ǰ�������ƶ��ľ���
        float moveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveSide = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float moveVertical = 0;

        // ���¿ո�������ƶ�
        if (Input.GetKey(KeyCode.Space))
        {
            moveVertical = verticalSpeed * Time.deltaTime;
        }
        // ������Shift�������ƶ�
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVertical = -verticalSpeed * Time.deltaTime;
        }

        // �����ܵ��ƶ�����������λ��
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