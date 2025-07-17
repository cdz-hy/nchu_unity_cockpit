using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minusDisplay : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite组件
    public Sprite[] pic;            // 图片数组（0-9的十位数字）
    public float Data;
    public float scaleMultiplier; // 缩放比例
    public int Magnitude;
    public int Case;

    void Start()
    {
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // 初始化时应用缩放
        UpdateDisplay();      // 初始更新一次
    }

    void Update()
    {
        //Data = DataCenter.Instance.VertSpeed;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // 更新图片
        if(Data < 0)
        {
            numbers.sprite = pic[0];
        }
        else
        {
            numbers.sprite = pic[1];
        }
            // 确保缩放比例始终生效
            ApplyStaticScale();
    }

    void ApplyStaticScale()
    {
        // 设置静态缩放比例
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}
