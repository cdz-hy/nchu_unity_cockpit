using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class N2_point : MonoBehaviour
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
        if (choice == 1)
        {
            data = DataCenter.Instance.N2_left;
        }
        else if (choice == 2)
        {
            data = DataCenter.Instance.N2_right;
        }
        float angle = data * (float)(200.0 / maxnum);
        angle = 180f - angle;
      
        rectTransform.localRotation = Quaternion.Euler(0, 0, angle); // ÈÆZÖáÐý×ª45¶È
    }
}
