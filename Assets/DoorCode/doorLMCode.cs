using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorLMCode : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float openSpeed = 1.0f; // �����ٶ���ʵ�ָ������ת  
    private bool isRotating = false;

    public Texture2D handCursor;    // �Զ������͹��  
    public Texture2D clickCursor;   // ���ʱ�Ĺ��  
    public Texture2D defaultCursor; // Ĭ�Ϲ��  

    void Start()
    {
        // ȷ������ǿɼ���  
        Cursor.visible = true;

        initialPos = transform.localPosition;
        initialRotation = transform.rotation; // ��¼��ʼ��ת  
        targetRotation = Quaternion.Euler(0, 0, 135); // Ŀ����ת  
    }

    void Update()
    {
        
    }

    // ʹ��OnMouseDown��������
    void OnMouseDown()
    {
        if (!isRotating)
        {
            isRotating = true;
            StartCoroutine(MoveDoor(initialRotation, targetRotation)); // ��ʼ��ת��Ŀ��  
        }
        else
        {
            isRotating = false; // ������ɺ������ٴ���ת 
            StartCoroutine(MoveDoor(targetRotation, initialRotation)); // ���ص���ʼλ��  
        }

        Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);  // ʹ�õ��ʱ�Ĺ��
    }

    void OnMouseUp()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
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
}
