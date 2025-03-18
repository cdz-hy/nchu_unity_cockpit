using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CursorChange : MonoBehaviour
{
    public bool isMouseOver = false;
    public Texture2D handCursor;    // �����Ҫʹ���Զ�����
    public Texture2D clickCursor;   // ���ʱ�Ĺ��
    public Texture2D defaultCursor; // Ĭ�Ϲ��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
