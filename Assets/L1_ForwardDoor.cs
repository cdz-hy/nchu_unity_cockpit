using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_ForwardDoor : MonoBehaviour
{
    private float targetRotationX = 169.831f; // Ŀ����ת�Ƕ�
    private float speed = 150f; // ��ת�ٶ�
    private Quaternion targetRotation; // Ŀ����Ԫ����ת
    // Start is called before the first frame update
    void Start()
    {
        // ����Ŀ����Ԫ����ת��ֻ�ı�X����ת�����������᲻��
        targetRotation = Quaternion.Euler(targetRotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    // Update is called once per frame
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
