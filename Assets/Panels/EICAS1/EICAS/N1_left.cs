using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N1_left : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite组件
    public Sprite[] pic;            // 图片数组（0-9的十位数字）
    public float N1_1;    // 当前速度值
    public float scaleMultiplier = 0.0172f; // 缩放比例
    public int digit;

    void Start()
    {
        
        ApplyStaticScale();  // 初始化时应用缩放
        UpdateDisplay();      // 初始更新一次
    }

    void Update()
    {
        N1_1 = 123;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        // 提取数字（airSpeed=123 → 2, airSpeed=5 → 0）
        int ten = Mathf.FloorToInt(N1_1 / digit) % 10;
        ten = Mathf.Clamp(ten, 0, 9); // 确保数组不越界

        // 更新图片
        numbers.sprite = pic[ten];

        // 确保缩放比例始终生效
        ApplyStaticScale();
    }

    void ApplyStaticScale()
    {
        // 设置静态缩放比例
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}
