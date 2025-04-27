using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject targetObject; // Ŀ������  
    public Vector3 offset; // ƫ����  
    private Quaternion initialRotation;
    private RectTransform uiTransform; // UI Ԫ�ص� RectTransform  

    void Start()
    {
        uiTransform = GetComponent<RectTransform>(); // ��ȡ UI Ԫ�ص� RectTransform  
        
    }

    void Update()
    {
        offset = uiTransform.position - targetObject.transform.localPosition;
        // ���Ŀ��������ڣ������ UI λ��  
        if (targetObject != null)
        {
            // ���� UI Ԫ�ص�λ��ΪĿ�������λ�ü���ƫ��  
            uiTransform.position = targetObject.transform.position + offset;

            // ʹ UI ����������� (��ѡ��ȡ��������)  
            uiTransform.LookAt(Camera.main.transform);
        }
    }
}
