using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorR1code : MonoBehaviour
{
    public GameObject nob;
    [Header("Door Movement Settings")]
    [SerializeField] private float openSpeed = 0.8f;          // �����˶��ٶ�  
    [SerializeField] private float innerPullDistance = 0.1f;  // �������ľ���  
    [SerializeField] private float outerPushAngle = 180f;     // �����ƵĽǶȣ�180�ȣ�    
    [SerializeField] private float finalOffset = 1f;          // ����λ��ƫ����  
    [SerializeField] private float extraLeftOffset = 0.5f;    // �������ƽ�Ƶľ���  

    private Vector3 initialPosition;    // ���ų�ʼλ��  
    private Quaternion initialRotation;  // ���ų�ʼ��ת  
    public bool isDoorOpened = false;   // ����״̬  
    public bool isDoorMoving = false;   // �����Ƿ������ƶ�  

    public BoxCollider[] colliders; // �洢����Box Collider���  
    public Texture2D handCursor;    // �Զ������͹��  
    public Texture2D clickCursor;   // ���ʱ�Ĺ��  
    public Texture2D defaultCursor; // Ĭ�Ϲ��  

    void Start()
    {
        // ȷ������ǿɼ���  
        Cursor.visible = true;

        colliders = GetComponentsInChildren<BoxCollider>();

        // �Զ���ֵΪ��ǰ����� Transform  
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        doorL1Nob com = nob.GetComponent<doorL1Nob>();

        // �����ŵĿ����߼�  
        if (!isDoorMoving)
        {
            if ((CheckLeftButton(colliders[0]) || CheckLeftButton(colliders[2])) && !isDoorOpened)
            {
                StartCoroutine(OpenDoorCoroutine());

                if (com != null)
                {
                    com.OpenDoor();
                }
            }
            else if (CheckLeftButton(colliders[1]) && isDoorOpened)
            {
                StartCoroutine(CloseDoorCoroutine());

                if (com != null)
                {
                    com.CloseDoor();
                }
            }
        }

        // ������꽻��  
        HandleMouseInteraction();
    }

    // ������꽻�����߼�  
    private void HandleMouseInteraction()
    {
        // ��������
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // �������Ƿ������������ײ����
        bool mouseOver = false;

        foreach (var collider in colliders)
        {
            if (collider.Raycast(ray, out hit, Mathf.Infinity))
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

        // �������������״̬���ı���Ϊ���״̬  
        if (Input.GetMouseButton(0) && IsMouseOverAnyBoxCollider()) // ����������  
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // ʹ�õ��ʱ�Ĺ��  
        }
        else if (Input.GetMouseButtonUp(0)) // ������δ��BoxCollider�����ڣ��ָ�Ĭ�Ϲ��  
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // �������Ƿ����κ�BoxCollider������  
    private bool IsMouseOverAnyBoxCollider()
    {
        foreach (BoxCollider collider in colliders)
        {
            if (IsMouseOverBoxCollider(collider))
            {
                return true; // �����ĳ��BoxCollider������  
            }
        }
        return false; // ��겻���κ�BoxCollider������  
    }

    // �������Ƿ���BoxCollider������  
    private bool IsMouseOverBoxCollider(BoxCollider boxCollider)
    {
        Ray ray = GetActiveCamera().ScreenPointToRay(Input.mousePosition);
        return boxCollider.Raycast(ray, out _, Mathf.Infinity); // �������Ƿ���BoxCollider������  
    }

    // ����Ƿ������������  
    private bool CheckLeftButton(BoxCollider collider)
    {
        if (Input.GetMouseButtonDown(0)) // ����������  
        {
            return IsMouseOverBoxCollider(collider); // �������Ƿ���BoxCollider��Χ��  
        }
        return false; // ���û�а���  
    }

    /// <summary>  
    /// ���ŵ�Э��  
    /// </summary>  
    private IEnumerator OpenDoorCoroutine()
    {
        isDoorMoving = true;

        // �׶�1����������  
        Vector3 targetInnerPosition = initialPosition + transform.InverseTransformDirection(transform.right) * innerPullDistance;
        yield return MoveDoor(targetInnerPosition, initialRotation); // ��������  

        // �׶�2�����ŵ�����Ϊ��ת������ת����ԭλ�ô�ֱ��90�ȣ�Χ������������ת��  
        Quaternion verticalRotation = initialRotation * Quaternion.Euler(0, -90, 0); // ��ת90��  
        yield return MoveDoor(targetInnerPosition, verticalRotation); // ������90��  

        // �׶�3�����ŵı�ԵΪ��ת����������ת��180�Ȳ�ƽ��  
        Quaternion targetRotation = verticalRotation * Quaternion.Euler(0, -90, 0); // ��90�ȱ仯��180��  
        Vector3 outerPosition = targetInnerPosition - transform.right * 0.3f + transform.forward * 0.35f;
        yield return MoveDoor(outerPosition, targetRotation); // �����ƶ�  

        // �׶�4������ƽ��һ�ξ���  
        Vector3 leftOffsetPosition = outerPosition - transform.right * 0.02f + transform.forward * 0.1f;
        yield return MoveDoor(leftOffsetPosition, targetRotation); // ����������ת������ƽ��  

        isDoorOpened = true;
        isDoorMoving = false;
    }

    // ���ŵ�Э�̣����������  
    private IEnumerator CloseDoorCoroutine()
    {
        isDoorMoving = true;

        // �׶�1�����ŵı�ԵΪ��ת���Ļص�180���������ڲ�  
        Quaternion verticalRotation = initialRotation * Quaternion.Euler(0, -90, 0);
        // �ȵ���Եλ��   
        Vector3 positionAfterPush = initialPosition + transform.right * 0.02f - transform.forward * 0.1f;
        yield return MoveDoor(positionAfterPush, verticalRotation);

        // �׶�2����ת��ԭλ��  
        yield return MoveDoor(initialPosition, initialRotation); // �ص���ʼλ��  

        isDoorOpened = false;
        isDoorMoving = false;
    }

    // ͨ���ƶ�����  
    private IEnumerator MoveDoor(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * openSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // ȷ����ɺ����õ�Ŀ��λ��  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
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