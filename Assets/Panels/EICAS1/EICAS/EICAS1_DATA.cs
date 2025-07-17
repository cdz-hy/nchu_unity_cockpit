using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EICAS1_DATA: MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite组件
    public Sprite[] pic;            // 图片数组（0-9的十位数字）
    private float data;    // 当前值
    public int choice;
    public float scaleMultiplier = 0.0172f; // 缩放比例
    public float digit;

    void Start()
    {
        
        ApplyStaticScale();  // 初始化时应用缩放
        UpdateDisplay();      // 初始更新一次
    }

    void Update()
    {
       
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        switch (choice)
        {
            case 1: data = DataCenter.Instance.N1_Left; break;//连接N1左边显示框数据
            case 2: data = DataCenter.Instance.N1_Right; break;//连接N1右边显示框数据
            case 3: data = DataCenter.Instance.EGT_Left; break;//连接EGT左边显示框数据
            case 4: data = DataCenter.Instance.EGT_Right; break;//连接EGT右边显示框数据
            case 5: data = DataCenter.Instance.FF_Left; break;//连接FF左边显示框数据
            case 6: data = DataCenter.Instance.FF_Right; break;//连接FF右边显示框数据
            case 7: data = DataCenter.Instance.FUEL_1; break;//连接FUEL第一个显示框数据
            case 8: data = DataCenter.Instance.FUEL_2; break;//连接FUEL第二个显示框数据
            case 9: data = DataCenter.Instance.FUEL_3; break;//连接FUEL第三个显示框数据
            case 10: data = DataCenter.Instance.FUEL_TOTAL; break;//连接TOTAL显示框数据
        }

        // 提取数字（airSpeed=123 → 2, airSpeed=5 → 0）
        int ten = Mathf.FloorToInt(data / digit) % 10;
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
