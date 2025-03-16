using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheel_2 : MonoBehaviour
{

    private float targetRotationX = -53f; // Ŀ����ת�Ƕ�
    private float speed = 100f; // ��ת�ٶ�
    private Quaternion targetRotation; // Ŀ����Ԫ����ת

    void Start()
    {
        // ����Ŀ����Ԫ����ת��ֻ�ı�X����ת�����������᲻��
        targetRotation = Quaternion.Euler(targetRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void Update()
    {
        // �����ǰ����ת�Ƕ���Ŀ����ת�ǶȲ�ͬ��������ת
        if (transform.localRotation != targetRotation)
        {
            // ʹ�� Quaternion.RotateTowards ��ƽ�����ɵ�Ŀ��Ƕ�
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, speed * Time.deltaTime);
        }
    }
}
