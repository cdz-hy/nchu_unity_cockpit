using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDisplayAuto : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite组件
    public Sprite[] pic;            // 图片数组（0-9的十位数字）
    public float Data;    
    public float scaleMultiplier; // 缩放比例
    public int Magnitude;
    public int Case;

    void Start()
    {
        Data = 0;
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // 初始化时应用缩放
        UpdateDisplay();      // 初始更新一次
    }

    void Update()
    {
       Data += 0.01f;
       if(Case == 0)
       {
            //Data = DataCenter.Instance.AirSpeed;
       }else if(Case == 1)
       {
            //Data = DataCenter.Instance.Altitude;
       }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        
    // 提取十位数字（airSpeed=123 → 2, airSpeed=5 → 0）
    int hun = Mathf.FloorToInt(Data / Magnitude) % 10;
    hun = Mathf.Clamp(hun, 0, 9); // 确保数组不越界

    // 更新图片
    numbers.sprite = pic[hun];

    // 确保缩放比例始终生效
    ApplyStaticScale();
    }

    void ApplyStaticScale()
    {
        // 设置静态缩放比例
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}
