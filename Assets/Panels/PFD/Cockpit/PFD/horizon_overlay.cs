using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class horizon_1 : MonoBehaviour
{
    public float rotationSpeed = 360f; // ÿ����ת�ĽǶ�  
    public float pitch;
    private Vector3 initialPosition;    
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;               
    }
    void Update()
    {       
        pitch = 90.0f * Mathf.Sin(Time.time);
        // �� Z ����ת  
        Control(pitch);
    }
    void Control(float angle)
    {
        angle *= 0.000805f;
        Vector3 targetPosition = initialPosition + new Vector3(0, angle, 0);
        StartCoroutine(Move(targetPosition, initialRotation));
    }

    public IEnumerator Move(Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;

        while (progress < 1)
        {
            progress += Time.deltaTime * rotationSpeed;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, progress);
            transform.localRotation = Quaternion.Slerp(startRot, targetRot, progress);
            yield return null;
        }

        // ȷ����ɺ����õ�Ŀ��λ��  
        transform.localPosition = targetPos;
        transform.localRotation = targetRot;
    }
}