using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alt_scrolling : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float resetYPosition1 = 100f; // ����λ�õ�Y��ֵ

    [Header("External Value")]
    public float externalValue1; // �ⲿ�ű��޸ĵ���ֵ
    public float Altitude;

    private Vector3 _initialPosition1;

    void Start()
    {
        _initialPosition1 = transform.localPosition;
    }

    void Update()
    {
        //Altitude = DataCenter.Instance.Altitude;
        //Altitude+=0.01f;
        float value = Altitude % 100;
        externalValue1 = value * 0.0001766f;

        // ֱ��ʹ���ⲿ��ֵ����Y��λ��
        Vector3 newPos = _initialPosition1 - Vector3.up * externalValue1;
        transform.localPosition = newPos;

        // ��ֵ������ֵʱ���ã���ѡ�߼���
        if (value >= resetYPosition1 || value <= -resetYPosition1)
        {
            transform.localPosition = _initialPosition1; // ����λ��
        }
    }
}
