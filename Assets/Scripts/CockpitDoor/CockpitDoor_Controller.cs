using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitDoor_Controller : MonoBehaviour
{
    public float targetRotationY = 0f;  // Ŀ��Y��ת�Ƕȣ��Źرյ�λ�ã�
    public float rotationSpeed = 50f;  // ��ת�ٶ�
    private bool shouldRotate = false;  // �����Ƿ�ʼ��ת

    private BoxCollider[] boxColliders;  // �����BoxCollider

    public Texture2D handCursor;    // �����Ҫʹ���Զ�����
    public Texture2D clickCursor;   // ���ʱ�Ĺ��
    public Texture2D defaultCursor; // Ĭ�Ϲ��

    void Start()
    {
        // ��ȡ�����BoxCollider���
        boxColliders = GetComponents<BoxCollider>();

        // ȷ������ǿɼ���
        Cursor.visible = true;

        // ��ʼ����Ĭ�Ϲ��
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    // ʹ��OnMouseDown��������
    void OnMouseDown()
    {
        // �ж����Ƿ��Ѿ�����Ŀ��λ�ã�����������л�Ŀ��λ��
        if (targetRotationY == 80.463f)
        {
            targetRotationY = 0f;  // ����Ŀ��Ϊ�ر�λ��
        }
        else
        {
            targetRotationY = 80.463f;  // ����Ŀ��Ϊ��λ��
        }

        // ��ʼ��ת
        shouldRotate = true;

        Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // ʹ�õ��ʱ�Ĺ��
    }

    void OnMouseUp()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        // ���Ӧ�ÿ�ʼ��ת��������ת����
        if (shouldRotate)
        {
            RotateSmoothly();
        }
    }

    // ƽ����ת��Ŀ��Y��Ƕ�
    void RotateSmoothly()
    {
        // ��ȡ��ǰ�����Y����ת�Ƕ�
        float currentRotationY = transform.rotation.eulerAngles.y;

        // ʹ��Mathf.MoveTowardsAngle����ƽ����ת
        float newRotationY = Mathf.MoveTowardsAngle(currentRotationY, targetRotationY, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, newRotationY, 0f);

        // �����ת�Ѿ��ӽ�Ŀ��Ƕȣ�ֹͣ��ת
        if (Mathf.Approximately(newRotationY, targetRotationY))
        {
            shouldRotate = false;
        }
    }

    // ��������BoxCollider����ʱ���ı������Ϊ����
    void OnMouseEnter()
    {
        // ���������BoxCollider������û�а����������ʾ���ι��
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // ������뿪BoxCollider����ʱ���ָ�Ĭ�Ϲ��
    void OnMouseExit()
    {
        // �뿪BoxCollider����ʱ�����û�а���������ָ�Ĭ�Ϲ��
        if (!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // �������Ƿ���BoxCollider������
    private bool IsMouseOverBoxCollider()
    {
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        foreach (var boxCollider in boxColliders) 
        {
            if (boxCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                return true;  // �����BoxCollider������
            }
            return false;  // ��겻��BoxCollider������
        }
        return false;
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
