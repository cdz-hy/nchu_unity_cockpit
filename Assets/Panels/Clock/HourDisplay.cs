using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockDisplay : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite���
    public Sprite[] pic;            // ͼƬ���飨0-9��ʮλ���֣�
    public DateTime time;
    public float scaleMultiplier = 0.013f;
    public int scale;

    public int hour;
    public int miunte;
    public int Status;
    void Start()
    {
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // ��ʼ��ʱӦ������
        UpdateDisplay();      // ��ʼ����һ��
    }

    void Update()
    {
        time = DateTime.Now;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        hour = time.Hour;
        //miunte = time.Minute;
        //int hour = Mathf.FloorToInt(time / 100) % 10;
        //hun = Mathf.Clamp(hun, 0, 9); // ȷ�����鲻Խ��


        int Data = Mathf.FloorToInt(hour / scale) % 10;
        // ����ͼƬ
        numbers.sprite = pic[Data];

        // ȷ�����ű���ʼ����Ч
        ApplyStaticScale();
    }

    void ApplyStaticScale()
    {
        // ���þ�̬���ű���
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}
