using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberDisplayAuto : MonoBehaviour
{
    public SpriteRenderer numbers;  // Sprite���
    public Sprite[] pic;            // ͼƬ���飨0-9��ʮλ���֣�
    public float Data;    
    public float scaleMultiplier; // ���ű���
    public int Magnitude;
    public int Case;

    void Start()
    {
        Data = 0;
        numbers = GetComponent<SpriteRenderer>();
        ApplyStaticScale();  // ��ʼ��ʱӦ������
        UpdateDisplay();      // ��ʼ����һ��
    }

    void Update()
    {
       Data += 0.01f;
       if(Case == 0)
       {
            Data = DataCenter.Instance.airSpeed;
       }else if(Case == 1)
       {
            Data = DataCenter.Instance.altitude;
       }else if(Case == 2)
       {
          //   Data = DataCenter.Instance.Course;
       }else if(Case == 3)
       {
          //   Data = DataCenter.Instance.mach;
       }else if(Case == 4)
       {
          //   Data = DataCenter.Instance.heading;
       }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        
    // ��ȡʮλ���֣�airSpeed=123 �� 2, airSpeed=5 �� 0��
    int hun = Mathf.FloorToInt(Data / Magnitude) % 10;
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
