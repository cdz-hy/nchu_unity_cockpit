using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractFrontWheels : MonoBehaviour
{
    public int isKeyPressed = 1;
    private float targetRotationX = 110f; // Ŀ����ת�Ƕ�
    private float speed = 70f; // ��ת�ٶ�
    private Quaternion targetRotation; // Ŀ����Ԫ����ת
    private Quaternion originalRotation; // ԭʼ��ת��Ԫ��
    private bool isReturning = false; // ���Э���Ƿ���������

    void Start()
    {
        originalRotation = transform.localRotation; // �洢ԭʼ��ת
        // ����Ŀ����Ԫ����ת��ֻ�ı�X����ת�����������᲻��
        targetRotation = Quaternion.Euler(targetRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    void Update()
    {
        if (isKeyPressed == 2)
        {
            // ʹ�� Quaternion.RotateTowards ��ƽ�����ɵ�Ŀ��Ƕ�
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, speed * Time.deltaTime);
        }
        else
        {
            StartCoroutine(ReturnToOriginalPosition(2f));
            if (isReturning) // ֻ����Э��δ����ʱ������
            {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, originalRotation, speed * Time.deltaTime);
                isReturning = false;
            }
        }
    }

    private IEnumerator ReturnToOriginalPosition(float delay)
    {
        yield return new WaitForSeconds(delay);

        isReturning = true;
    }
}
