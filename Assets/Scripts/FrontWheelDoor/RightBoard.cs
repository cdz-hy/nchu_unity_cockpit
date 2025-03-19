using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBoard : MonoBehaviour
{
    private Quaternion targetRotation;  // Ŀ����ת��(0, 0, 0)
    private Quaternion originalRotation; // ԭʼ��ת��Ԫ��
    public float rotationSpeed = 20f;  // ��ת�ٶ�
    public int isKeyPressed = 1;

    void Start()
    {
        originalRotation = transform.localRotation; // �洢ԭʼ��ת
        // ��ʼ��Ŀ����תΪ (0, 0, 0)
        targetRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        if (isKeyPressed == 2)
        {
            // ʹ�� Quaternion.RotateTowards ��ƽ�����ɵ�Ŀ����ת
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // ��������ת��ԭʼλ��
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
