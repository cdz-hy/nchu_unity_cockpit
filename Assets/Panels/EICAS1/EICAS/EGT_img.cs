using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EGT_img : MonoBehaviour
{
    public Image imageComponent;
    public int choice; 
    public float data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(choice == 1)
        {
            data = DataCenter.Instance.EGT_Left;
        }else if(choice == 2)
        {
            data = DataCenter.Instance.EGT_Right;
        }
        else if(choice == 3)
        {
            data = DataCenter.Instance.N2_left;
        }
        else if(choice == 4)
        {
            data = DataCenter.Instance.N2_right;
        }
        float fillAmount = 0;
        if (choice == 1 || choice == 2)
        {
            fillAmount = data * (float)(0.6 / 950);
        }else if (choice == 3|| choice == 4)
        {
             fillAmount = data * (float)(1/ 101.5) * (5.0f/9.0f);
        }
        
        imageComponent.fillAmount = fillAmount;
    }
}
