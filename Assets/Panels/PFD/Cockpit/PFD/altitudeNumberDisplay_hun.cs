using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altitudeNumberDisplay_hun : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite���
    public Sprite[] pic;            // ͼƬ���飨0-9��ʮλ���֣�
    public float altitude;    // ��ǰ�ٶ�ֵ
    public float scaleMultiplier = 0.013f; // ���ű���

    void Start()
    {
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // ��ʼ��ʱӦ������
        UpdateDisplay();      // ��ʼ����һ��
    }

    void Update()
    {
        altitude = DataCenter.Instance.altitude;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // ��ȡʮλ���֣�airSpeed=123 �� 2, airSpeed=5 �� 0��
        int hun = Mathf.FloorToInt(altitude / 100) % 10;
        hun = Mathf.Clamp(hun, 0, 9); // ȷ�����鲻Խ��

        // ����ͼƬ
        numbers.sprite = pic[hun];

        // ȷ�����ű���ʼ����Ч
        ApplyStaticScale();
    }

    void ApplyStaticScale()
    {
        // ���þ�̬���ű���
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}
