using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinDisplay : MonoBehaviour
{
    public SpriteRenderer numbers;  
    public Sprite[] pic;            
    public DateTime time;
    public float scaleMultiplier = 0.013f;
    public int scale;

    public int hour;
    public int miunte;
    public int Status;
    void Start()
    {
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  
        UpdateDisplay();      
    }

    void Update()
    {
        time = DateTime.Now;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        hour = time.Hour;
        miunte = time.Minute;

        int Data = Mathf.FloorToInt(miunte / scale) % 10;
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
