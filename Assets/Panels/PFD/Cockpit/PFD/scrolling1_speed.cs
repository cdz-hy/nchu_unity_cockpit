using UnityEngine;

public class Scrolling1 : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float resetYPosition1 = 10f; // ����λ�õ�Y��ֵ

    [Header("External Value")]
    public float externalValue1; // �ⲿ�ű��޸ĵ���ֵ
    public float airSpeed;

    private Vector3 _initialPosition1;

    void Start()
    {
        //airSpeed = 3;
        _initialPosition1 = transform.localPosition;
    }

    void Update()
    {
        airSpeed = DataCenter.Instance.airSpeed;
        float value = airSpeed % 10;
        externalValue1 = value * 0.004282f;

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