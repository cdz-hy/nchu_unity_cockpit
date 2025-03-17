using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightBoard : MonoBehaviour
{
    private Quaternion targetRotation;  // Ŀ����ת��(0, 0, 0)
    public float rotationSpeed = 0.05f;  // ��ת�ٶ�
    private bool startRotation = false;  // �����Ƿ�ʼ��ת

    void Start()
    {
        // ��ʼ��Ŀ����תΪ (0, 0, 0)
        targetRotation = Quaternion.Euler(0f, 0f, 0f);

        // ����Э�̣��ȴ� 2 ���ʼ��ת
        StartCoroutine(StartRotationAfterDelay(1f));
    }

    void Update()
    {
        // ����Ѿ��ȴ��� 2 �룬�Ϳ�ʼ��ת
        if (startRotation)
        {
            // ʹ�� Quaternion.RotateTowards ��ƽ�����ɵ�Ŀ����ת
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Э�̣��ȴ�ָ��ʱ���ʼ��ת
    private IEnumerator StartRotationAfterDelay(float delay)
    {
        // �ȴ�ָ����ʱ�䣨�ӳ� 2 �룩
        yield return new WaitForSeconds(delay);

        // �ӳٺ����ÿ�ʼ��ת
        startRotation = true;
    }
}
