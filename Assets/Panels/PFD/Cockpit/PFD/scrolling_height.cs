using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float resetYPosition = 100f; // ����λ�õ�Y��ֵ

    [Header("External Value")]
    public float externalValue; // �ⲿ�ű��޸ĵ���ֵ
    public float height;

    private Vector3 _initialPosition;

    void Start()
    {
        height = 10000;
        _initialPosition = transform.localPosition;
    }

    void Update()
    {
        height-=1;
        float value = height % 100;
        externalValue = value * 0.0001467f;

        // ֱ��ʹ���ⲿ��ֵ����Y��λ��
        Vector3 newPos = _initialPosition + Vector3.up * externalValue;
        transform.localPosition = newPos;

        // ��ֵ������ֵʱ���ã���ѡ�߼���
        if (value >= resetYPosition || value <= -resetYPosition)
        {
            transform.localPosition = _initialPosition; // ����λ��
        }
    }
}