using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class knob_contral_light: MonoBehaviour

{
    public Camera cam;
    public GameObject KnobObj;
    public Image img;
    private float angle=0;
    private Vector3 past;//存储鼠标之前的位置
    private Vector3 present;//存储鼠标现在的位置
    public int select = 0;
    public int oselect = 0;
    public Button mybutton;
    public int otherchoice;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    // Update is called once per frame
    void Update()
    {
        mybutton.onClick.AddListener(() =>
        {
            if(oselect == 0)
            {
                oselect = 1;
            }
            else
            {
                oselect = 0;
            }
        });
        // 检测鼠标左键是否被按下(0表示左键，1右键，2中键)
        if (Input.GetMouseButtonDown(0))
        {
            select = 1;
            past = Input.mousePosition; // 记录按下时的鼠标位置
        }
        if (Input.GetMouseButtonUp(0))
        {
            select = 0; 
        }

        if(select == 1&&oselect ==1)
        {
            present = Input.mousePosition;
            float changeY = present.y - past.y;
            if(angle + changeY>=0&&angle + changeY <= 255)
            {
                angle = angle + changeY;
                transform.Rotate(0, changeY, 0, Space.Self);
            }
                
            past = present;
            
        }
        if (otherchoice == 1)
        {
            DataCenter.Instance.cockpitLight1 = angle / 255f;
        }
        else
        {
            Color tempColor = img.color;
            tempColor.a = angle / 255f;
            img.color = tempColor;
        }

    }
  
  
}
