using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AutoPilotNumberDisplay : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite组件
    public Sprite[] pic;            // 图片数组（0-9的十位数字）
    public float Data;
    public float scaleMultiplier; // 缩放比例
    public int Magnitude;
    public string Case;
    public bool Display0;

    void Start()
    {
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // 初始化时应用缩放
        UpdateDisplay();      // 初始更新一次
    }

    void Update()
    {
        if (Case.Equals("AirSpeed"))
        {
            Data = DataCenter.Instance.TargetAirSpeed;
            UpdateDisplay();
        }
        else if (Case.Equals("Course1"))
        {
            Data = DataCenter.Instance.Course1;
            UpdateDisplay();
        }
        else if (Case.Equals("Course2"))
        {
            Data = DataCenter.Instance.Course2;
            UpdateDisplay();
        }
        else if (Case.Equals("Heading"))
        {
            Data = DataCenter.Instance.TargetHeading;
            UpdateDisplay();
        }
        else if (Case.Equals("Altitude"))
        {
            //Data = 10000;
            Data = DataCenter.Instance.TargetAlt;
            DisapearZero();
        }
        else if (Case.Equals("VertSpeed"))
        {
            Data = DataCenter.Instance.VertSpeed;
            vertDisplay();
        }
        else if (Case.Equals("IAS"))
        {
            Data = DataCenter.Instance.IAS;
            UpdateDisplay();
        }
        else if (Case.Equals("0"))
        {
            Data = 0;
            if (!Display0)
            {
                Data = 1;
            }
            UpdateDisplay();
        }

    }
    void DisapearZero()
    {        
        int number = Mathf.FloorToInt(Data / Magnitude) % 10;
        number = Mathf.Clamp(number, 0, 9); // 确保数组不越界
        
        // 更新图片
        numbers.sprite = pic[number];
        if (number == 0 && Data / Magnitude < 1 && Magnitude != 100)
        {
            numbers.sprite = pic[10];
        }
        // 确保缩放比例始终生效
        ApplyStaticScale();
    }

    void UpdateDisplay()
    {

        // 提取十位数字（airSpeed=123 → 2, airSpeed=5 → 0）
        int number = Mathf.FloorToInt(Data / Magnitude) % 10;
        number = Mathf.Clamp(number, 0, 9); // 确保数组不越界

        // 更新图片
        numbers.sprite = pic[number];

        // 确保缩放比例始终生效
        ApplyStaticScale();
    }

    void vertDisplay()
    {
        if(Data >= Magnitude)
        {
            // 提取十位数字（airSpeed=123 → 2, airSpeed=5 → 0）
            int number = Mathf.FloorToInt(Data / Magnitude) % 10;
            number = Mathf.Clamp(number, 0, 9); // 确保数组不越界

            // 更新图片
            numbers.sprite = pic[number];

            // 确保缩放比例始终生效
            ApplyStaticScale();
        }
        else
        {
            numbers.sprite = pic[10];
        }
        
    }
    void ApplyStaticScale()
    {
        // 设置静态缩放比例
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}
