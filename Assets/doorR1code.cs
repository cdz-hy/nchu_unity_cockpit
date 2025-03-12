using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorR1code : MonoBehaviour
{
    [Header("Door Movement Settings")]
    [SerializeField] private float openSpeed = 0.8f;          // �����˶��ٶ�  
    [SerializeField] private float innerPullDistance = 0.1f;  // �������ľ���  
    [SerializeField] private float outerPushAngle = 180f;     // �����ƵĽǶȣ�180�ȣ�    
    [SerializeField] private float finalOffset = 1f;      // ����λ��ƫ����  
    [SerializeField] private float extraLeftOffset = 0.5f;    // �������ƽ�Ƶľ���  

    private Vector3 initialPosition;    // ���ų�ʼλ��  
    private Quaternion initialRotation;  // ���ų�ʼ��ת  
    private bool isDoorOpened = false;   // ����״̬  
    private bool isDoorMoving = false;   // �����Ƿ������ƶ�  

    private void Start()
    {
        // �Զ���ֵΪ��ǰ����� Transform  
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void Update()
    {
        // ���� F ���л�����״̬  
        if (Input.GetKeyDown(KeyCode.F) && !isDoorMoving)
        {
            if (!isDoorOpened)
            {
                StartCoroutine(OpenDoorSequence());
            }
            else
            {
                StartCoroutine(CloseDoorSequence());
            }
        }
    }

    // ����Э�̣��ֽ׶��˶���  
    private System.Collections.IEnumerator OpenDoorSequence()
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

    // ����Э�̣����������  
    private System.Collections.IEnumerator CloseDoorSequence()
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
    private System.Collections.IEnumerator MoveDoor(Vector3 targetPos, Quaternion targetRot)
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
}