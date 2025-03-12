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
        // ����ӽǿ���
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90, 90); // ������ת��

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);

        // �������������
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            // ���ݹ��������������
            camera.fieldOfView -= scrollInput * zoomSpeed; // ʹ�� fieldOfView ��ʵ������
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFocalLength, maxFocalLength); // ���ƽ��෶Χ
        }

        // �ƶ�
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
        camera.fieldOfView = 60.0f;
    }
}