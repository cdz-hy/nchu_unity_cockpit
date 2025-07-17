using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N1_pointer : MonoBehaviour
{
    public float data;
    public float maxnum;
    public float choice;
    public RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(choice == 1)
        {
            data = DataCenter.Instance.N1_Left;
        }else if(choice == 2)
        {
            data = DataCenter.Instance.N1_Right;
        }
        float angle = data * (float)(200.0 / maxnum);
        angle = (float)-180 + angle;
        rectTransform.localRotation = Quaternion.Euler(0, 0, angle); // 绕Z轴旋转45度
    }
}
