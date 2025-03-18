using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorR1Mcode : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float openSpeed = 1.0f; // �����ٶ���ʵ�ָ������ת  
    private bool isRotating = false;
    public BoxCollider[] colliders; // �洢����Box Collider���  

    public Texture2D handCursor;    // �Զ������͹��  
    public Texture2D clickCursor;   // ���ʱ�Ĺ��  
    public Texture2D defaultCursor; // Ĭ�Ϲ��  

    void Start()
    {
        // ȷ������ǿɼ���  
        Cursor.visible = true;

        // ��ʼ����Ĭ�Ϲ��  
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);

        initialPos = transform.localPosition;
        initialRotation = transform.rotation; // ��¼��ʼ��ת  
        targetRotation = Quaternion.Euler(0, 0, -135); // Ŀ����ת  
    }

    void Update()
    {
        if (CheckLeftButton(colliders[0]) && !isRotating)
        {
            isRotating = true;
            StartCoroutine(MoveDoor(initialRotation, targetRotation)); // ��ʼ��ת��Ŀ��  
        }
        else if (CheckLeftButton(colliders[0]) && isRotating)
        {
            isRotating = false; // ������ɺ������ٴ���ת 
            StartCoroutine(MoveDoor(targetRotation, initialRotation)); // ���ص���ʼλ��  
        }
    }

    private IEnumerator MoveDoor(Quaternion startRot, Quaternion targetRot)
    {
        float progress = 0;
        Quaternion currentRot = startRot;

        while (progress < 1)
        {
            progress += Time.deltaTime * openSpeed;
            transform.localRotation = Quaternion.Slerp(currentRot, targetRot, progress);
            yield return null; // �ȴ���һ֡  
        }

        // ȷ����ɺ����õ�Ŀ����ת  
        transform.localRotation = targetRot; 
    }

    // ������꽻�����߼�  
    private void HandleMouseInteraction()
    {
        // �������������״̬���ı���Ϊ���״̬  
        if (Input.GetMouseButton(0)) // ����������  
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // ʹ�õ��ʱ�Ĺ��  
        }
        else if (!IsMouseOverAnyBoxCollider()) // ������δ��BoxCollider�����ڣ��ָ�Ĭ�Ϲ��  
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // �������Ƿ����κ�BoxCollider������  
    private bool IsMouseOverAnyBoxCollider()
    {
        foreach (var collider in colliders)
        {
            if (IsMouseOverBoxCollider(collider))
            {
                OnMouseEnter();
                return true; // �����ĳ��BoxCollider������  
            }
        }
        OnMouseExit();
        return false; // ��겻���κ�BoxCollider������  
    }

    // �������Ƿ���BoxCollider������  
    private bool IsMouseOverBoxCollider(BoxCollider boxCollider)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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

    // ������뿪BoxCollider����ʱ���ָ�Ĭ�Ϲ��  
    void OnMouseExit()
    {
        if (!Input.GetMouseButton(0)) // ���û�а���������ָ�Ĭ�Ϲ��  
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // ��������BoxCollider����ʱ���ı������Ϊ����  
    void OnMouseEnter()
    {
        if (!Input.GetMouseButton(0)) // ���������BoxCollider������û�а����������ʾ���ι��  
        {
            Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    // ���¹��״̬  
    private void UpdateCursor()
    {
        if (IsMouseOverAnyBoxCollider())
        {
            if (Input.GetMouseButton(0)) // ����������  
            {
                Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto); // ʹ�õ��ʱ�Ĺ��  
            }
            else
            {
                Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto); // ʹ�����ι��  
            }
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto); // �ָ�Ĭ�Ϲ��  
        }
    }
}